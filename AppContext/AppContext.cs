using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace API_Tic_Tac_Toe_Game.AppContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<GameMetaData> GameData { get; set; }
        public DbSet<GameRoomBase> GameRooms { get; set; }
        public DbSet<Player> Players { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
