namespace Business.Abstraction.Exceptions
{
    /// <summary>
    /// Exception thrown in Business Layer in case of Business error
    /// </summary>
    public class BusinessException : Exception
    {
        public string ObjectType { get; init; }

        public IEnumerable<BusinessError> BusinessErrors { get; init; }

        public BusinessException(IEnumerable<BusinessError> businessErrors, string objectType = null) : base($"{businessErrors.Count()} business error(s) detected" + (objectType == null ? "" : $" [Type: {objectType}] "))
        {
            BusinessErrors = businessErrors;
            ObjectType = objectType;
        }

        public BusinessException(BusinessErrorCode businessError, string message, string objectType = null, string valueInFailure = null) : this(businessErrors: new BusinessError[] { new BusinessError { Code = businessError, Message = message, ValueInFailure = valueInFailure}}, objectType: objectType)
        {
        }
    }
}
