using System.Net;
using System.Text.RegularExpressions;

namespace HTMLInformer;

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
