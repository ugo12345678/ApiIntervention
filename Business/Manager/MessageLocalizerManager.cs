using Business.Abstraction.Manager;


public class MessageLocalizerManager : IMessageLocalizerManager
{
    private readonly ICurrentLanguageManager _languageManager;

    private readonly Dictionary<string, Dictionary<string, string>> _messages = new()
    {
        ["AlreadyExists"] = new()
        {
            ["en"] = "'{PropertyName}' already exists.",
            ["fr"] = "'{PropertyName}' existe déjà."
        },
        ["DoesNotExist"] = new()
        {
            ["en"] = "Resource for '{PropertyName}' with value '{PropertyValue}' does not exist.",
            ["fr"] = "La ressource pour '{PropertyName}' avec la valeur '{PropertyValue}' n'existe pas."
        },
        ["InvalidIdMessage"] = new()
        {
            ["en"] = "The identifier defined in the resource path is different from the one given in the body object.",
            ["fr"] = "L'identifiant défini dans le chemin de la ressource est différent de celui fourni dans l'objet de la requête."
        },
        ["NotAlphanumeric"] = new()
        {
            ["en"] = "'{PropertyName}' with value '{PropertyValue}' should be alphanumeric.",
            ["fr"] = "'{PropertyName}' avec la valeur '{PropertyValue}' doit être alphanumérique."
        },
        ["WrongBoolValue"] = new()
        {
            ["en"] = "'{PropertyName}' should not be {PropertyValue}.",
            ["fr"] = "'{PropertyName}' ne devrait pas être {PropertyValue}."
        },
        ["CannotBeUpdated"] = new()
        {
            ["en"] = "'{PropertyName}' should not be updated.",
            ["fr"] = "'{PropertyName}' ne devrait pas être modifié."
        }
    };


    public MessageLocalizerManager(ICurrentLanguageManager languageManager)
    {
        _languageManager = languageManager;
    }

    public string Get(string key)
    {
        var lang = _languageManager.CurrentLanguage;
        return _messages.TryGetValue(key, out var translations) && translations.TryGetValue(lang, out var message)
            ? message
            : key;
    }
}
