using Blog.DAL.Repository;

namespace Blog.DATA.Entity
{
   public partial class Article : ISoftDeleteEntity
   {
      public int ArticleId { get; set; }
      public string Name { get; set; }
      public string NameUrl { get; set; }
      public string Content { get; set; }
      public string Summary { get; set; }
      public string Author { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }
      public DateTime? DeletionDate { get; set; }
      public bool IsDeleted { get; set; }
   }
}