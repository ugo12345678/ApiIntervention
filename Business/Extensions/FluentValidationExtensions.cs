using Business.Abstraction.Exceptions;
using Business.Validators;
using FluentValidation;


namespace Business.Extensions
{
    public static class FluentValidationExtensions
    {
        private const string _authorizedColumnNamesContextKey = "AuthorizedColumnNames";

        public static void SetContextType<TModelType>(this ValidationContext<TModelType> context,
                                                      ValidationContextType contextType)
        {
            context.AddContextData(nameof(ValidationContextType), contextType);
        }

        public static void SetAuthorizedColumnNames<TModelType>(this ValidationContext<TModelType> context, IEnumerable<string> authorizedColumnNames)
        {
            context.AddContextData(_authorizedColumnNamesContextKey, authorizedColumnNames);
        }

        private static void AddContextData<TModelType, TDataType>(this ValidationContext<TModelType> context,
                                                                 string key,
                                                                 TDataType contextData)
        {
            if (context.RootContextData.ContainsKey(key))
            {
                context.RootContextData[key] = contextData;
            }
            else
            {
                context.RootContextData.Add(key, contextData);
            }
        }

        public static bool IsContextType<TModelType>(this ValidationContext<TModelType> context,
                                                     ValidationContextType contextType)
        {
            return (context.GetContextData<TModelType, ValidationContextType>(nameof(ValidationContextType)) == contextType);
        }

        public static bool IsAuthorizedColumnName<TModelType>(this ValidationContext<TModelType> context,
                                                              string columnName)
        {
            IEnumerable<string> authorizedColumnNames = context.GetContextData<TModelType, IEnumerable<string>>(_authorizedColumnNamesContextKey);
            return authorizedColumnNames is not null && authorizedColumnNames.Contains(columnName, StringComparer.InvariantCultureIgnoreCase);
        }

        public static IEnumerable<string> GetAuthorizedColumnNames<TModelType>(this ValidationContext<TModelType> context)
        {
            return context.GetContextData<TModelType, IEnumerable<string>>(_authorizedColumnNamesContextKey) ?? Enumerable.Empty<string>();
        }

        private static TDataType GetContextData<TModelType, TDataType>(this ValidationContext<TModelType> context, string key)
        {
            TDataType result = default;
            if (context.RootContextData.TryGetValue(key, out object contextData) &&
                contextData != null)
            {
                result = (TDataType)contextData;
            }
            return result;
        }

        public static IRuleBuilderOptions<TModelType, TPropertyType> WithErrorCode<TModelType, TPropertyType>(this IRuleBuilderOptions<TModelType, TPropertyType> ruleOptions,
                                                                                                              BusinessErrorCode errorCode)
        {
            return ruleOptions.WithErrorCode(errorCode.ToString());
        }

        public static async Task ValidateAndThrowAsync<TModel>(this IValidator<TModel> validator,
                                                               TModel instance,
                                                               ValidationContextType contextType,
                                                               CancellationToken cancellationToken = default)
        {
            ValidationContext<TModel> context = ValidationContext<TModel>.CreateWithOptions(instance, options => options.ThrowOnFailures());
            context.SetContextType(contextType);
            await validator.ValidateAsync(context, cancellationToken);
        }

        public static async Task ValidateAndThrowAsync<TModel>(this IValidator<TModel> validator,
                                                               TModel instance,
                                                               IEnumerable<string> authorizedColumnNames,
                                                               CancellationToken cancellationToken = default)
        {
            ValidationContext<TModel> context = ValidationContext<TModel>.CreateWithOptions(instance, options => options.ThrowOnFailures());
            context.SetAuthorizedColumnNames(authorizedColumnNames);
            await validator.ValidateAsync(context, cancellationToken);
        }

        public static async Task ValidateAndThrowAsync<TModel>(this IValidator<TModel> validator,
                                                               TModel instance,
                                                               ValidationContextType contextType,
                                                               IEnumerable<string> authorizedColumnNames,
                                                               CancellationToken cancellationToken = default)
        {
            ValidationContext<TModel> context = ValidationContext<TModel>.CreateWithOptions(instance, options => options.ThrowOnFailures());
            context.SetAuthorizedColumnNames(authorizedColumnNames);
            context.SetContextType(contextType);
            await validator.ValidateAsync(context, cancellationToken);
        }

        public static async Task ValidateUpdateAndThrowAsync<TModel>(this IValidator<TModel> validator,
                                                               TModel instance,
                                                               ValidationContextType contextType,
                                                               long entityId,
                                                               CancellationToken cancellationToken = default)
        {
            ValidationContext<TModel> context = ValidationContext<TModel>.CreateWithOptions(instance, options => options.ThrowOnFailures());
            context.SetContextType(contextType);
            context.AddContextData("Id",entityId);
            await validator.ValidateAsync(context, cancellationToken);
        }

    }
}
