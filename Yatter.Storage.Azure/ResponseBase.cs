using System;
using System.Net;
using Azure.Storage.Blobs.Models;

namespace Yatter.Storage.Azure
{
    public abstract class ResponseBase
    {
        public string Content { get; set; }
        public virtual void AddBlobDownloadResult(BlobDownloadResult blobDownloadResult) { }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}

