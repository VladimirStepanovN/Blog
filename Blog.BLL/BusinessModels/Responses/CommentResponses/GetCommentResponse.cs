﻿using Blog.BLL.BusinessModels.Requests.UsersRequests;
using Blog.DAL.Entities;

namespace Blog.BLL.BusinessModels.Responses.CommentResponses
{
    public class GetCommentResponse
    {
		public int CommentId { get; set; }
		public string Content { get; set; }
		public int UserId { get; set; }
		public int ArticleId { get; set; }
		public virtual GetUserRequest Author { get; set; }
	}
}
