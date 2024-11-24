using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CommonBoilerPlateEight.Web.Helper
{
    [HtmlTargetElement(Attributes = "active-when-controllers, active-when-actions")]
    public class ActiveTag : TagHelper
    {
        public string? ActiveWhenControllers { get; set; }

        public string? ActiveWhenActions { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContextData { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ActiveWhenControllers) || string.IsNullOrEmpty(ActiveWhenActions))
                return;

            var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString();
            var currentAction = ViewContextData?.RouteData.Values["action"]?.ToString();

            var controllers = ActiveWhenControllers.Split(',').Select(c => c.Trim());
            var actions = ActiveWhenActions.Split(',').Select(a => a.Trim());

            if (controllers.Any(c => string.Equals(c, currentController, StringComparison.OrdinalIgnoreCase)) &&
                actions.Any(a => string.Equals(a, currentAction, StringComparison.OrdinalIgnoreCase)))
            {
                if (output.Attributes.ContainsName("class"))
                    output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value} active");
                else
                    output.Attributes.SetAttribute("class", "active");
            }
        }
    }

}
