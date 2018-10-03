namespace Ecard.Mvc.Models
{
    public class ViewObjectModelBase<T> : ViewModelBase
    {
        private T _innerObject;
        protected T InnerObject
        {
            get { return _innerObject; }
            set
            {
                _innerObject = value;
            }
        }
    }
}