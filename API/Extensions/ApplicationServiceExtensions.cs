using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interface;
using API.Mappers;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        
        public static IServiceCollection AddApplicationServiceExtension (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMaperUsertoUserDto,MaperUsertoUserDto>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddDbContext<DataContext>(options=>{
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}