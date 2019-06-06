using GRS.Data.Model.Repositories.Utilities;

namespace GRS.Data.Model.Repositories
{
   public interface IBaseRepository
   {
      string ConnectionString { get; }

      string CurrentUserName { get; }

      IRepositoryHelper Helper { get; }
   }

   public class BaseRepository : IBaseRepository
   {
      private readonly GRSDBContextOptions _options;

      public BaseRepository(GRSDBContextOptions options)
      {
         _options = options;
      }

      public string ConnectionString => _options.ConnectionString;

      public string CurrentUserName => _options.Username;

      public IRepositoryHelper Helper => new RepositoryHelper(this);
   }
}
