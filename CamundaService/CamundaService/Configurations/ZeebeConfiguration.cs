namespace CamundaApp.Configurations
{
    public class ZeebeConfiguration
    {
        public const string Name = "ZeebeConfig";
        public string? ClientId { get; set; }
        public  string? ClientSecret { get; set; }
        public string ZeebeUrl { get; set; } = String.Empty;
    }
}
