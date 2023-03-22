using API_Tic_Tac_Toe_Game.AppContext;
using API_Tic_Tac_Toe_Game.Controllers;
using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace API_Tic_Tac_Toe_Game.UnitTests.UnitTestController
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class UnitTestGameController_Player
    {
        private Player GetTP() =>
            new Player { Id = 1, name = "Tom", state = PlayerState.Offline };

        private List<Player> GetAllTP() =>
             new List<Player>()
             {
                new Player { Id = 1, name = "Tom", state = PlayerState.Offline },
                new Player { Id = 2, name = "TIm", state = PlayerState.Offline },
                new Player { Id = 3, name = "Tic", state = PlayerState.Offline },
                new Player { Id = 4, name = "Tac", state = PlayerState.Offline },
                new Player { Id = 5, name = "Toe", state = PlayerState.Offline },
             };

        [Fact]
        public void Get()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            PlayerProvider.Setup(x => x.Get(GetTP().Id)).Returns(GetTP());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var GetPlayer = JObject.FromObject(controller.PlayerGet(1).Value);
            var CorrectPlayer = JObject.FromObject(GetTP());
            Assert.Equal(GetPlayer, CorrectPlayer);
        }

        [Fact]
        public void GetAll()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            PlayerProvider.Setup(x => x.GetAll()).Returns(GetAllTP());

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = JArray.FromObject(controller.GetAllPlayers().Value);
            var Correct = JArray.FromObject(GetAllTP());
            Assert.Equal(Get, Correct);
        }

        [Fact]
        public void Delete()
        {
            var GameProvider = new Mock<IDbProvider<GameRoomBase>>();
            var GameDataProvider = new Mock<IDbProvider<GameMetaData>>();
            var PlayerProvider = new Mock<IDbProvider<Player>>();

            var player = GetTP();

            PlayerProvider.Setup(x => x.Get(player.Id)).Returns(player);
            PlayerProvider.Setup(x => x.Delete(GetTP().Id));

            GameController controller = new GameController(GameProvider.Object, PlayerProvider.Object, GameDataProvider.Object);
            var Get = controller.DeletePLayer(GetTP().Id).Value;
            Assert.True((bool)Get);
        }


    }
}
