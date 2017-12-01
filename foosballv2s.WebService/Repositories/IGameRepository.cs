using System.Collections.Generic;

namespace foosballv2s.WebService.Models
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAll();
        Game Get(int id);
        Game Add(Game game);
        bool Remove(int id);
        bool Update(int id, Game game);
    }
}