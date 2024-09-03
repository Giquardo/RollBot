namespace RollBotApi.Configuration;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string UsersCollection { get; set; } = string.Empty;
    public string SeriesCollection { get; set; } = string.Empty;
    public string CharactersCollection { get; set; } = string.Empty;
    public string TagsCollection { get; set; } = string.Empty;
    public string CardPacksCollection { get; set; } = string.Empty;

    //public string CardsCollection { get; set; }
}
