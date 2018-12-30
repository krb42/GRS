using System;

namespace GRS.Core
{
   public class GRSException : Exception
   {
      public GRSException()
      {
      }

      public GRSException(string message) : base(message)
      {
      }

      public GRSException(string message, Exception innerExcpetion) : base(message, innerExcpetion)
      {
      }
   }
}
