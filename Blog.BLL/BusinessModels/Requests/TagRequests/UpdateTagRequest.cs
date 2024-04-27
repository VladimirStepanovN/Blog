﻿using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.BusinessModels.Requests.TagRequests
{
    public class UpdateTagRequest
    {
        public int TagId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Название")]
        public string Name { get; set; }
    }
}
