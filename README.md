This project is currently on hold. But you can code this yourself. 
Using the Microsoft.PowerBI.Api NuGet Package, do the following:
1) Create a Report Template. (In Power BI, not using the nuget package. Make sure you keep note of the tables you are using. ex => table people has 3 columns: first_name, last_name, age. These tables can contain data)
2) Save it as a power BI report.
3) Upload the Report using the api.
4) Create a Push-dataset using the api => IMPORTANT: Use the exact same tables and columns, case-sensitive!!
5) Get the ID for the report and the push dataset.
6) Clone the template report and change the dataset to the newly created push dataset.
7) Delete the template Dataset (it created a dataset when you uploaded the template report, deleting the dataset will delete the report)
8) You can now get the embedded view and refresh it every 15 seconds
9) To add data to the push dataset, you can push it using the API.

This will still require some research, but it should give you the basic idea. 
