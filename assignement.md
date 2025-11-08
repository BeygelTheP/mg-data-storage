Backend Developer Home Task - .NET API with Multi-Layered Data Storage

Objective
Develop a .NET Web API that provides a data retrieval service while using caching, file storage, and a database.
The service must follow a layered architecture with good design patterns and security mechanisms.
Task Requirements
1. API Development
a. Develop a .NET Web API (ASP.NET Core).
b. Implement an endpoint /data/{id} to retrieve data.
c.
2. Data Retrieval Logic
The data should be fetched in the following order:
1. Cache (Redis or in-memory cache, stored for 10 minutes).
2. File (saved as JSON, stored for 30 minutes, filename includes expiration timestamp).
3. Database (SQL or NoSQL, based on your choice).
• If data is found in the cache, return it immediately.
• If not found in the cache, check the file storage. If found, return it and store it in the cache.
• If not found in the file, check the database. If found, return it and store it in both the file and cache.

3. Design Patterns – highly important
Implement dependency injection properly.
Use the Factory Pattern

Submission Requirements
• Share the project as a GitHub repository.
• Include a README.md with setup instructions.
• Provide a working Postman collection.
• The code needs to be generic so any change in the logic will lead to minimal changes