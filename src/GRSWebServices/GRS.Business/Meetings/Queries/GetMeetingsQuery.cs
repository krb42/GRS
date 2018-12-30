using GRS.Dto;
using MediatR;
using System.Collections.Generic;

namespace GRS.Business.Meetings.Queries
{
   public class GetMeetingsQuery : IRequest<List<MeetingDto>>
   {
      public GetMeetingsQuery(QueryParameters parameters)
      {
         Parameters = parameters;
      }

      public QueryParameters Parameters { get; }
   }
}
