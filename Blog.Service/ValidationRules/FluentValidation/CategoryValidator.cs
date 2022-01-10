using Blog.Service.Dto;
using FluentValidation;

namespace Blog.Service.ValidationRules.FluentValidation
{
   public class CategoryValidator : AbstractValidator<CategoryDto>
   {
      public CategoryValidator()
      {
         RuleFor(x => x.Name).NotEmpty();
         RuleFor(x => x.UploadDate).NotEmpty();
      }
   }
}