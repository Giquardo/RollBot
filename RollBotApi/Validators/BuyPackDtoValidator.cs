using FluentValidation;
using RollBotApi.DTOs;
using RollBotApi.Models;

namespace RollBotApi.Validators
{
    public class BuyPackDtoValidator : AbstractValidator<BuyPackDto>
    {
        public BuyPackDtoValidator()
        {
            RuleFor(x => x.DiscordId)
                .NotEmpty()
                .WithMessage("DiscordId is required.");

            RuleFor(x => x.PackType)
                .Must(packType => Enum.IsDefined(typeof(PackType), packType))
                .WithMessage("Invalid PackType value.")
                .Must(packType => packType == PackType.Normal || packType == PackType.Jumbo || packType == PackType.Huge)
                .WithMessage("PackType must be one of the following values: 0 (Normal), 1 (Jumbo), 2 (Huge).");
        }
    }
}