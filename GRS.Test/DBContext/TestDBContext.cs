using GRS.Data.Model;
using GRS.Data.Model.Repositories;

namespace GRS.Test.DBContext
{
   public class TestDBContext : IGRSDBContext
   {
      public IMeetingRepository Meeting => new TestMeetingRepository();
   }
}
