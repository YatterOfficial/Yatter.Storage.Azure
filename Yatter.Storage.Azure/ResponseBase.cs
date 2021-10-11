using System;
using System.Net;
using Azure.Storage.Blobs.Models;
using Yatter.Invigoration;

namespace Yatter.Storage.Azure
{
    public abstract class ResponseBase : IOutcome
    {
        /// <summary>
        /// The content of the blob that has been retrieved
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// An override to access the underlying Azure.Storage.Blob.Models.BlobDownloadResult that was returned with the content
        /// </summary>
        /// <param name="blobDownloadResult">Azure.Storage.Blob.Models.BlobDownloadResult</param>
        public virtual void AddBlobDownloadResult(BlobDownloadResult blobDownloadResult) { }
        /// <summary>
        /// Is the Operation successful?
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// In the case that the operation was not succesful, the accompanying message explaining why.
        /// </summary>
        public string Message { get; set; }
    }
}

