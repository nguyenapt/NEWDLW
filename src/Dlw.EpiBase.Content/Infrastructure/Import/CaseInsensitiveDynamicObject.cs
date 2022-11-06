using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Dynamic object which handles property names case-insenstivie.
    /// Only allows getting, setting and removing members.
    /// Source: SitecoreBase
    /// </summary>
    public class CaseInsensitiveDynamicObject : DynamicObject, IDictionary<string, object>
    {
        public IDictionary<string, object> Dictionary { get; private set; }

        public CaseInsensitiveDynamicObject(IDictionary<string, object> dictionary)
        {
            Dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public CaseInsensitiveDynamicObject()
        {
            Dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return Dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            Dictionary.Remove(binder.Name);
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Dictionary.Keys;
        }


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dictionary).GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Dictionary.Add(item);
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return Dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            Dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Dictionary.Remove(item);
        }

        public int Count
        {
            get { return Dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return Dictionary.IsReadOnly; }
        }

        public bool ContainsKey(string key)
        {
            return Dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            Dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return Dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get { return Dictionary[key]; }
            set { Dictionary[key] = value; }
        }

        public ICollection<string> Keys
        {
            get { return Dictionary.Keys; }
        }

        public ICollection<object> Values
        {
            get { return Dictionary.Values; }
        }
    }
}