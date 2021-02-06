using AutoMapper;
using GRS.Data.Model;
using GRS.Dto;
using MediatR;
using System;
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

         if (meeting == null && request.MeetingId == 5)
         {
            meeting = new Meeting()
            {
               MeetingID = 5,
               Name = "hello",
               Description = "hello hello",
               ReportTitle = "hello report",
               StartDate = DateTime.Now,
               EndDate = DateTime.Now,
               VersionAutoID = 5,
            };
         }

         return Task.FromResult(_mapper.Map<MeetingDto>(meeting));
      }
   }
}
