using System;
using System.Linq;
using System.Reflection;
using foosballv2s.WebService.Models;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Params
{
    public class SortParams
    {
        public string SortBy { get; set; } = "";

        public IQueryable<Team> ApplyTeamSortParams(DbSet<Team> set)
        {
            if (SortBy.Length == 0)
            {
                return set;
            }
            IQueryable<Team> queryableSet = set;
            
            foreach (string sortByParam in SplitSortByParam())
            {
                char direction = sortByParam[0];
                string name = sortByParam.Substring(1);
                string nameFirstCapital = name.First().ToString().ToUpper() + name.Substring(1);
                PropertyInfo property = typeof(Team).GetProperty(nameFirstCapital);
                
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