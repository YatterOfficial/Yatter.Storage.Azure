using System;
namespace Yatter.Storage.Azure
{
    public class ExistsResponse
    {
        public string DataType { get; set; }
        public bool Exists { get; set; }

        public ExistsResponse()
        {
            DataType = GetType().ToString();
        }
    }
}
