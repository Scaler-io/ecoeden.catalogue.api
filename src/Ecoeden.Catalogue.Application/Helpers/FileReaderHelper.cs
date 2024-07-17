
using Newtonsoft.Json;

namespace Ecoeden.Catalogue.Application.Helpers;
public class FileReaderHelper<T> where T: class
{
    public static List<T> ReadFile(string filename, string path) 
    {
        var data = File.ReadAllText($"{path}/{filename}.json");
        var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
        return jsonData;
    }
}
