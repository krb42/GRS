using FluentValidation.Results;
using FluentValidation.TestHelper;
using GRS.Business.Meetings.Validators;
using GRS.Data.Model;
using GRS.Test.DBContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRS.Test.ValidationTests
{
   [TestClass]
   public class MeetingDtoValidationTests
   {
      private IGRSDBContext dbContext;
      private MeetingDtoValidator validator;

      [TestMethod]
      public void Error_when_Name_is_null()
      {
         validator.ShouldHaveValidationErrorFor(m => m.Name, null as string);
      }

      [TestMethod]
      public void Noerror_when_Name_is_specified()
      {
         var repository = (ITestMeetingRepository)dbContext.Meeting;
         var meeting = repository.GetTestMeetingDto(1);
         validator.ShouldNotHaveValidationErrorFor(x => x.Name, meeting);
      }

      [TestInitialize]
      public void Setup()
      {
         dbContext = new TestDBContext();
         validator = new MeetingDtoValidator(dbContext);
      }

      [TestMethod]
      public void ValidateInstance()
      {
         var repository = (ITestMeetingRepository)dbContext.Meeting;
         var meeting = repository.GetTestMeetingDto(1);

         ValidationResult result = validator.Validate(meeting);

         Assert.AreEqual(true, result.IsValid);
      }
   }
}
