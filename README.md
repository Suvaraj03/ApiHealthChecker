# API Health Check CLI

## DESCRIPTION:

A lightweight single file CLI tool that helps developers to check API health status, response time, and availability directly from the command line.

Supports checking:

* Single API URL
* Multiple API URLs using configuration file

---

## SUPPORTING COMMANDS:

```
health-check <URL>

health-check -c <CONFIGFILEPATH>
```

---

## COMMANDS:

### Check Single API:

```
health-check <URL>
```

`<URL>` : API endpoint URL that needs to be checked.

Example:

```
health-check https://jsonplaceholder.typicode.com/users
```

Output:

```
✔ API - 200 (120ms)
```

---

### Check Multiple APIs:

```
health-check -c <CONFIGFILEPATH>
```

`<CONFIGFILE>` : JSON file containing multiple API details.

Example:

```
health-check -c "C:\Work\Projects\ApiHealthChecker\check.json"
```

---

## CONFIG FILE FORMAT:

Example `check.json`

```json
{
  "Services": [
    {
      "Name": "Auth API",
      "Url": "http://localhost:5231/api/User/authping"
    },
    {
      "Name": "Order API",
      "Url": "https://jsonplaceholder.typicode.com/users"
    }
  ]
}
```

---

## OUTPUT:

Example:

```
Checking Services...

✔ Auth API - 200 (45ms)

✔ Order API - 200 (120ms)


Summary

Total : 2
Healthy : 2
Failed : 0
```

---

## OPTIONS:

```
-c
--config
```

### -c / --config

Checks multiple APIs using the provided configuration file.

Example:

```
health-check -c check.json
```

---

## STATUS:

```
✔ Healthy

✘ Failed
```

### Healthy:

API is reachable and returned successful response.

### Failed:

API is unreachable or returned an error response.

---

## BUILD:

Create executable file:

```
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Generated file:

```
health-check.exe
```

---

## SAMPLE USAGE:

```
health-check https://jsonplaceholder.typicode.com/users

health-check http://localhost:5231/api/User/authping

health-check -c "C:\Work\Projects\ApiHealthChecker\check.json"
```

---

## TECHNOLOGY USED:

* C#
* .NET
* HttpClient
* Spectre.Console
