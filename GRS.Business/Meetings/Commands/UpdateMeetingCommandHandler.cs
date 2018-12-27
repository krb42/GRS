using AutoMapper;
using GRS.Data.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Meetings.Commands
{
   public class UpdateMeetingCommandHandler : IRequestHandler<UpdateMeetingCommand>
   {
      private readonly IGRSDBContext _dbContext;
      private readonly IMapper _mapper;

      public UpdateMeetingCommandHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      public Task<Unit> Handle(UpdateMeetingCommand request, CancellationToken cancellationToken)
      {
         var meeting = _dbContext.Meeting.GetMeetingByID(request.MeetingDto.MeetingID);

         _mapper.Map(request.MeetingDto, meeting);

         _dbContext.Meeting.UpdateMeeting(meeting);

         return Task.FromResult(new Unit());
      }
   }
}
