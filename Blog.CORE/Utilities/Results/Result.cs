namespace Blog.CORE.Utilities.Results
{
   public class Result : IResult
   {
      public Result(bool succcess, string message) : this(succcess)
      {
         Message = message;
      }
      public Result(bool success)
      {
         Success = success;
      }
      public bool Success { get; }

      public string Message { get; }
   }
}