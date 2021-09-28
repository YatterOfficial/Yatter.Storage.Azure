using System;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Yatter.Storage.Azure.Exceptions;

namespace Yatter.Storage.Azure
{
    public interface IResponsiveBlobManager
    {
        Task<TResponse> GetBlobAsync<TResponse, TRequest>(TRequest request) where TRequest : RequestBase, new() where TResponse : ResponseBase, new();
        Task<TResponse> UploadBlobAsync<TResponse, TRequest>(TRequest request) where TRequest : RequestBase, new() where TResponse : ResponseBase, new();
        Task<TResponse> DeleteBlobAsync<TResponse, TRequest>(TRequest request) where TRequest : RequestBase, new() where TResponse : ResponseBase, new();
        Task<TResponse> ExistsBlobAsync<TResponse, TRequest>(TRequest request) where TRequest : RequestBase, new() where TResponse : ResponseBase, new();
    }

    public class ResponsiveBlobManager : IResponsiveBlobManager
    {
        private BlobServiceClient blobServiceClient;

        string GetRequestObjectFullName<T>(T instance) { return typeof(T).FullName; }

        public ResponsiveBlobManager()
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

                if(string.IsNullOrEmpty(containerName))
                {
                    throw new ContainerNameNotSpecifiedException(string.Format("TRequest object does not have a ContainerName specified in {0}", this.GetType()));
                }

                string blobPath = request.BlobPath;
                if(string.IsNullOrEmpty(blobPath))
                {
                    throw new BlobPathNotSpecifiedException(string.Format("TRequest object does not have a BlobPath specified in {0}", this.GetType()));
                }

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
                    throw new BlobDoesNotExistException(string.Format("Blob does not exist in {0}", this.GetType()));
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

