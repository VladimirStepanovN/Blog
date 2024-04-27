namespace Blog.BLL.BusinessModels.Requests.TagRequests
{
    public class TagRequest
    {
        public int TagId { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
