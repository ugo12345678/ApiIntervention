namespace Business.Abstraction.Exceptions
{
    /// <summary>
    /// Generic business error codes
    /// </summary>
    public enum BusinessErrorCode
    {
        // Input validation errors start at 1000
        ModelNotSupplied = 1000,
        NameAlreadyExists = 1010,
        UserAlreadyExists = 1020,
        UserWithSameEmailAlreadyExists = 1030,
        InconsistentModel = 1040,
        EmptyListSupplied = 1050,
        InvalidFileContent = 1060,
        InvalidTotpCode = 1070,
        InvalidUri = 1080,
        EmailAlreadyExists = 1100,
        InterventionNameAlreadyExists = 1200,

        // NotFound errors start at 2000
        ResourceNotFound = 2000,
        ResourceDependencyNotFound = 2010,

        // Unauthorized errors start at 3000
        UnauthorizedDeletion = 3000,
        UnauthorizedUpdate = 3010,

    }
}
