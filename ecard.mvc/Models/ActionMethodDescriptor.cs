namespace Ecard.Mvc.Models
{
    public class ActionMethodDescriptor
    {
        private readonly string _action;
        private readonly string _controller;
        private readonly object _routes; 

        public ActionMethodDescriptor(string action, string controller = null, object routes = null )
        { 
            _action = action;
            _controller = controller;
            _routes = routes;
        } 

        public string Action
        {
            get { return _action; }
        }

        public string Controller
        {
            get { return _controller; }
        }

        public object Routes
        {
            get { return _routes; }
        }

        public bool? IsPost { get; set; }
    }
}