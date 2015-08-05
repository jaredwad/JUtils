using System;
using System.Collections.Generic;
using System.Linq;

namespace JUtils
{
	public static class JMath
	{
		////// Statistics //////
		public static double Mean(List<double> values)
		{
			return values.Count == 0 ? 0 : Mean(values, 0, values.Count);
		}

		public static double Mean(List<double> values, int start, int end)
		{
			double s = 0;

			for (int i = start; i < end; i++)
			{
				s += values[i];
			}

			return s / (end - start);
		}

		public static double Variance(List<double> values)
		{
			return Variance(values, Mean(values), 0, values.Count);
		}

		public static double Variance(List<double> values, double mean)
		{
			return Variance(values, mean, 0, values.Count);
		}

		public static double Variance(List<double> values, double mean, int start, int end)
		{
			double variance = 0;

			for (int i = start; i < end; i++)
			{
				variance += Math.Pow((values[i] - mean), 2);
			}

			int n = end - start;
			if (start > 0) n -= 1;

			return variance / (n);
		}

		public static double StandardDeviation(List<double> values)
		{
			return values.Count == 0 ? 0 : StandardDeviation(values, 0, values.Count);
		}

		public static double StandardDeviation(List<double> values, int start, int end)
		{
			double mean = Mean(values, start, end);

			return StandardDeviation(values, mean, start, values.Count);
		}

		public static double StandardDeviation(List<double> values, double mean)
		{
			return StandardDeviation(values, mean, 0, values.Count);
		}

		public static double StandardDeviation(List<double> values, double mean, int start, int end)
		{
			double variance = Variance(values, mean, start, end);

			return StandardDeviation(variance);
		}

		public static double StandardDeviation(double variance)
		{
			return Math.Sqrt(variance);
		}







		/// <summary>
		/// Partitions the given list around a pivot element such that all elements on left of pivot are <= pivot
		/// and the ones at thr right are > pivot. This method can be used for sorting, N-order statistics such as
		/// as median finding algorithms.
		/// Pivot is selected ranodmly if random number generator is supplied else its selected as last element in the list.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 171
		/// </summary>
		private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
		{
			if (rnd != null)
				list.Swap(end, rnd.Next(start, end));

			var pivot = list[end];
			var lastLow = start - 1;
			for (var i = start; i < end; i++)
			{
				if (list[i].CompareTo(pivot) <= 0)
					list.Swap(i, ++lastLow);
			}
			list.Swap(end, ++lastLow);
			return lastLow;
		}

		/// <summary>
		/// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
		/// Note: specified list would be mutated in the process.
		/// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
		/// </summary>
		public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T>
		{
			return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
		}
		private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
		{
			while (true)
			{
				var pivotIndex = list.Partition(start, end, rnd);
				if (pivotIndex == n)
					return list[pivotIndex];

				if (n < pivotIndex)
					end = pivotIndex - 1;
				else
					start = pivotIndex + 1;
			}
		}

		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			if (i==j)
				//This check is not required but Partition function may make many calls so its for perf reason
				return;
			var temp = list[i];



			list[i] = list[j];
			list[j] = temp;
		}

		/// <summary>
		/// Note: specified list would be mutated in the process.
		/// </summary>
		public static T Median<T>(this IList<T> list) where T : IComparable<T>
		{
			return list.NthOrderStatistic((list.Count - 1)/2);
		}

		public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
		{
			var list = sequence.Select(getValue).ToList();
			var mid = (list.Count - 1) / 2;
			return list.NthOrderStatistic(mid);
		}

		public static double GetMedian(this IEnumerable<double> source)
		{
			// Create a copy of the input, and sort the copy
			double[] temp = source.ToArray();    
			Array.Sort(temp);

			int count = temp.Length;
			if (count == 0)
			{
				throw new InvalidOperationException("Empty collection");
			}
			else if (count % 2 == 0)
			{
				// count is even, average two middle elements
				double a = temp[count / 2 - 1];
				double b = temp[count / 2];
				return (a + b) / 2d;
			}
			else
			{
				// count is odd, return the middle element
				return temp[count / 2];
			}
		}

	}

	/// <summary>
	/// An optimized way of calculating min, max, sum, mean, median, variance, and standard deviation
	/// </summary>
	public class MathSet {

		public double Min { get { return Min; } }
		public double Max { get { return Max; } }
		public double Sum { get { return sum; } }
		public double Mean { get { return mean; } }
		public double Count { get { return count; } }
		public double Median { get { return median; } }
		public double StdDev { get { return stdDev; } }
//		public double FstDev { get { return fstDev; } }
		public double Variance { get { return variance; } }

		double min;
		double max;
		double sum;
		double mean;
		double count;
		double median;
		double stdDev;
//		double fstDev;
		double variance;

		List<double> data;

		public MathSet(List<double> pList) {
			
			data = new List<double>(pList);
			count = data.Count;

			median = new List<double>(data).Median();

			stdDev = std_dev();
		}

		private double std_dev() {
			sum = 0;
			min = data[0];
			max = min;

			for(int i = 0; i < count; ++i)
				sum += data[i];
			mean = sum / count;
			double sq_diff_sum = 0;
			for(int i = 0; i < count; ++i) {
				double diff = data[i] - mean;
				sq_diff_sum += diff * diff;
			}

			variance = sq_diff_sum / count;

			return Math.Sqrt(variance);
		}
	}
}

