using AutoMapper;
using Business.Abstraction.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Validators
{
    /// <summary>
    /// Base class of validators used in the application.
    /// The default exception handling has been overriden, so when a validation occurs, a <see cref="BusinessException"/> is thrown. This exception
    /// contains all validation errors.
    /// </summary>
    /// <typeparam name="T">Type of object to validate</typeparam>
    public class ValidatorBase<T> : AbstractValidator<T>
    {
        private readonly IMapper _mapper;

        public const int MAX_LENGTH_255 = 255;
        public const int MAX_LENGTH_50 = 50;

        protected const string REGEX_ALPHANUMERIC = @"^[0-9a-zA-Z]+$";
        protected const string REGEX_CODEPIN = @"^[\d]{6}$";

        /// <summary>
        /// Constructor defining a rule ensuring the object being validated is not null.
        /// </summary>
        /// <param name="mapper">Mapper used to generate <see cref="BusinessException"/>.</param>
        public ValidatorBase(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Virtual method to override when calling async operations on PreValidate.
        /// </summary>
        protected virtual async Task PreValidateAsync(ValidationContext<T> context) => await Task.FromResult(Task.CompletedTask);

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                // if model is null validation process is stopped
                result.Errors.Add(new ValidationFailure(nameof(BusinessErrorCode.ModelNotSupplied),
                                                        "Ensure a model was supplied"));

                return false;
            }

            PreValidateAsync(context).GetAwaiter().GetResult();

            return base.PreValidate(context, result);
        }

        /// <inheritdoc/>
        protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
        {
            IEnumerable<BusinessError> businessErrors = _mapper.Map<List<BusinessError>>(result.Errors);
            throw new BusinessException(businessErrors, typeof(T).Name);
        }

        #region Custom messages

        public const string InvalidIdMessage = "The identifier defined in the resource path is different from the one given in the body object";

        protected const string DoesNotExist = "Resource corresponding to '{PropertyName}' with value '{PropertyValue}' does not exist";

        protected const string AlreadyExists = "'{PropertyName}' value already exists for the current ressource. It must be unique";

        protected const string NotAlphanumeric = "'{PropertyName}' with value '{PropertyValue}' should be alphanumeric";

        protected string WrongBoolValue => "'{PropertyName}' should not be {PropertyValue}";
        protected string CannotBeUpdated => "'{PropertyName}' should not be updated";

        #endregion Custom messages
    }
}
