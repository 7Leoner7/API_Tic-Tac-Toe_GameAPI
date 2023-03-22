using API_Tic_Tac_Toe_Game.AppContext;
using Microsoft.AspNetCore.Mvc;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game
{
    interface IGameDataProcess
    {
        public GameRoomBase GameRoomBase { get; set; }
    }

    interface IGameFuncProcess
    {
        public int InitGame();
        public bool ReqForCloseOrDeleteRoom(bool delete);
        public char CheckWinner();
    }

    public class GameProcess : IGameFuncProcess, IGameDataProcess
    {
        public GameRoomBase GameRoomBase { get; set; }
        private IDbProvider<GameMetaData> GameDataProvider { get; set; }
        private IDbProvider<GameRoomBase> GameProvider { get; set; }

        public GameProcess(IDbProvider<GameRoomBase> GameProvider, IDbProvider<GameMetaData> GameDataProvider, GameRoomBase gameRoom)
        {
            this.GameProvider = GameProvider;
            this.GameDataProvider = GameDataProvider;
            this.GameRoomBase = gameRoom;
            if(gameRoom.Id == 0)
                InitGame();
        }

        public int InitGame()
        {
            return GameProvider.Create(GameRoomBase).Id;
        }

        private bool AnyPlayerIsExistInGame(GameRoomBase data) =>
            (data.playerO_ID != -1) && (data.playerX_ID != -1);

        public bool ReqForCloseOrDeleteRoom(bool delete)
        {
            try
            {
                var toUpdate = GameProvider.Get(GameRoomBase.Id);
                if (delete&&(!AnyPlayerIsExistInGame(toUpdate)||(toUpdate.gameState == GameState.Completed))) DeleteGame(GameRoomBase.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool DeleteGame(int id)
        {
            try
            {
                GameProvider.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public char CheckWinner()
        {
            GameMetaData gameData = GameDataProvider.Get(GameRoomBase.gameDataId);

            char Winner = gameData.VoidCell;

            var MetaData = gameData;

            char[][] GameMapSlice = GameRoomBase.GameMap.Chunk(3).ToArray();

            int X = 0;
            int O = 0;

            for (int i = 0; i < 3; i++)
            {
                X = 0;
                O = 0;

                for (int j = 0; j < 3; j++)
                {
                    X += GameMapSlice[i][j] == MetaData.X ? 1 : 0;
                    O += GameMapSlice[i][j] == MetaData.O ? 1 : 0;
                }
                Winner = X == 3 ? MetaData.X : O == 3 ? MetaData.O : Winner;
            }

            for (int i = 0; i < 3; i++)
            {
                X = 0;
                O = 0;
                for (int j = 0; j < 3; j++)
                {
                    X += GameMapSlice[j][i] == MetaData.X ? 1 : 0;
                    O += GameMapSlice[j][i] == MetaData.O ? 1 : 0;
                }
                Winner = X == 3 ? MetaData.X : O == 3 ? MetaData.O : Winner;
            }


            X = 0;
            O = 0;
            for (int i = 0; i < 3; i++)
            {
                X += GameMapSlice[i][i] == MetaData.X ? 1 : 0;
                O += GameMapSlice[i][i] == MetaData.O ? 1 : 0;
            }
            Winner = X == 3 ? MetaData.X : O == 3 ? MetaData.O : Winner;

            X = 0;
            O = 0;
            int d = 2;
            for (int i = 0; i < 3; i++)
            {
                X += GameMapSlice[i][d] == MetaData.X ? 1 : 0;
                O += GameMapSlice[i][d] == MetaData.O ? 1 : 0;
                d--;
            }
            Winner = X == 3 ? MetaData.X : O == 3 ? MetaData.O : Winner;

            return Winner;

            /*
             oxo
             xox
             xxo
             */
        }
    }
}
