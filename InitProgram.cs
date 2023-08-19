
using BCrypt.Net;

class InitProgram
{
    public static async void init(ApplicationDBContext? ctx)
    {
        Console.WriteLine("-- DB INIT --");
        Console.WriteLine();

        var cfg = ctx?.AdminConfigs?.ToList()?.FirstOrDefault();

        if (cfg == null)
        {
            Console.WriteLine("Adminconfig not found");
            var newCfg = new AdminConfig();
            ctx?.Update(newCfg);

            ctx?.SaveChanges();

            cfg = newCfg;
        }

        if (cfg.SuperAdminPassword == null)
        {
            Console.WriteLine("Superadmin password null.");
            cfg.SuperAdminPassword = BCrypt.Net.BCrypt.HashPassword("laundromatadmin");

        }

        if (cfg.JwtSecret == null)
        {
            Console.WriteLine("jwtsecret null.");
            cfg.JwtSecret = Guid.NewGuid().ToString("n").Substring(0, 23);

        }



        ctx?.Update(cfg);
        ctx?.SaveChanges();
    }
}