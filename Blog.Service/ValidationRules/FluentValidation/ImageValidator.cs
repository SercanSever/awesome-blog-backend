using Blog.Service.Dto;
using FluentValidation;

namespace Blog.Service.ValidationRules.FluentValidation
{
   public class ImageValidator : AbstractValidator<ImageDto>
   {
      public ImageValidator()
      {
         RuleFor(ı => ı.ArticleId).NotEmpty();
         RuleFor(ı => ı.Path).NotEmpty();
         RuleFor(ı => ı.UploadDate).NotEmpty();
      }
   }
}