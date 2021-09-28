using System;
using Azure.Storage.Blobs;

namespace Yatter.Storage.Azure
{
    public abstract class RequestBase
    {
        /// <summary>
        /// The connection string used by the internal BlobServiceClient
        /// </summary>
        public string ConnectionString { get; protected set; }
        /// <summary>
        /// The Container name to be used by the internal BlobServiceClient
        /// </summary>
        public string ContainerName { get; protected set; }
        /// <summary>
        /// The Blob Path to be used by the internal BlobServiceClient
        /// </summary>
        public string BlobPath { get; protected set; }
        /// <summary>
        /// When uploading, the content to upload. BlobContent can be string.Empty if an empty blob is intended.
        /// </summary>
        public string BlobContent { get; set; }
        /// <summary>
        /// Override to access the internal BlobClient
        /// </summary>
        /// <param name="client">Azure.Storage.Blobs.BlobClient</param>
        public virtual void AddBlobClient(BlobClient client) { }
        /// <summary>
        /// Override to access the internal BlobServiceClient
        /// </summary>
        /// <param name="client">Azure.Storage.Blobs.BlobServiceClient</param>
        public virtual void AddBlobServiceClient(BlobServiceClient client) { }
        /// <summary>
        /// Override to access the internal BlobContainerClient
        /// </summary>
        /// <param name="client">BlobContainerClient</param>
        public virtual void AddBlobContainerClient(BlobContainerClient client) { }
    }
}

