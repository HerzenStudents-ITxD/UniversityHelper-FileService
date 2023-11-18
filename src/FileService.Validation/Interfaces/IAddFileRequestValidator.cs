using FluentValidation;
using UniversityHelper.Core.Attributes;
using Microsoft.AspNetCore.Http;

namespace UniversityHelper.FileService.Validation.Interfaces
{
    [AutoInject]
    public interface IAddFileRequestValidator : IValidator<IFormFileCollection>
    {
    }
}
