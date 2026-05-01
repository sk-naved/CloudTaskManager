using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Amazon.S3.Util;
namespace CloudTaskManager.Server.Services
{
	public class LocalStackInitializer(IAmazonDynamoDB dynamoClient, IAmazonS3 s3Client, ILogger<LocalStackInitializer> logger)
	{
		public async Task InitializeAsync()
		{
			logger.LogInformation("Initializing LocalStack Resources...");

			// 1. Initialize S3 Bucket
			const string bucketName = "task-assets";
			if (!await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName))
			{
				await s3Client.PutBucketAsync(bucketName);
				logger.LogInformation("S3 Bucket '{Bucket}' created.", bucketName);
			}

			// 2. Initialize DynamoDB Table
			const string tableName = "TasksTable";
			var tables = await dynamoClient.ListTablesAsync();

			if (!tables.TableNames.Contains(tableName))
			{
				await dynamoClient.CreateTableAsync(new CreateTableRequest
				{
					TableName = tableName,
					AttributeDefinitions = [
						new() { AttributeName = "Id", AttributeType = ScalarAttributeType.S }
					],
					KeySchema = [
						new() { AttributeName = "Id", KeyType = KeyType.HASH }
					],
					ProvisionedThroughput = new ProvisionedThroughput(5, 5)
				});
				logger.LogInformation("DynamoDB Table '{Table}' created.", tableName);
			}
		}
	}
}
