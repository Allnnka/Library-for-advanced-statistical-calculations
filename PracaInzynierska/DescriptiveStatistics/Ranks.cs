using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaInzynierska.DescriptiveStatistics
{
    public static class Ranks
    {
        public struct Rank
        {
            public int NumberOfRanks;
            public double SumOfPositiveRanks;
            public double SumOfNegativeRanks;
            public double CorrectionForTiedRanks;
        }
        public static Rank CalculateRank(this IEnumerable<double> list)
        {
            list = list.Where(x => x != 0);
            int n = list.Count();
            list = list.OrderBy(x => Math.Abs(x)).ToList();

            Dictionary<double, double> dictOfPairs = list.GroupBy(x => Math.Abs(x)).ToDictionary(x => Math.Abs(x.Key), x => (double)0);
            List<double> listOfRanks = list.ToList();
            int ranksTabN = list.Select(x => Math.Abs(x)).Distinct().Count();
            double numberTiedRanks = list.GroupBy(x => Math.Abs(x)).Where(x => x.Count() > 1).Sum(x => x.Count());

            Dictionary<double, double> tiedPairs = list.GroupBy(x => Math.Abs(x)).ToDictionary(x => Math.Abs(x.Key), x => (double)x.Count());
            double sumPositive = 0;
            double sumNegative = 0;

            if (n == 1)
            {
                if (dictOfPairs[1] > 0)
                {
                    sumPositive++;
                }
                else
                {
                    sumNegative++;
                }
            }
            double m = 0;
            double nSum = 0;
            for (int i = 0; i < n - 1; i++)
            {
                m += 1;
                nSum += (i + 1);
                if (Math.Abs(listOfRanks.ElementAt(i)) != Math.Abs(listOfRanks.ElementAt(i + 1)))
                {
                    if (m == 0)
                    {
                        dictOfPairs[Math.Abs(listOfRanks.ElementAt(i))] = nSum;
                    }
                    else
                    {
                        dictOfPairs[Math.Abs(listOfRanks.ElementAt(i))] = (nSum / m);
                    }
                    m = 0;
                    nSum = 0;
                }
                if (Math.Abs(listOfRanks.ElementAt(n - 2)) != Math.Abs(listOfRanks.ElementAt(n - 1)))
                {
                    dictOfPairs[Math.Abs(listOfRanks.ElementAt(n - 1))] = n;
                }
                if (Math.Abs(listOfRanks.ElementAt(n - 2)) == Math.Abs(listOfRanks.ElementAt(n - 1)))
                {
                    dictOfPairs[Math.Abs(listOfRanks.ElementAt(n - 1))] = ((nSum + n) / (m + 1));
                }
            }


            foreach (double item in listOfRanks)
            {
                if (item > 0)
                {
                    sumPositive += dictOfPairs[Math.Abs(item)];
                }
                else
                {
                    sumNegative += dictOfPairs[Math.Abs(item)];
                }
                Console.WriteLine(dictOfPairs[Math.Abs(item)]);
            }

            double sum = 0;

            foreach (var i in tiedPairs)
            {
                sum += (i.Value * i.Value * i.Value) - i.Value;
            }
            double correctionForTied = (sum / 48.0);

            return new Rank
            {
                NumberOfRanks = n,
                SumOfPositiveRanks = sumPositive,
                SumOfNegativeRanks = sumNegative,
                CorrectionForTiedRanks = correctionForTied
            };
        }

        public struct RanksForKruskalaWallisa
        {
            public int NumberOfRanks;
            public double sumValue;
            public double nValue;
            public double CorrectionForTiedRanks;
        }

        public static RanksForKruskalaWallisa CalculateRankForKruskalaWallisa(this IEnumerable<double> list)
        {
            list = list.Where(x => x != 0);
            int n = list.Count();
            list = list.OrderBy(x => Math.Abs(x)).ToList();

            Dictionary<double, double> dictOfPairs = list.GroupBy(x => Math.Abs(x)).ToDictionary(x => Math.Abs(x.Key), x => (double)0);
            List<double> listOfRanks = list.ToList();
            int ranksTabN = list.Select(x => Math.Abs(x)).Distinct().Count();
            double numberTiedRanks = list.GroupBy(x => Math.Abs(x)).Where(x => x.Count() > 1).Sum(x => x.Count());

            Dictionary<double, double> tiedPairs = list.GroupBy(x => Math.Abs(x)).ToDictionary(x => Math.Abs(x.Key), x => (double)x.Count());


            double m = 0;
            double nSum = 0;
            for (int i = 0; i < n - 1; i++)
            {
                m += 1;
                nSum += (i + 1);
                if (Math.Abs(listOfRanks.ElementAt(i)) != Math.Abs(listOfRanks.ElementAt(i + 1)))
                {
                    if (m == 0)
                    {
                        dictOfPairs[Math.Abs(listOfRanks.ElementAt(i))] = nSum;
                    }
                    else
                    {
                        dictOfPairs[Math.Abs(listOfRanks.ElementAt(i))] = (nSum / m);
                    }
                    m = 0;
                    nSum = 0;
                }
                if (Math.Abs(listOfRanks.ElementAt(n - 2)) != Math.Abs(listOfRanks.ElementAt(n - 1)))
                {
                    dictOfPairs[Math.Abs(listOfRanks.ElementAt(n - 1))] = n;
                }
                if (Math.Abs(listOfRanks.ElementAt(n - 2)) == Math.Abs(listOfRanks.ElementAt(n - 1)))
                {
                    dictOfPairs[Math.Abs(listOfRanks.ElementAt(n - 1))] = ((nSum + n) / (m + 1));
                }
            }

            double sumN=0;
            foreach (double item in list)
            {
                sumN += item;
            }
            double sumR = 0;
            double sumKW=0;
            foreach (double item in listOfRanks)
            {
                sumR += (dictOfPairs[Math.Abs(item)]* dictOfPairs[Math.Abs(item)]);
                sumKW += (sumR) / n;
            }

            double sum = 0;

            foreach (var i in tiedPairs)
            {
                sum += (i.Value * i.Value * i.Value) - i.Value;
            }
            double correctionForTied =1- (sum / (sumN*sumN*sumN-sumN));

            return new RanksForKruskalaWallisa
            {
                NumberOfRanks = n,
                sumValue = sumKW,
                nValue = sumN,
                CorrectionForTiedRanks = correctionForTied
            };
        }
    }
}
