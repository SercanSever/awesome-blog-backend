namespace Blog.DATA.Entity
{
   public partial class Category
   {
      public int CategoryId { get; set; }
      public string Name { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }
   }
}