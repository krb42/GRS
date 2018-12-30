using AutoMapper;
using GRS.Business.Meetings.Queries;
using GRS.Core;
using GRS.Data.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Meetings.Commands
{
   public class DeleteMeetingCommandHandler : IRequestHandler<DeleteMeetingCommand>, IRequestHandler<CanDeleteMeetingQuery, bool>
   {
      private readonly IGRSDBContext _dbContext;
      private readonly IMapper _mapper;

      public DeleteMeetingCommandHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      /// <summary>
      /// Check to see if the Meeting can be deleted
      /// </summary>
      /// <param name="request">
      /// </param>
      /// <param name="cancellationToken">
      /// </param>
      /// <returns>
      /// </returns>
      public Task<bool> Handle(CanDeleteMeetingQuery request, CancellationToken cancellationToken)
      {
         return Task.FromResult(_dbContext.Meeting.CanDeleteMeeting(request.MeetingId));
      }

      public Task<Unit> Handle(DeleteMeetingCommand request, CancellationToken cancellationToken)
      {
         var canDelete = _dbContext.Meeting.CanDeleteMeeting(request.MeetingId);
         if (!canDelete)
         {
            throw new GRSException(ValidationMessages.CannotDeleteEntityError);
         }

         var meeting = _dbContext.Meeting.GetMeetingByID(request.MeetingId);

         if (meeting != null)
         {
            meeting.Deleted = true;
            _dbContext.Meeting.UpdateMeeting(meeting);
         }

         return Task.FromResult(new Unit());
      }
   }
}
