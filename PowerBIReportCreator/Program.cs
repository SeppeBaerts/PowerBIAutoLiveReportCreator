using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PowerBIReportCreator.Models;
using PowerBIReportCreator.Services;

namespace PowerBIReportCreator;

internal class Program
{
    private static ILogger<Program>? _logger;
    private static PowerBIConfig? _config;
    private static PowerBIService? _powerBIService;
    private static AdvancedPowerBIService? _advancedPowerBIService;
    private static SampleDataService? _sampleDataService;

    static async Task Main(string[] args)
    {
        try
        {
            // Setup configuration and logging
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile(Path.Combine("PowerBIReportCreator", "appsettings.json"), optional: true)
                .Build();

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<Program>();

            _config = new PowerBIConfig();
            configuration.GetSection("PowerBI").Bind(_config);

            if (string.IsNullOrEmpty(_config.ClientId) || string.IsNullOrEmpty(_config.ClientSecret) || 
                string.IsNullOrEmpty(_config.TenantId) || string.IsNullOrEmpty(_config.WorkspaceId))
            {
                Console.WriteLine("❌ PowerBI configuration is incomplete!");
                Console.WriteLine("Please update appsettings.json with your PowerBI credentials.");
                Console.WriteLine("See appsettings.example.json for the required format.");
                return;
            }

            _powerBIService = new PowerBIService(_config, loggerFactory.CreateLogger<PowerBIService>());
            _advancedPowerBIService = new AdvancedPowerBIService(_config, loggerFactory.CreateLogger<PowerBIService>());
            _sampleDataService = new SampleDataService();

            Console.WriteLine("🚀 PowerBI Auto Live Report Creator");
            Console.WriteLine("=====================================");
            
            await ShowMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Fatal error: {ex.Message}");
            _logger?.LogError(ex, "Fatal error in main program");
        }
    }

