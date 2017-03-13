using System.Collections.Generic;

namespace aisilol
{
	public static class DictionaryExtension
	{
		public static void Upsert<T, U>(this Dictionary<T, U> _dictionary, T _key, U _value)
		{
			if (!_dictionary.ContainsKey(_key))
				_dictionary.Add(_key, _value);

			_dictionary[_key] = _value;
		}
		public static U Find<T, U>(this Dictionary<T, U> _dictionary, T _key)
		{
			if (_dictionary.ContainsKey(_key))
				return _dictionary[_key];

			return default(U);
		}
		public static U Get<T, U>(this Dictionary<T, U> _dictionary, T _key) where U : new()
		{
			if (!_dictionary.ContainsKey(_key))
				_dictionary.Add(_key, new U());

			return _dictionary[_key];
		}
	}
}