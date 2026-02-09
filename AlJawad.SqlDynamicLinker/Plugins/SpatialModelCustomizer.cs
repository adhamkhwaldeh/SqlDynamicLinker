using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlJawad.SqlDynamicLinker.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace AlJawad.SqlDynamicLinker.Plugins
{
  
    public sealed class SpatialModelCustomizer : ModelCustomizer
    {
        public SpatialModelCustomizer(ModelCustomizerDependencies dependencies)
            : base(dependencies) { }

        public override void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            base.Customize(modelBuilder, context);
            SpatialFunctions.Register(modelBuilder);
        }
    }

}
