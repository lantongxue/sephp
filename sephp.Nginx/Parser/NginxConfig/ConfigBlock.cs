using sephp.Nginx.Parser.NginxConfig.Directive;

namespace sephp.Nginx.Parser.NginxConfig;

public class ConfigBlock
{
    public List<ConfigStatement> Statements { get; } = new();

    public ConfigDirective? Find(string name)
    {
        var stmt = Statements.Where(stmt => {
            if(stmt is ConfigDirective)
            {
                var d = (ConfigDirective)stmt;
                return d.Name == name;
            }
            return false;
        }).FirstOrDefault();
        return (ConfigDirective?)stmt;
    }

    public IEnumerable<T>? FindDirectives<T>()
    {
        return (IEnumerable<T>?)Statements.Where(stmt => stmt is T);
    }
    
    public string ToString(bool debug = false)
    {
        return PrintStatements(Statements, 0, debug);
    }

    private string PrintStatements(List<ConfigStatement> stmts, int indent, bool debug)
    {
        var sb = new System.Text.StringBuilder();
        string pad = new string(' ', indent * 4);

        foreach (var stmt in stmts)
        {
            switch (stmt)
            {
                case ConfigComment comment:
                    if (debug)
                    {
                        sb.AppendLine($"{pad}# [Comment] {comment.Text}");
                    }
                    else
                    {
                        sb.AppendLine($"{pad}{comment.Text}");
                    }
                    break;
                case ConfigDirective { Block: not null } dir:
                    if (debug)
                    {
                        sb.AppendLine($"{pad}{dir.Name} {string.Join(" ", dir.Args)} {{   // [Block]");
                    }
                    else
                    {
                        sb.AppendLine($"{pad}{dir.Name} {string.Join(" ", dir.Args)} {{");
                    }
                    sb.Append(PrintStatements(dir.Block.Statements, indent + 1, debug));
                    sb.AppendLine($"{pad}}}");
                    break;
                case ConfigDirective dir:
                    if (debug)
                    {
                        sb.AppendLine($"{pad}{dir.Name} {string.Join(" ", dir.Args)};   // [Directive]");
                    }
                    else
                    {
                        sb.AppendLine($"{pad}{dir.Name} {string.Join(" ", dir.Args)};");
                    }
                    break;
            }
        }

        return sb.ToString();
    }
}