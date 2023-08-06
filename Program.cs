
using System.Text.Json.Serialization;
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
}

app.UseDefaultFiles();
app.UseStaticFiles();


Console.WriteLine("Open http://localhost:5021 to run app.");

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

app.MapGet("/api/testdt", (ApplicationDBContext ctx) =>
    DateTime.Now.ToUniversalTime().ToString("o")
);
app.MapGet("/api/laundryrecords", (ApplicationDBContext ctx) =>
    ctx.LaundryRecords
        ?.Include(r => r.Customer)
        ?.Include(r => r.RecordItems)
);
app.MapGet("/api/laundryrecords/{id}", (ApplicationDBContext ctx, int id) =>
    ctx.LaundryRecords
        ?.Where(r => r.Id == id)
        ?.Include(r => r.Customer)
        ?.Include(r => r.RecordItems)
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
app.MapPost("/api/customers-save-bulk", (
    ApplicationDBContext ctx,
    List<Customer> custs
) =>
{
    ctx.UpdateRange(custs);
    ctx.SaveChanges();
});

app.MapGet("/api/users", (ApplicationDBContext ctx) => ctx.Users);
app.MapPost("/api/users-save-bulk", (
    ApplicationDBContext ctx,
    List<User> users
) =>
{
    var mappedUsers = users.Select(u =>
    {
        var foundUser = ctx.Users?.Where(ux => ux.Id == u.Id).FirstOrDefault();

        // return foundUser; 
        ctx.Entry(foundUser).State = EntityState.Detached;

        if (u.PasswordStr == null || u.PasswordStr == "")
        {
            u.Password = foundUser?.Password;
        }

        return u;
    });

    // return users;
    // return mappedUsers;

    ctx.UpdateRange(mappedUsers);
    ctx.SaveChanges();
});


app.Run();

