🚀 Cloud Task Manager

A full-stack task management application built with React and ASP.NET Core, featuring a local cloud infrastructure powered by LocalStack.

This project demonstrates how to build and test AWS-native applications (S3 for file storage and DynamoDB for data) entirely on a local machine without incurring cloud costs.
🛠️ Tech Stack
Layer	Technology
Frontend	React 19 (Vite), JavaScript/JSX
Backend	.NET Core 10 API
Cloud Services	LocalStack (S3, DynamoDB)
Containerization	Docker Desktop
SDKs	AWS SDK for .NET
🏗️ Architecture

The application uses a Proxy Architecture. The React frontend communicates with the .NET Backend, which acts as a secure gateway to the AWS services running inside a LocalStack Docker container.

    DynamoDB: Stores task metadata (IDs, Titles, Timestamps).

    S3: Stores physical file attachments linked to tasks.

    LocalStack Initializer: A custom C# service that automatically creates the necessary buckets and tables on application startup.

🚀 Getting Started
1. Prerequisites

    Visual Studio 2026 (with ASP.NET and web development workload)

    Docker Desktop (Required for LocalStack)

    Node.js & npm

    AWS CLI (optional, for manual verification)

2. Setup LocalStack

Run the following command to start the local AWS environment:
Bash

docker run --rm -it -p 4566:4566 -p 4510-4559:4510-4559 localstack/localstack

3. Run the Application

    Open the solution in Visual Studio.

    Ensure both the Server and Client projects are set to start (Multiple Startup Projects).

    Press F5.

        The backend will automatically initialize the TasksTable and task-assets bucket.

        The frontend will be available at http://localhost:5173.

🧪 How to Verify

To verify that data is correctly persisting in the local cloud, use the awslocal CLI:
Bash

# Check DynamoDB items
awslocal dynamodb scan --table-name TasksTable

# Check S3 files
awslocal s3 ls s3://task-assets --recursive

📜 License

Distributed under the MIT License.
