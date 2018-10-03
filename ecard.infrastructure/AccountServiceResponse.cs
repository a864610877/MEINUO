using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Ecard.Models;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Linq.Expressions;
using Moonlit.Reflection;
using Moonlit.Reflection.Emit;

namespace Ecard
{
    public interface IQueryRequest
    {
        string OrderBy { get; }

        int PageIndex { get; set; }

        int PageSize { get; set; }
    }

    public class BoundedAttribute : Attribute, IValueConverter, IBounded
    {
        Dictionary<object, FieldDescriptor> _value2Fields = new Dictionary<object, FieldDescriptor>();
        public BoundedAttribute(Type type)
        {
            if (AppDefaults.Container == null) return;
            while (type != null && type != typeof(object))
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (var fieldInfo in fields)
                {
                    _value2Fields.Add(fieldInfo.GetValue(null), new FieldDescriptor((I18NManager)AppDefaults.Container.Resolve(typeof(I18NManager), null), type, fieldInfo));
                }
                type = type.BaseType;
            }
        }

        public string ConvertTo(object value)
        {
            if (_value2Fields.ContainsKey(value))
                return _value2Fields[value].Name;
            return "";
        }

        public IEnumerable<IdNamePair> GetItems()
        {
            var dict = new List<IdNamePair>();
            foreach (var kv in _value2Fields.OrderBy(x => x.Value.Order))
            {
                dict.Add(new IdNamePair() { Name = kv.Value.Name, Key = (int)kv.Key });
            }
            return dict;
        }
    }

    public interface IBounded
    {
        IEnumerable<IdNamePair> GetItems();
    }

    public interface IValueConverter
    {
        string ConvertTo(object value);
    }
    public static class AppDefaults
    {
        public static IUnityContainer Container { get; set; }
    }
    public class ViewModelDescriptor
    {
        private readonly I18NManager _i18NManager;
        private readonly Type _type;
        private List<PropertyDescriptor> _properties = new List<PropertyDescriptor>();
        private List<MethodDescriptor> _methods = new List<MethodDescriptor>();
        private List<FieldDescriptor> _fields = new List<FieldDescriptor>();
        class UpdateFieldItems<T>
        {
            private static readonly List<string> Properties = new List<string>();
            public string[] Fields()
            {
                return Properties.ToArray();
            }
            static UpdateFieldItems()
            {
                var type = typeof(T);
                var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var query = from x in propertyInfos
                            where x.GetSetMethod() != null && (x.GetAttribute<HiddenAttribute>(false) == null || x.GetAttribute<ClearModelStateAttribute>(false) != null)
                            select x.Name;
                Properties.AddRange(query);
            }
        }
        static ViewModelDescriptor Empty = new ViewModelDescriptor();
        static Dictionary<Type, ViewModelDescriptor> _type2Properties = new Dictionary<Type, ViewModelDescriptor>();

        ViewModelDescriptor()
        {

        }
        private ViewModelDescriptor(I18NManager i18NManager, Type type)
        {
            _i18NManager = i18NManager;
            _type = type;
        }

        public IEnumerable<PropertyDescriptor> Properties
        {
            get { return _properties.AsReadOnly(); }
        }

        public IEnumerable<MethodDescriptor> Methods
        {
            get { return _methods; }
        }

        public string Description
        {
            get { return GetTypeValue(I18NCategories.Description, FullName); }
        }
        public string Name
        {
            get { return GetTypeValue(I18NCategories.Name, FullName); }
        }
        public string LongName
        {
            get { return GetTypeValue(I18NCategories.LongName, FullName); }
        }

        private string GetTypeValue(string category, string defaultValue)
        {
            return _i18NManager.Get(FullName, category, defaultValue);
        }

        protected string FullName
        {
            get { return _type.FullName; }
        }

        public static ViewModelDescriptor GetTypeDescriptor(object type)
        {
            if (!(type is Type))
            {
                if (type != null)
                {
                    type = type.GetType();
                }
            }
            if (type == null)
                return ViewModelDescriptor.Empty;

            Type typeItem = (Type)type;

            EnsureTypeDescriptor(typeItem);
            return _type2Properties[typeItem];
        }

        private static void EnsureTypeDescriptor(Type typeItem)
        {
            var i18NManager = (I18NManager)AppDefaults.Container.Resolve(typeof(I18NManager), null);
            if (!_type2Properties.ContainsKey(typeItem))
            {
                lock (_type2Properties)
                {
                    if (!_type2Properties.ContainsKey(typeItem))
                    {
                        var descriptor = new ViewModelDescriptor(i18NManager, typeItem);
                        foreach (var propInfo in typeItem.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            descriptor.Add(propInfo);
                        }
                        foreach (var methodInfo in typeItem.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                        {
                            descriptor.Add(methodInfo);
                        }
                        _type2Properties.Add(typeItem, descriptor);
                    }
                }
            }
        }

        private void Add(PropertyInfo propInfo)
        {
            var prop = new PropertyDescriptor(_i18NManager, _type, propInfo);
            _properties.Add(prop);
        }

        private void Add(MethodInfo methodInfo)
        {
            var prop = new MethodDescriptor(_i18NManager, _type, methodInfo);
            _methods.Add(prop);
        }
        private void Add(FieldInfo methodInfo)
        {
            var prop = new FieldDescriptor(_i18NManager, _type, methodInfo);
            _fields.Add(prop);
        }

        public static string[] UpdateFields<T>()
        {
            return new UpdateFieldItems<T>().Fields();
        }

        public PropertyDescriptor GetProperty(string propertyName)
        {
            return _properties.FirstOrDefault(x => string.Equals(propertyName, x.PropertyName, StringComparison.InvariantCultureIgnoreCase));
        }

        public MethodDescriptor GetMethod(string methodName)
        {
            return _methods.FirstOrDefault(x => string.Equals(methodName, x.MethodName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class ClearModelStateAttribute : Attribute
    {
    }

    public interface IShortNameEntity
    {
        string ShortName { get; }
        string ShortNameFormat { get; }
    }

    public interface IPermissionAttribute
    {
        bool Check(User user);
    }

    public interface IRole
    {
        bool HasPermissions(string permissions);
    }

    public class Allow : IPermissionAttribute
    {
        public bool Check(User user)
        {
            return true;
        }
    }
    public class MethodDescriptor : IShortNameEntity
    {
        private readonly I18NManager _i18NManager;
        private readonly Type _parent;
        private readonly MethodInfo _methodInfo;

        public MethodDescriptor(I18NManager i18NManager, Type parent, MethodInfo methodInfo)
        {
            _i18NManager = i18NManager;
            _parent = parent;
            _methodInfo = methodInfo;
            var p = methodInfo.GetCustomAttributes(false).OfType<IPermissionAttribute>();
            Permission = new CompositePermission(p, true);
        }


        public string ShortName
        {
            get { return GetMethodValue(I18NCategories.ShortName, _methodInfo.Name); }
        }

        public string ShortNameFormat
        {
            get { return GetMethodValue(I18NCategories.ShortNameFormat, "{0}"); }
        }

        public int Order
        {
            get { return Convert.ToInt32(GetMethodValue(I18NCategories.Order, "1")); }
        }

        public string MethodName
        {
            get { return _methodInfo.Name; }
        }

        public IPermissionAttribute Permission;

        public string Name
        {
            get { return GetMethodValue(I18NCategories.Name, _methodInfo.Name); }
        }

        public string ToolbarIcon
        {
            get { return GetMethodValue(I18NCategories.ToolbarIcon, ""); }
        }

        public string Description
        {
            get { return GetMethodValue(I18NCategories.Description, _methodInfo.Name); }
        }

        public string Confirm
        {
            get { return GetMethodValue(I18NCategories.Confirm, ""); }
        }


        string GetMethodValue(string category, string defaultValue)
        {
            return _i18NManager.Get(_parent.FullName, I18NCategories.Combine(_methodInfo.Name, category), defaultValue);
        }
    }

    public class CompositePermission : IPermissionAttribute
    {
        private readonly bool _defaultValue;
        private List<IPermissionAttribute> _permissionAttributes;
        public CompositePermission(IEnumerable<IPermissionAttribute> permissionAttributes, bool defaultValue)
        {
            _defaultValue = defaultValue;
            _permissionAttributes = new List<IPermissionAttribute>(permissionAttributes);
        }

        public bool Check(User user)
        {
            if (!_permissionAttributes.Any())
                return _defaultValue;
            if (_permissionAttributes.Any(x => !x.Check(user)))
                return false;
            return true;
        }
    }

    public class PropertyDescriptor : IShortNameEntity
    {
        private readonly I18NManager _i18NManager;
        private readonly Type _parent;
        private readonly PropertyInfo _propertyInfo;
        private readonly IValueConverter _valueConverter;
        private readonly IBounded _bounded;

        public bool IsReadOnly
        {
            get { return _propertyInfo.GetSetMethod() == null; }
        }
        public string Explain { get { return GetPropertyValue(I18NCategories.Explain, _propertyInfo.Name); } }
        public bool Show { get; private set; }
        public string Name { get { return GetPropertyValue(I18NCategories.Name, _propertyInfo.Name); } }
        public string TemplateHint { get; private set; }
        public string LongName { get { return GetPropertyValue(I18NCategories.LongName, _propertyInfo.Name); } }
        public string PropertyName { get { return _propertyInfo.Name; } }
        public string ValidatePropertyName { get; private set; }
        public string ShortNameFormat { get { return GetPropertyValue(I18NCategories.ShortNameFormat, "{0}"); } }
        public Type PropertyType { get; private set; }
        public int Order { get { return Convert.ToInt32(GetPropertyValue(I18NCategories.Order, "1")); } }
        public string Sort { get; private set; }
        public bool Sortable { get; private set; }
        public bool Hidden { get; private set; }

        public string ShortName
        {
            get { return GetPropertyValue(I18NCategories.ShortName, _propertyInfo.Name); }
        }

        public IValueConverter ValueConverter
        {
            get { return _valueConverter; }
        }

        public IBounded Bounded
        {
            get
            {
                if (_bounded == null)
                    throw new MoonlitException(string.Format("{0}.{1} has not bounded", _parent.FullName, _propertyInfo.Name));
                return _bounded;
            }
        }

        public IPermissionAttribute Permission { get; private set; }

        public PropertyDescriptor(I18NManager i18NManager, Type parent, PropertyInfo propertyInfo)
        {
            _i18NManager = i18NManager;
            _parent = parent;
            _propertyInfo = propertyInfo;

            var uihint = propertyInfo.GetAttribute<UIHintAttribute>(false);
            if (uihint != null) TemplateHint = uihint.UIHint;

            Show = propertyInfo.GetAttribute<NoRenderAttribute>(false) == null;
            Hidden = propertyInfo.GetAttribute<HiddenAttribute>(false) != null;
            PropertyType = propertyInfo.PropertyType;
            if (propertyInfo.GetGetMethod(false) != null)
                _accessor = new DynamicPropertyGetAccessor(propertyInfo);

            var sort = propertyInfo.GetAttribute<SortAttribute>(false);
            Sort = sort == null ? propertyInfo.Name : sort.Sort;
            _valueConverter = propertyInfo.GetCustomAttributes(false).OfType<IValueConverter>().FirstOrDefault() ?? new DefaultValueConverter();
            _bounded = propertyInfo.GetCustomAttributes(false).OfType<IBounded>().FirstOrDefault();


            var p = propertyInfo.GetCustomAttributes(false).OfType<IPermissionAttribute>();
            Permission = new CompositePermission(p, true);

            var validatePropertyName = propertyInfo.PropertyType.GetAttribute<ValidatePropertyNameAttribute>(false);
            this.ValidatePropertyName = propertyInfo.Name;
            if (validatePropertyName != null)
            {
                ValidatePropertyName += "." + validatePropertyName.ValidatePropertyName;
            }
        }
        string GetPropertyValue(string category, string defaultValue)
        {
            return _i18NManager.Get(_parent.FullName, I18NCategories.Combine(_propertyInfo.Name, category), defaultValue);
        }

        private readonly DynamicPropertyGetAccessor _accessor;
        public object GetValue(object item)
        {
            return _accessor.GetValue(item);
        }

        public object FormatedShortName(object value)
        {
            if (value == null) return value;
            object formatedShortName;
            if (TryFormatedShortName(value, out formatedShortName)) return formatedShortName;
            if (string.IsNullOrWhiteSpace(this.ShortNameFormat)) return value;
            return string.Format(ShortNameFormat, value);
        }

        private static bool TryFormatedShortName(object value, out object formatedShortName)
        {
            formatedShortName = null;
            if (value is bool)
            {
                if ((bool)value)
                {
                    formatedShortName = "ÊÇ";
                    return true;
                }
                if (!(bool)value)
                {
                    formatedShortName = "·ñ";
                    return true;
                }
            }
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ValidatePropertyNameAttribute : Attribute
    {
        public string ValidatePropertyName { get; set; }

        public ValidatePropertyNameAttribute(string validatePropertyName)
        {
            ValidatePropertyName = validatePropertyName;
        }
    }
    internal static class I18NCategories
    {
        public const string Order = "order";
        public const string Confirm = "confirm";
        public const string Operation = "operation";
        public const string ToolbarIcon = "toolbarIcon";
        public const string Name = "name";
        public const string LongName = "name";
        public const string Icon = "icon";
        public const string ShortName = "shortName";
        public const string ShortNameFormat = "shortNameFormat";
        public const string Description = "description";
        public const string Tooltip = "tooltip";
        public const string Explain = "explain";

        public static string Combine(params string[] names)
        {
            if (names.IsNullOrEmpty()) return "";
            return string.Join(".", names);
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SortAttribute : Attribute
    {
        public string Sort { get; private set; }
        public SortAttribute(string sort)
        {
            Sort = sort;
        }
    }
    public class DefaultValueConverter : IValueConverter
    {
        public string ConvertTo(object value)
        {
            return value == null ? "" : value.ToString();
        }
    }

    public class FieldDescriptor : IShortNameEntity
    {
        private readonly I18NManager _i18NManager;
        private readonly Type _parent;
        private readonly FieldInfo _FieldInfo;

        public bool Show { get; private set; }
        public string Name { get { return GetFieldValue(I18NCategories.Name, _FieldInfo.Name); } }
        public string TemplateHint { get; private set; }
        public string LongName { get { return GetFieldValue(I18NCategories.LongName, _FieldInfo.Name); } }
        public string FieldName { get { return _FieldInfo.Name; } }
        public string ShortNameFormat { get { return GetFieldValue(I18NCategories.ShortNameFormat, "{0}"); } }
        public Type FieldType { get; private set; }
        public int Order { get { return Convert.ToInt32(GetFieldValue(I18NCategories.Order, "1")); } }

        public string ShortName
        {
            get { return GetFieldValue(I18NCategories.ShortName, _FieldInfo.Name); }
        }

        public FieldDescriptor(I18NManager i18NManager, Type parent, FieldInfo FieldInfo)
        {
            _i18NManager = i18NManager;
            _parent = parent;
            _FieldInfo = FieldInfo;

            var uihint = FieldInfo.GetAttribute<UIHintAttribute>(false);
            if (uihint != null) TemplateHint = uihint.UIHint;

            Show = FieldInfo.GetAttribute<HiddenAttribute>(false) == null;
            FieldType = FieldInfo.FieldType;

        }
        string GetFieldValue(string category, string defaultValue)
        {
            return _i18NManager.Get(_parent.FullName, I18NCategories.Combine(_FieldInfo.Name, category), defaultValue);
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class HiddenAttribute : UIHintAttribute
    {
        public HiddenAttribute()
            : base("hiddenme")
        {

        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NoRenderAttribute : Attribute
    {
    }
    public class ModelHelper
    {
        public static string GetBoundText<T, TValue>(T item, Expression<Func<T, TValue>> func)
        {
            var memberExpr = func.Body as MemberExpression;
            ViewModelDescriptor t = ViewModelDescriptor.GetTypeDescriptor(typeof(T));
            PropertyDescriptor property = t.Properties.First(x => x.PropertyName == memberExpr.Member.Name);
            return property.ValueConverter.ConvertTo(property.GetValue(item));
        }
    }
}