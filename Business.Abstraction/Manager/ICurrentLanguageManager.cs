using Business.Abstraction.Models;

namespace Business.Abstraction.Manager
{
    public interface ICurrentLanguageManager
    {
        string CurrentLanguage { get; }
    }
}
