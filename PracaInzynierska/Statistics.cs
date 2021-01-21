using PracaInzynierska.DescriptiveStatistics;
using PracaInzynierska.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using static PracaInzynierska.DescriptiveStatistics.Ranks;

namespace PracaInzynierska.Statystyka
{
    public static class Statystyki
    {
        public static double CalculateMean(this IEnumerable<double> data)
        {
            if (data.Count() == 0) throw new EmptyCollectionException();
            return data.Average();
        }

        public static double CalculateMean(this IEnumerable<int> data)
        {
            if (data.Count() == 0) throw new EmptyCollectionException();
            return data.Select(i => (double)i).CalculateMean();
        }

        public static double CalculateMean(this IEnumerable<long> data)
        {
            if (data.Count() == 0) throw new EmptyCollectionException();
            return data.Select(l => (double)l).CalculateMean();
        }

        public static double CalculateSampleMeans(this IEnumerable<double> list)
        {
            double sampleMeans = 0;
            double mean = list.CalculateMean();
            int n = list.Count();
            foreach (double item in list)
            {
                sampleMeans += (item - mean) * (item - mean);
            }
            sampleMeans = sampleMeans / ((double)n - 1);
            return sampleMeans;
        }

        public static double CalculateRange(this IEnumerable<double> list)
        {
            return list.Max() - list.Min();
        }

        public enum StandardDeviationType { Samples, Population };

        public static double CalculateStandardDeviation(this IEnumerable<double> list, StandardDeviationType type = StandardDeviationType.Samples)
        {
            double mean = list.Average();
            double deviation = 0;
            foreach (double item in list)
            {
                deviation += (item - mean) * (item - mean);
            }
            switch (type)
            {
                case StandardDeviationType.Population:
                    if (list.Count() == 0) throw new Exception("Standard deviation calculation error. The collection is empty");
                    deviation /= list.Count();
                    break;
                case StandardDeviationType.Samples:
                    if (list.Count() <= 1) throw new Exception("Standard deviation calculation error. Population insufficient");
                    deviation /= list.Count() - 1;
                    break;
                default:
                    throw new Exception("Unrecognized type of standard deviation");
            }
            deviation = Math.Sqrt(deviation);
            return deviation;
        }

        public static double CalculateStandardDeviation(this IEnumerable<float> list, StandardDeviationType type = StandardDeviationType.Population)
        {
            List<double> copy = new List<double>();
            foreach (float item in list) copy.Add((double)item);
            return CalculateStandardDeviation(copy, type);
        }

        public static double CalculateStandardDeviation(this IEnumerable<int> list, StandardDeviationType type = StandardDeviationType.Population)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return CalculateStandardDeviation(copy, type);
        }
        public struct Quartile
        {
            public double q1;
            public double q2;
            public double q3;
        }
        public static Quartile CalculateQuartile(this IEnumerable<double> list)
        {
            if (list.Count() == 0) throw new EmptyCollectionException();
            List<double> _list = list.ToList();
            _list.Sort();

            List<double> lowerHalf = new List<double>();
            List<double> upperHalf = new List<double>();
            double q1, q2, q3;
            q2 = CalculateMedian(list);
            if (_list.Count % 2 != 0)
            {
                for (int i = 0; i < _list.Count / 2 + 1; ++i) lowerHalf.Add(_list[i]);
                for (int i = _list.Count / 2; i < _list.Count; ++i) upperHalf.Add(_list[i]);
            }
            else
            {
                for (int i = 0; i < _list.Count / 2; ++i) lowerHalf.Add(_list[i]);
                for (int i = _list.Count / 2; i < _list.Count; ++i) upperHalf.Add(_list[i]);
            }

            q1 = _calculateMedian(lowerHalf);
            q3 = _calculateMedian(upperHalf);
            return new Quartile
            {
                q1 = q1,
                q2 = q2,
                q3 = q3
            };
        }

        public static double CalculateQuarterRange(this IEnumerable<double> list)
        {
            if (list.Count() < 3) throw new NotTheRightSizeException();
            CalculateQuartile(list);
            return CalculateQuartile(list).q3 - CalculateQuartile(list).q1;
        }

        public static double CalculateQuarterlyVariance(this IEnumerable<double> list)
        {
            return CalculateQuarterRange(list) / 2;
        }

        private static double _calculateMedian(List<double> data)
        {
            if (data.Count == 0) throw new EmptyCollectionException();

            data.Sort();

            if (data.Count % 2 != 0)
                return data[data.Count / 2];
            else
                return (data[data.Count / 2 - 1] + data[data.Count / 2]) / 2.0;
        }

