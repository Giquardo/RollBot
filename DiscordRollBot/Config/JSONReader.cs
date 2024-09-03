using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rollbot.Config;
internal class JSONReader
{
    public string token { get; set; } = string.Empty;
    public string prefix { get; set; } = string.Empty;

    public async Task ReadJSON()
    {
        using (StreamReader sr = new StreamReader("config.json"))
        {
            string json = await sr.ReadToEndAsync();
            JSONStructure? data = JsonConvert.DeserializeObject<JSONStructure>(json);

            if (data != null)
            {
                token = data.token ?? string.Empty;
                prefix = data.prefix ?? string.Empty;
            }
        }
    }
}

internal sealed class JSONStructure
{
    public string? token { get; set; }
    public string? prefix { get; set; }
}
