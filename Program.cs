
using System.Text.Json;
using System.Text.Json.Serialization;
using JWT.Algorithms;
using JWT.Builder;
using laundromatweb.Migrations;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder?.Services.AddSqlite<ApplicationDBContext>("Data Source=laundromatweb.sqlite3");
builder?.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseSqlite("Data Source=laundromatweb.sqlite3")
);
builder?.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.Converters.Add(new JsonDateTimeConverter());
    o.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

});

var app = builder?.Build();



using (var scope = app?.Services.CreateScope())
{
    var ctx = scope?.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    ctx?.Database.Migrate();

    InitProgram.init(ctx);
}

app.UseDefaultFiles();
app.UseStaticFiles();


Console.WriteLine("Open http://localhost:5021 or Open http://localhost:5000 to run app.");

var routeWhitelists = new List<string>();
routeWhitelists.Add("/api/login");

// Middlewar
app?.Use(async (ctx, next) =>
{
    Console.WriteLine($"{ctx.Request.Method} {ctx.Request.Path}");

    if (routeWhitelists.Where(w => ctx.Request.Path.ToString().StartsWith(w)).FirstOrDefault() != null)
    {
        Console.WriteLine($"Whitelist {ctx.Request.Path}");
        await next.Invoke();
        return;
    }

    await next.Invoke();
});


app.MapPost("/api/login", (ApplicationDBContext ctx, LoginBody b) =>
{

    var adminConfig = ctx?.AdminConfigs?.FirstOrDefault();
    var adminPassword = adminConfig?.SuperAdminPassword;
    var jwtSecret = adminConfig?.JwtSecret;

    // Check admin login first
    if (b.Username?.ToLower() == "admin" || b.Username?.ToLower() == "administrator")
    {



        if (BCrypt.Net.BCrypt.Verify(b.Password, adminPassword))
        {
            var tok = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(jwtSecret)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddYears(1).ToUnixTimeSeconds())
                    .AddClaim("admin", true)
                    .Encode();

            return Results.Ok(new LoginReturn()
            {
                Token = tok
            });

        }
        else
        {
            return Results.Problem(detail: "Admin password wrong", statusCode: 401);

        }
    }

    var foundUser = ctx.Users?.Where(u => u.Username.ToLower() == b.Username).ToList().FirstOrDefault();


    if (foundUser == null)
    {
        return Results.Problem(detail: "User not found", statusCode: 401);
    }

    if (!BCrypt.Net.BCrypt.Verify(b.Password, foundUser.Password))
    {
        return Results.Problem(detail: "incorrect password", statusCode: 401);
    }


    var tokx = JwtBuilder.Create()
                       .WithAlgorithm(new HMACSHA256Algorithm())
                       .WithSecret(jwtSecret)
                       .AddClaim("exp", DateTimeOffset.UtcNow.AddYears(1).ToUnixTimeSeconds())
                       .AddClaim("jti", foundUser?.Id)
                       .Encode();

    return Results.Ok(new LoginReturn()
    {
        Token = tokx
    });

});

app.MapGet("/api/inventory-summary", (ApplicationDBContext ctx) =>
{
    var stores = ctx.Stores?.ToList();
    var items = ctx.Items?.ToList();
    var inventory = ctx.Inventory?.ToList();
    var recordItems = ctx.RecordItems?.ToList();


    var inventorySummary = new List<InventorySummary>();

    stores?.ForEach(s =>
    {
        items?.ForEach(i =>
        {
            // By In/Out
            var foundQty = inventory
                ?.Where(inv => inv.ItemId == i.Id && inv.StoreId == s.Id)
                ?.Aggregate(0.0, (acc, inv) =>
                {
                    var qty = inv.Qty;


                    if (inv.Mode == InventoryTransactionMode.OUT)
                    {
                        qty = -(qty);
                    }

                    return acc + (qty ?? 0.0);
                });

            // TODO: By recorditems
            var foundRecordItems = recordItems
                ?.Where(r => r.ItemId == i.Id && r.StoreId == s.Id)
                .Aggregate(0.0, (acc, r) =>
                {
                    return acc + (r.Qty ?? 0);
                }) ?? 0;

            var newInventorySummary = new InventorySummary();
            newInventorySummary.Item = i;
            newInventorySummary.Store = s;
            newInventorySummary.Qty = foundQty - foundRecordItems;


            inventorySummary.Add(newInventorySummary);
        });
    });

    return inventorySummary;
});

app.MapGet("/api/inventory", (ApplicationDBContext ctx) =>
    ctx.Inventory
    ?.Include(i => i.Item)
    ?.Include(i => i.Store)

);
app.MapPost("/api/inventory-save-bulk", (
    ApplicationDBContext ctx,
    List<Inventory> inventory
) =>
{
    ctx.UpdateRange(inventory);
    ctx.SaveChanges();
});

