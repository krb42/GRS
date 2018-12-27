using GRS.Data.Model.Repositories;

namespace GRS.Data.Model
{
   public interface IGRSDBContext
   {
      IMeetingRepository Meeting { get; }
   }
}
