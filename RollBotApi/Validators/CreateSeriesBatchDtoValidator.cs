using FluentValidation;
using RollBotApi.DTOs;

namespace RollBotApi.Validators;
public class CreateSeriesBatchDtoValidator : AbstractValidator<CreateSeriesBatchDto>
{
    public CreateSeriesBatchDtoValidator()
    {
        RuleFor(x => x.Series)
            .NotEmpty()
            .WithMessage("The series list cannot be empty.")
            .ForEach(series => series.SetValidator(new CreateSerieDtoValidator()));
    }
}
