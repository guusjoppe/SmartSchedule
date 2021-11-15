using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Boilerplate.Logging.Utility
{
    internal class TypeNameTrie<TValue> : IEnumerable<TValue>
    {
        private readonly IDictionary<string, TypeNameTrie<TValue>> _children = new Dictionary<string, TypeNameTrie<TValue>>();
        private TValue _value = default!;
        private bool _hasValue;

        public TypeNameTrie(IDictionary<string, TValue> values)
        {
            foreach (var keyValuePair in values)
            {
                Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public TypeNameTrie(TValue value)
        {
            _value = value;
            _hasValue = true;
        }

        public TypeNameTrie()
        {
        }

        public void Add(string prefix, TValue value)
        {
            Add(prefix.Split('.'), value);
        }

        public void Add(Span<string> prefix, TValue value)
        {
            if (prefix.Length == 0)
            {
                _value = value;
                _hasValue = true;
                return;
            }

            var child = _children.TryGetValue(prefix[0], out var c) ? c : _children[prefix[0]] = new TypeNameTrie<TValue>();
            child.Add(prefix.Slice(1), value);
        }

        public bool GetLongestMatch(string testString, out TValue value)
        {
            return GetLongestMatch(testString.Split('.'), out value);
        }

        public bool GetLongestMatch(Span<string> testString, out TValue value)
        {
            if (testString.Length != 0 && _children.TryGetValue(testString[0], out var child))
            {
                return child.GetLongestMatch(testString.Slice(1), out value);
            }

            value = _value;
            return _hasValue;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (_hasValue)
            {
                yield return _value;
            }

            foreach (var value in _children.Values.SelectMany(v => v))
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}