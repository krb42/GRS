using GRS.Data.Model;
using GRS.Data.Model.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GRS.Test.Repositories
{
   public class TestMeetingRepository : IMeetingRepository
   {
      private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private List<Meeting> _baseData { get; }

      private ITestBaseRepository _baseRepository { get; }

      public TestMeetingRepository(ITestBaseRepository baseRepository, List<Meeting> meetings)
      {
         _baseRepository = baseRepository;
         _baseData = new List<Meeting>(meetings);
      }

      public bool CanDeleteMeeting(int meetingID)
      {
         return meetingID == 0;
      }

      public Meeting GetMeetingByID(int meetingID)
         => _baseData.Find(m => m.MeetingID == meetingID);

      public IEnumerable<Meeting> GetMeetings() => _baseData.ToArray();

      public Meeting InsertMeeting(Meeting meeting)
      {
         var maxID = _baseData.Max(m => m.MeetingID) + 1;

         var newMeeting = meeting.Clone();
         newMeeting.MeetingID = maxID;
         _baseData.Add(newMeeting);

         return newMeeting;
      }

      public void UpdateMeeting(Meeting meeting)
      {
         var newMeeting = _baseData.FirstOrDefault(m => m.MeetingID == meeting.MeetingID);
         if (newMeeting == null) throw new Exception($"Meeting not found");

         _baseData.Remove(newMeeting);

         newMeeting = meeting.Clone();
         newMeeting.TSModifyDate = DateTime.Now;
         newMeeting.TSModifyUser = _baseRepository.CurrentUserName;
         newMeeting.VersionAutoID = _baseRepository.NextVersionAutoID;

         _baseData.Add(newMeeting);
      }
   }
}
