using System;
namespace IoT_Cloud.Models
{
    public class Container
    {
        public Container()
        {
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
