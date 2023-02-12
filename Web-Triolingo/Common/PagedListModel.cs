using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_Triolingo.Common
{
    public class PagedListModel<T>
    {
        public IPagedList<T> List { get; set; }
    }
}
