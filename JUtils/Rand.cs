using System;

namespace JUtils
{
	public static class Rand
	{
		private static Random rand = new Random();

		/// <summary>
		/// Returns a random integer between pMax and pMin
		/// </summary>
		/// <returns>A random int</returns>
		/// <param name="pMax">The maximum integer to return (defaults to 100)</param>
		/// <param name="pMin">The minimum integer to return (defaults to 0)</param>
		public static int Int( int pMax = 100, int pMin = 0) {
			return rand.Next(pMin, pMax);
		}

		/// <summary>
		/// Returns a random double between 0.0 and 1.0
		/// </summary>
		/// <returns>A random double</returns>
		public static double Double(double pMax = 1.0, double pMin = 0.0) {
			return rand.NextDouble() * (pMax - pMin) + pMin;
		}

		/// <summary>
		/// Returns a random boolean
		/// </summary>
		/// <returns>A random boolean</returns>
		public static bool Bool() {
			return rand.NextDouble() > .5;
		}
	}
}

