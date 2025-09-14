namespace PowerBIReportCreator.Models;

public class PowerBIConfig
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public string AuthorityUrl { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = string.Empty;
    public string EmbedUrl { get; set; } = string.Empty;
}