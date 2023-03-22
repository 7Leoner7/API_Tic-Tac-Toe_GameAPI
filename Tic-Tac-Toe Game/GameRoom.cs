using API_Tic_Tac_Toe_Game.AppContext;
using Newtonsoft.Json;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game
{
    [System.ComponentModel.DataAnnotations.Schema.Table("GameRooms")]
    public class GameRoomBase : BaseModel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("gameData")]
        public int gameDataId { get; set; }
        [JsonProperty("GameMap")]
        public string GameMap { get; set; }
        [JsonProperty("GameState")]
        public GameState gameState { get; set; }
        [JsonProperty("PlayerX")]
        public int playerX_ID { get; set; }
        [JsonProperty("PlayerY")]
        public int playerO_ID { get; set; }
    }

    public class GameRoom : GameRoomBase
    {
        private GameProcess g_proc { get; set; }
        private IDbProvider<GameMetaData> GameDataProvider { get; set; }
        private IDbProvider<GameRoomBase> GameProvider { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр игры
        /// </summary>
        /// <returns></returns>
        public void GameProcessInit()
        {
            this.GameMap = "*********";
            this.gameState = 0;
            this.playerX_ID = -1;
            this.playerO_ID = -1;

            g_proc = new GameProcess(GameProvider, GameDataProvider, this);
        }

        public bool ReqForClose() => g_proc.ReqForCloseOrDeleteRoom(false);

        public bool ReqForDelete() => g_proc.ReqForCloseOrDeleteRoom(true);

        public char ReturnWinner() => g_proc.CheckWinner();

        public GameRoom(IDbProvider<GameRoomBase> GameProvider, IDbProvider<GameMetaData> GameDataProvider, string? name)
        {
            this.GameProvider = GameProvider;
            this.GameDataProvider = GameDataProvider;
            this.Name = name != null ? name : string.Empty;
        }

        public GameRoom(GameRoomBase game, IDbProvider<GameRoomBase> GameProvider, IDbProvider<GameMetaData> GameDataProvider) 
        {
            Id= game.Id;
            Name = game.Name;
            gameDataId = game.gameDataId;
            this.GameProvider = GameProvider;
            this.GameDataProvider = GameDataProvider;
            this.GameMap = game.GameMap;
            this.gameState = game.gameState;
            this.playerX_ID = game.playerX_ID;
            this.playerO_ID = game.playerO_ID;
            g_proc = new GameProcess(this.GameProvider, this.GameDataProvider, this);
        }
    }
}
