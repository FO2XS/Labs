using System.Numerics;

namespace UsualLibrary
{
	public class FibCalculator
	{
		private static BigInteger[] cache;

		static FibCalculator()
		{
			cache = new BigInteger[100000];
			cache[0] = 1;
			cache[1] = 1;
		}

		private void fillCache(int num)
		{
			for (var i = 2; i <= num; i++)
			{
				if(cache[i] != 0)
					continue;
				
				cache[i] = cache[i - 1] + cache[i - 2];
			}
		}

		public BigInteger GetFibonachiSequence(int position)
		{

			if (cache[position] == 0)
			{
				fillCache(position);
			}

			return cache[position];
		}

		public BigInteger GetRecursiveFib(BigInteger position)
		{
			if (position == 1 || position == 2)
			{
				return 1;
			}

			return GetRecursiveFib(position - 1) + GetRecursiveFib(position - 2);
		}
	}
}