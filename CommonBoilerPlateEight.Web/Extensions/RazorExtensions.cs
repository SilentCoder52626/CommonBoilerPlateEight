using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Mvc.Core;
namespace CommonBoilerPlateEight.Web.Extensions;

public static class RazorExtensions
{
    public static IHtmlContent PagedListPagerBT4(this IHtmlHelper helper, IPagedList myList, Func<int, string> generatePageUrl)
    {
        return helper.PagedListPager(myList, generatePageUrl, new PagedListRenderOptions
        {
            UlElementClasses = new string[] { "pagination", "justify-content-center" },
            LiElementClasses = new string[] { "page-item", "paginate_button" },
            PageClasses = new string[] { "page-link" },
            DisplayLinkToFirstPage = PagedListDisplayMode.Always,
            DisplayLinkToLastPage = PagedListDisplayMode.Always,
        });
    }
}