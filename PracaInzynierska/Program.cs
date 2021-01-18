using PracaInzynierska.Distribution;
using PracaInzynierska.Statystyka;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PracaInzynierska
{
    class Program
    {
        static void Main(string[] args)
        {
            //double[] tablicaDouble1 = {4.13,4.53,4.69, 4.76,4.77,4.96,4.97,5,5.04,5.10,5.25,5.36,5.94,6.06,6.19,6.30,7.73};
           
           
            double[] t1 = { 56, 75, 45, 71, 62, 64, 58, 80, 76, 61 };
            double[] t2 = { 66, 70, 40, 60, 65, 56, 59, 77, 67, 63 };

            double[] tablicaDouble1 = { 1, 2, 3, 4, 5 };
            double[] tablicaDouble2 = { 1, 2, 3, 4 };
            double[] tablicaDouble3 = { 1, 1, 1, 2, 2 };
            double[] tablicaDouble4 = { 1,1,1,2};

            double[] x = { 44.4, 45.9, 41.9, 53.3, 44.7, 44.1, 50.7, 45.2, 60.1 };
            double[] y = { 2.6, 3.1, 2.5, 5.0, 3.6, 4.0, 5.2, 2.8, 3.8 };

            double[] mem = { 7, 15, 36, 39, 40, 41 };

            double[] da = {2,3.6,2.6,2.6,7.3,3.4,14.9,6.6,2.3,2,6.8,8.5 };
            double[] db = {3.5,5.7,2.9,2.4,9.9,3.3,16.7,6.0,3.8,4,9.1,20.9 };

            List<double> list = new List<double>{ 1, 1, 1, 2, 2 };

            double q1 = 0, q2 = 0, q3 = 0;
            double chi_sqrt= Statystyki.CalculatePearsonsCorrelation(t1,t2).StudentsTValue;
            // double pval = Statystyki.CalculateKruskalaWalisaTest(tablicaDouble2, tablicaDouble4).PValue;

            double ss = (54 * 54 * (1 - 0.6727273 * 0.6727273)) / 0.6727273 * 0.6727273 + 2;
            double pval = ContinuousDistribution.Student(0.975, 3);


            double[]a1 = {6,8,4,5,3,4  };
            double[] a2 = {8,12,9,11,6,8  };
            double[] a3 = {13,9,11,8,7,12 };
            //double anova = ANOVA.AnovaFriedmanaTest(a1, a2, a3).TestValue;
            //Console.WriteLine("anova " + anova);


            double[] k1 = { 3415,1593,1976,1526,1538,983,1050,1861,1714,1320,1276,1263,1271,1436};
            double[] k2 = { 4556,1937,2056,1594,1634,1086,1209,2087,2415,1621,1377,1279,1417,1310};
            double[] k3 = {5772,2242,2240,1644,1866,1135,1245,2054,2361,1624,1522,1350,1583,1357};
            double[] k4 = { 5772, 2794,2085,1705,1769,1177,977,2018,2424,1551,1412,1490,1513,1468};
            double anova = ANOVA.OneWayAnalysisOfVariance(a1, a2, a3).TestValue;
            Console.WriteLine("anova " + anova);


        }
    }
}
