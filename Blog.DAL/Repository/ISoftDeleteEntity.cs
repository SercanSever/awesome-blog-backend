namespace Blog.DAL.Repository
{
   public interface ISoftDeleteEntity
   {
      DateTime? DeletionDate { get; set; }
      bool IsDeleted { get; set; }
   }
}