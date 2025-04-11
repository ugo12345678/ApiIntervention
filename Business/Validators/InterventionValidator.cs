using AutoMapper;
using Business.Abstraction.Exceptions;
using Business.Abstraction.Manager;
using Business.Abstraction.Models;
using Business.Extensions;
using DataAccess.Abstraction.Entity;
using DataAccess.Abstraction.Repositories;
using DataAccess.Repositories;
using FluentValidation;

namespace Business.Validators.Version
{
    public class InterventionValidator : ValidatorBase<InterventionModel>
    {
        public InterventionValidator(
            IMapper mapper,
            IInterventionRepository interventionRepository,
            IUserRepository userRepository,
            IMessageLocalizerManager localizer
        ) : base(mapper)
        {
            RuleFor(e => e.Name)
                .MustAsync(async (@version, name, context, _) =>
                {
                    if (context.IsContextType(ValidationContextType.CREATE))
                        return !await interventionRepository.ExistsAsync(e => e.Name == name);

                    if (context.IsContextType(ValidationContextType.UPDATE))
                        return !await interventionRepository.ExistsAsync(e => e.Name == name && e.Id != (long)context.RootContextData["Id"]);

                    return true;
                })
                .WithErrorCode(BusinessErrorCode.InterventionNameAlreadyExists)
                .WithMessage(localizer.Get("AlreadyExists"));

            RuleFor(e => e.TechniciansNames)
                .MustAsync(async (@version, techniciansNames, context, _) =>
                {
                    if (techniciansNames is null || !techniciansNames.Any())
                        return true;

                    foreach (var name in techniciansNames)
                    {
                        var exists = await userRepository.ExistsAsync(u => u.UserName.ToLower() == name.ToLower());
                        if (!exists)
                            return false;
                    }

                    return true;
                })
                .WithErrorCode(BusinessErrorCode.ResourceNotFound)
                .WithMessage(localizer.Get("DoesNotExist"));

            RuleFor(e => e.ClientName)
                .MustAsync(async (@version, clientName, context, _) =>
                {
                    if (string.IsNullOrWhiteSpace(clientName))
                        return false;

                    return await userRepository.ExistsAsync(u => u.UserName.ToLower() == clientName.ToLower());
                })
                .WithErrorCode(BusinessErrorCode.ResourceNotFound)
                .WithMessage(localizer.Get("DoesNotExist"));
        }
    }
}
