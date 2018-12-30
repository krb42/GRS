using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Meetings.Queries
{
   public class GetMeetingByIdHandler : IRequestHandler<GetMeetingByIdQuery, MeetingDto>
   {
      private readonly IGRSDBContext _dbContext;
      private readonly IMapper _mapper;

      public GetMeetingByIdHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      public Task<MeetingDto> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
      {
         var meeting = _dbContext.Meeting.GetMeetingByID(request.MeetingId);

         return Task.FromResult(_mapper.Map<MeetingDto>(meeting));
      }
   }
}
