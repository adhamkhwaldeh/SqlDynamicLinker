using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using AlJawad.SqlDynamicLinker.Models;

namespace AlJawad.SqlDynamicLinker.Models
{
    public class EntityColumnSort : ColumnBase 
    {

        /*Mapping to Data*/
        public int SortIndex { set; get; } = 0;
        public bool IsAscending { set; get; } = false;

        public bool IsJsonProperty { set; get; } = false;

        public String JsonIsJsonProperty { set; get; } = "";

        public EntityColumnSort()
        {

        }
        public EntityColumnSort(String dataName, int sortIndex, bool isAscending) => (DataName, SortIndex, IsAscending) = (dataName, sortIndex, isAscending);

        public EntityColumnSort(String dataName, int sortIndex, bool isAscending, bool isJsonProperty, String jsonIsJsonProperty) => (DataName, SortIndex, IsAscending, IsJsonProperty, JsonIsJsonProperty) = (dataName, sortIndex, isAscending, isJsonProperty, jsonIsJsonProperty);

        //public EntityColumnSort(string dataName, int sortIndex, bool isAscending)
        //{
        //    DataName = dataName;
        //    SortIndex = sortIndex;
        //    IsAscending = isAscending;
        //}


        //public string OrderDirection { set; get; } // desc or asc

        //public SortDirections SortDirection { set; get; }

        public static EntityColumnSort Parse(string sortString)
        {
            if (string.IsNullOrEmpty(sortString))
                return null;

            var parts = sortString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return null;

            var sort = new EntityColumnSort { DataName = parts[0]?.Trim() };

            if (parts.Length >= 2)
                sort.IsAscending = parts[1]?.Trim() == "false";

            return sort;
        }

        public override int GetHashCode()
        {
            const int m = -1521134295;
            var hashCode = -2111805952;

            hashCode = hashCode * m + EqualityComparer<string>.Default.GetHashCode(DataName);
            hashCode = hashCode * m + EqualityComparer<string>.Default.GetHashCode(IsAscending.ToString());

            return hashCode;
        }

    }
}