﻿using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.ArticleResponses
{
    public class GetArticleResponse
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public virtual User Author { get; set; }
    }
}
