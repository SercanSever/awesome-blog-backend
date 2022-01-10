using Blog.Service.Dto;
using FluentValidation;

namespace Blog.Service.ValidationRules.FluentValidation
{
   public class CommentValidator : AbstractValidator<CommentDto>
   {
      public CommentValidator()
      {
         RuleFor(c => c.UserEmail).NotEmpty();
         RuleFor(c => c.Content).NotEmpty();
         RuleFor(c => c.UploadDate).NotEmpty();
         RuleFor(c => c.ArticleId).NotEmpty();
         RuleFor(c => c.UserName).NotEmpty();
      }
   }
}