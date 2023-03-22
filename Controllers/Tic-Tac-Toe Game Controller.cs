using API_Tic_Tac_Toe_Game.AppContext;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game.ErrorGames;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_Tic_Tac_Toe_Game.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private IDbProvider<GameMetaData> GameDataProvider { get; set; }
        private IDbProvider<GameRoomBase> GameProvider { get; set; }
        private IDbProvider<Player> PlayerProvider { get; set; }

        public GameController(IDbProvider<GameRoomBase> GameProvider, IDbProvider<Player> PlayerProvider, IDbProvider<GameMetaData> GameDataProvider)
        {
            this.GameProvider = GameProvider;
            this.PlayerProvider = PlayerProvider;
            this.GameDataProvider = GameDataProvider;
        }

        //Player

        /// <summary>
        /// �������� ������ ���� �������
        /// </summary>
        [Tags("Player")]
        [HttpGet("/GameTic-Tac-Toe/Player/GetAll")]
        public JsonResult GetAllPlayers() =>
            new JsonResult(PlayerProvider.GetAll());

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [Tags("Player")]
        [HttpPost("/GameTic-Tac-Toe/Player/Create")]
        public JsonResult PlayerCreate(Player player) =>
            new JsonResult(PlayerProvider.Create(player));

        /// <summary>
        /// �������� ������.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Tags("Player")]
        [HttpGet("/GameTic-Tac-Toe/Player/Get")]
        public JsonResult PlayerGet(int id) =>
            new JsonResult(PlayerProvider.Get(id));

        /// <summary>
        /// ��������� �������� PlayerState ������.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [Tags("Player")]
        [HttpPut("/GameTic-Tac-Toe/Player/SetState")]
        public JsonResult PlayerSetState(int id, PlayerState state)
        {
            try
            {
                var player = PlayerProvider.Get(id);
                player.state = state;
                return new JsonResult(PlayerProvider.Update(player));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Tags("Player")]
        [HttpDelete("/GameTic-Tac-Toe/Player/Delete")]
        public JsonResult DeletePLayer(int id)
        {
            try
            {
                PlayerProvider.Delete(id);
                return new JsonResult(true);
            }
            catch
            {
                return new JsonResult(false);
            }
        }

        //GameRoom

        /// <summary>
        /// ���������� ��� ������� �������
        /// </summary>
        [Tags("GameRoom")]
        [HttpGet("/GameTic-Tac-Toe/GameRoom/GetAll")]
        public JsonResult GetAllGames() =>
            new JsonResult(GameProvider.GetAll());

        /// <summary>
        /// ���������� ����������� ������� �������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpGet("/GameTic-Tac-Toe/GameRoom/GetGame")]
        public JsonResult GetGame(int id) =>
            new JsonResult(GameProvider.Get(id));

        /// <summary>
        /// ������ ������� � ���������.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPost("/GameTic-Tac-Toe/GameRoom/Create")]
        public JsonResult CreateGame(string? name)
        {
            var ModelGameRoom = new GameRoom(GameProvider, GameDataProvider, name);
            ModelGameRoom.GameProcessInit();
            return new JsonResult(ModelGameRoom);
        }

        /// <summary>
        /// ������������� GameMetaData ��� �������
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="Meta_Id"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/SetGameMetaData")]
        public JsonResult SetGameMetaData(int id_game, int Meta_Id)
        {
            try
            {
                var ModelGameRoom = GameProvider.Get(id_game);
                ModelGameRoom.gameDataId = GameDataProvider.Get(Meta_Id) != null ? Meta_Id : 0;
                if (ModelGameRoom.gameDataId == 0) throw new Exception("GameMetaDate with given Id not found");
                GameProvider.Update(ModelGameRoom);
                return new JsonResult(ModelGameRoom);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// ���������� ������ �� �������� �������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpDelete("/GameTic-Tac-Toe/GameRoom/Delete")]
        public JsonResult DeleteGame(int id)
        {
            GameRoomBase ModelGameRoom = GameProvider.Get(id);
            GameRoom gameRoom = new GameRoom(ModelGameRoom, GameProvider, GameDataProvider);
            return new JsonResult(gameRoom.ReqForDelete());
        }

        /// <summary>
        /// ������������� ��������� ��� ��������� ������� �������
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/Update")]
        public JsonResult UpdateGame(GameRoomBase game) =>
            new JsonResult(GameProvider.Update(game));

        /// <summary>
        /// ����� ����� � ������� �� ������� X
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="id_player"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/AddPlayerX")]
        public JsonResult AddPlayerX_InGame(int id_game, int id_player)
        {
            var ModelGameRoom = GameProvider.Get(id_game);
            ModelGameRoom.playerX_ID = id_player;
            return UpdateGame(ModelGameRoom);
        }

        /// <summary>
        /// ����� ����� � ������� �� ������� O
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="id_player"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/AddPlayerO")]
        public JsonResult AddPlayerO_InGame(int id_game, int id_player)
        {
            var ModelGameRoom = GameProvider.Get(id_game);
            ModelGameRoom.playerO_ID = id_player;
            return UpdateGame(ModelGameRoom);
        }

        /// <summary>
        /// ����� X ����� �� �������
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="id_player"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/ExitPlayerX")]
        public JsonResult ExitPlayerX_OfGame(int id_game, int id_player)
        {
            var ModelGameRoom = GameProvider.Get(id_game);
            ModelGameRoom.playerX_ID = -1;
            var player = PlayerProvider.Get(id_player);
            player.state = PlayerState.Online;
            return PlayerSetState(id_player, player.state);
        }

        /// <summary>
        /// ����� O ����� �� �������
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="id_player"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/ExitPlayerO")]
        public JsonResult ExitPlayerO_OfGame(int id_game, int id_player)
        {
            var ModelGameRoom = GameProvider.Get(id_game);
            ModelGameRoom.playerO_ID = -1;
            var player = PlayerProvider.Get(id_player);
            player.state = PlayerState.Online;
            return PlayerSetState(id_player, player.state);
        }

        /// <summary>
        /// ������������� ������ ��������� ����
        /// </summary>
        /// <param name="id_game"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/SetGameState")]
        public JsonResult SetGameState(int id_game, GameState state)
        {
            var ModelGameRoom = GameProvider.Get(id_game);
            ModelGameRoom.gameState = state;
            GameProvider.Update(ModelGameRoom);
            return new JsonResult(ModelGameRoom);
        }

        /// <summary>
        /// ���������� ������ �� �������� ������� (���������� ����)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/ReqForClose")]
        public JsonResult ReqForCloseGame(int id)
        {
            GameRoom ModelGameRoom = (GameRoom)GameProvider.Get(id);
            return new JsonResult(ModelGameRoom.ReqForClose());
        }

        /// <summary>
        /// ������ ������� ����� � ���������� ����������
        /// </summary>
        /// <remarks>
        /// ������� ����� Map ������� �� ����� ������ ������ 9 ��������.
        /// 
        /// Ÿ ��������� �� 3 ������� ����� �� 3 �������.
        /// 
        /// � ����� ����� �������������� ������ �� �������, ������� �������� � ����-������ ���� (X, O, VoidCell)
        /// 
        /// ���� ��������� ������ X - ���������� Player_X
        /// 
        /// ���� ��������� ������ O - ���������� Player_O
        /// 
        /// ���� ��������� ������ * - �����, ���� ���� �����������
        /// 
        /// ������ �����: oxoxox*xo
        /// 
        /// MetaData = { x: "x", o: "o", voidcell: "*" }
        /// 
        /// oxo
        /// 
        /// xox
        /// 
        /// *xo
        /// 
        /// ���������: o
        /// 
        /// </remarks>
        /// <returns></returns>
        [Tags("GameRoom")]
        [HttpPut("/GameTic-Tac-Toe/GameRoom/SetGameMapAndReturnTheWinner")]
        public JsonResult SetGameMap(int id_game, string Map)
        {
            try
            {
                GameRoomBase GameRoomBase = GameProvider.Get(id_game);
                if (GameRoomBase.gameDataId == 0) throw new Exception("Missing gameDataId (You should be use SetGameMetaData)");
                GameRoomBase.GameMap = Map;
                GameProvider.Update(GameRoomBase);
                GameRoom gameRoom = new GameRoom(GameRoomBase, GameProvider, GameDataProvider);
                return new JsonResult(gameRoom.ReturnWinner());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

        }

        //GameMetaData

        /// <summary>
        /// ������� ��� ��������� ����-������ ���
        /// </summary>
        /// <returns></returns>
        [Tags("GameMetaData")]
        [HttpGet("/GameTic-Tac-Toe/GameMetaData/GetAll")]
        public JsonResult GameMetaData_GetAll() =>
            new JsonResult(GameDataProvider.GetAll());

        /// <summary>
        /// ������� ����-������ �� �� Id
        /// </summary>
        /// <returns></returns>
        [Tags("GameMetaData")]
        [HttpGet("/GameTic-Tac-Toe/GameMetaData/Get")]
        public JsonResult GameMetaData_Get(int id) =>
            new JsonResult(GameDataProvider.Get(id));

        /// <summary>
        /// �������� ����-������ ����
        /// </summary>
        /// <remarks>
        ///     ������:
        /// 
        ///     POST /GameTic-Tac-Toe/GameMetaData/Create
        /// 
        ///     {
        ///     
        ///         "id": 0,
        ///     
        ///         "x": "x",
        ///     
        ///         "o": "o",
        ///     
        ///         "voidCell": "*"
        ///     
        ///      }
        ///      
        ///    ����� x, o, voidCell ��������� ��� ������ char
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(GameMetaData), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequestResult), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ConflictResult), (int)HttpStatusCode.Conflict)]
        [Tags("GameMetaData")]
        [HttpPost("/GameTic-Tac-Toe/GameMetaData/Create")]
        public IActionResult GameMetaData_Create(GameMetaData data)
        {
            ErrorGame<GameMetaData> dataError = new ErrorGame<GameMetaData>(data);
            var res = dataError.IsValid(ErrorGameMetaData.HasError);
            if (res == ErrorGame<BaseModel>.HasNoErrors)
                return new JsonResult(GameDataProvider.Create(data));
            else return Conflict(res.Item1 + ", " + res.Item2);
        }

        /// <summary>
        /// ��������� ����-������ ���
        /// </summary>
        /// <returns></returns>
        [Tags("GameMetaData")]
        [HttpPut("/GameTic-Tac-Toe/GameMetaData/Update")]
        public JsonResult GameMetaData_Update(GameMetaData data) =>
            new JsonResult(GameDataProvider.Update(data));

        /// <summary>
        /// ������� ����-������ ���
        /// </summary>
        /// <returns></returns>
        [Tags("GameMetaData")]
        [HttpDelete("/GameTic-Tac-Toe/GameMetaData/Delete")]
        public JsonResult GameMetaData_Delete(int id)
        {
            try
            {
                GameDataProvider.Delete(id);
                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}