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
                public Source? _source { get; set; }
                public Source? getSource() => _source;
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
                Console.WriteLine("first hits is null");
            }
            else if (this.hits.hits is null)
            {
                Console.WriteLine("second hits is null");
            }
            else if (this.hits.hits.Length == 0)
            {
                Console.WriteLine("second hits is empty");
            }
            else if (this.hits.hits[0].getSource() is null)
            {
                Console.WriteLine("source is null");
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            else if (this.hits.hits[0].getSource().masterImage is null)
            {
                Console.WriteLine("image is null");
            }
            else
            {
                return this.hits.hits[0].getSource().masterImage;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return null;
        }
    }
}
