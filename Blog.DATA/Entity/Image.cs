namespace Blog.DATA.Entity
{
   public partial class Image
   {
      public int ImageId { get; set; }
      public string Path { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public int ArticleId { get; set; }
      public virtual Article Article { get; set; }
   }
}