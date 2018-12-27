using GRS.Data.Model.Repositories;

namespace GRS.Data.Model
{
   public class GRSDBContext : BaseRepository, IGRSDBContext
   {
      public GRSDBContext(GRSDBContextOptions options) : base(options)
      {
      }

      public IMeetingRepository Meeting => new MeetingRepository(this);
   }
}
