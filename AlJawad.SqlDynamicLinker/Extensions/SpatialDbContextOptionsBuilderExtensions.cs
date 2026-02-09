using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlJawad.SqlDynamicLinker.Extensions
{
    public static class SpatialDbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseSpatialExtensions(this DbContextOptionsBuilder optionsBuilder)
        {
            var extension = new SpatialDbContextOptionsExtension();
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
            return optionsBuilder;
        }
    }
}
