using CoreGram.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Registers
{
    public static class CustomRegisters
    {
        public static IServiceCollection addCustomRegisters(this IServiceCollection services)
        {
            services.AddTransient(typeof(UserRepository));
            services.AddTransient(typeof(UserProfileRepository));
            return services;
        }
    }
}
