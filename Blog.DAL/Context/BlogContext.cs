using Blog.DATA.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.DAL.Context
{
   public class BlogContext : DbContext
   {
      public BlogContext()
      {

      }
      public BlogContext(DbContextOptions<BlogContext> options) : base(options)
      {

      }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         var builder = new ConfigurationBuilder();
         builder.SetBasePath(Directory.GetCurrentDirectory());
         builder.AddJsonFile("appsettings.json");
         IConfiguration Configuration = builder.Build();

         optionsBuilder.UseSqlServer(
             Configuration.GetConnectionString("BlogConStr"));
         base.OnConfiguring(optionsBuilder);
      }

      public DbSet<Article> Articles { get; set; }
      public DbSet<Comment> Comments { get; set; }
      public DbSet<ArticleCategory> ArticleCategories { get; set; }
      public DbSet<Category> Categories { get; set; }
      public DbSet<Image> Images { get; set; }
   }
}