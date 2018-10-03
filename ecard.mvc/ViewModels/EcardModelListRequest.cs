using Ecard.Mvc.Models;

namespace Ecard.Mvc.ViewModels
{
    public class EcardModelListRequest<T> : EcardModelList<T>, IQueryRequest
    {
        public EcardModelListRequest()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        [Hidden]
        public string OrderBy { get; set; }

        [Hidden]
        public int PageIndex { get; set; }

        [Hidden]
        public int PageSize { get; set; }
        [Hidden]
        public string pageHtml { get; set; }
    }
}