namespace SeniorConnect.API.Models
{
    public class BaseListItem
    {
        public int BaselistItemId { get; set; }
        public int BaselistId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public BaseListItem() { }
    }
}
