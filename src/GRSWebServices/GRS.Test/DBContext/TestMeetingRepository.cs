using GRS.Core;
using GRS.Data.Model;
using GRS.Data.Model.Repositories;
using GRS.Dto;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GRS.Test.DBContext
{
   public interface ITestMeetingRepository : IMeetingRepository
   {
      Meeting GetTestMeeting(int meetingID);

      MeetingDto GetTestMeetingDto(int meetingID);
   }

   public class TestMeetingRepository : ITestMeetingRepository
   {
      private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private List<Meeting> _baseData { get; }

      public TestMeetingRepository()
      {
         _baseData = new List<Meeting>()
         {
            GetTestMeeting(1),
         };
      }

      public bool CanDeleteMeeting(int meetingID)
      {
         return false;
      }

      public Meeting GetMeetingByID(int meetingID)
         => _baseData.Find(m => m.MeetingID == meetingID);

      public IEnumerable<Meeting> GetMeetings() => _baseData.ToArray();

      public Meeting GetTestMeeting(int meetingID)
      {
         return new Meeting
         {
            MeetingID = meetingID,
            Deleted = false,
            Name = "GRS2006                                                                         ",
            Description = $"Test Meeting {meetingID}",
            ReportTitle = $"Test Meeting {meetingID}",
            StartDate = new DateTime(2018, 1, 6, 0, 0, 0),
            EndDate = new DateTime(2018, 1, 7, 0, 0, 0),

            TSCreateUser = "Test Seed Data",
            TSCreateDate = new DateTime(2018, 1, 1, 17, 36, 4, 167),
            TSModifyUser = null,
            TSModifyDate = null,
            VersionAutoID = 2,
         };
      }

      public MeetingDto GetTestMeetingDto(int meetingID)
      {
         return new MeetingDto
         {
            MeetingID = meetingID,
            Deleted = false,
            Name = "GRS2006                                                                         ",
            Description = $"Test Meeting {meetingID}",
            ReportTitle = $"Test Meeting {meetingID}",
            StartDate = new DateTime(2018, 1, 6, 0, 0, 0),
            EndDate = new DateTime(2018, 1, 7, 0, 0, 0),

            TSCreateUser = "Test Seed Data",
            TSCreateDate = new DateTime(2018, 1, 1, 17, 36, 4, 167),
            TSModifyUser = null,
            TSModifyDate = null,
            VersionAutoID = 2,
         };
      }

      public int InsertMeeting(Meeting meeting)
      {
         return 5;
      }

      public void UpdateMeeting(Meeting meeting)
      {
         if (meeting.MeetingID == 0) throw new InvalidArgumentException();

         //var dbMeeting = GetMeetingByID(meeting.MeetingID);
         //var command = Helper.BuildUpdateCommand<Meeting>(meeting, dbMeeting);

         return;
      }
   }
}
