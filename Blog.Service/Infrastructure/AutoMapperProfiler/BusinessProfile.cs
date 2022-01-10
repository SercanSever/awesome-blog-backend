using AutoMapper;
using Blog.DATA.Entity;
using Blog.Service.Dto;

namespace Blog.Service.Infrastructure.AutoMapperProfiler
{
   public class BusinessProfile : Profile
   {
      public BusinessProfile()
      {
         CreateMap<Comment, CommentDto>();
         CreateMap<CommentDto, Comment>();

         CreateMap<Article, ArticleDto>();
         CreateMap<ArticleDto, Article>();

         CreateMap<Category, CategoryDto>();
         CreateMap<CategoryDto, Category>();

         CreateMap<Image, ImageDto>();
         CreateMap<Image, ImageDto>();

         CreateMap<ArticleCategory, ArticleCategoryDto>();
         CreateMap<ArticleCategoryDto, ArticleCategory>();

      }
   }
}