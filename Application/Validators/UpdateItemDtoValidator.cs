using Application.DTOs.Item;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
        }
    }
}
