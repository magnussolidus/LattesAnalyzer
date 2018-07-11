using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LattesAnalyzer
{
    public class UnorderedTupleComparer<T> : IEqualityComparer<Tuple<T, T>>
    {
        private IEqualityComparer<T> comparer;
        public UnorderedTupleComparer(IEqualityComparer<T> comparer = null)
        {
            this.comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(Tuple<T, T> x, Tuple<T, T> y)
        {
            return comparer.Equals(x.Item1, y.Item1) && comparer.Equals(x.Item2, y.Item2) ||
                    comparer.Equals(x.Item1, y.Item2) && comparer.Equals(x.Item1, y.Item2);
        }

        public int GetHashCode(Tuple<T, T> obj)
        {
            return comparer.GetHashCode(obj.Item1) ^ comparer.GetHashCode(obj.Item2);
        }
    }
}
