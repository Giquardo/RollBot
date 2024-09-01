namespace RollBotApi.Configuration;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string UsersCollection { get; set; } = string.Empty;

    //public string CardsCollection { get; set; }
    //public string CardPacksCollection { get; set; }
}
