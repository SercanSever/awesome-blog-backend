using Blog.DAL.Repository;

namespace Blog.Service.Dto
{
   public class CategoryDto : ISoftDeleteEntity
   {
      public int CategoryId { get; set; }
      public string Name { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public DateTime? DeletionDate { get; set; }
      public bool IsDeleted { get; set; }
   }
}