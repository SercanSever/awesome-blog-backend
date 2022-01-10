namespace Blog.CORE.Utilities.Results
{
   public class ErrorDataResult<T> : DataResult<T>
   {
      public ErrorDataResult() : base(default, false)
      {

      }
      public ErrorDataResult(T data, string message) : base(default, false, message)
      {
      }

      public ErrorDataResult(T data, bool success) : base(data, false)
      {
      }

      public ErrorDataResult(T data, bool success, string message) : base(data, false, message)
      {
      }
   }
}