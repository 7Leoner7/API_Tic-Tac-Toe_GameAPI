using API_Tic_Tac_Toe_Game.Tic_Tac_Toe_Game;
using Microsoft.EntityFrameworkCore;

namespace API_Tic_Tac_Toe_Game.AppContext
{
    public interface IDbProvider<TModel> where TModel : BaseModel
    {
        public List<TModel> GetAll();
        public TModel Get(int id);
        public TModel Create(TModel model);
        public TModel Update(TModel model);
        public void Delete(int id);
    }

    public class DbProvider<TModel> : IDbProvider<TModel> where TModel : BaseModel
    {
        private ApplicationContext Context { get; set; }

        public DbProvider(ApplicationContext context)
        {
            Context = context;
        }

        public TModel GetByCondition(Func<TModel, bool> condition)
        {
            return Context.Set<TModel>().FirstOrDefault(condition);
        }

        public List<TModel> GetAll()=>
            Context.Set<TModel>().ToList();

        public TModel Get(int id) =>
            Context.Set<TModel>().FirstOrDefault(data => data.Id == id);

        public TModel Create(TModel model)
        {
            Context.Set<TModel>().Add(model);
            Context.SaveChanges();
            return model;
        }

        public TModel Update(TModel model)
        {
            var toUpdate = Context.Set<TModel>().AsNoTracking().FirstOrDefault(data => data.Id == model.Id);
            if (toUpdate != null)
                toUpdate = model;
           
            Context.Update(toUpdate);
            Context.SaveChanges();
            return model;
        }

        public void Delete(int id)
        {
            var toDelete = Context.Set<TModel>().FirstOrDefault(room => room.Id == id);
            Context.Set<TModel>().Remove(toDelete);
            Context.SaveChanges();
        }
    }
}
