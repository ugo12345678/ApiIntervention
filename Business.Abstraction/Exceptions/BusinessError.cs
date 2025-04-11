
namespace Business.Abstraction.Exceptions
{
    public class BusinessError
	{
		public BusinessErrorCode Code { get; set; }

        public string Message { get; set; } = string.Empty;
        
        public string? ValueInFailure { get; set; }
    }
}