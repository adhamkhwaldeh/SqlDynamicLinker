using AlJawad.SqlDynamicLinker.Plugins;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class SpatialExtensions
    {
        public static IServiceCollection AddSpatialConvention(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer();

            // Manually register your convention plugin
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IConventionSetPlugin, SpatialConventionSetPlugin>()
            );

            services.Replace(ServiceDescriptor.Singleton<IModelCustomizer, SpatialModelCustomizer>()); // 👈 key line

            return services;
        }
    }
}
