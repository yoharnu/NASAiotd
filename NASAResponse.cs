using System.Text.Json;
using System.Text.Json.Serialization;

namespace NASAiotd
{
    internal class NASAResponse
    {
        public Hits? hits { get; set; }
        internal class Hits
        {
            public Hit[]? hits { get; set; }
            internal class Hit
            {
                [JsonPropertyName("_source")]
                public Source? source { get; set; }
                public Source? getSource() => source;
                internal class Source
                {
                    [JsonPropertyName("master-image")]
                    public Image? masterImage { get; set; }

                    internal class Image
                    {
                        public String? uri { get; set; }
                        public String? title { get; set; }

                    }
                }
            }
        }

        public Hits.Hit.Source.Image? getImage()
        {
            if (this.hits == null)
            {
                throw new JsonException("Unable to read NASA response");
            }
            else if (this.hits.hits is null)
            {
                throw new JsonException("Unable to read NASA response");
            }
            else if (this.hits.hits.Length == 0)
            {
                throw new JsonException("Unable to read NASA response");
            }
            else if (this.hits.hits[0].getSource() is null)
            {
                throw new JsonException("Unable to read NASA response");
            }
            else if (this.hits.hits[0].getSource().masterImage is null)
            {
                throw new JsonException("Unable to read NASA response");
            }
            else
            {
                return this.hits.hits[0].getSource().masterImage;
            }
        }
    }
}
