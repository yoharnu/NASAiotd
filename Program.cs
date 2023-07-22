using NASAiotd;
using System.Text.Json;

internal class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public static readonly HttpClient httpClient = new HttpClient();
    private static async Task Main(string[] args)
    {
        Logger.Info("Contacting NASA...");
        HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://www.nasa.gov/api/2/ubernode/_search?size=1&from=0&sort=promo-date-time%3Adesc&q=((ubernode-type%3Afeature%20OR%20ubernode-type%3Aimage)%20AND%20(routes%3A1446))&_source_include=promo-date-time%2Cmaster-image%2Cnid%2Ctitle%2Ctopics%2Cmissions%2Ccollections%2Cother-tags%2Cubernode-type%2Cprimary-tag%2Csecondary-tag%2Ccardfeed-title%2Ctype%2Ccollection-asset-link%2Clink-or-attachment%2Cpr-leader-sentence%2Cimage-feature-caption%2Cattachments%2Curi");

        HttpResponseMessage response = await httpClient.SendAsync(request);
        string text = await response.Content.ReadAsStringAsync();
        Logger.Debug("Response from NASA API: "+ text);
        try
        {
            NASAResponse? NResponse = JsonSerializer.Deserialize<NASAResponse>(text);
            if (NResponse is not null)
            {
                if (NResponse.getImage() is null)
                {
                    throw new JsonException("Unable to read NASA response");
                }
                else
                {
                    if (NResponse.getImage().uri is null)
                    {
                        throw new JsonException("Unable to read NASA response");
                    }
                    else
                    {
                        string URL = NResponse.getImage().uri.Replace("public://", "https://www.nasa.gov/sites/default/files/");
                        Logger.Debug("Image URL: " + URL);
                        Wallpaper.Set(new Uri(URL), Wallpaper.Style.Fill);
                    }
                }
            }
            else
            {
                throw new JsonException("Unable to read NASA response");
            }
        }
        catch (JsonException e)
        {
            Logger.Fatal(e.Message);
            Logger.Debug(e.StackTrace);
            return;
        }
    }
}