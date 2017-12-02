using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using foosballv2s.WebService.Models;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Params
{
    public class SortParams
    {
        public string SortBy { get; set; } = "";

        public IEnumerable<T> ApplySortParams<T>(IEnumerable<T> set) where T : class
        {
            if (SortBy.Length == 0)
            {
                return set;
            }
            IEnumerable<T> queryableSet = set;
            
            foreach (string sortByParam in SplitSortByParam())
            {
                char direction = sortByParam[0];
                string name = sortByParam.Substring(1);
                string nameFirstCapital = name.First().ToString().ToUpper() + name.Substring(1);
                PropertyInfo property = typeof(T).GetProperty(nameFirstCapital);

                if (property == null)
                {
                    continue;
                }
                
                if (direction == '+')
                {
                    queryableSet = queryableSet.OrderBy(t => property.GetValue(t, null));
                }
                else if (direction == '-')
                {
                    queryableSet = queryableSet.OrderByDescending(t => property.GetValue(t, null));
                }
                else
                {
                    continue;
                }
                
            }
            return queryableSet;
        }

        private string[] SplitSortByParam()
        {
            return SortBy.Split(",");
        }
    }
}