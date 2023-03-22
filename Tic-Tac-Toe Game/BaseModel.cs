using Newtonsoft.Json;

namespace API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game
{
    abstract public class BaseModel
    {
        /// <summary>
        /// Id модели. Генерируется автоматически.
        /// </summary>
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
