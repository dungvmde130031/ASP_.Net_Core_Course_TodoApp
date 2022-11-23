using System;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TodoApp.WebMvc.HtmlHelpers
{
    public static class CustomHtmlHelper
    {
        public static HtmlString EmailCustom (this IHtmlHelper helper, string mailTo)
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", $"mailTo:{mailTo}");
            tagBuilder.InnerHtml.AppendHtml(mailTo);

            string result;

            using (var writer = new StringWriter())
            {
                tagBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                result = writer.ToString();
            }

            return new HtmlString(result);
        }
    }
}

