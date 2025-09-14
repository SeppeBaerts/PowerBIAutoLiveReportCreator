using Microsoft.PowerBI.Api.Models;

namespace PowerBIReportCreator.Models;

public class DatasetSchema
{
    public string Name { get; set; } = string.Empty;
    public List<Table> Tables { get; set; } = new List<Table>();
}

public class SampleData
{
    public List<Dictionary<string, object>> People { get; set; } = new List<Dictionary<string, object>>();
}

public class ReportCreationResult
{
    public bool Success { get; set; }
    public string? ReportId { get; set; }
    public string? DatasetId { get; set; }
    public string? EmbedUrl { get; set; }
    public string? AccessToken { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Steps { get; set; } = new List<string>();
}