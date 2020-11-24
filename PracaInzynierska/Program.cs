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
           
           
            double[] t1 = {7,87,12,9 };
            double[] t2 = { 4,102,7,8};

            double[] tablicaDouble1 = { 1, 2, 3, 4, 5 };
            double[] tablicaDouble2 = { 1, 2, 3, 4 };
            double[] tablicaDouble3 = { 1, 1, 1, 2, 2 };
            double[] tablicaDouble4 = { 1,1,1,2};


            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).WValue);
            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).SumOfPositiveRanks);
            //Console.WriteLine("W: " + Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).SumOfNegativeRanks);

            //Console.WriteLine("P: " + Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).NormalDistributionPValue);
            //Console.WriteLine("P: " + Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).PearsonsCorrelationCoefficient);


            // double t1t3 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3).NormalDistributionPValue;
            //Console.WriteLine("df" + t1t3);
            double test = Statystyki.CalculateChiSquaredTest(t1,t2).TestValue;
           // double pval = Statystyki.CalculateKruskalaWalisaTest(tablicaDouble2, tablicaDouble4).PValue;

            Console.WriteLine("test " + test);
            //Console.WriteLine("pval " + pval);
            //Console.WriteLine("Max " + Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble2).SumOfPositiveRanks);
            //Console.WriteLine("Min " + Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble2).SumOfNegativeRanks);
        }
    }
}