        public static double CalculateMedian(this IEnumerable<double> data)
        {
            List<double> copy = new List<double>();
            foreach (double item in data) copy.Add(item);
            return _calculateMedian(copy);
        }

        public static double CalculateMedian(this IEnumerable<int> data)
        {
            List<double> copy = new List<double>();
            foreach (int item in data) copy.Add((double)item);
            return _calculateMedian(copy);
        }

        public static double CalculateMedian(this IEnumerable<long> data)
        {
            List<double> copy = new List<double>();
            foreach (long item in data) copy.Add((double)item);
            return _calculateMedian(copy);
        }
        public enum SkewnessType { type1, type2,type3 };

        private static double _calculateSkewness(List<double> list, SkewnessType type=SkewnessType.type3)
        {
            if (list.Count == 0) throw new EmptyCollectionException();

            double m3 = 0;
            double m2 = 0;
            int n = list.Count();
            double mean = CalculateMean(list);
            double temp, _m2;
            foreach (double item in list)
            {
                temp = (item - mean);
                _m2 = temp * temp;
                m3 += _m2*temp;
                m2 += _m2;
            }
            double A;
            switch (type)
            {
                case SkewnessType.type1:
                    A = Math.Sqrt(n / m2 * m2 * m2) * m3;
                    break;
                case SkewnessType.type2:
                    A = (n * Math.Sqrt((n - 1)/ m2 * m2 * m2) * m3) / (n - 2);
                    break;
                case SkewnessType.type3:
                    A = (Math.Sqrt(n * (1 - 1.0 / n) * (1 - 1.0 / n) * (1 - 1.0 / n) / m2 * m2 * m2) * m3);
                    break;
                default:
                    throw new InvalidArgument("type");
            }

            return Math.Round(A, 7);
        }
        public static double CalculateSkewness(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateSkewness(copy);
        }

        public static double CalculateSkewness(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateSkewness(copy);
        }

        public static double[] _calculateMode(List<double> data)
        {
            if (data.Count == 0) throw new EmptyCollectionException();

            List<double> currentMax = new List<double>();

            Dictionary<double, int> dictNumberOfOccurrencesOfElements = data.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            if (!((dictNumberOfOccurrencesOfElements.Values.GroupBy(x => x).Where(x => x.Count() >= 1))
                .Count() > 1)) throw new Exception("There is no dominant in a given set");

            int occurences = 0;

            foreach (KeyValuePair<double, int> x in dictNumberOfOccurrencesOfElements)
            {
                if (occurences < x.Value)
                {
                    currentMax.Clear();
                    currentMax.Add(x.Key);
                    occurences = x.Value;
                }
                else if (occurences == x.Value)
                {
                    currentMax.Add(x.Key);
                }
            }
            return currentMax.ToArray();
        }

        public static double[] CalculateMode(this IEnumerable<double> data)
        {
            List<double> copy = new List<double>();
            foreach (int item in data) copy.Add(item);
            return _calculateMode(copy);

        }
        public static double[] CalculateMode(this IEnumerable<long> data)
        {
            List<double> copy = new List<double>();
            foreach (int item in data) copy.Add((double)item);
            return _calculateMode(copy);

        }
        public static double[] CalculateMode(this IEnumerable<int> data)
        {
            List<double> copy = new List<double>();
            foreach (int item in data) copy.Add((double)item);
            return _calculateMode(copy);
        }

        private static double _calculateKurtosis(List<double> data)
        {
            if (data.Count() == 0) throw new EmptyCollectionException();
            int n = data.Count();
            double mean = CalculateMean(data);
            double m2 = 0;
            double m4 = 0;
            double help, _m2;
            foreach (double item in data)
            {
                help = (item - mean);
                _m2 = help*help;
                m2 += _m2;
                m4 += _m2*_m2;
            }
            double k = (n * m4) / (m2 * m2);
            return Math.Round(k, 6);
        }

        public static double CalculateKurtosis(this IEnumerable<double> data)
        {
            List<double> copy = new List<double>();
            foreach (double item in data) copy.Add(item);
            return _calculateKurtosis(copy);
        }
        public static double CalculateKurtosis(this IEnumerable<int> data)
        {
            List<double> copy = new List<double>();
            foreach (int item in data) copy.Add((double)item);
            return _calculateKurtosis(copy);
        }
        public static double CalculateKurtosis(this IEnumerable<long> data)
        {
            List<double> copy = new List<double>();
            foreach (long item in data) copy.Add((double)item);
            return _calculateKurtosis(copy);
        }
        public struct Correlation
        {
            public int SampleSize;
            public double CorrelationCoefficient;
            public double PValue;
            public double StudentsTValue;
        }

