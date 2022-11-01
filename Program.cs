using System.Net;
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

class HTMLInformer
{
    public string HTML { get; set; }

    public void DownloadHtml(string uri)
    {
        using (WebClient client = new WebClient())
        {
            try
            {
                HTML = client.DownloadString(uri);
            }
            catch
            {
                Console.WriteLine("Something wrong. Check ref or your internet");
                Environment.Exit(-1);
            }
        }
    }

    public List<string> GetAllAnchors()
    {
        return (from m in Regex.Matches(HTML, @"<a[^>]*>[^<]*</a>") select m.Value).ToList();
    }

    public List<string> GetAllReferences()
    {
        return (from m in Regex.Matches(HTML, @"<a[^>]*href=""(http.*)""[^>]*>[^<]*<\/a>")
                select m.Groups[1].Value)
                .ToList();
    }

    public List<string> GetAllEmails()
    {
        return (from m in Regex.Matches(HTML, @"[\w-\.\+]*@[\w-]*\.[\w]{2,4}") select m.Value).ToList();
    }
}
