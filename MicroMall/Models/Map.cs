using System;
using System.Collections;

namespace Common
{
    public class Map : IDictionary
    {
        private Hashtable ht;

        public Map()
        {
            this.ht = new Hashtable();
        }

        public void Add(object key, object value)
        {
            ht.Add(key, value);
        }

        public void Clear()
        {
            ht.Clear();
        }

        public bool Contains(object key)
        {
            return ht.Contains(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return ht.GetEnumerator();
        }

        public bool IsFixedSize
        {
            get { return ht.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return ht.IsReadOnly; }
        }

        public ICollection Keys
        {
            get { return ht.Keys; }
        }

        public void Remove(object key)
        {
            ht.Remove(key);
        }

        public ICollection Values
        {
            get { return ht.Values; }
        }

        public object this[object key]
        {
            get
            {
                return ht[key];
            }
            set
            {
                ht[key] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            ht.CopyTo(array, index);
        }

        public int Count
        {
            get { return ht.Count; }
        }

        public bool IsSynchronized
        {
            get { return ht.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ht.SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int getInt(string key)
        {
            int val;
            int.TryParse(ht[key].ToString(), out val);
            return val;
        }

        public string getString(string key)
        {
            return ht[key].ToString();
        }
    }
}
