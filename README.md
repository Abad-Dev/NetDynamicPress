# NetDynamicPress
A backend for DynamicPress

There is no appsettings.json because the connection String is there as in the [Microsoft Docs](https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)

You have to add the file manually with the next settings:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Presupuestos": "Server=;Database=;Uid=;Pwd=;"
  }
}
```