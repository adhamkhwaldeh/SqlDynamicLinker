using Xunit;
using AlJawad.SqlDynamicLinker.Extensions;
using AlJawad.SqlDynamicLinker.Models;
using AlJawad.SqlDynamicLinker;
using System.Linq;
using System.Collections.Generic;
using AlJawad.SqlDynamicLinker.Enums;

namespace AlJawad.SqlDynamicLinker.Tests
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<SubEntity> Subs { get; set; } = new List<SubEntity>();
    }

    public class SubEntity
    {
        public string SubName { get; set; }
    }

    public class FilterTests
    {
        private IQueryable<TestEntity> GetTestData()
        {
            return new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", Age = 30, Subs = new List<SubEntity> { new SubEntity { SubName = "Sub1" } } },
                new TestEntity { Id = 2, Name = "alice", Age = 25, Subs = new List<SubEntity> { new SubEntity { SubName = "sub1" } } },
                new TestEntity { Id = 3, Name = "Bob", Age = 40 }
            }.AsQueryable();
        }

        [Fact]
        public void Filter_IgnoreCaseTrue_ShouldBeCaseInsensitive()
        {
            var data = GetTestData();
            var filter = new EntityFilter
            {
                DataName = "Name",
                Operator = "eq",
                Value = "ALICE",
                IgnoreCase = true
            };

            var result = data.Filter(filter).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Filter_IgnoreCaseFalse_ShouldBeCaseSensitive()
        {
            var data = GetTestData();
            var filter = new EntityFilter
            {
                DataName = "Name",
                Operator = "eq",
                Value = "Alice",
                IgnoreCase = false
            };

            var result = data.Filter(filter).ToList();

            Assert.Single(result);
            Assert.Equal("Alice", result[0].Name);
        }

        [Fact]
        public void Filter_DefaultIgnoreCase_ShouldApply()
        {
            SqlDynamicLinkerConfig.DefaultIgnoreCase = true;
            var data = GetTestData();
            var filter = new EntityFilter
            {
                DataName = "Name",
                Operator = "eq",
                Value = "ALICE",
                IgnoreCase = null
            };

            var result = data.Filter(filter).ToList();
            Assert.Equal(2, result.Count);

            SqlDynamicLinkerConfig.DefaultIgnoreCase = false;
            result = data.Filter(filter).ToList();
            Assert.Empty(result);
        }

        [Fact]
        public void Filter_Contains_IgnoreCase()
        {
            var data = GetTestData();
            var filter = new EntityFilter
            {
                DataName = "Name",
                Operator = "Contains",
                Value = "LIC",
                IgnoreCase = true
            };

            var result = data.Filter(filter).ToList();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Filter_CollectionContains_IgnoreCase()
        {
            var data = GetTestData();
            var filter = new EntityFilter
            {
                DataName = "Name",
                Operator = "Contains",
                Value = new[] { "ALICE", "BOB" },
                IgnoreCase = true
            };

            var result = data.Filter(filter).ToList();
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Filter_MultipleConditions_IgnoreCase()
        {
            var data = GetTestData();
            var filter = new EntityMultilpleConditionsFilter
            {
                DataName = "Subs",
                NamePropertyOfCollectionList = new List<string> { "SubName" },
                OperatorList = new List<string> { "eq" },
                ValueList = new List<object> { "SUB1" },
                InnerLogicList = new List<string> { "and" },
                IgnoreCase = true
            };

            var result = data.Filter(filter).ToList();
            Assert.Equal(2, result.Count);
        }
    }
}
