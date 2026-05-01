using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace CloudTaskManager.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(IAmazonDynamoDB dynamoDb, IAmazonS3 s3Client) : ControllerBase
{
	private readonly DynamoDBContext _context = new(dynamoDb);
	private const string BucketName = "task-assets";

	[HttpGet]
	public async Task<IActionResult> GetTasks()
	{
		// Scan the table for all tasks
		var search = _context.ScanAsync<CloudTask>(new List<ScanCondition>());
		var results = await search.GetRemainingAsync();
		return Ok(results);
	}

	[HttpPost]
	public async Task<IActionResult> CreateTask([FromForm] string title, IFormFile? file)
	{
		var task = new CloudTask
		{
			Id = Guid.NewGuid().ToString(),
			Title = title,
			CreatedAt = DateTime.UtcNow
		};

		if (file != null)
		{
			// Upload file to S3
			using var stream = file.OpenReadStream();
			var uploadRequest = new TransferUtilityUploadRequest
			{
				InputStream = stream,
				Key = $"{task.Id}_{file.FileName}",
				BucketName = BucketName
			};

			var fileTransferUtility = new TransferUtility(s3Client);
			await fileTransferUtility.UploadAsync(uploadRequest);

			// Construct LocalStack S3 URL
			task.FileUrl = $"http://localhost:4566/{BucketName}/{uploadRequest.Key}";
		}

		await _context.SaveAsync(task);
		return Ok(task);
	}
}

// Simple Model for DynamoDB
[DynamoDBTable("TasksTable")]
public class CloudTask
{
	[DynamoDBHashKey] public string Id { get; set; }
	public string Title { get; set; }
	public string? FileUrl { get; set; }
	public DateTime CreatedAt { get; set; }
}