        public static Correlation CalculatePearsonsCorrelation(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculatePearsonsCorrelation(copy1, copy2);
        }
        public static Correlation CalculatePearsonsCorrelation(this IEnumerable<int> list1, IEnumerable<int> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculatePearsonsCorrelation(copy1, copy2);
        }

        private static Correlation _calculatePearsonsCorrelation(List<double> list1, List<double> list2)
        {
            if (list1.Count() != list2.Count()) throw new SizeOutOfRangeException();
            int n = list1.Count();
            if (n == 0) throw new EmptyCollectionException();
            if (n < 3) throw new NotTheRightSizeException();
            double average1 = CalculateMean(list1);
            double average2 = CalculateMean(list2);

            double covariance = 0;
            double standardDeviation1 = 0;
            double standardDeviation2 = 0;


            for (int i = 0; i < n; ++i)
            {
                double s1 = (list1.ElementAt(i) - average1);
                double s2 = (list2.ElementAt(i) - average2);
                covariance += s1 * s2;
                standardDeviation1 += s1 * s1;
                standardDeviation2 += s2 * s2;
            }


            double degreeOfFreedom = n - 2.0;

            double r = covariance / Math.Sqrt(standardDeviation1 * standardDeviation2);
            if (r >= 1 || r <= -1) throw new ArgumentException("Pearson correlation coefficient mus be in <-1;1>");
            double tDistribution = r * Math.Sqrt((n - 2) / (1 - r * r));
            double p = ContinuousDistribution.Student(tDistribution, n-2);


            double tValue = (average1-average2) / Math.Sqrt(standardDeviation1 * standardDeviation1 + standardDeviation2 * standardDeviation2);
            double pForT = ContinuousDistribution.Student(tValue, degreeOfFreedom);
            return new Correlation()
            {
                SampleSize = n,
                CorrelationCoefficient = Math.Round(r,7),
                PValue = Math.Round(p,5),
                StudentsTValue= tDistribution
            };
        }
        
