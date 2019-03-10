using MyApp.Model;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Logic
{
    public class CreatePlaceValidator
        : AbstractValidator<CreatePlaceToVisit>
    {
        public CreatePlaceValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Name.Length).GreaterThan(2);
            RuleFor(r => r.Name).NotEqual("hola");
        }
    }

    public class UpdatePlaceValidator :
AbstractValidator<UpdatePlaceToVisit>
    {
        public UpdatePlaceValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
        }
    }
    public class DeletePlaceValidator :
    AbstractValidator<DeletePlaceToVisit>
    {
        public DeletePlaceValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Id).NotEqual(1);
        }
    }
}
