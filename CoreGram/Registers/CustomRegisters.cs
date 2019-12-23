using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CoreGram.Registers
{
    public static class CustomRegisters
    {
        /// <summary>
        /// Registro de los servicios/repositorios personalizados
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection addCustomRegisters(this IServiceCollection services)
        {
            services.AddTransient(typeof(UserRepository));
            services.AddTransient(typeof(UserProfileRepository));
            services.AddTransient(typeof(PostRepository));
            services.AddTransient(typeof(FollowerRepository));
            services.AddTransient(typeof(LikeRepository));
            services.AddTransient(typeof(CommentRepository));
            return services;
        }
    }
}
