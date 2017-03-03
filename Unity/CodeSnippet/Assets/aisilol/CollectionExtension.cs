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
	}
}