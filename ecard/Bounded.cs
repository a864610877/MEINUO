using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ecard.Models;
using Moonlit;

namespace Ecard
{
    [ValidatePropertyName("Key")]
    public class Bounded
    {
        public static implicit operator int(Bounded bounded)
        {
            return bounded.Key;
        }

        [Required(ErrorMessage = "请输入必填字段")]
        public int Key { get; set; }
        public List<IdNamePair> Items { get; set; }

        public string Callback { get; set; }

        public bool IsReadOnly { get; set; }

        public Bounded(int defaultValue, IEnumerable<IdNamePair> items = null)
        {
            Items = new List<IdNamePair>(items ?? new List<IdNamePair>());
            Key = defaultValue;
        }

        public static Bounded CreateEmpty(string propertyName, int defaultValue)
        {
            var bounded = new Bounded(defaultValue);
            return bounded;
        }

        public static Bounded Create<T>(string propertyName, int defaultValue)
        {
            var typeDescriptor = ViewModelDescriptor.GetTypeDescriptor(typeof(T));
            var property = typeDescriptor.GetProperty(propertyName);
            var bounded = new Bounded(defaultValue, property.Bounded.GetItems());
            return bounded;
        }

        public void BindAll()
        {
            Items.Insert(0, new IdNamePair() { Key = Globals.All, Name = "全部" });
        }
        public void Bind(IEnumerable<IdNamePair> items, bool hasAll = false, string allText = "全部")
        {
            if (items != null)
                Items = new List<IdNamePair>(items);
            Items = Items ?? new List<IdNamePair>();
            if (hasAll)
                Items.Insert(0, new IdNamePair() { Key = Globals.All, Name = allText });
        }
        private List<int> _emptyValues = new List<int>();
        public void AddEmptyValue(int value)
        {
            _emptyValues.Add(value);
        }

        public string GetKey(int value)
        {
            if (_emptyValues.Contains(value))
                return "";
            return value.ToString();
        }
    }
}