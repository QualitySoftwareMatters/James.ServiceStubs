using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace James.ServiceStubs.Core.Rz
{
    public class MassiveExpando : DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item);
        }

        public int Count => _dictionary.Keys.Count;
        public bool IsReadOnly => _dictionary.IsReadOnly;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                result = _dictionary[binder.Name];
                return true;
            }
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!_dictionary.ContainsKey(binder.Name))
                _dictionary.Add(binder.Name, value);
            else
                _dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name) && this._dictionary[binder.Name] is Delegate)
            {
                Delegate del = this._dictionary[binder.Name] as Delegate;
                result = del.DynamicInvoke(args);
                return true;
            }
            return base.TryInvokeMember(binder, args, out result);
        }

        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary.Remove(binder.Name);
                return true;
            }
            return base.TryDeleteMember(binder);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public ICollection<string> Keys => _dictionary.Keys;

        public ICollection<object> Values => _dictionary.Values;
    }
}