    private static async Task ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Create live report from template");
            Console.WriteLine("2. Generate sample data schema");
            Console.WriteLine("3. Push sample data to existing dataset");
            Console.WriteLine("4. Create sample template instructions");
            Console.WriteLine("5. Test PowerBI connection");
            Console.WriteLine("6. List workspace datasets");
            Console.WriteLine("7. List workspace reports");
            Console.WriteLine("0. Exit");
            Console.Write("\nEnter your choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateLiveReport();
                    break;
                case "2":
                    GenerateSampleDataSchema();
                    break;
                case "3":
                    await PushSampleData();
                    break;
                case "4":
                    CreateSampleTemplateInstructions();
                    break;
                case "5":
                    await TestConnection();
                    break;
                case "6":
                    await ListDatasets();
                    break;
                case "7":
                    await ListReports();
                    break;
                case "0":
                    Console.WriteLine("👋 Goodbye!");
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static async Task CreateLiveReport()
    {
        try
        {
            Console.WriteLine("\n📊 Creating Live Report");
            Console.WriteLine("=======================");

            Console.Write("Enter path to PowerBI template file (.pbix): ");
            var templatePath = Console.ReadLine();

            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
            {
                Console.WriteLine("❌ Template file not found!");
                return;
            }

            Console.WriteLine("📋 Using sample schema (People & Sales tables)...");
            var schema = _sampleDataService!.CreateSampleSchema();

            Console.WriteLine("\n🔄 Starting report creation process...");
            var result = await _advancedPowerBIService!.CreateFullLiveReportAsync(templatePath, schema);

            Console.WriteLine("\n📝 Process Steps:");
            foreach (var step in result.Steps)
            {
                Console.WriteLine($"   {step}");
            }

            if (result.Success)
            {
                Console.WriteLine("\n✅ Live report created successfully!");
                Console.WriteLine($"📊 Report ID: {result.ReportId}");
                Console.WriteLine($"📚 Dataset ID: {result.DatasetId}");
                Console.WriteLine($"🌐 Embed URL: {result.EmbedUrl}");
                Console.WriteLine("\n💡 You can now push data to this dataset and refresh the report every 15 seconds!");
                
                // Automatically push some sample data
                if (!string.IsNullOrEmpty(result.DatasetId))
                {
                    Console.WriteLine("\n🔄 Pushing sample data...");
                    var peopleData = _sampleDataService.GenerateSamplePeopleData(10);
                    var salesData = _sampleDataService.GenerateSampleSalesData(15);
                    
                    await _powerBIService.PushDataToDatasetAsync(result.DatasetId, "People", peopleData);
                    await _powerBIService.PushDataToDatasetAsync(result.DatasetId, "Sales", salesData);
                    
                    Console.WriteLine("✅ Sample data pushed successfully!");
                }
            }
            else
            {
                Console.WriteLine($"\n❌ Failed to create live report: {result.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error creating live report");
        }
    }

    private static void GenerateSampleDataSchema()
    {
        try
        {
            Console.WriteLine("\n📋 Sample Data Schema");
            Console.WriteLine("=====================");

            var schema = _sampleDataService!.CreateSampleSchema();
            
            Console.WriteLine($"Dataset Name: {schema.Name}");
            Console.WriteLine("\nTables:");
            
            foreach (var table in schema.Tables)
            {
                Console.WriteLine($"\n  📊 {table.Name}:");
                foreach (var column in table.Columns)
                {
                    Console.WriteLine($"    - {column.Name} ({column.DataType})");
                }
            }

            Console.WriteLine("\n💡 Use this schema when creating your PowerBI template!");
            Console.WriteLine("   Make sure table and column names match exactly (case-sensitive)!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error generating sample data schema");
        }
    }

    private static async Task PushSampleData()
    {
        try
        {
            Console.WriteLine("\n📤 Push Sample Data");
            Console.WriteLine("===================");

            Console.Write("Enter Dataset ID: ");
            var datasetId = Console.ReadLine();

            if (string.IsNullOrEmpty(datasetId))
            {
                Console.WriteLine("❌ Dataset ID is required!");
                return;
            }

            Console.WriteLine("🔄 Generating and pushing sample data...");
            
            var peopleData = _sampleDataService!.GenerateSamplePeopleData(15);
            var salesData = _sampleDataService.GenerateSampleSalesData(25);

            var peopleSuccess = await _powerBIService!.PushDataToDatasetAsync(datasetId, "People", peopleData);
            var salesSuccess = await _powerBIService.PushDataToDatasetAsync(datasetId, "Sales", salesData);

            if (peopleSuccess && salesSuccess)
            {
                Console.WriteLine("✅ Sample data pushed successfully!");
                Console.WriteLine($"   📊 {peopleData.Count} people records");
                Console.WriteLine($"   💰 {salesData.Count} sales records");
                Console.WriteLine("\n💡 Your PowerBI report should now show the updated data!");
            }
            else
            {
                Console.WriteLine("❌ Some data push operations failed. Check the logs for details.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error pushing sample data");
        }
    }

    private static void CreateSampleTemplateInstructions()
    {
        try
        {
            Console.WriteLine("\n📋 Creating Sample Template Instructions");
            Console.WriteLine("=========================================");

            var instructionsPath = Path.Combine(Directory.GetCurrentDirectory(), "PowerBI_Template_Instructions.txt");
            _sampleDataService!.CreateSampleReportTemplate(instructionsPath);

            Console.WriteLine($"✅ Instructions created: {instructionsPath}");
            Console.WriteLine("\n💡 Follow these instructions to create a compatible PowerBI template!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error creating sample template instructions");
        }
    }

    private static async Task TestConnection()
    {
        try
        {
            Console.WriteLine("\n🔌 Testing PowerBI Connection");
            Console.WriteLine("==============================");

            Console.WriteLine("🔄 Getting access token...");
            var token = await _powerBIService!.GetAccessTokenAsync();
            
            if (!string.IsNullOrEmpty(token))
            {
                Console.WriteLine("✅ Successfully connected to PowerBI!");
                Console.WriteLine($"🔑 Token received (length: {token.Length})");
            }
            else
            {
                Console.WriteLine("❌ Failed to get access token!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Connection test failed: {ex.Message}");
            _logger?.LogError(ex, "Connection test failed");
        }
    }

    private static async Task ListDatasets()
    {
        try
        {
            Console.WriteLine("\n📚 Workspace Datasets");
            Console.WriteLine("======================");

            Console.WriteLine("🔄 Fetching datasets...");
            var datasets = await _advancedPowerBIService!.ListDatasetsAsync();
            
            if (datasets.Any())
            {
                Console.WriteLine($"\nFound {datasets.Count} dataset(s):");
                foreach (var dataset in datasets)
                {
                    Console.WriteLine($"📊 {dataset.Name}");
                    Console.WriteLine($"   ID: {dataset.Id}");
                    Console.WriteLine($"   Is Refreshable: {dataset.IsRefreshable}");
                    Console.WriteLine($"   Configured By: {dataset.ConfiguredBy}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("📭 No datasets found in the workspace.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error listing datasets");
        }
    }

    private static async Task ListReports()
    {
        try
        {
            Console.WriteLine("\n📄 Workspace Reports");
            Console.WriteLine("=====================");

            Console.WriteLine("🔄 Fetching reports...");
            var reports = await _advancedPowerBIService!.ListReportsAsync();
            
            if (reports.Any())
            {
                Console.WriteLine($"\nFound {reports.Count} report(s):");
                foreach (var report in reports)
                {
                    Console.WriteLine($"📊 {report.Name}");
                    Console.WriteLine($"   ID: {report.Id}");
                    Console.WriteLine($"   Dataset ID: {report.DatasetId}");
                    Console.WriteLine($"   Embed URL: {report.EmbedUrl}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("📭 No reports found in the workspace.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            _logger?.LogError(ex, "Error listing reports");
        }
    }
}
