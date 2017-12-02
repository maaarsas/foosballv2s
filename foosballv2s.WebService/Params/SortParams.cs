using System;
using System.Linq;
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
                
                
                if (direction == '+')
                {
                    queryableSet = queryableSet.OrderBy(t => t.GetType().GetProperty(name.ToUpper()).GetValue(t));
                }
                else if (direction == '-')
                {
                    queryableSet = queryableSet.OrderByDescending(t => t.GetType().GetProperty(name.ToUpper()).GetValue(t));
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