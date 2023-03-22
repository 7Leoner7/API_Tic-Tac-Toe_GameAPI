using API_Tic_Tac_Toe_Game.AppContext;
using API_Tic_Tac_Toe_Game.Controllers;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace API_Tic_Tac_Toe_Game.UnitTests.UnitTestController
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class UnitTestGameController_GameRoom
    {

        private GameRoomBase GetGR() =>
            new GameRoomBase { Id = 1, gameDataId = 1, GameMap = "*********", gameState = GameState.Wait, Name = "Test", playerO_ID = 1, playerX_ID = 2 };

        private List<GameRoomBase> GetAllGR() =>
           new List<GameRoomBase>()
           {
                new GameRoomBase { Id = 1, gameDataId = 1, GameMap = "*********", gameState = GameState.Wait, Name = "Test0", playerO_ID = 1, playerX_ID = 2 },
                new GameRoomBase { Id = 2, gameDataId = 1, GameMap = "*********", gameState = GameState.Wait, Name = "Test1", playerO_ID = 1, playerX_ID = 2 },
                new GameRoomBase { Id = 3, gameDataId = 1, GameMap = "*********", gameState = GameState.Wait, Name = "Test2", playerO_ID = 1, playerX_ID = 2 },
                new GameRoomBase { Id = 4, gameDataId = 1, GameMap = "*********", gameState = GameState.Wait, Name = "Test3", playerO_ID = 1, playerX_ID = 2 },
           };

        [Fact]
        public void Get()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            GameProvider.Setup(x => x.Get(GetGR().Id)).Returns(GetGR());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = JObject.FromObject(controller.GetGame(1).Value);
            var Correct = JObject.FromObject(GetGR());
            Assert.Equal(Get, Correct);
        }

        [Fact]
        public void GetAll()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            GameProvider.Setup(x => x.GetAll()).Returns(GetAllGR());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = JArray.FromObject(controller.GetAllGames().Value);
            var Correct = JArray.FromObject(GetAllGR());
            Assert.Equal(Get, Correct);
        }

        [Fact]
        public void Delete()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            var gameRoom = GetGR();

            GameProvider.Setup(x => x.Get(gameRoom.Id)).Returns(gameRoom);
            GameProvider.Setup(x => x.Delete(GetGR().Id));

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = controller.DeletePLayer(GetGR().Id).Value;
            Assert.True((bool)Get);
        }
    }
}
