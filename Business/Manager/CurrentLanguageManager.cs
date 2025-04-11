using Business.Abstraction.Manager;
using Microsoft.AspNetCore.Http;

public class CurrentLanguageManager : ICurrentLanguageManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentLanguageManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CurrentLanguage
    {
        get
        {
            var lang = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();
            if (string.IsNullOrWhiteSpace(lang)) return "fr";

            var langCode = lang.Split(',')[0].ToLower();

            var supportedLanguages = new[] { "en", "fr" };
            return supportedLanguages.Contains(langCode) ? langCode : "fr";
        }
    }
}
