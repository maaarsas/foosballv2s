using System.Linq;
using foosballv2s.WebService.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace foosballv2s.WebService.Test
{
    class TestGameDbSet : TestDbSet<Game>
    {
        public override Game Find(params object[] keyValues)
        {
            return this.SingleOrDefault(game => game.Id == (int)keyValues.Single());
        }
        
        public override EntityEntry<Game> Update(Game game)
        {
            var currentItem = Find(game.Id);
            Update(currentItem, game);
            return null;
        }
    }
}