using System.Collections.Generic;
using foosballv2s.WebService.Params;

namespace foosballv2s.WebService.Models
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAll(GameParams gameParams, SortParams sortParams, User user);
        Game Get(int id);
        Game Add(Game game);
        bool Remove(int id);
        bool Update(int id, Game game);
    }
}