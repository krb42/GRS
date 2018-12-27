using GRS.Core;
using GRS.Data.Model.Extensions;
using GRS.Data.Model.Repositories.Utilities;
using log4net;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace GRS.Data.Model.Repositories
{
   public interface IMeetingRepository
   {
      bool CanDeleteMeeting(int meetingID);

      Meeting GetMeetingByID(int meetingID);

      IEnumerable<Meeting> GetMeetings();

      int InsertMeeting(Meeting meeting);

      void UpdateMeeting(Meeting meeting);
   }

   public class MeetingRepository : IMeetingRepository
   {
      private const string SELECT_COMMAND = "SELECT * FROM GRS_Meeting";
      private const string TABLE_NAME = "GRS_Meeting";
      private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private IBaseRepository _baseRepository { get; }

      // Meeting is populated using the DbColumn attributes
      private Meeting ReadData(SqlDataReader reader) => reader.Populate<Meeting>();

      public MeetingRepository(IBaseRepository baseRepository)
      {
         _baseRepository = baseRepository;
      }

      private IRepositoryHelper Helper => _baseRepository.Helper;

      public bool CanDeleteMeeting(int meetingID)
      {
         return false;
      }

      public Meeting GetMeetingByID(int meetingID)
         => Helper.GetSingle($"{SELECT_COMMAND} WHERE MeetingID = @MeetingID", ReadData, new SqlParameter("@MeetingID", meetingID));

      public IEnumerable<Meeting> GetMeetings() => Helper.GetList(SELECT_COMMAND, ReadData);

      public int InsertMeeting(Meeting meeting)
      {
         return 5;
      }

      public void UpdateMeeting(Meeting meeting)
      {
         if (meeting.MeetingID == 0) throw new InvalidArgumentException();

         var dbMeeting = GetMeetingByID(meeting.MeetingID);
         var command = Helper.BuildUpdateCommand<Meeting>(meeting, dbMeeting);
         command.CommandText = $"UPDATE {TABLE_NAME} SET {command.CommandText} WHERE MeetingID = @MeetingID";
         command.Parameters.Add(new SqlParameter("@MeetingID", meeting.MeetingID));
         Helper.ExecuteNonQuery(command);

         return;
      }
   }
}
