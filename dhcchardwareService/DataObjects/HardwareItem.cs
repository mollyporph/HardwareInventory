using Microsoft.WindowsAzure.Mobile.Service;

namespace dhcchardwareService.DataObjects
{
    public class HardwareItem 
    {
        
        public string Id { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string Category { get; set; }
    }

    public class HardwareItemDTO: EntityData
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }
}