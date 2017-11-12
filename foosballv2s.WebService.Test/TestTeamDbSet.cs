using System;
using System.Linq;
using foosballv2s.WebService.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace foosballv2s.WebService.Test
{
    class TestTeamDbSet : TestDbSet<Team>
    {
        public override Team Find(params object[] keyValues)
        {
            return this.SingleOrDefault(team => team.Id == (int)keyValues.Single());
        }

        public override EntityEntry<Team> Update(Team team)
        {
            var currentItem = Find(team.Id);
            Update(currentItem, team);
            return null;
        }
    }
}