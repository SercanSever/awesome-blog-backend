using Autofac;
using Blog.DAL.Context;
using Blog.DAL.Repository;
using Blog.Service.Abstract;
using Blog.Service.Concrete;
using Blog.Service.Dto;
using Blog.Service.ValidationRules.FluentValidation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.DependencyResolver.Autofac
{
   public class BlogModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterType<CommentManager>().As<ICommentService>();
         builder.RegisterType<ImageManager>().As<IImageService>();
         builder.RegisterType<ArticleManager>().As<IArticleService>();
         builder.RegisterType<CategoryManager>().As<ICategoryService>();
         builder.RegisterType<ArticleCategoryManager>().As<IArticleCategoryService>();
         builder.RegisterType<ImageManager>().As<IImageService>();

         builder.RegisterType<BlogContext>().As<DbContext>().SingleInstance();
         builder.RegisterType<Repository>().As<IRepository>().SingleInstance();

         builder.RegisterType<CategoryValidator>().As<IValidator<CategoryDto>>().SingleInstance();
         builder.RegisterType<ArticleValidator>().As<IValidator<ArticleDto>>().SingleInstance();
         builder.RegisterType<CommentValidator>().As<IValidator<CommentDto>>().SingleInstance();
         builder.RegisterType<ImageValidator>().As<IValidator<ImageDto>>().SingleInstance();
         builder.RegisterType<ArticleCategoryValidator>().As<IValidator<ArticleCategoryDto>>().SingleInstance();
         builder.RegisterType<CategoryValidator>().As<IValidator<CategoryDto>>().SingleInstance();

      }
   }
}