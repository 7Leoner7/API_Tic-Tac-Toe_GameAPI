using API_Tic_Tac_Toe_Game.AppContext;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game
{
    public enum PlayerState
    {
        Offline = 0,
        Online = 1,
        InGame = 2,
    }

    [Table("Players")]
    public class Player : BaseModel
    {
        [JsonProperty("Player_Nick_Name")]
        public string name { get; set; }
        [JsonProperty("PlayerState")]
        public PlayerState state { get; set; }
    }
}
