using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public static class HashSetExt
	{
		// really could be better looking...
		public static HashSet<T> ToHashSet<T>(
			this IEnumerable<T> source,
			IEqualityComparer<T> comparer)
		{
			return new HashSet<T>(source, comparer);
		}
	}
}