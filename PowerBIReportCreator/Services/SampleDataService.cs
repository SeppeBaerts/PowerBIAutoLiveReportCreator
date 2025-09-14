using Microsoft.PowerBI.Api.Models;
using PowerBIReportCreator.Models;

namespace PowerBIReportCreator.Services;

public class SampleDataService
{
    public DatasetSchema CreateSampleSchema()
    {
        var schema = new DatasetSchema
        {
            Name = "SampleLiveDataset",
            Tables = new List<Table>
            {
                new Table
                {
                    Name = "People",
                    Columns = new List<Column>
                    {
                        new Column { Name = "first_name", DataType = "String" },
                        new Column { Name = "last_name", DataType = "String" },
                        new Column { Name = "age", DataType = "Int64" },
                        new Column { Name = "email", DataType = "String" },
                        new Column { Name = "department", DataType = "String" },
                        new Column { Name = "salary", DataType = "Double" },
                        new Column { Name = "hire_date", DataType = "DateTime" }
                    }
                },
                new Table
                {
                    Name = "Sales",
                    Columns = new List<Column>
                    {
                        new Column { Name = "id", DataType = "Int64" },
                        new Column { Name = "product_name", DataType = "String" },
                        new Column { Name = "quantity", DataType = "Int64" },
                        new Column { Name = "price", DataType = "Double" },
                        new Column { Name = "sale_date", DataType = "DateTime" },
                        new Column { Name = "customer_name", DataType = "String" }
                    }
                }
            }
        };

        return schema;
    }

    public List<Dictionary<string, object>> GenerateSamplePeopleData(int count = 10)
    {
        var random = new Random();
        var firstNames = new[] { "John", "Jane", "Mike", "Sarah", "David", "Lisa", "Chris", "Emma", "Ryan", "Amy" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
        var departments = new[] { "Engineering", "Sales", "Marketing", "HR", "Finance", "Operations" };

        var data = new List<Dictionary<string, object>>();

        for (int i = 0; i < count; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            
            data.Add(new Dictionary<string, object>
            {
                { "first_name", firstName },
                { "last_name", lastName },
                { "age", random.Next(22, 65) },
                { "email", $"{firstName.ToLower()}.{lastName.ToLower()}@company.com" },
                { "department", departments[random.Next(departments.Length)] },
                { "salary", Math.Round(random.NextDouble() * (120000 - 40000) + 40000, 2) },
                { "hire_date", DateTime.Now.AddDays(-random.Next(1, 3650)).ToString("yyyy-MM-ddTHH:mm:ss") }
            });
        }

        return data;
    }

    public List<Dictionary<string, object>> GenerateSampleSalesData(int count = 20)
    {
        var random = new Random();
        var products = new[] { "Laptop", "Mouse", "Keyboard", "Monitor", "Headphones", "Tablet", "Phone", "Speaker", "Camera", "Printer" };
        var customers = new[] { "Acme Corp", "Tech Solutions", "Global Industries", "Innovation Labs", "Future Systems", "Digital World" };

        var data = new List<Dictionary<string, object>>();

        for (int i = 0; i < count; i++)
        {
            var product = products[random.Next(products.Length)];
            var basePrice = random.NextDouble() * (2000 - 50) + 50;
            var quantity = random.Next(1, 10);
            
            data.Add(new Dictionary<string, object>
            {
                { "id", i + 1 },
                { "product_name", product },
                { "quantity", quantity },
                { "price", Math.Round(basePrice, 2) },
                { "sale_date", DateTime.Now.AddDays(-random.Next(0, 30)).ToString("yyyy-MM-ddTHH:mm:ss") },
                { "customer_name", customers[random.Next(customers.Length)] }
            });
        }

        return data;
    }

    public void CreateSampleReportTemplate(string outputPath)
    {
        // Create a simple Power BI template instructions
        var instructions = @"
CREATING A SAMPLE POWER BI TEMPLATE:

1. Open Power BI Desktop
2. Create a new report
3. Add the following data sources (you can use 'Enter Data' option):

TABLE: People
Columns: first_name (Text), last_name (Text), age (Number), email (Text), department (Text), salary (Number), hire_date (Date)
Sample Data:
- John, Smith, 30, john.smith@company.com, Engineering, 75000, 2022-01-15
- Jane, Doe, 28, jane.doe@company.com, Sales, 65000, 2021-03-20

TABLE: Sales  
Columns: id (Number), product_name (Text), quantity (Number), price (Number), sale_date (Date), customer_name (Text)
Sample Data:
- 1, Laptop, 2, 1200.00, 2024-01-15, Acme Corp
- 2, Mouse, 5, 25.99, 2024-01-16, Tech Solutions

4. Create some visualizations (charts, tables, etc.) using these tables
5. Save the report as a .pbix file
6. Place the .pbix file in the same directory as this application
7. Update the templateReportPath in the application to point to your .pbix file

IMPORTANT: The table names and column names must match exactly (case-sensitive) with the schema defined in the application!
";

        File.WriteAllText(outputPath, instructions);
    }
}