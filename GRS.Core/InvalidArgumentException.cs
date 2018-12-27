using System;
using System.Runtime.Serialization;

namespace GRS.Core
{
   [Serializable]
   public class InvalidArgumentException : Exception
   {
      protected InvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
      }

      public InvalidArgumentException()
      {
      }

      public InvalidArgumentException(string message) : base(message)
      {
      }

      public InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}
