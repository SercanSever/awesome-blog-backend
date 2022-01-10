
namespace Blog.CORE.Utilities.Results
{
   public interface IDataResult<T> : IResult
   {
      T Data { get; }
   }
}