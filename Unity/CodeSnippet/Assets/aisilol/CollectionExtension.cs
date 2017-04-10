using System;
using System.Collections.Generic;

namespace aisilol
{
	public static class DictionaryExtension
	{
		public static void Upsert<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, TKey _key, TValue _value)
		{
			if (!_dictionary.ContainsKey(_key))
				_dictionary.Add(_key, _value);

			_dictionary[_key] = _value;
		}
		public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, TKey _key)
		{
			if (_dictionary.ContainsKey(_key))
				return _dictionary[_key];

			return default(TValue);
		}
		public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, TKey _key) where TValue : new()
		{
			if (!_dictionary.ContainsKey(_key))
				_dictionary.Add(_key, new TValue());

			return _dictionary[_key];
		}
		public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, Action<TKey, TValue> _caller)
		{
			foreach (var pair in _dictionary)
			{
				_caller(pair.Key, pair.Value);
			}
		}
		public static void ForEachKeys<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, Action<TKey> _caller)
		{
			_dictionary.ForEach((_key, _value) => _caller(_key));
		}
		public static void ForEachValues<TKey, TValue>(this Dictionary<TKey, TValue> _dictionary, Action<TValue> _caller)
		{
			_dictionary.ForEach((_key, _value) => _caller(_value));
		}
	}
}