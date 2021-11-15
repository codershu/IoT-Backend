using System;
namespace IoT_Cloud.Models
{
    public class BlobFile
    {
        public BlobFile()
        {
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public string Container { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
