using FA1.Entities;
using FluentValidation;

namespace F19.Presentation.Filters.Validation;

public sealed class F19ValidationProfile : AbstractValidator<F19Request>
{
    public F19ValidationProfile()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(prop => prop.TodoTaskId).Must(prop => prop >= 0);

        RuleFor(prop => prop.Content)
            .NotEmpty()
            .MaximumLength(TodoTaskEntity.Metadata.Properties.Content.MaxLength);
    }
}
