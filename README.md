# Online-Experiment-Basics
This is a basic system which currently is designed to work in unison with online platforms such as Prolific,  allowing the management of participants and storing of log details

**In order to set up this project to work the following steps needs to be followed:  (this process will generally need to be completed on each pull)**

1. Clone the repository
2. Open project in Visual Studio (or equivalent)
3. Add in Database Connection String in appsettings.Development.json in the ConnectionStrings setting in the following format: "ConnectionString": "<your connection string>"
4. Run all migrations: Set the Api as the Startup project, Open Package Manager Console, switch default project to MainEnvironment.Database, then type in the command "Update-Database" and press enter
5. Click run under the IISExpress build definition (should be set as default)
6. To test you can use the included Postman collection in the PostmanTests folder, this includes tests for obtaining an experiment definition and adding a log

**To integrate with Unity:**

1. Build the solution under the release build
2. Navigate to the MainEnvironment.Core/Bin/Release/netcoreapp1.1 folder
3. Copy the MainEnvironment.Core dll into your Unity project's Assets/Libraries folder (create if it doesn't exist)
4. Navigate to the MainEnvironment.Web/bin/Release/netcoreapp3.1 folder
5. Copy the Newtonsoft.Json dll into your Unity project's Asset/Libraries folder
6. The project should now be referenceable in Unity scripts
7. To send a request, instantiate an instance of either the 'LogService' or 'ExperimentService', passing in the Host of your API, for most testing this will be https://localhost:44314.
8.Update the method to have the async signature e.g. public async void MethodName()
9. Add the await keyword when calling into the required method of the Api service
* N.B. Be careful this is not done in Unity's Update method (or make sure to handle appropriately)

**Please look at the ConsoleTest Project for examples on how to potentially create an experiment definition / configuration file (although this will need work)**
