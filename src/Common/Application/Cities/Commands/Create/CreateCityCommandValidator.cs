﻿namespace Application.Cities.Commands.Create
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using FluentValidation;

    using Common.Interfaces;

    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateCityCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .MustAsync(BeUniqueTitle).WithMessage("The specified city already exists.")
                .NotEmpty().WithMessage("Name is required.");
        }

        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            //TODO: Büyük küçük harf ve türkçe karaktere göre kontrol sağla
            return await _context.Cities.AllAsync(x => x.Name != title, cancellationToken);
        }
    }
}
