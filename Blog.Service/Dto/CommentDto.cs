namespace Blog.Service.Dto
{
   public class CommentDto
   {
      public CommentDto()
      {
         UploadDate = DateTime.UtcNow;
      }
      public int CommentId { get; set; }
      public string Content { get; set; }
      public string UserName { get; set; }
      public string UserEmail { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public int ArticleId { get; set; }
      public virtual ArticleDto Article { get; set; }
   }
}