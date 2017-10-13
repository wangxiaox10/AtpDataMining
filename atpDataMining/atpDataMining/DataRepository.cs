using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atpDataMining
{
    public class DataRepository : IDataRepository
    {
        public IEnumerable<Player> GetPlayers()
        {
            using (var db = new DataDbContext())
            {
                return db.Players;
            }
        }

        public void SavePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void SavePlayers(IEnumerable<Player> players)
        {
            if (players == null || !players.Any())
            {
                return;
            }
            using (var db = new DataDbContext())
            {
                db.Players.AddRange(players);
                using (var tr = db.Database.BeginTransaction())
                {
                    db.SaveChanges();
                    tr.Commit();
                }
            }
        }
    }
}
