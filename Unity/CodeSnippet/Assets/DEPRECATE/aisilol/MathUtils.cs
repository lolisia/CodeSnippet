using UnityEngine;

namespace aisilol
{
	public static class Math_
	{
		public static float Divide(float _numerator, float _denominator)
		{
			return Mathf.Approximately(_denominator, 0) ? 0 : (_numerator / _denominator);
		}
		public static float Remainder(float _numerator, float _denominator)
		{
			return Mathf.Approximately(_denominator, 0) ? 0 : (_numerator % _denominator);
		}

		public static bool InRange(float _rate)
		{
			return Random.Range(0f, 1f) <= _rate;
		}
		public static bool InRange(int _rate)
		{
			return Random.Range(0, 100) <= _rate;
		}

        public static bool Approximately(float _lhs, float _rhs, float _epsilon = 0.0001f)
        {
            return Mathf.Abs(_lhs - _rhs) < _epsilon;
        }
	}
}