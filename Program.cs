using System.Text.RegularExpressions;

namespace HTMLInformer;

internal class Program
{
    private static HTMLInformer _htmlInformer = new HTMLInformer();
    private static List<string> _matchesList;

    static void Main(string[] args)
    {
        ProcessArgs(args);

        foreach (var match in _matchesList)
        {
            Console.WriteLine(match);
        }
    }

    private static void ProcessArgs(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("ref");
            Console.WriteLine("anchors");
            Console.WriteLine("emails");

            Environment.Exit(-1);
        }
        if (args.Length == 1 || args.Length > 2)
        {
            Console.WriteLine("Enter ref or path to html file.");
            Environment.Exit(-1);
        }
        else if (Regex.IsMatch(args[1], @"^http"))
        {
            _htmlInformer.DownloadHtml(args[1]);
        }
        else
        {
            try
            {
                string html = File.ReadAllText(args[1]);
                _htmlInformer.HTML = html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        _matchesList = null;

        switch (args[0].ToLower())
        {
            case "anchors":
                _matchesList = _htmlInformer.GetAllAnchors();
                break;
            case "ref":
                _matchesList = _htmlInformer.GetAllReferences();
                break;
            case "emails":
                _matchesList = _htmlInformer.GetAllEmails();
                break;
        }

        if (_matchesList is null)
        {
            Console.WriteLine("Unknown command. Try again.");
        }
    }
}
