namespace Blog.DATA.Entity
{
   public partial class Comment
   {

      public int CommentId { get; set; }
      public string Content { get; set; }
      public string UserName { get; set; }
      public string UserEmail { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public int ArticleId { get; set; }
      public virtual Article Article { get; set; }
   }
}