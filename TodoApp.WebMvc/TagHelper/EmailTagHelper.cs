using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TodoApp.WebMvc.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string mailTo { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"mailTo:{mailTo}");
            output.Content.SetContent(mailTo);
        }
    }
}

