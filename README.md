# Yatter.Storage.Azure

Our ResponsiveBlobManager uses a ```TRequest``` / ```TResponse``` pattern, vis-a-vis:

```
var response = await blobManager.GetBlobAsync<BlobResponse, BlobRequest>(blobRequest);
```

Where BlobRequest and BlobResponse are implementations of [RequestBase](https://github.com/HarrisonOfTheNorth/Yatter.Storage.Azure/blob/main/Yatter.Storage.Azure/RequestBase.cs) and [ResponseBase](https://github.com/HarrisonOfTheNorth/Yatter.Storage.Azure/blob/main/Yatter.Storage.Azure/ResponseBase.cs), respectively.

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

A minimal BlobResponse example from above is:

```
    public class BlobResponse : ResponseBase
    {
        public BlobResponse()
        {
        }
    }
```

A minimal BlobRequest example from above is:

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

