using GRS.Data.Model;
using System;
using System.Collections.Generic;

namespace GRS.Test.Data
{
   public static class MeetingTestData
   {
      private static Meeting TestMeeting(int meetingID)
      {
         return new Meeting
         {
            MeetingID = meetingID,
            Deleted = false,
            Name = "GRS2006                                                                         ",
            Description = $"Test Meeting {meetingID}",
            ReportTitle = $"Test Meeting {meetingID}",
            StartDate = new DateTime(2018, meetingID, 6, 0, 0, 0),
            EndDate = new DateTime(2018, meetingID, 6 + meetingID, 0, 0, 0),

            TSCreateUser = "Test Seed Data",
            TSCreateDate = new DateTime(2018, meetingID, 1, 17, 36, 4, 167),
            TSModifyUser = null,
            TSModifyDate = null,
            VersionAutoID = 2 + meetingID,
         };
      }

      public static List<Meeting> Meetings => new List<Meeting>()
         {
            TestMeeting(1),
            TestMeeting(2),
         };
   }
}
