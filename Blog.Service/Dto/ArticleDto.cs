using Blog.DAL.Repository;

namespace Blog.Service.Dto
{
   public class ArticleDto : ISoftDeleteEntity
   {
      public int ArticleId { get; set; }
      public string Name { get; set; }
      public string NameUrl { get; set; }
      public string Summary { get; set; }
      public string Content { get; set; }
      public string Author { get; set; }
      public int? CommentId { get; set; }
      public int? ImageId { get; set; }
      public List<string>? CategoryNames { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }
      public DateTime? DeletionDate { get; set; }
      public bool IsDeleted { get; set; }
   }
}