using System.Net;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game.ErrorGames
{
    public class ErrorGame<TModel> where TModel : BaseModel
    {
        protected Dictionary<int, string> DictErrors = new Dictionary<int, string>();
        protected TModel MainModel;
        static public (int, string)  HasNoErrors { get => new (200, "Succesfull"); }

        public ErrorGame(TModel model) 
        {
            DictErrors.Add(409, "Conflict");
            DictErrors.Add(403, "Forbiden");
            DictErrors.Add(400, "BadRequest");
            
            MainModel= model;
        }

        public (int, string) IsValid(Func<TModel, (int, string?)> condition)
        {
            var Error = condition(MainModel);
            if (DictErrors.ContainsKey(Error.Item1))
                return new (Error.Item1, Error.Item2 ?? ((HttpStatusCode)Error.Item1).ToString());
            
            return HasNoErrors;
        }
    }

    public class ErrorGameMetaData
    {
        static public (int, string?) HasError(GameMetaData data)
        {
            if(data.X.Equals(data.VoidCell) || data.X.Equals(data.O) || data.O.Equals(data.VoidCell)) 
                return (409, "Параметры X, O, VoidCell - ни один из данных параметров не должен быть равен другому");

            return (200, "OK");
        }
    }
}
