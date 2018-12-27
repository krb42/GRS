using System.ComponentModel.DataAnnotations;

namespace GRS.Dto
{
   /// <summary>
   /// This is a generic class used when you need to filter by VersionAutoId and limit the number of
   /// results returned
   /// </summary>
   public class QueryParameters
   {
      private int _limit = DefaultLimit;
      public const int DefaultLimit = 1000;
      public const int MaximumLimit = 10000;

      public QueryParameters()
      {
      }

      public QueryParameters(long versionAutoId, int limit)
      {
         VersionAutoId = versionAutoId;
         Limit = limit;
      }

      /// <summary>
      /// The number of results you would like to get back. Default = 1,000, Maximum is 10,000.
      /// </summary>
      [Range(0, 10000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
      public int Limit
      {
         get => _limit;

         set
         {
            if (value == 0)
               value = DefaultLimit;

            if (value > MaximumLimit)
               value = MaximumLimit;

            _limit = value;
         }
      }

      /// <summary>
      /// Minimum version auto id
      /// </summary>
      public long VersionAutoId { get; set; }
   }
}
