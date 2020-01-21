using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using gymit.Services;
using gymit.Models.DBContexts;


namespace gymit.Installers
{
    public class DBInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>
                (opt => opt.UseSqlServer(configuration["Data:GymitAPIConnection:ConnectionString"]));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<ITestService, TestService>();
        }
    }
}
