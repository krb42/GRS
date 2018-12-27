using FluentValidation;
using GRS.Data.Model;
using GRS.Dto;
using System;

namespace GRS.Business.Meetings.Validators
{
   public class MeetingDtoValidator : AbstractValidator<MeetingDto>
   {
      private readonly IGRSDBContext _dbContext;

      private bool IsMeetingNameUnique(string meetingName)
      {
         return true;

         //TODO return !await _dbContext.Meeting.AnyAsync(m => m.Name == meetingDto.Name && m.MeetingId != meetingDto.MeetingID);
      }

      public MeetingDtoValidator(IGRSDBContext dbContext)
      {
         _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

         RuleFor(meeting => meeting.Name)
            .NotEmpty()
            .Must(IsMeetingNameUnique)

            //.When(meeting => meeting.MeetingID <= 0)
            .WithMessage(ValidationMessages.NotUniqueMeetingNameError).WithName("Meeting");

         RuleFor(meeting => meeting.Description).NotEmpty();
      }
   }
}
