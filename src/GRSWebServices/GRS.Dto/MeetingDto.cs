using System;
using System.Diagnostics;

namespace GRS.Dto
{
   [DebuggerDisplay("[{MeetingID}] - {Description}")]
   public class MeetingDto
   {
      public bool Deleted { get; set; }

      public string Description { get; set; }

      public DateTime EndDate { get; set; }

      public int MeetingID { get; set; }

      public string Name { get; set; }

      public string ReportTitle { get; set; }

      public DateTime StartDate { get; set; }

      public DateTime TSCreateDate { get; set; }

      public string TSCreateUser { get; set; }

      public DateTime? TSModifyDate { get; set; }

      public string TSModifyUser { get; set; }

      public long VersionAutoID { get; set; }

      public override string ToString()
      {
         return Description;
      }
   }
}
