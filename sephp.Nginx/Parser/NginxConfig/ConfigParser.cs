using sephp.Nginx.Parser.NginxConfig.Directive;
using System.Text.RegularExpressions;

namespace sephp.Nginx.Parser.NginxConfig;

public static class ConfigParser
{
    public static ConfigRoot Parse(string path)
    {
        var lines = File.ReadAllLines(path);
        var root = new ConfigRoot();
        var stack = new Stack<ConfigBlock>();
        stack.Push(root);
        foreach (string line in lines)
        {
            var text = line.Trim().TrimEnd(';');
            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }
            
            // 当前 block
            var currentBlock = stack.Peek();

            if (text.StartsWith("#"))
            {
                var comment = new ConfigComment()
                {
                    Text = text
                };
                currentBlock.Statements.Add(comment);
                continue;
            }

            var isBlock = text.EndsWith("{");
            var blockEnd = text == "}";
            
            if (blockEnd)
            {
                stack.Pop(); // 回到上一级 block
                continue;
            }

            var arr = Regex.Split(text, @"\s+");

            string[] args = new string[arr.Length - 1];
            Array.Copy(arr, 1, args, 0, args.Length);

            ConfigDirective stmt;
            switch (arr[0])
            {
                case "server":
                    stmt = new ServerDirective();
                    break;
                default:
                    stmt = new ConfigDirective();
                    break;
            }
            
            stmt.Name = arr[0];
            stmt.Args.AddRange(args);

            currentBlock.Statements.Add(stmt);

            if (isBlock)
            {
                if (args[^1] == "{")
                {
                    stmt.Args.RemoveAt(stmt.Args.Count - 1);
                }
                stmt.Block = new ConfigBlock();
                stack.Push(stmt.Block); // 进入新 block
            }
        }
        return root;
    }
}