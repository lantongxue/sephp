namespace sephp.Nginx.Parser.NginxConfig;

public class ConfigDirective : ConfigStatement
{
    public string? Name { get; set; }
    public List<string> Args { get; } = new();
    public ConfigBlock? Block { get; set; }
    
    public ConfigComment? Comment { get; set; }
}