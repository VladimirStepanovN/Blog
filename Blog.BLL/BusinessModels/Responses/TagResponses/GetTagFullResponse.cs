using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.TagResponses
{
    public class GetTagFullResponse
    {
        public string Name { get; set; }
        public virtual Article[] Articles { get; set; }
    }
}
