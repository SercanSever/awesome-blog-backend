namespace Blog.DATA.Entity
{
   public partial class ArticleCategory
   {
      public int ArticleCategoryId { get; set; }
      public int CategoryId { get; set; }
      public virtual Category Category { get; set; }
      public int ArticleId { get; set; }
      public virtual Article Article { get; set; }
   }
}