namespace GRS.Business
{
   public static class ValidationMessages
   {
      #region Generic Error Messages

      public const string InvalidDate = "The date specified for {PropertyName} of '{PropertyValue}' is not a valid date";
      public const string InvalidDateTime = "The time specified for {PropertyName} of '{PropertyValue}' is not a valid time";
      public const string NotUniqueNameError = "The name specified {PropertyName} '{PropertyVaue}' already exists";
      public const string NotUniqueRecordError = "The specified {PropertyName} '{PropertyVaue}' already exists";
      public const string RecordDependencyNotFound = "The provided {PropertyName} '{PropertyValue}' could not be found or has been deleted";
      public const string RecordNotFound = "The provided {PropertyName} '{PropertyValue}' could not be found or is deleted";
      public const string Required = "{PropertyName} is required";

      #endregion Generic Error Messages

      #region Record Dependencies

      public const string CannotDeleteEntityError = "The item is used in the system by other entities and cannot be deleted";
      public const string RecordHasDependenciesError = "Cannot update or delete {0} because it has records associated with it. Please checked you don't have any {1} associated with this {0}";

      #endregion Record Dependencies

      public const string PropertyValueTooLong = "The specified value is too long for the database, Maximum length is {MaxLength], {PropertyName} was {TotalLength} characters long";
      public const string RecordIsNotEditable = "Cannot update or delete {0}. {1}";

      #region Meeting specific Error Messages

      public const string NotUniqueMeetingNameError = "The name specified {PropertyName} '{PropertyVaue}' already exists";

      #endregion Meeting specific Error Messages
   }
}
