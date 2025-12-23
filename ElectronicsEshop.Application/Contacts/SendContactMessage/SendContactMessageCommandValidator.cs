using FluentValidation;

namespace ElectronicsEshop.Application.Contacts.SendContactMessage;

public sealed class SendContactMessageCommandValidator : AbstractValidator<SendContactMessageCommand>
{
    public SendContactMessageCommandValidator()
    {
        RuleFor(x => x.ReplyEmail)
            .NotEmpty().WithMessage("E-Mail je povinný údaj.")
            .EmailAddress().WithMessage("Zadejte E-Mail ve správném formátu.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Předmět je povinný údaj.")
            .Length(10, 120).WithMessage("Předmět musí obsahovat rozmezí 10 - 120 znaků.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Zpráva je povinné pole.")
            .Length(10, 2000).WithMessage("Zpráva musí obsahovat rozmezí 10 - 2000 znaků.");
    }
}
