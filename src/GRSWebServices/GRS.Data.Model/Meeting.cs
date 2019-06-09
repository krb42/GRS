using GRS.Data.Model.Repositories.Utilities;
using System;

namespace GRS.Data.Model
{
   public class Meeting
   {
      [DbColumnOptions(isImmutable: true)]
      public bool Deleted { get; set; }

      public string Description { get; set; }

      public DateTime EndDate { get; set; }

      [DbColumnOptions(isImmutable: true)]
      public int MeetingID { get; set; }

      public string Name { get; set; }

      public string ReportTitle { get; set; }

      public DateTime StartDate { get; set; }

      [DbColumnOptions(isImmutable: true)]
      public DateTime TSCreateDate { get; set; }

      [DbColumnOptions(isImmutable: true)]
      public string TSCreateUser { get; set; }

      [DbColumnOptions(isOptional: true, isImmutable: true)]
      public DateTime? TSModifyDate { get; set; }

      [DbColumnOptions(isOptional: true, isImmutable: true)]
      public string TSModifyUser { get; set; }

      [DbColumnOptions(isImmutable: true)]
      public long VersionAutoID { get; set; }

      public Meeting Clone()
      {
         return new Meeting()
         {
            MeetingID = MeetingID,
            Name = Name,
            Description = Description,
            ReportTitle = ReportTitle,
            StartDate = StartDate,
            EndDate = EndDate,

            Deleted = Deleted,
            TSCreateDate = TSCreateDate,
            TSCreateUser = TSCreateUser,
            TSModifyDate = TSModifyDate,
            TSModifyUser = TSModifyUser,
            VersionAutoID = VersionAutoID,
         };
      }
   }
}
