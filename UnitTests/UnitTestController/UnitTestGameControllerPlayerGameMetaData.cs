using API_Tic_Tac_Toe_Game.AppContext;
using API_Tic_Tac_Toe_Game.Controllers;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace API_Tic_Tac_Toe_Game.UnitTests.UnitTestController
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class UnitTestGameController_GameMetaData
    {

        private GameMetaData GetGMD() =>
            new GameMetaData { Id = 1, O = 'o', X = 'x', VoidCell = '*' };

        private List<GameMetaData> GetAllGMD() =>
            new List<GameMetaData>()
            {
                  new GameMetaData { Id = 1, O = 'o', X = 'x', VoidCell = '*' },
                  new GameMetaData { Id = 2, O = 'o', X = 'X', VoidCell = '|' },
                  new GameMetaData { Id = 3, O = 'O', X = 'x', VoidCell = '0' },
                  new GameMetaData { Id = 4, O = 'O', X = 'X', VoidCell = '-' }
            };

        [Fact]
        public void Get()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            GameDataProvider.Setup(x => x.Get(GetGMD().Id)).Returns(GetGMD());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var GetPlayer = JObject.FromObject(controller.GameMetaData_Get(1).Value);
            var CorrectPlayer = JObject.FromObject(GetGMD());
            Assert.Equal(GetPlayer, CorrectPlayer);
        }

        [Fact]
        public void GetAll()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            GameDataProvider.Setup(x => x.GetAll()).Returns(GetAllGMD());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = JArray.FromObject(controller.GameMetaData_GetAll().Value);
            var Correct = JArray.FromObject(GetAllGMD());
            Assert.Equal(Get, Correct);
        }

        [Fact]
        public void Delete()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            var data = GetGMD();

            GameDataProvider.Setup(x => x.Get(data.Id)).Returns(data);
            GameDataProvider.Setup(x => x.Delete(GetGMD().Id));

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = controller.GameMetaData_Delete(GetGMD().Id).Value;
            Assert.True((bool)Get);
        }
    }
}
