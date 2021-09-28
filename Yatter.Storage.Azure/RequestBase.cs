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
        /// Override to access the internal BlobContainerClient
        /// </summary>
        public virtual void AddBlobClient(BlobClient client) { }

        public virtual void AddBlobServiceClient(BlobServiceClient client) { }

        public virtual void AddBlobContainerClient(BlobContainerClient client) { }


    }
}

