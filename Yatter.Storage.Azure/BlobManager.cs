using System;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Yatter.Storage.Azure
{
    public class BlobManager
    {
        private BlobServiceClient blobServiceClient;

        string GetRequestObjectFullName<T>(T instance) { return typeof(T).FullName; }

        public BlobManager()
        {
        }

        public async Task<TResponse> GetBlobAsync<TResponse, TRequest>(TRequest request)
    where TRequest : RequestBase, new() where TResponse : ResponseBase, new()
        {
            TResponse response = default(TResponse);

            response = new TResponse();

            try // If need be, we throw exceptions that we can catch and thus assign to TResponse: IsSuccess = false, as well as a Message
            {
                if (request == null) throw new NullRequestObjectException(string.Format("TRequest object is null in {0}", this.GetType()));

                if (string.IsNullOrEmpty(request.ConnectionString))
                {
                    throw new MissingConnectionStringException(string.Format("TRequest object does not have a ConnectionString in {0}", this.GetType()));
                }

                blobServiceClient = new BlobServiceClient(request.ConnectionString);

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobServiceClient") != null)
                {
                    request.AddBlobServiceClient(blobServiceClient);
                }

                string containerName = request.ContainerName;

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var containerExists = await containerClient.ExistsAsync();

                if(!containerExists)
                {
                    throw new ContainerDoesNotExistException(string.Format("Container does not exist in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobContainerClient") != null)
                {
                    request.AddBlobContainerClient(containerClient);
                }

                var blobClient = containerClient.GetBlobClient(request.BlobPath);

                var blobExists = await blobClient.ExistsAsync();

                if(!blobExists)
                {
                    throw new BlobDoesNotExistException(string.Format("Container does not exist in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobClient") != null)
                {
                    request.AddBlobClient(blobClient);
                }
               
                BlobDownloadResult blobDownloadResult = await blobClient.DownloadContentAsync();
                response.AddBlobDownloadResult(blobDownloadResult);

                var blobContent = blobDownloadResult.Content;

                response.Content = blobContent.ToString();
                response.IsSuccess = true;

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = String.Format(ex.Message);
            }
                       
            return response;
        }
    }
}

