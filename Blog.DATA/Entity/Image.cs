using Blog.DAL.Repository;

namespace Blog.DATA.Entity
{
   public partial class Image : ISoftDeleteEntity
   {
      public int ImageId { get; set; }
      public string Path { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public int ArticleId { get; set; }
      public virtual Article Article { get; set; }
      public DateTime? DeletionDate { get; set; }
      public bool IsDeleted { get; set; }
   }
}