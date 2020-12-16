using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaInzynierska
{
    public static class Util
    {
        //Function from
        //https://www.johndcook.com/blog/csharp_phi/
        //Expected Cumulative Frequency
        public static double Phi(double x)
        {
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x) / Math.Sqrt(2.0);

            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return 0.5 * (1.0 + sign * y);
        }

        public static List<double> DifferenceBetweenPairsOfMeasurements(this IEnumerable<double> list1, IEnumerable<double> list2)
        {

            List<double> listOfDifference = new List<double>();
            int minN = Math.Min(list1.Count(), list2.Count());
            int maxN = Math.Max(list1.Count(), list2.Count());
            for (int i = 0; i < minN; i++)
            {
                listOfDifference.Add(list1.ElementAt(i) - list2.ElementAt(i));
            }
            if(list1.Count() != list2.Count())
            {
                if (list1.Count() == maxN)
                {
                    for(int i = minN; i < maxN; i++)
                    {
                        listOfDifference.Add(list1.ElementAt(i));
                    }
                }
                if (list2.Count() == maxN)
                {
                    for (int i = minN; i < maxN; i++)
                    {
                        listOfDifference.Add(list2.ElementAt(i));
                    }
                }
            }
            return listOfDifference;
        }
        public static List<double> DifferenceBetweenPairsOfMeasurements(this IEnumerable<double> list1, double number)
        {

            List<double> listOfDifference = new List<double>();
            int n = list1.Count();

            for (int i = 0; i < n; ++i)
            {
                listOfDifference.Add(list1.ElementAt(i) - number);
            }
            return listOfDifference;
        }

    }
}
