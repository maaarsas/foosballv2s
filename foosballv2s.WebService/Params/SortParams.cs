using System;
using System.Linq;
using foosballv2s.WebService.Models;
using Microsoft.EntityFrameworkCore;

namespace foosballv2s.WebService.Params
{
    public class SortParams
    {
        public string SortBy { get; set; } = "";

        public DbSet<Team> ApplyTeamSortParams(DbSet<Team> set)
        {
            foreach (string sortByParam in SplitSortByParam())
            {
                char direction = sortByParam[0];
                string name = sortByParam.Substring(1);

                if (direction == '+')
                {
                    set.OrderBy(t => t.GetType().GetProperty(name.ToUpper()));
                }
                else if (direction == '-')
                {
                    set.OrderByDescending(t => t.GetType().GetProperty(name.ToUpper()));
                }
                else
                {
                    continue;
                }
                
            }
            return set;
        }

        private string[] SplitSortByParam()
        {
            return SortBy.Split(",");
        }
    }
}