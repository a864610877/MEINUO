namespace Ecard.Mvc.Models
{
    public class LinkObject
    {
        private readonly string _text;
        private readonly object _routeObject;
        private readonly string _controller;
        private readonly string _action;
        private readonly object _key;

        public string Text
        {
            get { return _text; }
        }
        public object Key
        {
            get { return _key; }
        }
        public object RouteObject
        {
            get { return _routeObject; }
        }

        public string Controller
        {
            get { return _controller; }
        }

        public string Action
        {
            get { return _action; }
        }
        public override string ToString()
        {
            return this.Text;
        }

        private string _target = "_self";
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public LinkObject(string text, int key, string controller, string action)
        {
            _text = text;
            _routeObject = new { id = key };
            _controller = controller;
            _action = action;
            _key = key;
        }

        public LinkObject(string text, string controller, string action, object routeObject)
        {
            _text = text;
            _routeObject = routeObject;
            _controller = controller;
            _action = action;
        }
        public LinkObject(string text, object key, string controller, string action)
        {
            _text = text;
            _key = key;
            _controller = controller;
            _action = action;
        }
    }
}