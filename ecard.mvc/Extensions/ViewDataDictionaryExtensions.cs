using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc
{ 
    public static class ViewDataDictionaryExtensions
    {
        public static MvcHtmlString ToHtmlAttributes(this ViewDataDictionary viewData)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in viewData.Keys)
            {
                builder.AppendFormat(" {0}='{1}' ", item, viewData[item] ?? "");
            }
            return MvcHtmlString.Create(builder.ToString());
        }
        public static void SetMessage(this ViewDataDictionary dict, string message)
        {
            dict["message"] = message;
        }
        public static string GetMessage(this ViewDataDictionary dict)
        {
            if (dict.ContainsKey("message"))
            {
                var s = (string)dict["message"];
                dict.Remove("message");
                return s;
            }
            return "";
        }
        public static IDictionary<string, object> GetViewState(this ViewDataDictionary dict)
        {
            if (!dict.ContainsKey("__viewState"))
                dict.Add("__viewState", new Dictionary<string, object>());
            return (IDictionary<string, object>)dict["__viewState"];
        }
        public static void SetViewState(this ViewDataDictionary dict, string key, object value)
        {
            var viewState = dict.GetViewState();
            viewState[key] = value;
        }
        public static IDictionary<string, object> GetModelStates(this   ViewDataDictionary dict)
        {
            IDictionary<string, object> di = new Dictionary<string, object>();
            foreach (var key in dict.ModelState.Keys)
            {
                var state = dict.ModelState[key];
                if (state.Errors.Count == 0)
                {
                    di.Add(key, state.Value.AttemptedValue);
                }
            }
            return di;
        }
    }
}