        public async Task<TResponse> UploadBlobAsync<TResponse, TRequest>(TRequest request)
            where TResponse : ResponseBase, new()
            where TRequest : RequestBase, new()
        {
            TResponse response = default(TResponse);

            response = new TResponse();

            response.IsSuccess = false;

            try // If need be, we throw exceptions that we can catch and thus assign to TResponse: IsSuccess = false, as well as a Message
            {
                if (request == null) throw new NullRequestObjectException(string.Format("TRequest object is null in {0}", this.GetType()));

                if (string.IsNullOrEmpty(request.ConnectionString))
                {
                    throw new MissingConnectionStringException(string.Format("TRequest object does not have a ConnectionString in {0}", this.GetType()));
                }

                blobServiceClient = new BlobServiceClient(request.ConnectionString);
                string containerName = request.ContainerName;

                if (string.IsNullOrEmpty(containerName))
                {
                    throw new ContainerNameNotSpecifiedException(string.Format("TRequest object does not have a ContainerName specified in {0}", this.GetType()));
                }

                string blobPath = request.BlobPath;
                if (string.IsNullOrEmpty(blobPath))
                {
                    throw new BlobPathNotSpecifiedException(string.Format("TRequest object does not have a BlobPath specified in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobServiceClient") != null)
                {
                    request.AddBlobServiceClient(blobServiceClient);
                }

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var containerExists = await containerClient.ExistsAsync();

                if (!containerExists)
                {
                    throw new ContainerDoesNotExistException(string.Format("Container does not exist in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobContainerClient") != null)
                {
                    request.AddBlobContainerClient(containerClient);
                }

                var blobClient = containerClient.GetBlobClient(request.BlobPath);

                var blobExists = await blobClient.ExistsAsync();

                if (blobExists)
                {
                    throw new BlobExistsException(string.Format("Blob exists in {0}, you must delete it before uploading a new one with the same BlobPath.", this.GetType()));
                }
                else
                {
                    try
                    {
                        var uploadResponse = await blobClient.UploadAsync(new BinaryData(request.BlobContent));

                        response.IsSuccess = true;
                    }
                    catch(Exception ex)
                    {
                        throw new BlobUploadException(string.Format("Blob upload error in {0} with Message: {1}", this.GetType(), ex.Message));
                    }
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<TResponse> DeleteBlobAsync<TResponse, TRequest>(TRequest request)
            where TResponse : ResponseBase, new()
            where TRequest : RequestBase, new()
        {
            TResponse response = default(TResponse);

            response = new TResponse();

            response.IsSuccess = false;

            try // If need be, we throw exceptions that we can catch and thus assign to TResponse: IsSuccess = false, as well as a Message
            {
                if (request == null) throw new NullRequestObjectException(string.Format("TRequest object is null in {0}", this.GetType()));

                if (string.IsNullOrEmpty(request.ConnectionString))
                {
                    throw new MissingConnectionStringException(string.Format("TRequest object does not have a ConnectionString in {0}", this.GetType()));
                }

                blobServiceClient = new BlobServiceClient(request.ConnectionString);

                string containerName = request.ContainerName;

                if (string.IsNullOrEmpty(containerName))
                {
                    throw new ContainerNameNotSpecifiedException(string.Format("TRequest object does not have a ContainerName specified in {0}", this.GetType()));
                }

                string blobPath = request.BlobPath;
                if (string.IsNullOrEmpty(blobPath))
                {
                    throw new BlobPathNotSpecifiedException(string.Format("TRequest object does not have a BlobPath specified in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobServiceClient") != null)
                {
                    request.AddBlobServiceClient(blobServiceClient);
                }

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var containerExists = await containerClient.ExistsAsync();

                if (!containerExists)
                {
                    throw new ContainerDoesNotExistException(string.Format("Container does not exist in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobContainerClient") != null)
                {
                    request.AddBlobContainerClient(containerClient);
                }

                var blobClient = containerClient.GetBlobClient(request.BlobPath);

                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists)
                {
                    throw new BlobDoesNotExistException(string.Format("Blob does not exists in {0}, you cannot delete a blob that does not exist.", this.GetType()));
                }
                else
                {
                    try
                    {
                        var uploadResponse = await blobClient.DeleteAsync();

                        response.IsSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        throw new BlobUploadException(string.Format("Blob delete error in {0} with Message: {1}", this.GetType(), ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<TResponse> ExistsBlobAsync<TResponse, TRequest>(TRequest request)
            where TResponse : ResponseBase, new()
            where TRequest : RequestBase, new()
        {
            TResponse response = default(TResponse);

            response = new TResponse();

            response.IsSuccess = false;

            try // If need be, we throw exceptions that we can catch and thus assign to TResponse: IsSuccess = false, as well as a Message
            {
                if (request == null) throw new NullRequestObjectException(string.Format("TRequest object is null in {0}", this.GetType()));

                if (string.IsNullOrEmpty(request.ConnectionString))
                {
                    throw new MissingConnectionStringException(string.Format("TRequest object does not have a ConnectionString in {0}", this.GetType()));
                }

                blobServiceClient = new BlobServiceClient(request.ConnectionString);

                string containerName = request.ContainerName;

                if (string.IsNullOrEmpty(containerName))
                {
                    throw new ContainerNameNotSpecifiedException(string.Format("TRequest object does not have a ContainerName specified in {0}", this.GetType()));
                }

                string blobPath = request.BlobPath;
                if (string.IsNullOrEmpty(blobPath))
                {
                    throw new BlobPathNotSpecifiedException(string.Format("TRequest object does not have a BlobPath specified in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobServiceClient") != null)
                {
                    request.AddBlobServiceClient(blobServiceClient);
                }

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                var containerExists = await containerClient.ExistsAsync();

                if (!containerExists)
                {
                    throw new ContainerDoesNotExistException(string.Format("Container does not exist in {0}", this.GetType()));
                }

                if (typeof(TRequest).GetTypeInfo().GetDeclaredMethod("AddBlobContainerClient") != null)
                {
                    request.AddBlobContainerClient(containerClient);
                }

                var blobClient = containerClient.GetBlobClient(request.BlobPath);

                var blobExists = await blobClient.ExistsAsync();

                if (blobExists)
                {
                    response.IsSuccess = true;
                    response.Message = "Exists";
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Does Not Exist";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;

        }
    }
}

