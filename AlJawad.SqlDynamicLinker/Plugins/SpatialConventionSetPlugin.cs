using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;


namespace AlJawad.SqlDynamicLinker.Plugins
{

    public sealed class SpatialConventionSetPlugin : IConventionSetPlugin
    {
        public ConventionSet ModifyConventions(ConventionSet conventionSet)
        {
            conventionSet.ModelFinalizingConventions.Add(new SpatialModelFinalizingConvention());
            return conventionSet;
        }

        private sealed class SpatialModelFinalizingConvention : IModelFinalizingConvention
        {

            public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
            {
                Console.WriteLine("✅ SpatialModelFinalizingConvention invoked");
                // Ensure our custom spatial functions are registered automatically
                SpatialFunctions.Register((ModelBuilder)modelBuilder);
            }
        }
    }

}
