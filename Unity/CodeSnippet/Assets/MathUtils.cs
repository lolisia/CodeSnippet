using UnityEngine;

namespace aisilol
{
	public static class MathUtils
	{
		public static float Divide(float _numerator, float _denominator)
		{
			return _denominator == 0 ? 0 : (_numerator / _denominator);
		}
		public static float Remainder(float _numerator, float _denominator)
		{
			return _denominator == 0 ? 0 : (_numerator % _denominator);
		}

		public static bool InRange(float _rate)
		{
			return Random.Range(0f, 1f) <= _rate;
		}
		public static bool InRange(int _rate)
		{
			return Random.Range(0, 100) <= _rate;
		}
	}
}