        private static TestResult _calculateStudentsTTest(this IEnumerable<double> list1, double hypotheticalMean)
        {
            int n = list1.Count();
            if (n == 0) throw new EmptyCollectionException();
            double average = CalculateMean(list1);
            double standardDeviation = CalculateStandardDeviation(list1);

            double tforOneSample = ((average - hypotheticalMean) / standardDeviation) * Math.Sqrt(n);
            int df = 1;
            double p = ContinuousDistribution.Student(tforOneSample,n-1);
            return new TestResult()
            {
                TestValue = Math.Round(tforOneSample, 4),
                DegreesOfFreedom = df,
                PValue = Math.Round(p,5)
            };
        }
        private static TestResult _calculateStudentsTTest(this IEnumerable<double> list1,IEnumerable<double> list2,bool pairs=false)
        {
            int n1 = list1.Count();
            int n2 = list2.Count();

            double average1 = CalculateMean(list1);
            double average2 = CalculateMean(list2);

            double standardDeviation1 = CalculateStandardDeviation(list1);
            double standardDeviation2 = CalculateStandardDeviation(list2);
            double t, df;
           
            if (!pairs)
            {
                double sd1 = (standardDeviation1 * standardDeviation1) / (double)n1;
                double sd2 = (standardDeviation2 * standardDeviation2) / (double)n2;
                double standardDeviationForTwoList = ((((double)n1 - 1.0) * standardDeviation1 * standardDeviation1 + ((double)n2 - 1.0) * standardDeviation2 * standardDeviation2) / ((double)n1 + (double)n2 - 2.0));
                if (sd1 == sd2)
                {
                    t = (average1 - average2) / (Math.Sqrt(standardDeviationForTwoList * ((1.0 / (double)n1) + (1.0 / (double)n2))));
                    df = n1 + n2-2;
                }
                else
                {
                    t = (average1 - average2) / Math.Sqrt(sd1 + sd2);
                    df = ((sd1 + sd2) * (sd1 + sd2)) / ((sd1 * sd1 * (1 / ((double)n1 - 1))) + (sd2 * sd2 * (1 / ((double)n2 - 1))));
                }
            }
            else {
                if (n1 != n2) throw new SizeOutOfRangeException();
                List<double> listOfPairs = Util.DifferenceBetweenPairsOfMeasurements(list1, list2);
                double averageForPairs = CalculateMean(listOfPairs);
                double standardDeviationForPairs = CalculateStandardDeviation(listOfPairs);
                t = averageForPairs / standardDeviationForPairs * Math.Sqrt(listOfPairs.Count());
                df = n1 - 1;
            }

            double p = ContinuousDistribution.Student(t, df);
            return new TestResult()
            {
                TestValue = Math.Round(t,4),
                DegreesOfFreedom= Math.Round(df,4),
                PValue= Math.Round(p, 4)
            };
        }
        public static TestResult CalculateStudentsTTest(this IEnumerable<double> list, double hypotheticalMean = 0)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateStudentsTTest(copy, hypotheticalMean);
        }
        public static TestResult CalculateStudentsTTest(this IEnumerable<double> list1, IEnumerable<double> list2, bool pairs = false)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateStudentsTTest(copy1, copy2, pairs);
        }
        public static TestResult CalculateStudentsTTest(this IEnumerable<int> list, double hypotheticalMean = 0)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add((double)item);
            return _calculateStudentsTTest(copy, hypotheticalMean);
        }
        public static TestResult CalculateStudentsTTest(this IEnumerable<int> list1, IEnumerable<int> list2, bool pairs = false)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculateStudentsTTest(copy1, copy2, pairs);
        }
        public struct WilcoxonResults
        {
            public double T;
            public double ZStatistic;
            public double PValue;
        }
        private static WilcoxonResults _calculateWilcoxonTest(List<double> list,double hypotheticalMedian=0)
        {
            if (list.Count() == 0) throw new EmptyCollectionException();
            Rank r;
            if (hypotheticalMedian != 0)
                r = Ranks.CalculateRankForWilcoxonTest(Util.DifferenceBetweenPairsOfMeasurements(list,hypotheticalMedian));
            else
                r = Ranks.CalculateRankForWilcoxonTest(list);

            double t = Math.Max(r.SumOfNegativeRanks, r.SumOfPositiveRanks);

            double nRanks = r.NumberOfRanks;
            
            double zValue = (Math.Abs(t - ((nRanks * (nRanks + 1.0)) / 4.0)) - 0.5) / Math.Sqrt(((nRanks * (nRanks + 1.0) * (2.0 * nRanks + 1.0)) / 24.0) - r.CorrectionForTiedRanks);

            double p = 2*ContinuousDistribution.Gauss(-Math.Abs(zValue));

            return new WilcoxonResults
            {
                T = t,
                ZStatistic = zValue,
                PValue = Math.Round(p, 4)
            };
        }
        private static WilcoxonResults _calculateWilcoxonTest(List<double> list1,List<double> list2, bool pairs)
        {
            if (list1.Count() == 0 || list2.Count() == 0) throw new EmptyCollectionException();
            if (pairs) {
                if (list1.Count() != list2.Count()) throw new SizeOutOfRangeException();

                Rank r = Ranks.CalculateRankForWilcoxonTest(Util.DifferenceBetweenPairsOfMeasurements(list1, list2));
                
                double t = Math.Max(r.SumOfNegativeRanks, r.SumOfPositiveRanks);
                double nRanks = (double)r.NumberOfRanks;

                double zValue = ((Math.Abs(t - ((nRanks * (nRanks + 1)) / 4))) - 0.5) / Math.Sqrt(((nRanks * (nRanks + 1) * (2 * nRanks + 1)) / 24) - r.CorrectionForTiedRanks);

                double p = 2 * ContinuousDistribution.Gauss(-Math.Abs(zValue));

                return new WilcoxonResults
                {
                    T = t,
                    ZStatistic = zValue,
                    PValue = Math.Round(p, 4)
                };
            }
            else
            {
                return CalculateTestUMannWhitney(list1, list2);
            }
          
        }
        private static WilcoxonResults _calculateTestUManaWhitney(List<double> list1, List<double> list2)
        {
            if (list1.Count() == 0 || list2.Count() == 0) throw new EmptyCollectionException();
            int n1 = list1.Count();
            int n2 = list2.Count();
            List<double> list3 = list1.Concat(list2).OrderBy(x => Math.Abs(x)).ToList();
            Dictionary<double, double> dictOfPairs = Ranks.CalculateRanks(list3);
            double r1 = 0;
            double r2 = 0;

            double u1 = 0, u2 = 0;
            foreach(double item in list1)
            {
                r1+= dictOfPairs[Math.Abs(item)];
            }
            foreach (double item in list2)
            {
                r2 += dictOfPairs[Math.Abs(item)];
            }
            u1 = n1 * n2 + ((n1 * (n1 + 1.0)) / 2.0) - r1;

            u2 = n1 * n2 + ((n2 * (n2 + 1.0)) / 2.0) - r2;

            double correctionForTied = (n1 * n2 * Ranks.SumOfTiedPairs(list3)) / (12.0 * (n1 + n2) * (n1 + n2 - 1.0));
            double zValue = (Math.Abs(u2 - (n1 * n2) / 2.0)-0.5)/Math.Sqrt((n1*n2*(n1+n2+1.0))/12.0-correctionForTied);
            double p = 2 * ContinuousDistribution.Gauss(-Math.Abs(zValue));

          
            return new WilcoxonResults
            {
                T = u2,
                ZStatistic = zValue,
                PValue = Math.Round(p, 4)
            };
        }
        public static WilcoxonResults CalculateWilcoxonTest(this IEnumerable<double> list, double hypotheticalMedian = 0)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateWilcoxonTest(copy, hypotheticalMedian);
        }
        public static WilcoxonResults CalculateWilcoxonTest(this IEnumerable<int> list, double hypotheticalMedian = 0)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add((double)item);
            return _calculateWilcoxonTest(copy, hypotheticalMedian);
        }
        public static WilcoxonResults CalculateWilcoxonTest(this IEnumerable<double> list1, IEnumerable<double> list2, bool pairs = false)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateWilcoxonTest(copy1, copy2,pairs);
        }
        public static WilcoxonResults CalculateWilcoxonTest(this IEnumerable<int> list1, IEnumerable<int> list2, bool pairs = false)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculateWilcoxonTest(copy1, copy2,pairs);
        }
        public static WilcoxonResults CalculateTestUMannWhitney(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateTestUManaWhitney(copy1, copy2);
        }
        public static WilcoxonResults CalculateTestUMannWhitney(this IEnumerable<int> list1, IEnumerable<int> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculateTestUManaWhitney(copy1, copy2);
        }
        public struct TestResult
        {
            public double TestValue;
            public double PValue;
            public double DegreesOfFreedom;
        }
        private static TestResult _calculateKolmogorovSmirnovTestForNormality(List<double> list)
        {
            if (list.Count() == 0) throw new EmptyCollectionException();
            list.OrderBy(x => x);
            int n = list.Count();
            List<double> Di = new List<double>();
            List<double> D_i = new List<double>();

            double average = list.Average();
            double standartDeviation = CalculateStandardDeviation(list);


            for (int i = 0; i < n; i++)
            {
                Di.Add(Math.Abs((((double)i+1.0)/(double)n)-Util.Phi((list.ElementAt(i)-average)/standartDeviation)));
                D_i.Add(Math.Abs(Util.Phi((list.ElementAt(i) - average) / standartDeviation) - ((double)i / (double)n)));
            }
            double d = (Math.Max(Di.Max(), D_i.Max()));

            //P- value from 
            //https://stats.stackexchange.com/questions/389034/kolmogorov-smirnov-test-calculating-the-p-value-manually
            double pValue = 0;
            double zz = d*d*(double)n;
            for (int i =1; i < 1000; i++)
            {
                pValue += (Math.Pow((-1.0), ((double)i - 1.0))) * (Math.Exp((-2.0) * ((double)i * (double)i) * zz));
            }
            double p =2.0 * pValue;
            return new TestResult
            {
                TestValue = Math.Round(d, 5),
                PValue= Math.Round(p, 4)
            }; 
        }
        public static TestResult CalculateKolmogorovSmirnovTestForNormality(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateKolmogorovSmirnovTestForNormality(copy);
        }
        public static TestResult CalculateKolmogorovSmirnovTestForNormality(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateKolmogorovSmirnovTestForNormality(copy);
        }
        private static double poly(double[] cc, int nord, double x)
        {

            double ret_val = cc[0];
            if (nord > 1)
            {
                double p = x * cc[nord - 1];
                for (int j = nord - 2; j > 0; --j)
                {
                    p = (p + cc[j]) * x;
                }
                ret_val += p;
            }
            return ret_val;
        }
        private static int sign(double x)
        {
            if (x == 0)
            {
                return 0;
            }
            return (x > 0) ? 1 : -1;
        }

        private static TestResult _calculateShapiroWilkTest(List<double> list)
        {
            if (list.Count() < 3 || list.Count() > 5000) throw new NotTheRightSizeException();
            list.OrderBy(x => x);
            int n = list.Count();
            int nn2 = n / 2;
            double[] a = new double[nn2 + 1];

            double small = 1e-19;

            /* polynomial coefficients */
            double[] g = { -2.273, 0.459 };
            double[] c1 = { 0.0, 0.221157, -0.147981, -2.07119, 4.434685, -2.706056 };
            double[] c2 = { 0.0, 0.042981, -0.293762, -1.752461, 5.682633, -3.582633 };
            double[] nsmall = { 0.544, -0.39978, 0.025054, -6.714e-4 };
            double[] ssmall = { 1.3822, -0.77857, 0.062767, -0.0020322 };
            double[] nlg = { -1.5861, -0.31082, -0.083751, 0.0038915 };
            double[] slg= { -0.4803, -0.082676, 0.0030302 };

            int i, j, i1;

            double ssassx, summ2, ssumm2, gamma, range;
            double a1, a2, an, m, s, sa, xi, sx, xx, y, w1;
            double fac, asa, an25, ssa, sax, rsn, ssx, xsx;

            double pw = 1.0;
            an = (double)n;

            if (n == 3)
            {
                a[1] = Math.Sqrt(0.5);
            }
            else
            {
                an25 = an + 0.25;
                summ2 = 0.0;
                for (i = 1; i <= nn2; i++)
                {
                    a[i] = ContinuousDistribution.NormalQuantile((i - 0.375) / an25, 0, 1); 
                    summ2 += a[i] * a[i];
                }
                summ2 *= 2.0;
                ssumm2 = Math.Sqrt(summ2);
                rsn = 1.0 / Math.Sqrt(an);
                a1 = poly(c1, 6, rsn) - a[1] / ssumm2;


                if (n > 5)
                {
                    i1 = 3;
                    a2 = -a[2] / ssumm2 + poly(c2, 6, rsn);
                    fac = Math.Sqrt((summ2 - 2.0 * (a[1] * a[1]) - 2.0 * (a[2] * a[2])) / (1.0 - 2.0 * (a1 * a1) - 2.0 * (a2 * a2)));
                    a[2] = a2;
                }
                else
                {
                    i1 = 2;
                    fac = Math.Sqrt((summ2 - 2.0 * (a[1] * a[1])) / (1.0 - 2.0 * (a1 * a1)));
                }
                a[1] = a1;
                for (i = i1; i <= nn2; i++)
                {
                    a[i] /= -fac;
                }
            }

            range = list.ElementAt(n - 1) - list.ElementAt(0);
            if (range < small)
                throw new NotTheRightSizeException();



            xx = list.ElementAt(0) / range;
            sx = xx;
            sa = -a[1];
            for (i = 1, j = n - 1; i < n; j--)
            {
                xi = list.ElementAt(i) / range;
                if (xx - xi > small)
                {
                    throw new ArgumentException();
                }
                sx += xi;
                i++;
                if (i != j)
                {
                    sa += sign(i - j) * a[Math.Min(i, j)];
                }
                xx = xi;
            }


            sa /= n;
            sx /= n;
            ssa = ssx = sax = 0;
            for (i = 0, j = n - 1; i < n; i++, j--)
            {
                if (i != j)
                {
                    asa = sign(i - j) * a[1 + Math.Min(i, j)] - sa;
                }
                else
                {
                    asa = -sa;
                }
                xsx = list.ElementAt(i) / range - sx;
                ssa += asa * asa;
                ssx += xsx * xsx;
                sax += asa * xsx;
            }
            ssassx = Math.Sqrt(ssa * ssx);
            w1 = (ssassx - sax) * (ssassx + sax) / (ssa * ssx);
            double w = 1.0 - w1;
            double pValue=0;

            if (n == 3)
            {
                double pi6 = 1.90985931710274; 
                double stqr = 1.04719755119660; 
                pw = pi6 * (Math.Asin(Math.Sqrt(w)) - stqr);
                if (pw < 0)
                {
                    pw = 0;
                }
                //return w; 
                 pValue=pw;
            }
            y = Math.Log(w1);
            xx = Math.Log(an);
            if (n <= 11)
            {
                gamma = poly(g, 2, an);
                if (y >= gamma)
                {
                    pValue= 1e-99;
                }
                y = -Math.Log(gamma - y);
                m = poly(nsmall, 4, an);
                s = Math.Exp(poly(ssmall, 4, an));
            }
            else
            { 
                m = poly(nlg, 4, xx);
                s = Math.Exp(poly(slg, 3, xx));
            }

            double z = (y - m) / s;
            if(pValue == 0)
            {
                if(w>0.95)
                    pValue = Math.Round(1 - ContinuousDistribution.Gauss(-Math.Abs(z)),4);
                else
                    pValue = Math.Round(ContinuousDistribution.Gauss(-Math.Abs(z)),5);

            }
            return new TestResult
            {
                TestValue = Math.Round(w, 5),
                PValue =pValue
            };
        }
        public static TestResult CalculateShapiroWilkTest(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (float item in list) copy.Add(item);
            return _calculateShapiroWilkTest(copy);
        }
        public static TestResult CalculateShapiroWilkTest(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (float item in list) copy.Add((double)item);
            return _calculateShapiroWilkTest(copy);
        }
        //https://stats.stackexchange.com/questions/381873/meaning-of-chi-squared-in-r-kruskal-wallis-test
        private static TestResult _calculateKruskalWalisTest(List<double> list1,List<double> list2)
        {
            if (list1.Count() != list2.Count()) throw new SizeOutOfRangeException();
            if (list1.Count() ==0 || list2.Count()==0) throw new EmptyCollectionException();
            List<double> factorList = list2.Distinct().ToList();

            int n = list1.Count();
            list1 = list1.OrderBy(x => Math.Abs(x)).ToList();

            Dictionary<double, double> dictOfPairs = Ranks.CalculateRanks(list1);

            double sumValue = 0;
            double lengthValue = 0;

            List<double> sumValues = new List<double>();
            List<double> lengthValues = new List<double>();

            foreach (double item in factorList)
            {
                for (int i = 0; i < n; i++)
                {
                    if (list2.ElementAt(i) == item)
                    {
                        sumValue += dictOfPairs[Math.Abs(list1.ElementAt(i))];
                        lengthValue++;
                    }
                }
                sumValues.Add(sumValue);
                lengthValues.Add(lengthValue);
                sumValue = 0;
                lengthValue = 0;
            }

            double sumRij = 0;
            for (int i = 0; i < sumValues.Count(); i++)
            {
                sumRij += (sumValues.ElementAt(i) * sumValues.ElementAt(i)) / lengthValues.ElementAt(i);
            }

            double correctionForTied = 1.0 - (Ranks.SumOfTiedPairs(list1) / (n * n * n - n));

            double kwScore = ((12.0 * sumRij) / (n * (n + 1.0)) - 3.0 * (n + 1.0));
            kwScore =kwScore/ correctionForTied;
            kwScore =Math.Round(kwScore,1);
            int df = factorList.Count() - 1;
            double pVal = 1.0- ContinuousDistribution.ChiSquareCdf(kwScore,df);
            return new TestResult
            {
                TestValue = kwScore,
                PValue = Math.Round(pVal,4),
                DegreesOfFreedom=df
            };
        }

        public static TestResult CalculateKruskalWalisTest(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateKruskalWalisTest(copy1,copy2);
        }
        public static TestResult CalculateKruskalWalisTest(this IEnumerable<int> list1, IEnumerable<int> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (int item in list1) copy1.Add((double)item);
            foreach (int item in list2) copy2.Add((double)item);
            return _calculateKruskalWalisTest(copy1, copy2);
        }
        public struct FTest
        {
            public double RatioOfVariances;
            public int NumDf;
            public int DenomDf;
            public double PValue;
        }
        public static FTest _calculateFTestToCompareTwoVariances(List<double> list1, List<double> list2)
        {
            int n = list1.Count();
            int m = list2.Count();
            if (n == 0 || m == 0) throw new EmptyCollectionException();
            double f = CalculateSampleMeans(list1) / CalculateSampleMeans(list2);

            double p =2* ContinuousDistribution.FCdf(f, n-1, m-1);
            return new FTest
            {
                RatioOfVariances=Math.Round(f,4),
                NumDf=n-1,
                DenomDf=m-1,
                PValue = Math.Round(p,5)
                
            };
        }
        public static FTest CalculateFTestToCompareTwoVariances(this IEnumerable<int> list1, IEnumerable<int> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculateFTestToCompareTwoVariances(copy1, copy2);
        }
        public static FTest CalculateFTestToCompareTwoVariances(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateFTestToCompareTwoVariances(copy1, copy2);
        }
        //https://www.bmj.com/about-bmj/resources-readers/publications/statistics-square-one/8-chi-squared-tests

        private static TestResult _calculateChiSquaredTest(List<double> list)
        {
            
            int n = list.Count();
            if (n == 0) throw new EmptyCollectionException();
            double statistic = 0;

            double expectedA = list.Average();    

            for (int i = 0; i < n; i++)
            {
                statistic += ((list.ElementAt(i) - expectedA) * (list.ElementAt(i) - expectedA)) / expectedA;
            }
            int df = n - 1;
            double pval = 1.0 - ContinuousDistribution.ChiSquareCdf(statistic, df);

            return new TestResult
            {
                TestValue = Math.Round(statistic, 5),
                DegreesOfFreedom = df,
                PValue = Math.Round(pval, 4)
            };
        }
        private static TestResult _calculateChiSquaredTest(params List<double>[] args)
        {
            int nCols = args.Length;
            int nRows = args.FirstOrDefault().Count();
            foreach (List<double> list in args)
            {
                if (nRows != list.Count()) throw new SizeOutOfRangeException();
            }
            List<double> n = Enumerable.Repeat<double>(0, nRows).ToList();

            List<double> sumOfCols = new List<double>();

            foreach (IEnumerable<double> list in args)
            {
                for (int i = 0; i < nRows; i++)
                {
                    n[i] += list.ElementAt(i);
                }
                sumOfCols.Add(list.Sum());
            }
            double totalSum = sumOfCols.Sum();
            double statistic = 0;
            double expected;
            int rowNum = 0;
            foreach (IEnumerable<double> list in args)
            {
                for (int i = 0; i < nRows; i++)
                {
                    expected = (n.ElementAt(i) / totalSum) * sumOfCols.ElementAt(rowNum);
                    statistic += ((list.ElementAt(i) - expected) * (list.ElementAt(i) - expected)) / expected;
                }
                rowNum++;
            }
            int df = (nCols - 1) * (nRows - 1);
            double pval = 1.0 - ContinuousDistribution.ChiSquareCdf(statistic, df);
            return new TestResult
            {
                TestValue = Math.Round(statistic, 5),
                DegreesOfFreedom = df,
                PValue = Math.Round(pval, 4)
            };
        }
        public static TestResult CalculateChiSquaredTest(params IEnumerable<double>[] args)
        {
            if (args.Length == 1)
            {
                List<double> copy = new List<double>();
                foreach(IEnumerable<double> item in args)
                {
                    foreach (double element in item)
                        copy.Add(element);
                }
                return _calculateChiSquaredTest(copy);
            }
            else
            {
                List<double>[] copy = new List<double>[args.Length];
                for(int i=0; i<args.Length;i++)
                {
                    copy[i] = new List<double>();
                }
                int j = 0;
                foreach (IEnumerable<double> list in args)
                {
                    foreach(double element in list)
                    {
                        copy[j].Add(element);
                    }
                    j++;
                }
                return _calculateChiSquaredTest(copy);
            }
            
        }
        public static TestResult CalculateChiSquaredTest(params IEnumerable<int>[] args)
        {
            if (args.Length == 1)
            {
                List<double> copy = new List<double>();
                foreach (IEnumerable<int> item in args)
                {
                    foreach (int element in item)
                        copy.Add((double)element);
                }
                return _calculateChiSquaredTest(copy);
            }
            else
            {
                List<double>[] copy = new List<double>[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    copy[i] = new List<double>();
                }
                int j = 0;
                foreach (IEnumerable<int> list in args)
                {
                    foreach (int element in list)
                    {
                        copy[j].Add((double)element);
                    }
                    j++;
                }
                return _calculateChiSquaredTest(copy);
            }
        }

        public static Correlation CalculateSpearmanCorrelation(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add(item);
            foreach (double item in list2) copy2.Add(item);
            return _calculateSpearmanCorrelation(copy1, copy2);
        }
        public static Correlation CalculateSpearmanCorrelation(this IEnumerable<int> list1, IEnumerable<int> list2)
        {
            List<double> copy1 = new List<double>();
            List<double> copy2 = new List<double>();
            foreach (double item in list1) copy1.Add((double)item);
            foreach (double item in list2) copy2.Add((double)item);
            return _calculateSpearmanCorrelation(copy1, copy2);
        }
        private static Correlation _calculateSpearmanCorrelation(List<double> list1, List<double> list2)
        {
            if (list1.Count() != list2.Count()) throw new SizeOutOfRangeException();
            int n = list1.Count();
            if (n == 0) throw new EmptyCollectionException();
            if (n < 3) throw new NotTheRightSizeException();
            List<double> d = new List<double>();
            Dictionary<double, double> xRanks = Ranks.CalculateRanks(list1);
            Dictionary<double, double> yRanks = Ranks.CalculateRanks(list2);

            double rSum = 0;

            for (int i = 0; i < n; i++)
            {
                d.Add(xRanks[Math.Abs(list1.ElementAt(i))] - yRanks[Math.Abs(list2.ElementAt(i))]);
                rSum += d.ElementAt(i) * d.ElementAt(i);
            }

            double r;
            int nd = d.Count();
            if(Ranks.SumOfTiedPairs(list1)==0 && Ranks.SumOfTiedPairs(list2) == 0)
            {
                r= 1.0 - (6 * rSum) / ((double)nd * ((double)nd * (double)nd - 1.0));
            }
            else
            {
                double sumX = (nd * nd * nd - nd - Ranks.SumOfTiedPairs(list1)) / 12.0;
                double sumY = (nd * nd * nd - nd - Ranks.SumOfTiedPairs(list2)) / 12.0;
                r = (sumX + sumY - rSum) / (2 * Math.Sqrt(sumX*sumY));

            }

            double t = r * Math.Sqrt(((double)n - 2.0) / (1.0 - r * r));
            double pval =ContinuousDistribution.Student(t,n-2);

            return new Correlation
            {
                CorrelationCoefficient = Math.Round(r, 6),
                StudentsTValue = Math.Round(t, 6),
                PValue = Math.Round(pval, 5)
            };
        }
    }
}
