﻿using PracaInzynierska.DescriptiveStatistics;
using PracaInzynierska.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using static PracaInzynierska.DescriptiveStatistics.Ranks;

namespace PracaInzynierska.Statystyka
{
    //TODO: optymalizacja - wszystkie analizy od razu i zapisywanie do struktury

    public static class Statystyki
    {
        public static double CalculateMean(this IEnumerable<double> list)
        {
            return list.Average();
        }

        public static double CalculateMean(this IEnumerable<int> list)
        {
            return list.Select(i => (double)i).CalculateMean();
        }

        public static double CalculateMean(this IEnumerable<long> list)
        {
            return list.Select(l => (double)l).CalculateMean();
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

        public static double CalculateRange(this IEnumerable<double> list) //range
        { 
            return list.Max() - list.Min(); //nieoptymalne, wystarczyłaby jedna pętla zamiast dwóch     
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
            switch(type)
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
            List<double> kopia = new List<double>();
            foreach (float wartość in list) kopia.Add((double)wartość);
            return CalculateStandardDeviation(kopia, type);
        }

        public static double CalculateStandardDeviation(this IEnumerable<int> list, StandardDeviationType type = StandardDeviationType.Population)
        {
            List<double> kopia = new List<double>();
            foreach (int wartość in list) kopia.Add((double)wartość);
            return CalculateStandardDeviation(kopia, type);
        }

        //https://en.wikipedia.org/wiki/Quartile, Method2
        public static void CalculateQuartile(this IEnumerable<double> list, out double q1, out double q2, out double q3)
        {
            List<double> _list = list.ToList();
            _list.Sort();

            List<double> lowerHalf = new List<double>();
            List<double> upperHalf = new List<double>();

            if (_list.Count % 2 != 0)
            {
                q2 = _list[_list.Count / 2];
                for (int i = 0; i < _list.Count / 2+1; ++i) lowerHalf.Add(_list[i]);
                for (int i = _list.Count / 2; i < _list.Count; ++i) upperHalf.Add(_list[i]);
            }
            else
            {
                q2 = (_list[_list.Count / 2 - 1] + _list[_list.Count / 2]) / 2.0;
                for (int i = 0; i < _list.Count/2; ++i) lowerHalf.Add(_list[i]);
                for (int i = _list.Count / 2; i < _list.Count; ++i) upperHalf.Add(_list[i]);
            }

            q1 = _calculateMedian(lowerHalf);
            q3 = _calculateMedian(upperHalf);
        }

        public static double CalculateQuarterRange(this IEnumerable<double> list)
        {
            if (list.Count() < 3) throw new Exception("Danych jest zbyt mało, żeby obliczyć kwartyle (n=" + list.Count() + ")");
            double q1,q2,q3;
            CalculateQuartile(list, out q1, out q2, out q3);
            return q3 - q1;
        }

        public static double CalculateQuarterlyVariance(this IEnumerable<double> list)
        {
            return CalculateQuarterRange(list) / 2;
        }

        private static double _calculateMedian(List<double> list)
        {
            if (list.Count == 0) throw new Exception("The list does not contain items");

            list.Sort();

            if (list.Count % 2 != 0)
                return list[list.Count / 2];
            else
                return (list[list.Count / 2 - 1] + list[list.Count / 2]) / 2.0;
        }

        public static double CalculateMedian(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateMedian(copy);
        }

        public static double CalculateMedian(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateMedian(copy);
        }

        public static double CalculateMedian(this IEnumerable<long> list)
        {
            List<double> copy = new List<double>();
            foreach (long item in list) copy.Add((double)item);
            return _calculateMedian(copy);
        }
        private static double _calculateSkewness(List<double> list,int type=3)
        {
            if (list.Count == 0) throw new Exception("The list does not contain items");

            double m3 = 0;
            double m2 = 0;
            int n = list.Count();
            double mean = CalculateMean(list);
            foreach(double item in list)
            {
                m3 += (item - mean) * (item - mean) * (item - mean);
                m2 += (item - mean) * (item - mean) ;
            }
            double A;
            switch (type)
            {
                case 1:
                    A = (Math.Sqrt(n) * m3) / Math.Sqrt(m2 * m2 * m2);
                    break;
                case 2:
                    A= (n*Math.Sqrt(n - 1) * m3) / (Math.Sqrt(m2 * m2 * m2)* (n - 2));
                    break;
                case 3:
                    A = (Math.Sqrt(n) * m3) / Math.Sqrt(m2 * m2 * m2) 
                        *Math.Sqrt((1 - 1.0 / n)* (1 - 1.0 / n)* (1 - 1.0 / n));
                    break;
                default:
                    throw new Exception("Invalid 'type' argument.");
            }

            return Math.Round(A,7);
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

        //TODO: histogram, dominanta/moda/modalna, kurtoza
        public static List<double> _calculateMode(List<double> list)
        {
            if (list.Count == 0) throw new Exception("The list does not contain items");

            List<double> currentMax = new List<double>();

            Dictionary<double, int> dictNumberOfOccurrencesOfElements = list.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            if (!((dictNumberOfOccurrencesOfElements.Values.GroupBy(x => x).Where(x => x.Count() >= 1))
                .Count() > 1)) throw new Exception("There is no dominant feature in a given set");
            
            int occurences = 0;

            foreach (KeyValuePair<double,int> x in dictNumberOfOccurrencesOfElements)
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
            return currentMax;
        }


        public static List<double> CalculateMode(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add(item);
            return _calculateMode(copy);

        }
        public static List<double> CalculateMode(this IEnumerable<long> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateMode(copy);

        }
        public static List<double> CalculateMode(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateMode(copy);
        }

        private static double _calculateKurtosis(List<double> list)
        {
            int n = list.Count();
            double mean = CalculateMean(list);
            double m2 = 0;
            double m4 = 0;
            foreach (double item in list)
            {
                m2 += (item - mean) * (item - mean);
                m4 += (item - mean) * (item - mean) * (item - mean) * (item - mean);
            }
            double k = (n * m4) / (m2 * m2);
            return Math.Round(k, 6);
        }

        public static double CalculateKurtosis(this IEnumerable<double> list)
        {
            List<double> copy = new List<double>();
            foreach (double item in list) copy.Add(item);
            return _calculateKurtosis(copy);
        }
        public static double CalculateKurtosis(this IEnumerable<int> list)
        {
            List<double> copy = new List<double>();
            foreach (int item in list) copy.Add((double)item);
            return _calculateKurtosis(copy);
        }
        public static double CalculateKurtosis(this IEnumerable<long> list)
        {
            List<double> copy = new List<double>();
            foreach (long item in list) copy.Add((double)item);
            return _calculateKurtosis(copy);
        }
        public struct LinearCorrelation
        {
            public int SampleSize;
            public double PearsonsCorrelationCoefficient;
            public double NormalDistributionPValue;
            public double StudentsTValue;
            public double PValueForStudentTDistribution;//musi być mniejsza od 0.05, żeby odrzucić null hipothesis, czyli brak korelacji -> jest korelacja jeżeli p < 0.05

        }

        public static LinearCorrelation CalculatePearsonsCorrelationCoefficient(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            //wzór: https://pl.wikipedia.org/wiki/Wsp%C3%B3%C5%82czynnik_korelacji_Pearsona
            //powinno być dzielenie przez n, ale się znosi
            //p i t: http://support.minitab.com/en-us/minitab-express/1/help-and-how-to/modeling-statistics/regression/how-to/correlation/methods-and-formulas/
            //https://www.youtube.com/watch?v=QoV_TL0IDGA
            //wyjaśnienia: http://trendingsideways.com/index.php/the-p-value-formula-testing-your-hypothesis/  

            if (list1.Count() != list2.Count()) throw new Exception("Kolekcje, dla których liczony jest współczynnik korelacji nie są równoliczne");
            //int n = Math.Min(list1.Count(), list2.Count()); 
            int n = list1.Count(); //Czy nie wystarczy czegoś takiego? bo zakładamy że oni równe 
            double average1 = CalculateMean(list1);
            double average2 = CalculateMean(list2);

            //nie korzystam z metody do obliczania odchyleń standardowych, żeby zamiast trzech pętli była tylko jedna
            //we wszystkich pomijam dzielenie przez liczbę elementów
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

            double r = covariance / Math.Sqrt(standardDeviation1* standardDeviation2);
            double tDistribution = r * Math.Sqrt((n - 2) / (1 - r * r)); ; // Student's t-distribution with degrees of freedom n − 2
            double p = ContinuousDistribution.Student(tDistribution, n-2);


            double tValue = (average1-average2) / Math.Sqrt(standardDeviation1 * standardDeviation1 + standardDeviation2 * standardDeviation2);
            double pForT = ContinuousDistribution.Student(tValue, degreeOfFreedom);
            return new LinearCorrelation()
            {
                SampleSize = n,
                PearsonsCorrelationCoefficient = Math.Round(r,7),
                NormalDistributionPValue = Math.Round(p,5),
                StudentsTValue= tDistribution,
                PValueForStudentTDistribution= pForT
            };
        }
        
        public static Test CalculateStudentsTTest(this IEnumerable<double> list1, double hypotheticalMean=0)
        {
            int n = list1.Count();
            double average = CalculateMean(list1);
            double standardDeviation = CalculateStandardDeviation(list1);

            double tforOneSample = ((average - hypotheticalMean) / standardDeviation) * Math.Sqrt(n);
            int df = 1;
            double p = ContinuousDistribution.Student(tforOneSample,n-1);
            return new Test()
            {
                TestValue = Math.Round(tforOneSample, 4),
                DegreesOfFreedom = df,
                PValue = Math.Round(p,5)
            };
        }
        public static Test CalculateStudentsTTest(this IEnumerable<double> list1,IEnumerable<double> list2,bool pairs=false)
        {
            int n1 = list1.Count();
            int n2 = list2.Count();

            double average1 = CalculateMean(list1);
            double average2 = CalculateMean(list2);

            double standardDeviation1 = CalculateStandardDeviation(list1);
            double standardDeviation2 = CalculateStandardDeviation(list2);
            double t=0;
            double df=0;
           
            if (!pairs)
            {
                double sd1 = (standardDeviation1 * standardDeviation1) / (double)n1;
                double sd2 = (standardDeviation2 * standardDeviation2) / (double)n2;
                double standardDeviationForTwoList = ((((double)n1 - 1.0) * standardDeviation1 * standardDeviation1 + ((double)n2 - 1.0) * standardDeviation2 * standardDeviation2) / ((double)n1 + (double)n2 - 2.0));
                if (n1 == n2)
                {
                    t = (average1 - average2) / (Math.Sqrt(standardDeviationForTwoList * ((1.0 / (double)n1) + (1.0 / (double)n2))));
                    df = n1 + n2 - 1;
                }
                else
                {
                    //Test t-Studenta z korektą Cochrana-Coxa
                    //jest wyliczana wówczas, gdy wariancje badanych zmiennych w obu populacjach są różne.
                    t = (average1 - average2) / Math.Sqrt(sd1 + sd2);
                }
                df = ((sd1 + sd2) * (sd1 + sd2)) / ((sd1 * sd1 * (1 / ((double)n1 - 1))) + (sd2 * sd2 * (1 / ((double)n2 - 1))));
            }
            else {
                if (n1 != n2) throw new ArgumentException("nie wszystkie argumenty mają tę samą długość");
                List<double> listOfPairs = Util.DifferenceBetweenPairsOfMeasurements(list1, list2);
                double averageForPairs = CalculateMean(listOfPairs);
                double standardDeviationForPairs = CalculateStandardDeviation(listOfPairs);
                t = averageForPairs / standardDeviationForPairs * Math.Sqrt(listOfPairs.Count());
                df = n1 - 1;
            }

            double p = ContinuousDistribution.Student(t, df);
            return new Test()
            {
                TestValue = Math.Round(t,4),
                DegreesOfFreedom= Math.Round(df,4),
                PValue= Math.Round(p, 4)
            };
        }
     
        public struct WilcoxonTest
        {
            public double T;
            public double TCritic;
            public double ZStatistic;
            public double PValue;
        }
        public static WilcoxonTest CalculateWilcoxonTest(this IEnumerable<double> list,double hypotheticalMedian=0)
        {
            int n = list.Count();
            Rank r;
            if (hypotheticalMedian != 0)
                r = Ranks.CalculateRankForWilcoxonTest(Util.DifferenceBetweenPairsOfMeasurements(list, hypotheticalMedian));
            else
                r = Ranks.CalculateRankForWilcoxonTest(list);

            double t = Math.Max(r.SumOfNegativeRanks, r.SumOfPositiveRanks);

            double nRanks = r.NumberOfRanks;

            
            double zValue = (Math.Abs(t - ((nRanks * (nRanks + 1.0)) / 4.0)) - 0.5) / Math.Sqrt(((nRanks * (nRanks + 1.0) * (2.0 * nRanks + 1.0)) / 24.0) - r.CorrectionForTiedRanks);

            double p = 2*ContinuousDistribution.Gauss(-Math.Abs(zValue));

            return new WilcoxonTest
            {
                T = t,
                ZStatistic = zValue,
                PValue = Math.Round(p, 4)
            };
        }
        public static WilcoxonTest CalculateWilcoxonTest(this IEnumerable<double> list1,IEnumerable<double> list2, bool pairs = false)
        {
           
            if (pairs) {
                int n = Math.Max(list1.Count(), list2.Count());
                Rank r = Ranks.CalculateRankForWilcoxonTest(Util.DifferenceBetweenPairsOfMeasurements(list1, list2));
                foreach (var el in Util.DifferenceBetweenPairsOfMeasurements(list1, list2))
                {
                    Console.WriteLine(el);
                }
                double t = Math.Max(r.SumOfNegativeRanks, r.SumOfPositiveRanks);
                double nRanks = (double)r.NumberOfRanks;

                double zValue = ((Math.Abs(t - ((nRanks * (nRanks + 1)) / 4))) - 0.5) / Math.Sqrt(((nRanks * (nRanks + 1) * (2 * nRanks + 1)) / 24) - r.CorrectionForTiedRanks);

                double p = 2 * ContinuousDistribution.Gauss(-Math.Abs(zValue));

                return new WilcoxonTest
                {
                    T = t,
                    ZStatistic = zValue,
                    PValue = Math.Round(p, 4)
                };
            }
            else
            {
                return CalculateTestUMannaWhitneya(list1, list2);
            }
          
        }

        public static WilcoxonTest CalculateTestUMannaWhitneya(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
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

            return new WilcoxonTest
            {
                T = u2,
                ZStatistic = zValue,
                PValue = Math.Round(p, 4)
            };
        }

        public struct Test
        {
            public double TestValue;
            public double PValue;
            public double DegreesOfFreedom;
        }
        public static Test CalculateKolmogorovSmirnovTestForNormality(this IEnumerable<double> list)
        {
            list = list.OrderBy(x => x);
            int n = list.Count();
            List<double> Di = new List<double>();
            List<double> D_i = new List<double>();
           
            double average = CalculateMean(list);
            double standartDeviation = CalculateStandardDeviation(list);


            for (int i = 0; i < n; i++)
            {
                Di.Add(Math.Abs((((double)i+1.0)/(double)n)-Util.Phi((list.ElementAt(i)-average)/standartDeviation)));
                D_i.Add(Math.Abs(Util.Phi((list.ElementAt(i) - average) / standartDeviation) - ((double)i / (double)n)));
            }
            double d = (Math.Max(Di.Max(), D_i.Max()));
            double z = d * Math.Sqrt((double)n);

            //P- value from 
            //https://stats.stackexchange.com/questions/389034/kolmogorov-smirnov-test-calculating-the-p-value-manually
            double pValue = 0;
            double zz = z * z;
            for (int i =1; i < 1000; i++)
            {
                pValue += (Math.Pow((-1.0), ((double)i - 1.0))) * (Math.Exp((-2.0) * ((double)i * (double)i) * zz));
            }
            double p =2.0 * pValue;
            return new Test
            {
                TestValue = Math.Round(d, 5),
                PValue= Math.Round(p, 4)
            }; 
        }
        private static double poly(double[] cc, int nord, double x)
        {
            /* Algorithm AS 181.2    Appl. Statist.    (1982) Vol. 31, No. 2
            Calculates the algebraic polynomial of order nord-1 with array of coefficients cc. 
            Zero order coefficient is cc(1) = cc[0] */

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

        public static Test CalculateShapiroWilkTestForNormality(this IEnumerable<double> list)
        {
            if (list.Count()<3 || list.Count()>5000) throw new Exception("Kolekcja, dla której liczony jest współczynnik korelacji jest za mała lubza duża");
            list = list.OrderBy(x => x);
            int n = list.Count();
            int nn2 = n / 2;
            double[] a = new double[nn2 + 1]; /* 1-based */

            double small = 1e-19;

            /* polynomial coefficients */
            double[] g = { -2.273, 0.459 };
            double[] c1 = { 0.0, 0.221157, -0.147981, -2.07119, 4.434685, -2.706056 };
            double[] c2 = { 0.0, 0.042981, -0.293762, -1.752461, 5.682633, -3.582633 };
            double[] c3 = { 0.544, -0.39978, 0.025054, -6.714e-4 };
            double[] c4 = { 1.3822, -0.77857, 0.062767, -0.0020322 };
            double[] c5 = { -1.5861, -0.31082, -0.083751, 0.0038915 };
            double[] c6= { -0.4803, -0.082676, 0.0030302 };

            /* Local variables */
            int i, j, i1;

            double ssassx, summ2, ssumm2, gamma, range;
            double a1, a2, an, m, s, sa, xi, sx, xx, y, w1;
            double fac, asa, an25, ssa, sax, rsn, ssx, xsx;

            double pw = 1.0;
            an = (double)n;

            if (n == 3)
            {
                a[1] = 0.70710678;/* = sqrt(1/2) */
            }
            else
            {
                an25 = an + 0.25;
                summ2 = 0.0;
                for (i = 1; i <= nn2; i++)
                {
                    a[i] = ContinuousDistribution.NormalQuantile((i - 0.375) / an25, 0, 1); // p(X <= x),  
                    summ2 += a[i] * a[i];
                }
                summ2 *= 2.0;
                ssumm2 = Math.Sqrt(summ2);
                rsn = 1.0 / Math.Sqrt(an);
                a1 = poly(c1, 6, rsn) - a[1] / ssumm2;

                /* Normalize a[] */
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

            /* Check for zero range */

            range = list.ElementAt(n - 1) - list.ElementAt(0);
            if (range < small)
            {
                //console.log('range is too small!'); 
                throw new ArgumentException();
            }


            /* Check for correct sort order on range - scaled X */

            xx = list.ElementAt(0) / range;
            sx = xx;
            sa = -a[1];
            for (i = 1, j = n - 1; i < n; j--)
            {
                xi = list.ElementAt(i) / range;
                if (xx - xi > small)
                {
                    //console.log("xx - xi is too big.", xx - xi); 
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


            /* Calculate W statistic as squared correlation between data and coefficients */

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

            /* W1 equals (1-W) calculated to avoid excessive rounding error        for W very near 1 (a potential problem in very large samples) */

            ssassx = Math.Sqrt(ssa * ssx);
            w1 = (ssassx - sax) * (ssassx + sax) / (ssa * ssx);
            double w = 1.0 - w1;
            double pValue=0;
            /* Calculate significance level for W */

            if (n == 3)
            {/* exact P value : */
                double pi6 = 1.90985931710274; /* = 6/pi */
                double stqr = 1.04719755119660; /* = asin(sqrt(3/4)) */
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
                    pw = 1e-99; /* an "obvious" value, was 'small' which was 1e-19f */
                    //return w; 
                    pValue= pw;
                }
                y = -Math.Log(gamma - y);
                m = poly(c3, 4, an);
                s = Math.Exp(poly(c4, 4, an));
            }
            else
            { /* n >= 12 */
                m = poly(c5, 4, xx);
                s = Math.Exp(poly(c6, 3, xx));
            }

            double z = (y - m) / s;
            if(pValue == 0)
            {
                if(w>0.95)
                    pValue = Math.Round(1 - ContinuousDistribution.Gauss(-Math.Abs(z)),4);
                else
                    pValue = Math.Round(ContinuousDistribution.Gauss(-Math.Abs(z)),5);

            }
            return new Test
            {
                TestValue = Math.Round(w, 5),
                PValue =pValue
            };
        }
        //https://stats.stackexchange.com/questions/381873/meaning-of-chi-squared-in-r-kruskal-wallis-test
        public static Test CalculateKruskalaWalisaTest(this IEnumerable<double> list1,IEnumerable<double> list2)
        {
            if (list1.Count() != list2.Count()) throw new Exception("Kolekcje, dla których liczony jest współczynnik korelacji nie są równoliczne");

            List<double> factorList = list2.Distinct().ToList();

            List<double> list = list1.Concat(list2).ToList();
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
            double pVal = 1.0- ContinuousDistribution.chisquareCdf(kwScore,df);
            return new Test
            {
                TestValue = kwScore,
                PValue = Math.Round(pVal,4),
                DegreesOfFreedom=df
            };
        }

        public struct FTest
        {
            public double RatioOfVariances;
            public int NumDf;
            public int DenomDf;
            public double PValue;
        }

        //Use F test to compare two variances
        //
        public static FTest CalculateFTestToCompareTwoVariances(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            int n = list1.Count();
            int m = list2.Count();

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

        //https://www.bmj.com/about-bmj/resources-readers/publications/statistics-square-one/8-chi-squared-tests

        public static Test CalculateChiSquaredTest(this IEnumerable<double> list)
        {
            double sumN = 0;

            int n = list.Count();
            double statistic = 0;
            for (int i = 0; i < n; i++)
            {
                sumN += list.ElementAt(i);
            }

            double expectedA = sumN / n;

            for (int i = 0; i < n; i++)
            {
                statistic += ((list.ElementAt(i) - expectedA) * (list.ElementAt(i) - expectedA)) / expectedA;
            }
            int df = n - 1;
            double pval = 1.0 - ContinuousDistribution.chisquareCdf(statistic, df);

            return new Test
            {
                TestValue = Math.Round(statistic, 5),
                DegreesOfFreedom = df,
                PValue = Math.Round(pval, 4)
            };
        }
        public static Test CalculateChiSquaredTest(params IEnumerable<double>[] args)
        {
            int nCols = args.Length;
            int nRows = args.FirstOrDefault().Count();
            foreach (IEnumerable<double> list in args)
            {
                if (nRows != list.Count()) throw new Exception("Kolekcje, dla których liczony jest współczynnik korelacji nie są równoliczne");
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
            double pval = 1.0 - ContinuousDistribution.chisquareCdf(statistic, df);
            return new Test
            {
                TestValue = Math.Round(statistic, 5),
                DegreesOfFreedom = df,
                PValue = Math.Round(pval, 4)
            };
        }

        public static Test CalculateSpearmanCorrelation(this IEnumerable<double> list1, IEnumerable<double> list2)
        {
            int n = list1.Count();
            List<double> d = new List<double>();
            double r = 0;

            for (int i = 0; i < n; i++)
            {
                d.Add(Ranks.CalculateRanks(list1)[Math.Abs(list1.ElementAt(i))] - Ranks.CalculateRanks(list2)[Math.Abs(list2.ElementAt(i))]);
                r += d.ElementAt(i) * d.ElementAt(i);
            }
            Console.WriteLine("r: " + r);

            double p =1.0- (6 * r) / ((double)n * ((double)n * (double)n - 1.0));
            double t = p * Math.Sqrt((n - 2.0) / (1.0 - p * p));
            double pval =ContinuousDistribution.Student(t,n-2);


            return new Test
            {
                TestValue = Math.Round(p, 7),
                PValue = Math.Round(pval, 5)
            };
        }

      
    }
}
