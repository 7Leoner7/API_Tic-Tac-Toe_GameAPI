using Newtonsoft.Json;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game
{
    public enum GameState
    {
        Wait = 0,
        GameIsStart = 1,
        Completed = 2,
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("GameMetaData")]
    public class GameMetaData : BaseModel
    {
        [JsonProperty("SymbX")]
        public char X { get; set; }
        [JsonProperty("SymbO")]
        public char O { get; set; }
        [JsonProperty("SymbVoidCell")]
        public char VoidCell { get; set; }
    }

    public class BaseInitGameMetaData : GameMetaData
    {
        public BaseInitGameMetaData()
        {
            this.X = 'x';
            this.O = 'o';
            this.VoidCell = '*';
            this.Id = 0;
        }
    }
}
