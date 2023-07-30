using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder?.Services.AddSqlite<ApplicationDBContext>("Data Source=laundromatweb.sqlite3");
builder?.Services.AddDbContext<ApplicationDBContext>(o =>
    o.UseSqlite("Data Source=laundromatweb.sqlite3")
);


var app = builder?.Build();


using (var scope = app?.Services.CreateScope())
{
    var ctx = scope?.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    ctx?.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();


Console.WriteLine("Open http://localhost:5021 to run app.");

app.MapGet("/api/customers", (ApplicationDBContext ctx) =>
    ctx.Customers
        ?.Include(c => c.CreatedBy)
);
app.MapPost("/api/customers-save-bulk", (
    ApplicationDBContext ctx,
    [FromBody] List<Customer> custs
) =>
{
    ctx.UpdateRange(custs);
    ctx.SaveChanges();
});

app.MapGet("/api/users", (ApplicationDBContext ctx) => ctx.Users);
app.MapPost("/api/users-save-bulk", (
    ApplicationDBContext ctx,
    [FromBody] List<User> users
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