// app.MapGet("/api/testdt", (ApplicationDBContext ctx) =>
//     DateTime.Now.ToUniversalTime().ToString("o")
// );
app.MapGet("/api/laundryrecords", (ApplicationDBContext ctx, string? from, string? to) =>
{
    var fromDate = from != null ? DateTime.Parse(from) : DateTime.Now;
    var toDate = to != null ? DateTime.Parse(to) : DateTime.Now;


    return ctx.LaundryRecords
        ?.Where(r => from != null ? r.CreatedAt >= fromDate : true)
        ?.Where(r => to != null ? r.CreatedAt <= toDate : true)
        ?.Include(r => r.Customer)
        // ?.Include(r => r.Store)
        ?.Include(r => r.RecordItems)
        ?.Include(r => r.RecordExtraServices);

});
app.MapGet("/api/laundryrecords/{id}", (ApplicationDBContext ctx, int id) =>
    ctx.LaundryRecords
        ?.Where(r => r.Id == id)
        ?.Include(r => r.Customer)
        ?.Include(r => r.RecordItems)
        ?.Include(r => r.RecordExtraServices)
        ?.FirstOrDefault()
);

app.MapPost("/api/laundryrecords-save-bulk", (
    ApplicationDBContext ctx,
    List<LaundryRecord> laundryRecords
) =>
{
    ctx.UpdateRange(laundryRecords);
    ctx.SaveChanges();
});


app.MapGet("/api/stores", (ApplicationDBContext ctx) =>
    ctx.Stores
);
app.MapPost("/api/stores-save-bulk", (
    ApplicationDBContext ctx,
    List<Store> stores
) =>
{
    ctx.UpdateRange(stores);
    ctx.SaveChanges();
});

app.MapGet("/api/appstats", (ApplicationDBContext ctx) =>
    {
        var a = new AppStats();

        a.Stores = ctx.Stores?.Count();
        a.Items = ctx.Items?.Count();
        a.Inventory = ctx.Inventory?.Count();
        a.Customers = ctx.Customers?.Count();
        a.Users = ctx.Users?.Count();

        return a;
    }
);

app.MapGet("/api/items", (ApplicationDBContext ctx) =>
    ctx.Items
);
app.MapPost("/api/items-save-bulk", (
    ApplicationDBContext ctx,
    List<Item> items
) =>
{
    ctx.UpdateRange(items);
    ctx.SaveChanges();
});

app.MapGet("/api/customers", (ApplicationDBContext ctx) =>
    ctx.Customers
);
// app.MapGet("/api/testdt", (ApplicationDBContext ctx) =>
//     DateTime.Parse("2023-09-02T12:21:29.3371997ZZ", null, System.Globalization.DateTimeStyles.RoundtripKind)
// );

app.MapPost("/api/customers-save-bulk", (
    ApplicationDBContext ctx,
    List<Customer> custs
) =>
{
    ctx.UpdateRange(custs);
    ctx.SaveChanges();

    return custs;
});

app.MapGet("/api/adminconfig", (ApplicationDBContext ctx) => ctx.AdminConfigs?.FirstOrDefault());
app.MapPost("/api/adminconfig", (ApplicationDBContext ctx, AdminConfig adminConfig) =>
{
    ctx.Update(adminConfig);
    ctx.SaveChanges();
});


app.MapGet("/api/users", (ApplicationDBContext ctx) => ctx.Users);
app.MapGet("/api/user-info", (ApplicationDBContext ctx, HttpRequest r) =>
{
    // decode jwt;
    var adminConfig = ctx?.AdminConfigs?.FirstOrDefault();
    var adminPassword = adminConfig?.SuperAdminPassword;
    var jwtSecret = adminConfig?.JwtSecret;


    var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(jwtSecret)
                    .Decode<IDictionary<string, object>>(r.Headers["Authorization"]);


    var jti = int.Parse($"{json["jti"]}");


    return ctx.Users.Where(u => u.Id == jti).ToList().FirstOrDefault();

});

app.MapPost("/api/users-save-bulk", (
    ApplicationDBContext ctx,
    List<User> users
) =>
{
    var mappedUsers = users.Select(u =>
    {

        Console.WriteLine(JsonSerializer.Serialize(u));
        var foundUser = ctx.Users?.Where(ux => ux.Id == u.Id).ToList().FirstOrDefault();

        if (foundUser == null)
        {
            foundUser = u;

        }
        else
        {
            // return foundUser; 
            ctx.Entry(foundUser).State = EntityState.Detached;
        }

        Console.WriteLine("FOUNDUSER");

        Console.WriteLine(JsonSerializer.Serialize(foundUser));
        Console.WriteLine($"{foundUser?.Password} {foundUser?.PasswordStr} {u.PasswordStr}");


        if (u.PasswordStr == null || u.PasswordStr == "")
        {
            u.Password = foundUser?.Password;
        }
        else
        {
            // Console.WriteLine($"newpassword {u.Password} {BCrypt.Net.BCrypt.HashPassword(u.Password)}");
            u.Password = BCrypt.Net.BCrypt.HashPassword(u.PasswordStr);
        }

        return u;
    });

    Console.WriteLine(JsonSerializer.Serialize(mappedUsers));



    // return users;
    // return mappedUsers;

    ctx.UpdateRange(mappedUsers);
    ctx.SaveChanges();
});


app.Run();

