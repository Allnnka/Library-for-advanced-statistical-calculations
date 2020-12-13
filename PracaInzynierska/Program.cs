using PracaInzynierska.Statystyka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).WValue);
            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).SumOfPositiveRanks);
            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).SumOfNegativeRanks);

            //Console.WriteLine("P: " + Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).NormalDistributionPValue);
            //Console.WriteLine("P: " + Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).PearsonsCorrelationCoefficient);


            // double t1t3 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3).NormalDistributionPValue;
            //Console.WriteLine("df" + t1t3);
            double test = Statystyki.CalculateSpearmanCorrelation(x,y).PValue;
           // double pval = Statystyki.CalculateKruskalaWalisaTest(tablicaDouble2, tablicaDouble4).PValue;

            Console.WriteLine("test " + test);
            //Console.WriteLine("pval " + pval);
            //Console.WriteLine("Max " + Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble2).SumOfPositiveRanks);
            //Console.WriteLine("Min " + Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble2).SumOfNegativeRanks);
        }
    }
}
