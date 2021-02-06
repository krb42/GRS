using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GRS.Business.Meetings.Queries
{
   public class GetMeetingsQueryHandler : IRequestHandler<GetMeetingsQuery, IEnumerable<MeetingDto>>
   {
      private readonly IGRSDBContext _dbContext;

      private readonly IMapper _mapper;

      public GetMeetingsQueryHandler(IGRSDBContext dbContext, IMapper mapper)
      {
         _dbContext = dbContext;
         _mapper = mapper;
      }

      public Task<IEnumerable<MeetingDto>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
      {
         var meetings = _dbContext.Meeting.GetMeetings(); ///.CreateFilteredDtos<Meeting, MeetingDto>(request.Parameters);

         return Task.FromResult(_mapper.Map<IEnumerable<MeetingDto>>(meetings));
      }
   }
}
