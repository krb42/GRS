using FluentValidation.Results;
using FluentValidation.TestHelper;
using GRS.Business.Meetings.Validators;
using GRS.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRS.Test.ValidationTests
{
   [TestClass]
   public class MeetingDtoValidationTests
   {
      private TestDBContext dbContext;
      private MeetingDtoValidator validator;

      [TestMethod]
      public void Error_when_Name_is_null()
      {
         validator.ShouldHaveValidationErrorFor(m => m.Name, null as string);
      }

      [TestMethod]
      public void Noerror_when_Name_is_specified()
      {
         var meeting = dbContext.Meeting.GetMeetingByID(1);
         var meetingDto = dbContext.Mapper.Map<MeetingDto>(meeting);
         validator.ShouldNotHaveValidationErrorFor(x => x.Name, meetingDto);
      }

      [TestInitialize]
      public void Setup()
      {
         dbContext = new TestDBContext(new TestGRSDBContextOptions());
         validator = new MeetingDtoValidator(dbContext);
      }

      [TestMethod]
      public void ValidateInstance()
      {
         var meeting = dbContext.Meeting.GetMeetingByID(1);
         var meetingDto = dbContext.Mapper.Map<MeetingDto>(meeting);

         ValidationResult result = validator.Validate(meetingDto);

         Assert.AreEqual(true, result.IsValid);
      }
   }
}
