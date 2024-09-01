using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rollbot.Config;

internal class JSONReader
{
    public string token { get; set; }
    public string prefix { get; set; }

    public async Task ReadJSON()
    {
        using (StreamReader sr = new StreamReader("config.json"))
        {
            string json = await sr.ReadToEndAsync();
            JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);
            token = data.token;
            prefix = data.prefix;
        }
    }
}
internal sealed class JSONStructure
{
    public string token { get; set; }
    public string prefix { get; set; }
}
