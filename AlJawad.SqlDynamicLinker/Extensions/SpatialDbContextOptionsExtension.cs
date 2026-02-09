using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Plugins;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace AlJawad.SqlDynamicLinker.Extensions
{

    public class SpatialDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public void ApplyServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IModelCustomizer, SpatialModelCustomizer>());
        }

        public void Validate(IDbContextOptions options) { }

        public string LogFragment => " using SpatialExtension";
        public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) { }
            public override bool IsDatabaseProvider => false;
            public override string LogFragment => " using SpatialExtension";

            public override int GetServiceProviderHashCode()
            {
                return 0;
            }

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            {
                return false;
            }
        }
    }

}
