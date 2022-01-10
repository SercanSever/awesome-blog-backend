using Blog.Service.Dto;
using FluentValidation;

namespace Blog.Service.ValidationRules.FluentValidation
{
   public class ArticleCategoryValidator : AbstractValidator<ArticleCategoryDto>
   {
      public ArticleCategoryValidator()
      {
         RuleFor(a => a.ArticleId).NotEmpty();
         RuleFor(a => a.CategoryId).NotEmpty();
      }
   }
}