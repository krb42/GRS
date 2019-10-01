using FluentValidation.AspNetCore;
using GRS.Core;
using GRS.Dto;
using GRS.Service;
using GRS.WebService.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GRS.WebService.Controllers
{
   [Route("api/v1/[controller]")]
   public class MeetingController : Controller
   {
      private readonly ILogger<MeetingController> _logger;

      private readonly IMeetingService _meetingService;

      public MeetingController(IMeetingService meetingService, ILogger<MeetingController> logger)
      {
         _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
         _logger = logger;
      }

      /// <summary>
      /// Check to see if the Meeting can be deleted
      /// </summary>
      /// <param name="meetingId">
      /// The unique identifier of the meeting to be checked
      /// </param>
      /// <returns>
      /// True is the meeting can be deleted
      /// </returns>
      [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
      [HttpDelete("check/{meetingId:int}", Name = nameof(CanDeleteMeeting))]
      public async Task<IActionResult> CanDeleteMeeting([FromRoute] int meetingId)
      {
         _logger.LogWarning($"CanDeleteMeeting where meetingId = {meetingId}");
         var existingItem = await _meetingService.GetMeetingById(meetingId);
         if (existingItem == null)
         {
            return this.ItemNotFound($"No meeting found with id {meetingId}");
         }

         var result = await _meetingService.CanDeleteMeeting(meetingId);

         return Ok(result);
      }

      /// <summary>
      /// Add a new meeting
      /// </summary>
      /// <param name="meetingDto">
      /// The new meeting to be added
      /// </param>
      /// <returns>
      /// The new meeting created
      /// </returns>
      [ProducesResponseType(typeof(MeetingDto), (int)HttpStatusCode.OK)]
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
      [HttpPost]
      public async Task<IActionResult> CreateMeeting([FromBody] [CustomizeValidator(RuleSet = RuleSets.ApplyToCreate)] MeetingDto meetingDto)
      {
         if (meetingDto == null) return this.NullDtoOrFieldMismatchBadRequest(nameof(meetingDto));

         var createdMeeting = await _meetingService.CreateMeeting(meetingDto);

         return CreatedAtRoute(nameof(GetMeetingById), new { meetingId = createdMeeting.MeetingID }, createdMeeting);
      }

      /// <summary>
      /// Delete the selected Meeting
      /// </summary>
      /// <param name="meetingId">
      /// The unique identifier of the meeting to be deleted
      /// </param>
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
      [HttpDelete("{meetingId:int}")]
      public async Task<IActionResult> DeleteMeeting([FromRoute] int meetingId)
      {
         _logger.LogWarning($"DeleteMeeting where meetingId = {meetingId}");
         await _meetingService.DeleteMeeting(meetingId);

         return NoContent();
      }

      /// <summary>
      /// Retreives a meeting by its unique identifier
      /// </summary>
      /// <param name="meetingId">
      /// The unique identifier of the meeting to be retrieved
      /// </param>
      /// <returns>
      /// A single meeting
      /// </returns>
      [ProducesResponseType(typeof(MeetingDto), (int)HttpStatusCode.OK)]
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
      [HttpGet("{meetingId:int}", Name = nameof(GetMeetingById))]
      public async Task<IActionResult> GetMeetingById([FromRoute] int meetingId)
      {
         _logger.LogWarning($"GetMeetingByID where meetingId = {meetingId}");
         var meeting = await _meetingService.GetMeetingById(meetingId);
         if (meeting == null)
            return this.ItemNotFound($"No meeting found with id {meetingId}");

         return Ok(meeting);
      }

      /// <summary>
      /// Retreives all meetings
      /// </summary>
      /// <param name="parameters">
      /// </param>
      /// <returns>
      /// List of meetings
      /// </returns>
      [ProducesResponseType(typeof(List<MeetingDto>), (int)HttpStatusCode.OK)]
      [HttpGet]
      public async Task<IActionResult> GetMeetings(QueryParameters parameters)
      {
         return Ok(await _meetingService.GetMeetings(parameters));
      }

      /// <summary>
      /// Updates an existing meeting
      /// </summary>
      [ProducesResponseType((int)HttpStatusCode.NoContent)]
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.BadRequest)]
      [ProducesResponseType(typeof(ErrorModel), (int)HttpStatusCode.NotFound)]
      [HttpPut("{meetingId}")]
      public async Task<IActionResult> UpdateMeeting([FromRoute] int meetingId, [FromBody]MeetingDto meetingDto)
      {
         if (meetingDto == null || meetingDto.MeetingID != meetingId)
         {
            return this.NullDtoOrIdMismatchBadRequest(meetingDto?.MeetingID, meetingId);
         }

         var existingItem = await _meetingService.GetMeetingById(meetingId);
         if (existingItem == null)
         {
            return this.ItemNotFound($"No meeting found with id {meetingId}");
         }

         await _meetingService.UpdateMeeting(meetingDto);

         return NoContent();
      }
   }
}
