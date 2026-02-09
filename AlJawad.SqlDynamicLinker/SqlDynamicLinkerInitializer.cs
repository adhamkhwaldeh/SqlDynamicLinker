using AlJawad.SqlDynamicLinker.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AlJawad.SqlDynamicLinker.Extensions;
using AlJawad.SqlDynamicLinker.ModelBinder;

namespace AlJawad.SqlDynamicLinker
{
    public static class SqlDynamicLinkerInitializer
    {
        
        public static IServiceCollection InitializerSqlDynamicLinker(this IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Add(new BaseQueryableFilterBinderProvider());
                options.ModelBinderProviders.Add(new BasePagingFilterBinderProvider());
            });

            services.AddEntityFrameworkSqlServer();
            services.AddSpatialConvention();
            return services;
        }
    }
}
