using System.Text.Json.Serialization;

namespace Rogue;

public class Manual
{
    [JsonInclude]
    private List<String> _manual;
    public Manual()
    {
        _manual = new List<String>();
    }
    public void AddToManual(String line)
    {
        if(line.Length > Constants.MapWidth)
            return;
        _manual.Add(line);
    }
    public List<String> GetManual()
    {
        return _manual;
    }
}