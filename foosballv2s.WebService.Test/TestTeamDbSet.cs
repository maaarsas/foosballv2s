using System;
using System.Linq;
using foosballv2s.WebService.Models;

namespace foosballv2s.WebService.Test
{
    class TestTeamDbSet : TestDbSet<Team>
    {
        public override Team Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (int)keyValues.Single());
        }
    }
}