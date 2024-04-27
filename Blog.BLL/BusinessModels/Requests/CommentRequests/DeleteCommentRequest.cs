﻿using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.CommentRequests
{
	public class DeleteCommentRequest
	{
		public int CommentId { get; set; }

		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Комментарий", Prompt = "Введите ваш комментарий")]

		public string Content { get; set; }
		public int ArticleId { get; set; }
		public int UserId { get; set; }
	}
}
