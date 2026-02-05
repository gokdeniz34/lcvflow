using FluentValidation;
using LcvFlow.Service.Dtos.Guest;

namespace LcvFlow.Service;

public class GuestRsvpValidator : AbstractValidator<GuestRsvpDto>
{
    public GuestRsvpValidator()
    {
        // Erişim anahtarı mutlaka olmalı
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Geçersiz davetiye bağlantısı.");

        // Katılım durumu belirtilmeli
        RuleFor(x => x.IsAttending)
            .NotNull().WithMessage("Lütfen katılım durumunuzu belirtin.");

        // ŞARTLI VALIDATION: Eğer "Geliyorum" diyorsa (IsAttending == true)
        // En az 1 yetişkin olmalı.
        RuleFor(x => x.AdultCount)
            .InclusiveBetween(1, 10)
            .When(x => x.IsAttending == true)
            .WithMessage("Geliyorsanız en az 1, en fazla 10 yetişkin seçebilirsiniz.");

        // Eğer "Gelemiyorum" diyorsa yetişkin sayısı 0 olmalı (mantıksal kontrol)
        RuleFor(x => x.AdultCount)
            .Equal(0)
            .When(x => x.IsAttending == false)
            .WithMessage("Gelemiyorsanız yetişkin sayısı seçemezsiniz.");

        // Çocuk sayısı negatif olamaz
        RuleFor(x => x.ChildCount)
            .GreaterThanOrEqualTo(0).WithMessage("Çocuk sayısı negatif olamaz.");

        // Not alanı çok uzun olmamalı
        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Notunuz 500 karakterden uzun olamaz.");
    }
}