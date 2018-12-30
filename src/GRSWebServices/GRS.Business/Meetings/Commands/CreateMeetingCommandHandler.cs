using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Meetings.Commands
{
   public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, int>
   {
      private readonly IGRSDBContext _dbContext;
      private readonly IMapper _mapper;

      public CreateMeetingCommandHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      public async Task<int> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
      {
         var meeting = _mapper.Map<MeetingDto, Meeting>(request.MeetingDto);

         meeting.MeetingID = 0;
         meeting.Deleted = false;

         var newId = _dbContext.Meeting.InsertMeeting(meeting);

         return await Task.FromResult(newId);
      }
   }
}
