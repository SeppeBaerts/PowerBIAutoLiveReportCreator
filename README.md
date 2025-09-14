# PowerBI Auto Live Report Creator

A comprehensive .NET application that automates the creation of live PowerBI reports with push datasets, allowing real-time data updates every 15 seconds.

## Features

- 🚀 **Automated Live Report Creation**: Creates PowerBI reports with push datasets for real-time updates
- 📊 **Template-based Approach**: Upload your custom PowerBI template and automatically connect it to live datasets
- 🔄 **Real-time Data Push**: Push data to datasets and see updates in your reports within 15 seconds
- 🛠️ **User-friendly Interface**: Interactive console application with step-by-step guidance
- 📋 **Sample Data Generation**: Built-in sample data generators for testing
- 🔧 **Foolproof Setup**: Clear instructions and error handling for easy configuration

## Prerequisites

1. **PowerBI Pro/Premium License**: Required for API access and workspace operations
2. **Azure App Registration**: For authentication with PowerBI Service
3. **PowerBI Workspace**: A workspace where you have admin permissions
4. **.NET 8.0 Runtime**: Required to run the application

## Setup Instructions

### Step 1: Azure App Registration

1. Go to the [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** > **App registrations**
3. Click **New registration**
4. Configure your app:
   - **Name**: PowerBI Live Report Creator
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**: Not required for this application
5. After creation, note down:
   - **Application (client) ID**
   - **Directory (tenant) ID**
6. Go to **Certificates & secrets** > **Client secrets**
7. Click **New client secret** and note down the **Value**
8. Go to **API permissions**:
   - Click **Add a permission**
   - Select **PowerBI Service**
   - Choose **Application permissions**
   - Add these permissions:
     - `Dataset.ReadWrite.All`
     - `Report.ReadWrite.All`
     - `Workspace.ReadWrite.All`
   - Click **Grant admin consent**

### Step 2: PowerBI Service Configuration

1. Go to [PowerBI Admin Portal](https://app.powerbi.com/admin-portal)
2. Navigate to **Tenant settings** > **Developer settings**
3. Enable **Service principals can use PowerBI APIs**
4. Add your App Registration to the security group or allow for the entire organization
5. Create or identify a workspace where your app will operate
6. Note down the **Workspace ID** (found in the workspace URL)

### Step 3: Application Configuration

1. Clone or download this repository
2. Navigate to the project directory
3. Copy `appsettings.example.json` to `appsettings.json`
4. Update `appsettings.json` with your credentials:

```json
{
  "PowerBI": {
    "ClientId": "YOUR_CLIENT_ID_HERE",
    "ClientSecret": "YOUR_CLIENT_SECRET_HERE", 
    "TenantId": "YOUR_TENANT_ID_HERE",
    "WorkspaceId": "YOUR_WORKSPACE_ID_HERE",
    "AuthorityUrl": "https://login.microsoftonline.com/YOUR_TENANT_ID_HERE",
    "ResourceUrl": "https://analysis.windows.net/powerbi/api",
    "ApiUrl": "https://api.powerbi.com",
    "EmbedUrl": "https://app.powerbi.com"
  }
}
```

## How to Use

### Step 1: Build and Run the Application

```bash
# Build the application
dotnet build

# Run the application
dotnet run --project PowerBIReportCreator
```

### Step 2: Create a PowerBI Template

1. Run the application and select option **4** to create template instructions
2. Follow the generated instructions to create a PowerBI template
3. **Important**: Ensure your template uses the exact table and column names as shown in the schema (case-sensitive)

**Required Tables and Columns:**

- **People Table**: `first_name`, `last_name`, `age`, `email`, `department`, `salary`, `hire_date`
- **Sales Table**: `id`, `product_name`, `quantity`, `price`, `sale_date`, `customer_name`

### Step 3: Test Your Configuration

1. Select option **5** to test your PowerBI connection
2. If successful, you'll see a confirmation with token details
3. If it fails, check your configuration and Azure permissions

### Step 4: Create Your Live Report

1. Select option **1** to create a live report from template
2. Enter the path to your PowerBI template file (.pbix)
3. The application will:
   - Create a push dataset with the correct schema
   - Upload your template (requires additional setup)
   - Connect the template to the live dataset
   - Generate embed information

### Step 5: Push Data to Your Report

1. After creating the live report, note the **Dataset ID**
2. Select option **3** to push sample data
3. Enter your Dataset ID
4. The application will generate and push sample data
5. Your PowerBI report will update within 15 seconds

## Application Menu Options

1. **Create live report from template**: Main workflow to create a live reporting solution
2. **Generate sample data schema**: View the expected data structure
3. **Push sample data to existing dataset**: Update an existing dataset with new data
4. **Create sample template instructions**: Generate step-by-step template creation guide
5. **Test PowerBI connection**: Verify your configuration and permissions
0. **Exit**: Close the application

## Workflow Overview

The application follows the PowerBI Live Report creation process:

1. **Create Push Dataset**: Establishes a dataset that can receive real-time data
2. **Upload Template**: Your custom PowerBI report template
3. **Connect Template to Dataset**: Links your visualizations to the live data source
4. **Push Data**: Send data to the dataset for real-time updates
5. **Embed/View**: Access your live report through PowerBI Service

## Troubleshooting

### Authentication Issues
- Verify your Azure App Registration credentials
- Ensure admin consent has been granted for PowerBI API permissions
- Check that service principals are enabled in PowerBI tenant settings

### Permission Errors
- Confirm you have admin access to the specified workspace
- Verify the workspace ID is correct
- Ensure your app registration has the required PowerBI API permissions

### Data Push Failures
- Verify table and column names match exactly (case-sensitive)
- Check that the dataset ID is correct
- Ensure data types match the schema definition

### Template Upload Issues
- The current implementation provides guidance for manual template setup
- Ensure your template uses the exact schema provided by the application
- Template must be saved as a .pbix file

## Data Refresh

- **Push datasets** support real-time updates
- Data changes appear in reports within **15 seconds**
- No need for scheduled refresh - updates are immediate
- Maximum of **1 million rows** per table in push datasets

## Limitations

- Push datasets have a 1 million row limit per table
- Template upload requires additional authentication setup
- Some operations may require manual steps in PowerBI Service
- Real-time streaming is limited to push datasets

## Contributing

Feel free to submit issues, fork the repository, and create pull requests for any improvements.

## License

This project is licensed under the MIT License - see the LICENSE.txt file for details.

## Support

For issues related to:
- **PowerBI API**: Check [PowerBI REST API documentation](https://docs.microsoft.com/en-us/rest/api/power-bi/)
- **Azure Authentication**: Review [Azure App Registration guide](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)
- **This Application**: Create an issue in this repository 
