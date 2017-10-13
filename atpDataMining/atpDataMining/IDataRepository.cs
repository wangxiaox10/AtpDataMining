using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atpDataMining
{
    public interface IDataRepository
    {
        IEnumerable<Player> GetPlayers();
        void SavePlayers(IEnumerable<Player> players);
        void SavePlayer(Player player);
    }
}
