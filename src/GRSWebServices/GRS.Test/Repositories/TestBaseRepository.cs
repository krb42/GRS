using GRS.Data.Model;
using GRS.Data.Model.Repositories;

namespace GRS.Test.Repositories
{
   public interface ITestBaseRepository : IBaseRepository
   {
      long NextVersionAutoID { get; }
   }

   public class TestBaseRepository : BaseRepository, ITestBaseRepository
   {
      public TestBaseRepository(GRSDBContextOptions options) : base(options)
      {
      }

      public long CurrentVersionAutoID { get; set; }

      public long NextVersionAutoID => CurrentVersionAutoID++;
   }
}
