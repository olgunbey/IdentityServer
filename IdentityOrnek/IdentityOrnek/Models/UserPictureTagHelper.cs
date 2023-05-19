﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityOrnek.Models
{
    public class UserPictureTagHelper:TagHelper
    {
        public string? PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (!string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("class", "rounded w-100");
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureUrl}"); //attribute ekleyip valuesini veriyoruz
            }
        }
    }
}
