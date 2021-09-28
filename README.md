# Yatter.Storage.Azure

Our BlobManager uses a ```TRequest``` / ```TResponse``` pattern, vis-a-vis:

```
var response = await blobManager.GetBlobAsync<BlobResponse, BlobRequest>(blobRequest);
```

## Quickstart

```
var connectionString = "{SET CONNECTION STRING HERE}";
var containerName = "{SET CONTAINER NAME HERE}";
var blobPath = "{SET BLOB NAME HERE}";

var blobManager = new Yatter.Storage.Azure.BlobManager();

var blobRequest = new Models.BlobRequest();
blobRequest.SetConnectionString(connectionString);
blobRequest.SetContainerName(containerName);
blobRequest.SetBlobPath(blobPath);

var response = await blobManager.GetBlobAsync<BlobResponse, BlobRequest>(blobRequest);

var isSuccess = response.IsSuccess;
var message = response.Message;
var blobContent = response.Content;
```

Where BlobRequest and BlobResponse are implementations of RequestBase and ResponseBase, respectively.
