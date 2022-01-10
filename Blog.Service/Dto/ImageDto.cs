namespace Blog.Service.Dto
{
   public class ImageDto
   {
      public ImageDto()
      {
         UploadDate = DateTime.UtcNow;
      }
      public int ImageId { get; set; }
      public string Path { get; set; }
      public DateTime UploadDate { get; set; }
      public DateTime? UpdateDate { get; set; }

      public int ArticleId { get; set; }
      public virtual ArticleDto Article { get; set; }
   }
}