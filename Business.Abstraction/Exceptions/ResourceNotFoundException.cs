namespace Business.Abstraction.Exceptions
{
    public abstract class ResourceNotFoundException : Exception
    {
        protected ResourceNotFoundException(string message) : base(message)
        {
        }
    }

    public class ResourceNotFoundException<T> : ResourceNotFoundException where T : class
    {
        public ResourceNotFoundException(object entityId) : base($"Object of type {typeof(T).Name} with id [{entityId}] does not exist")
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
