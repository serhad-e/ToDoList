namespace todofinal.Application.Validators;
using FluentValidation;
using todofinal.Application.DTOs;
public class CreateTodoDtoValidator: AbstractValidator<CreateTodoDto>
{
    public CreateTodoDtoValidator()
    {
        // Başlık kuralı: Boş olamaz ve en az 3 karakter olmalı
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık alanı boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.");

        // Açıklama kuralı: Çok uzun olmasın (opsiyonel)
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama 500 karakterden fazla olamaz.");
    }
}