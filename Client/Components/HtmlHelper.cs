using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Components;

namespace Profile.Components
{
    public class HtmlHelper : ComponentBase
    {
       public static MarkupString RenderMultiline(string textWithLineBreaks)
        {
            var encodedLines = (textWithLineBreaks ?? string.Empty)
                .Split(new char[] { '\r', '\n'})
                .Select(line => HttpUtility.HtmlEncode(line))
                .ToArray();
            return (MarkupString)string.Join("<br />", encodedLines);
        }
    }
}


