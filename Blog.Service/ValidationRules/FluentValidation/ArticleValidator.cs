using Blog.Service.Dto;
using FluentValidation;

namespace Blog.Service.ValidationRules.FluentValidation
{
   public class ArticleValidator : AbstractValidator<ArticleDto>
   {
      public ArticleValidator()
      {
         RuleFor(a => a.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
         RuleFor(a => a.Author).NotEmpty().WithMessage("Yazar alanı boş bırakılamaz.");
         RuleFor(a => a.NameUrl).NotEmpty().WithMessage("Url boş bırakılamaz.");
         RuleFor(a => a.Summary).NotEmpty().WithMessage("Özet boş bırakalamaz.");
         RuleFor(a => a.Content).NotEmpty().WithMessage("İçerik boş bırakalamaz.");
      }
   }
}