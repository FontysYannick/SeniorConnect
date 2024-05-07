namespace SeniorConnect.API.Models
{
    public class BaseListItem
    {
        public int BaselistItemId { get; set; }
        public int BaselistId { get; set; }
        public int Code { get; set; }
        public int Description { get; set; }

        public BaseListItem() { }
    }
}
