# Yatter.Storage.Azure

<a href="https://www.nuget.org/packages/Yatter.Storage.Azure/" target="_blank" rel="noreferrer noopener"><img alt="Nuget" src="https://img.shields.io/nuget/v/Yatter.Storage.Azure?color=blue&style=for-the-badge"></a>

![GitHub](https://img.shields.io/github/license/yatterofficial/Yatter.Storage.Azure?style=for-the-badge)

[![Yatter.Storage.Azure on fuget.org](https://www.fuget.org/packages/Yatter.Storage.Azure/badge.svg)](https://www.fuget.org/packages/Yatter.Storage.Azure)

Our **ResponsiveBlobManager** uses a ```TRequest``` / ```TResponse``` pattern, vis-a-vis:

```
var response = await responsiveBlobManager.GetBlobAsync<BlobResponse, BlobRequest>(blobRequest);
```

Where BlobRequest and BlobResponse are implementations of [RequestBase](https://github.com/YatterOfficial/Yatter.Storage.Azure/blob/main/Yatter.Storage.Azure/RequestBase.cs) and [ResponseBase](https://github.com/YatterOfficial/Yatter.Storage.Azure/blob/main/Yatter.Storage.Azure/ResponseBase.cs), respectively.

Both base objects expose the underlying objects so that any advanced work can be done in the base overrides.

## Quickstart

```
var connectionString = "{SET CONNECTION STRING HERE}";
var containerName = "{SET CONTAINER NAME HERE}";
var blobPath = "{SET BLOB NAME HERE}";

var responsiveBlobManager = new Yatter.Storage.Azure.ResponsiveBlobManager();

var blobRequest = new Models.BlobRequest();
blobRequest.SetConnectionString(connectionString);
blobRequest.SetContainerName(containerName);
blobRequest.SetBlobPath(blobPath);

var response = await responsiveBlobManager.GetBlobAsync<BlobResponse, BlobRequest>(blobRequest);

var isSuccess = response.IsSuccess;
var message = response.Message;
var blobContent = response.Content;
```

Methods:

- GetBlobAsync
- UploadBlobAsync
- ExistsBlobAsync
- DeleteBlobAsync

Limitations:

- None, the Azure objects are exposed in the TRequest and TResponse base classes.

Minimal Example TResponse class

```
    public sealed class BlobResponse : ResponseBase
    {
        public BlobResponse()
        {
        }
    }
```

Minimal Example TRequest class

```
    public sealed class BlobRequest : RequestBase
    {
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SetContainerName(string name)
        {
            ContainerName = name;
        }

        public void SetBlobPath(string path)
        {
            BlobPath = path;
        }
    }
```

Advanced Example of TResponse Access to Underlying Azure objects

```
    public class BlobResponse : ResponseBase
    {
        public BlobResponse()
        {
        }

        public override void AddBlobDownloadResult(BlobDownloadResult blobDownloadResult)
        {
            // Do something with blobDownloadResult here!
            base.AddBlobDownloadResult(blobDownloadResult);
        }
    }
```

Advanced Example of TRequest Access to Underlying Azure objects

```
    public sealed class BlobRequest : RequestBase
    {
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SetContainerName(string name)
        {
            ContainerName = name;
        }

        public void SetBlobPath(string path)
        {
            BlobPath = path;
        }
        
        public override void AddBlobClient(BlobClient client)
        {
            // Do something with client here!
            base.AddBlobClient(client);
        }

        public override void AddBlobContainerClient(BlobContainerClient client)
        {
            // Do something with client here!
            base.AddBlobContainerClient(client);
        }

        public override void AddBlobServiceClient(BlobServiceClient client)
        {
            // Do something with client here!
            base.AddBlobServiceClient(client);
        }
    }
```



