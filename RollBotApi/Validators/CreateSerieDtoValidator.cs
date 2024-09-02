using FluentValidation;
using RollBotApi.DTOs;

namespace RollBotApi.Validators
{
    public class CreateSerieDtoValidator : AbstractValidator<CreateSerieDto>
    {
        public CreateSerieDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(50)
                .WithMessage("Name can have a maximum length of 50 characters.");

            RuleFor(x => x.Tags)
                .NotEmpty()
                .WithMessage("At least one tag is required.")
                .Must(tags => tags.Count <= 10)
                .WithMessage("A maximum of 10 tags are allowed.")
                .ForEach(tag => tag
                    .NotEmpty()
                    .WithMessage("Tag cannot be empty.")
                    .MaximumLength(20)
                    .WithMessage("Each tag can have a maximum length of 20 characters."));
        }
    }
}