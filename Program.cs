using HtmlAgilityPack;

namespace NASAiotd;

internal class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public static readonly HttpClient httpClient = new HttpClient();
    private static async Task Main(string[] args)
    {
        Logger.Info("Contacting NASA...");
        HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.nasa.gov/image-of-the-day/");

        HttpResponseMessage response = await httpClient.SendAsync(request);
        string text = await response.Content.ReadAsStringAsync();
        Logger.Debug("Response from NASA API: " + text);
        try
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(text);
            var imageNode = htmlDoc.DocumentNode.SelectNodes("//article/section/div/div[2]/div/div[1]/a/img").FirstOrDefault();// .Attributes["src"].Value;
            if (imageNode is null)
            {
                throw new HttpRequestException("Unable to read NASA response");
            }

            var imageURL = imageNode.Attributes.FirstOrDefault(x => x.Name == "src")?.Value ?? string.Empty;

            if (string.IsNullOrWhiteSpace(imageURL))
            {
                throw new HttpRequestException("Unable to parse URL");
            }

            Console.WriteLine(imageURL);
            Wallpaper.Set(new Uri(imageURL), Wallpaper.Style.Fill);
            return;
        }
        catch (HttpRequestException e)
        {
            Logger.Fatal(e.Message);
            Logger.Debug(e.StackTrace);
            return;
        }
    }
}