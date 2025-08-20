# cloudscribe.SimpleContent.Storage.EFCore.Common

This package provides the base Entity Framework Core implementation for cloudscribe SimpleContent commands and queries. It is intended to be used as a foundational library for database-specific EFCore storage providers within the cloudscribe.SimpleContent ecosystem.

## Features
- Common command and query logic for EFCore-based storage
- Designed for extension by MSSQL, MySQL, PostgreSQL, SQLite providers
- Used internally by other cloudscribe.SimpleContent.Storage.EFCore.* projects

## Usage
Reference this package from your database-specific provider or application project. Typically, you will not use this package directly, but through one of the database provider packages (e.g., MSSQL, MySQL, PostgreSQL, SQLite).

```
// Example usage (in your DbContext registration)
services.AddSimpleContentEFCoreStorage();
```

## License
This project is licensed under the Apache-2.0 License. See the LICENSE file for details.

## Repository
[cloudscribe.SimpleContent on GitHub](https://github.com/cloudscribe/cloudscribe.SimpleContent)
