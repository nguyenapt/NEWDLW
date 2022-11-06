using System;
using System.Collections;
using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Import
{
    /// <summary>
    /// Data wrapper for any kind of data to import.
    /// Source: SitecoreBase
    /// </summary>
    public class DynamicData : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly IDictionary<string, object> _dictionary;

        /// <summary>
        /// Dynamic object holding the data.
        /// </summary>
        public dynamic Data { get; private set; }

        /// <summary>
        /// Diagnostic source information for this data, e.g. a row number.
        /// </summary>
        public object SourceInfo { get; private set; }

        public DynamicData(dynamic data = null, object sourceInfo = null)
        {
            Data = data ?? new CaseInsensitiveDynamicObject();
            _dictionary = (IDictionary<string, object>)Data;
            SourceInfo = sourceInfo;
        }

        public bool TryGetValue(string name, out object value)
        {
            if (name.Equals("SourceInfo", StringComparison.OrdinalIgnoreCase))
            {
                value = SourceInfo;
                return true;
            }
            return _dictionary.TryGetValue(name, out value);
        }

        public object this[string name]
        {
            get
            {
                object value;
                return TryGetValue(name, out value) ? value : null;
            }
            set { _dictionary[name] = value; }
        }

        public bool HasValue(string name)
        {
            return _dictionary.ContainsKey(name);
        }

        public ICollection<object> Values()
        {
            return _dictionary.Values;
        }

        public ICollection<string> FieldNames()
        {
            return _dictionary.Keys;
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");

            _dictionary.Remove(name);
        }

        public void Add(string name, object value)
        {
            _dictionary.Add(name, value);
        }

        public override string ToString()
        {
            return string.Join(",", ((IDictionary<string, object>)Data).Values);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}