using System.Text.Json.Serialization;

namespace CamundaApp.Models
{
    public class StartProcess
    {
        [JsonPropertyName("imageType")]
        public string ImageType { get; set; }
    }
}
