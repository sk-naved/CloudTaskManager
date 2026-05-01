using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup; // Requires AWSSDK.Extensions.NETCore.Setup NuGet package
using CloudTaskManager.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1. Register AWS Clients (Pointed to LocalStack)
//var awsOptions = builder.Configuration.GetAWSOptions();
//awsOptions.DefaultClientConfig.ServiceURL = "http://localhost:4566"; // LocalStack URL

// 1. Configure the S3 Client
var s3Config = new AmazonS3Config
{
	ServiceURL = "http://localhost:4566",
	ForcePathStyle = true
};
// Register using the INTERFACE IAmazonS3
builder.Services.AddSingleton<IAmazonS3>(sp => new AmazonS3Client(s3Config));

// 2. Configure the DynamoDB Client
var dynamoConfig = new AmazonDynamoDBConfig
{
	ServiceURL = "http://localhost:4566"
};
// Register using the INTERFACE IAmazonDynamoDB
builder.Services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(dynamoConfig));

// 3. Register your initializer
builder.Services.AddScoped<LocalStackInitializer>();
var app = builder.Build();
app.MapControllers();

// 3. Trigger Initialization on Startup
using (var scope = app.Services.CreateScope())
{
	var initializer = scope.ServiceProvider.GetRequiredService<LocalStackInitializer>();
	await initializer.InitializeAsync();
}


app.Run();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();