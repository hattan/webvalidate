# webvalidate - A web request validation testing tool

![License](https://img.shields.io/badge/license-MIT-green.svg)

Run the sample Validation Test against ```microsoft.com```

```bash

# run the tests from Docker
docker run -it --rm retaildevcrew/webvalidate --host https://www.microsoft.com --files msft.json

```

Optionally you can run the test a sample [IMDB Movie API](https://github.com/retaildevcrews/helium) hosted at `https://froyo.azurewebsites.net` by using:

```bash
docker run -it --rm retaildevcrew/webvalidate --host https://froyo.azurewebsites.net --files baseline.json
```

## Test Validation Files

Validation files are located in the src/TestFiles directory and are json files that control the validation tests.

- HTTP redirects are not followed
- The test files must parse and validate or no tests will be executed
  - only display the first 10 validation errors
- All string comparisons are case sensitive

- Path (required)
  - path to resource (do not include http or dns name)
  - valid: must begin with /
- Verb
  - default: GET
  - valid: HTTP verbs
- Validation (optional)
  - if not specified in test file, no validation checks will run
  - StatusCode
    - http status code
    - default: 200
    - valid: 100-599
  - ContentType
    - MIME type
    - default: application/json
    - valid: null or non-empty string
  - Length
    - length of content
      - cannot be combined with MinLength or MaxLength
    - valid: null or >= 0
  - MinLength
    - minimum content length
    - valid: null or >= 0
  - MaxLength
    - maximum content length
    - valid: null or > MinLength
    - valid: if MinLength == null >= 0
  - MaxMilliSeconds
    - maximum duration in ms
    - valid: null or > 0
  - ExactMatch
    - Body exactly matches value
    - valid: non-empty string
  - Contains[string]
    - case sensitive string "contains"
    - string
      - valid: non-empty string
  - JsonArray
    - valid: parses into json array
    - Count
      - exact number of items
      - Valid: >= 0
      - valid: cannot be combined with MinCout or MaxCount
    - MinCount
      - minimum number of items
      - valid: >= 0
        - can be combined with MaxCount
    - MaxCount
      - maximum number of items
      - valid: > MinCount
        - can be comined with MinCount
    - ForEach[JsonObject]
      - checks each json object in the array
    - Objects[]
      - validates object[index]
      - Index
        - Index of object to check
        - valid: >= 0
      - JsonObject
        - JsonObject definition to check
        - valid: JsonObject rules
  - JsonObject[]
    - valid: parses into json object
    - Field
      - name of field
      - valid: non-empty string
    - Value (optional)
      - if not specified, verifies the Field exists in the json document
      - valid: null, number or string
- PerfTarget (optional)
  - Category
    - used to group requests into categories for reporting
    - see [helium](https://github.com/retaildevcrews/helium) examples below
    - valid: non-empty string
  - Targets[3]
    - maximum quartile value in ascending order

## Sample ```microsoft.com``` validation tests

The msft.json file contains sample validation tests that will will successfully run against the ```https://www.microsoft.com/``` endpoint (assuming content hasn't changed)

- note that http status codes are not specified when 200 is expected
- note that ContentType is not specified when the default of application/json is expected

### Redirect from home page

- Note that redirects are not followed

```json

{
  "Url":"/",
  "Validation":
  {
    "Code":302
  }
}

```

### home page (en-us)

```json

{
  "Url":"/en-us",
  "Validation":
  {"
    ContentType":"text/html",
    "Contains":
    [
      { "Value":"<title>Microsoft - Official Home Page</title>" },
      { "Value":"<head data-info=\"{" }
    ]
  }
}

```

### favicon

```json
{
  "Url": "/favicon.ico",
  "Validation":
  {
    "ContentType":"image/x-icon"
  }
}
```

### robots.txt

```json
{
  "Url": "/robots.txt",
  "Validation":
  {
    "ContentType": "text/plain",
    "MinLength": 200,
    "Contains":
    [
      { "Value": "User-agent: *" },
      { "Value": "Disallow: /en-us/windows/si/matrix.html"}
    ]
  }
}
```

## Sample [helium](https://github.com/retaildevcrews/helium) tests

```json

{
    "Url":"/version",
    "Validation":
    {
        "Code":200,
        "ContentType":"text/plain"
    }
}

{
    "Url":"/healthz",
    "PerfTarget":
    {
        "Category":"healthz"
    },
    "Validation":
    {
        "ContentType":"text/plain",
        "ExactMatch":
        {
            "Value":"pass"
        }
    }
}

{
    "Url":"/healthz/ietf",
    "PerfTarget":
    {
        "Category":"healthz"
    },
    "Validation":
    {
        "ContentType":"application/health+json",
        "JsonObject":
        [
          {
            "Field":"status",
            "Value":"pass"
          }
        ]
    }
}

{
    "Url":"/api/actors",
    "PerfTarget":
    {
        "Category":"PagedRead"
    },
    "Validation":
    {
        "JsonArray":
        {
            "Count":100
        }
    }
}

```

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit [Microsoft Contributor License Agreement](https://cla.opensource.microsoft.com).

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
