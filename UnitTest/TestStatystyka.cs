﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PracaInzynierska.Statystyka;


namespace TestAnalizaWyników
{
    [TestClass]
    public class TestStatystyka
    {
        readonly double[] tableDouble1 = { 1, 2, 3, 4, 5 };
        readonly double[] tableDouble2 = { 1, 2, 3, 4 };
        readonly double[] tableDouble3 = { 1, 1, 1, 2, 2 };
        readonly double[] tableDouble4 = { 1, 1, 1, 2 };

        readonly int[] tableInt1 = { 1, 2, 3, 4, 5 };
        readonly int[] tableInt2 = { 1, 2, 3, 4 };
        readonly int[] tableInt3 = { 1, 1, 1, 2, 2 };
        readonly int[] tableInt4 = { 1, 1, 1, 2 };

        [TestMethod]
        public void TestObliczMedianęDouble()
        {
            double median1 = Statystyki.CalculateMedian(tableDouble1);
            double median2 = Statystyki.CalculateMedian(tableDouble2);
            double median3 = Statystyki.CalculateMedian(tableDouble3);
            double median4 = Statystyki.CalculateMedian(tableDouble4);

            Assert.AreEqual(3, median1);
            Assert.AreEqual(2.5, median2);
            Assert.AreEqual(1, median3);
            Assert.AreEqual(1, median4);
        }

        [TestMethod]
        public void TestObliczMedianęInt()
        {
            double median1 = Statystyki.CalculateMedian(tableInt1);
            double median2 = Statystyki.CalculateMedian(tableInt2);
            double median3 = Statystyki.CalculateMedian(tableInt3);
            double median4 = Statystyki.CalculateMedian(tableInt4);

            Assert.AreEqual(3, median1);
            Assert.AreEqual(2.5, median2);
            Assert.AreEqual(1, median3);
            Assert.AreEqual(1, median4);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPróbkiDouble()
        {
            double standardDeviation1 = Statystyki.CalculateStandardDeviation(tableDouble1, Statystyki.StandardDeviationType.Samples);
            double standardDeviation2 = Statystyki.CalculateStandardDeviation(tableDouble2, Statystyki.StandardDeviationType.Samples);
            double standardDeviation3 = Statystyki.CalculateStandardDeviation(tableDouble3, Statystyki.StandardDeviationType.Samples);
            double standardDeviation4 = Statystyki.CalculateStandardDeviation(tableDouble4, Statystyki.StandardDeviationType.Samples);

            //wartości wzorcowe z eksela =ODCH.STANDARD.PRÓBKI(A1:A5)
            double permissibleError = 0.000001;
            Assert.AreEqual(1.58113883, standardDeviation1, permissibleError);
            Assert.AreEqual(1.290994449, standardDeviation2, permissibleError);
            Assert.AreEqual(0.547722558, standardDeviation3, permissibleError);
            Assert.AreEqual(0.5, standardDeviation4, permissibleError);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPopulacjiDouble()
        {
            double standardDeviation1 = Statystyki.CalculateStandardDeviation(tableDouble1, Statystyki.StandardDeviationType.Population);
            double standardDeviation2 = Statystyki.CalculateStandardDeviation(tableDouble2, Statystyki.StandardDeviationType.Population);
            double standardDeviation3 = Statystyki.CalculateStandardDeviation(tableDouble3, Statystyki.StandardDeviationType.Population);
            double standardDeviation4 = Statystyki.CalculateStandardDeviation(tableDouble4, Statystyki.StandardDeviationType.Population);

            //wartości wzorcowe z eksela =ODCH.STANDARD.POPUL.A(A1:A5)
            double permissibleError = 0.000001;            
            Assert.AreEqual(1.414213562, standardDeviation1, permissibleError);
            Assert.AreEqual(1.118033989, standardDeviation2, permissibleError);
            Assert.AreEqual(0.489897949, standardDeviation3, permissibleError);
            Assert.AreEqual(0.433012702, standardDeviation4, permissibleError);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPopulacjiInt()
        {
            double standardDeviation1 = Statystyki.CalculateStandardDeviation(tableInt1, Statystyki.StandardDeviationType.Population);
            double standardDeviation2 = Statystyki.CalculateStandardDeviation(tableInt2, Statystyki.StandardDeviationType.Population);
            double standardDeviation3 = Statystyki.CalculateStandardDeviation(tableInt3, Statystyki.StandardDeviationType.Population);
            double standardDeviation4 = Statystyki.CalculateStandardDeviation(tableInt4, Statystyki.StandardDeviationType.Population);

            //wartości wzorcowe z eksela =ODCH.STANDARD.POPUL.A(A1:A5)
            double permissibleError = 0.000001;
            Assert.AreEqual(1.414213562, standardDeviation1, permissibleError);
            Assert.AreEqual(1.118033989, standardDeviation2, permissibleError);
            Assert.AreEqual(0.489897949, standardDeviation3, permissibleError);
            Assert.AreEqual(0.433012702, standardDeviation4, permissibleError);
        }

        [TestMethod]
        public void TestObliczDominante()
        {
            List<double> moda3 = Statystyki.CalculateMode(tableDouble3);
            List<double> moda4 = Statystyki.CalculateMode(tableDouble4);


            Assert.ThrowsException<Exception>(() => Statystyki.CalculateMode(tableDouble1));
            Assert.ThrowsException<Exception>(() => Statystyki.CalculateMode(tableDouble2));
            CollectionAssert.AreEqual(new List<double>() {1}, moda3);
            CollectionAssert.AreEqual(new List<double>() {1}, moda4);

        }

        [TestMethod]
        public void TestPearsonsCorrelationCoefficient()
        {
            double cor1 = Statystyki.CalculatePearsonsCorrelationCoefficient(tableDouble1, tableDouble3).PearsonsCorrelationCoefficient;
            double cor2 = Statystyki.CalculatePearsonsCorrelationCoefficient(tableDouble2, tableDouble4).PearsonsCorrelationCoefficient;

            double cor1PValue = Statystyki.CalculatePearsonsCorrelationCoefficient(tableDouble1, tableDouble3).NormalDistributionPValue;
            double cor2PValue = Statystyki.CalculatePearsonsCorrelationCoefficient(tableDouble2, tableDouble4).NormalDistributionPValue;

            Assert.AreEqual(0.8660254, cor1);
            Assert.AreEqual(0.7745967, cor2);

            Assert.AreEqual(0.05767, cor1PValue);
            Assert.AreEqual(0.2254, cor2PValue);
        }

        [TestMethod]
        public void TestCalculateSingleSampleStudentsTTest()
        {
            double t1 = Statystyki.CalculateStudentsTTest(tableDouble1).TestValue;
            double t2 = Statystyki.CalculateStudentsTTest(tableDouble2).TestValue;
            double t3 = Statystyki.CalculateStudentsTTest(tableDouble3).TestValue;
            double t4 = Statystyki.CalculateStudentsTTest(tableDouble4).TestValue;

            Assert.AreEqual(4.2426, t1);
            Assert.AreEqual(3.873, t2);
            Assert.AreEqual(5.7155, t3);
            Assert.AreEqual(5, t4);

            double t1PValue = Statystyki.CalculateStudentsTTest(tableDouble1).PValue;
            double t2PValue = Statystyki.CalculateStudentsTTest(tableDouble2).PValue;
            double t3PValue = Statystyki.CalculateStudentsTTest(tableDouble3).PValue;
            double t4PValue = Statystyki.CalculateStudentsTTest(tableDouble4).PValue;
            Assert.AreEqual(0.01324, t1PValue);
            Assert.AreEqual(0.03047, t2PValue);
            Assert.AreEqual(0.00464, t3PValue);
            Assert.AreEqual(0.01539, t4PValue);

            double t1mu = Statystyki.CalculateStudentsTTest(tableDouble1, 3).TestValue;
            double t2mu = Statystyki.CalculateStudentsTTest(tableDouble2, 3).TestValue;
            double t3mu = Statystyki.CalculateStudentsTTest(tableDouble3, 2).TestValue;
            double t4mu = Statystyki.CalculateStudentsTTest(tableDouble4, 2).TestValue;


            Assert.AreEqual(0, t1mu);
            Assert.AreEqual(-0.7746, t2mu);
            Assert.AreEqual(-2.4495, t3mu);
            Assert.AreEqual(-3, t4mu);

            double t1muPValue = Statystyki.CalculateStudentsTTest(tableDouble1, 3).PValue;
            double t2muPValue = Statystyki.CalculateStudentsTTest(tableDouble2, 3).PValue;
            double t3muPvalue = Statystyki.CalculateStudentsTTest(tableDouble3, 2).PValue;
            double t4muPValue = Statystyki.CalculateStudentsTTest(tableDouble4, 2).PValue;

            Assert.AreEqual(1, t1muPValue);
            Assert.AreEqual(0.49503, t2muPValue);
            Assert.AreEqual(0.07048, t3muPvalue);
            Assert.AreEqual(0.05767, t4muPValue);

        }

        [TestMethod]
        public void TestCalculateStudentsTtestForIndependentGroups()
        {
            double t1t2 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble2).TestValue;
            double df12= Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble2).DegreesOfFreedom;
            double p12 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble2).PValue;

            double t2t3 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble3).TestValue;
            double df23 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble3).DegreesOfFreedom;
            double p23 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble3).PValue;

            double t1t3 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3).TestValue;
            double df13 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3).DegreesOfFreedom;
            double p13 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3).PValue;

            double t2t4 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4).TestValue;
            double df24 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4).DegreesOfFreedom;
            double p24 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4).PValue;

            Assert.AreEqual(0.5222, t1t2);
            Assert.AreEqual(6.9808, df12);
            Assert.AreEqual(0.6177,p12);

            Assert.AreEqual(1.5933, t2t3);
            Assert.AreEqual(3.8661, df23);
            Assert.AreEqual(0.1888, p23);

            Assert.AreEqual(2.1381, t1t3);
            Assert.AreEqual(4.9464, df13);
            Assert.AreEqual(0.0861, p13);

            Assert.AreEqual(1.8058, t2t4);
            Assert.AreEqual(3.8802, df24);
            Assert.AreEqual(0.1475, p24);
        }

        [TestMethod]
        public void TestCalculateStudentsTtestForDependentGroups()
        {
            double t1t3 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3, true).TestValue;
            double df13 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3, true).DegreesOfFreedom;
            double p13 = Statystyki.CalculateStudentsTTest(tableDouble1, tableDouble3, true).PValue;

            double t2t4 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4, true).TestValue;
            double df24 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4, true).DegreesOfFreedom;
            double p24 = Statystyki.CalculateStudentsTTest(tableDouble2, tableDouble4, true).PValue;


            Assert.AreEqual(3.1379, t1t3);
            Assert.AreEqual(4, df13);
            Assert.AreEqual(0.0349, p13);

            Assert.AreEqual(2.6112, t2t4);
            Assert.AreEqual(3, df24);
            Assert.AreEqual(0.0796, p24);
        }


        [TestMethod]
        public void TestCalculateWilcoxonTest()
        {
            double w1 = Statystyki.CalculateWilcoxonTest(tableDouble1).WValue;
            double w1PValue = Statystyki.CalculateWilcoxonTest(tableDouble1).PValue;

            double w1mu = Statystyki.CalculateWilcoxonTest(tableDouble1, 2).WValue;
            double w1muPValue = Statystyki.CalculateWilcoxonTest(tableDouble1, 2).PValue;

            double w2 = Statystyki.CalculateWilcoxonTest(tableDouble2).WValue;
            double w2PValue = Statystyki.CalculateWilcoxonTest(tableDouble2).PValue;


            double w3 = Statystyki.CalculateWilcoxonTest(tableDouble3).WValue;
            double w3PValue = Statystyki.CalculateWilcoxonTest(tableDouble3).PValue;

            double w4 = Statystyki.CalculateWilcoxonTest(tableDouble4).WValue;
            double w4PValue = Statystyki.CalculateWilcoxonTest(tableDouble4).PValue;

            double w4mu = Statystyki.CalculateWilcoxonTest(tableDouble4, 1).WValue;
            double w4muPvalue = Statystyki.CalculateWilcoxonTest(tableDouble4, 1).PValue;



            Assert.AreEqual(15, w1);
            //Assert.AreEqual(0.0625, w1PValue);

            Assert.AreEqual(8.5, w1mu);
            Assert.AreEqual(0.2693, w1muPValue);

            Assert.AreEqual(10, w2);
            //Assert.AreEqual(0.125, w2PValue);

            Assert.AreEqual(15, w3);
            //Assert.AreEqual(0.05334,w3PValue);

            Assert.AreEqual(10, w4);
            //Assert.AreEqual(0.08897, w4PValue);

            Assert.AreEqual(1, w4mu);
            Assert.AreEqual(1, w4muPvalue);

        }

        [TestMethod]
        public void TestWilcoxonMatchedPairsTest()
        {
            double w13 = Statystyki.CalculateWilcoxonTest(tableDouble1, tableDouble3).WValue;
            double w13PValue = Statystyki.CalculateWilcoxonTest(tableDouble1, tableDouble3).PValue;

            double w24 = Statystyki.CalculateWilcoxonTest(tableDouble2, tableDouble4).WValue;
            double w24PValue = Statystyki.CalculateWilcoxonTest(tableDouble2, tableDouble4).PValue;


            Assert.AreEqual(10, w13);
            Assert.AreEqual(0.0975,w13PValue);
            //wilcox.test(d2,d4, paired=TRUE) in RStudio
            Assert.AreEqual(6, w24);
            Assert.AreEqual(w24PValue,0.1736);

            double w14PValue = Statystyki.CalculateWilcoxonTest(tableDouble1, tableDouble4).PValue;
            //Assert.AreEqual(0.09481, w14PValue);
        }
        [TestMethod]
        public void TestKolmogorovSmirnovTests()
        {
            double d1 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble1).TestValue;
            double d1PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble1).PValue;

            double d2 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble2).TestValue;
            double d2PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble2).PValue;

            double d3 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble3).TestValue;
            double d3PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble3).PValue;

            double d4 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble4).TestValue;
            double d4PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tableDouble4).PValue;

            //ks.test(d1, "pnorm", mean=mean(d1), sd=sd(d1))
            Assert.AreEqual(0.13646, d1);
            //Assert.AreEqual(0.9998, d1PVal);

            Assert.AreEqual(0.15073, d2);
           // Assert.AreEqual(0.9998, d2PVal);

            Assert.AreEqual(0.3674, d3);
            Assert.AreEqual(0.5096, d3PVal);

            Assert.AreEqual(0.44146, d4);
            Assert.AreEqual(0.4167, d4PVal);

        }

        [TestMethod]
        public void TestShapiroWilkTest()
        {
            double w1 = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble1).TestValue;
            double w1PVal = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble1).PValue;

            double w2 = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble2).TestValue;
            double w2PVal = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble2).PValue;

            double w3 = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble3).TestValue;
            double w3PVal = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble3).PValue;

            double w4 = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble4).TestValue;
            double w4PVal = Statystyki.CalculateShapiroWilkTestForNormality(tableDouble4).PValue;


            //shapiro.test(d1)

            Assert.AreEqual(0.98676, w1);
            Assert.AreEqual(0.9672, w1PVal);

            Assert.AreEqual(0.99291, w2);
            Assert.AreEqual(0.9719, w2PVal);

            Assert.AreEqual(0.68403,w3);
            Assert.AreEqual(0.00647, w3PVal);

            Assert.AreEqual(0.62978, w4);
            Assert.AreEqual(0.00124, w4PVal);
        }

        [TestMethod]
        public void TestKruskalaWalisaTest()
        {
            double ch13 = Statystyki.CalculateKruskalaWalisaTest(tableDouble1, tableDouble3).TestValue;
            double ch13PVal = Statystyki.CalculateKruskalaWalisaTest(tableDouble1, tableDouble3).PValue;
            double ch13Df = Statystyki.CalculateKruskalaWalisaTest(tableDouble1, tableDouble3).DegreesOfFreedom;

            double ch31 = Statystyki.CalculateKruskalaWalisaTest(tableDouble3, tableDouble1).TestValue;
            double ch31PVal = Statystyki.CalculateKruskalaWalisaTest(tableDouble3, tableDouble1).PValue;
            double ch31Df = Statystyki.CalculateKruskalaWalisaTest(tableDouble3, tableDouble1).DegreesOfFreedom;

            double ch24 = Statystyki.CalculateKruskalaWalisaTest(tableDouble2, tableDouble4).TestValue;
            double ch24PVal = Statystyki.CalculateKruskalaWalisaTest(tableDouble2, tableDouble4).PValue;
            double ch24Df = Statystyki.CalculateKruskalaWalisaTest(tableDouble2, tableDouble4).DegreesOfFreedom;

            double ch42 = Statystyki.CalculateKruskalaWalisaTest(tableDouble4, tableDouble2).TestValue;
            double ch42PVal = Statystyki.CalculateKruskalaWalisaTest(tableDouble4, tableDouble2).PValue;
            double ch42Df = Statystyki.CalculateKruskalaWalisaTest(tableDouble4, tableDouble2).DegreesOfFreedom;

            Assert.AreEqual(3, ch13);
            Assert.AreEqual(0.0833, ch13PVal);
            Assert.AreEqual(1, ch13Df);

            Assert.AreEqual(4, ch31);
            Assert.AreEqual(0.406, ch31PVal);
            Assert.AreEqual(4, ch31Df);

            Assert.AreEqual(1.8, ch24);
            Assert.AreEqual(0.1797, ch24PVal);
            Assert.AreEqual(1, ch24Df);

            Assert.AreEqual(3, ch42);
            Assert.AreEqual(0.3916, ch42PVal);
            Assert.AreEqual(3, ch42Df);
        }

        [TestMethod]
        public void FTestToCompareTwoVariances()
        {
            //var.test(d1,d2)
            double f12 = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble2).RatioOfVariances;
            double f12numDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble2).NumDf;
            double f12DenomDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble2).DenomDf;
            double f12pVal = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble2).PValue;

            Assert.AreEqual(f12, 1.5);
            Assert.AreEqual(f12numDf, 4);
            Assert.AreEqual(f12DenomDf, 3);
            Assert.AreEqual(f12pVal, 0.7698);

            double f13 = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble3).RatioOfVariances;
            double f13numDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble3).NumDf;
            double f13DenomDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble3).DenomDf;
            double f13pVal = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble1, tableDouble3).PValue;

            Assert.AreEqual(f13, 8.3333);
            Assert.AreEqual(f13numDf, 4);
            Assert.AreEqual(f13DenomDf, 4);
            Assert.AreEqual(f13pVal, 0.06396);

            double f24 = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble2, tableDouble4).RatioOfVariances;
            double f24numDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble2, tableDouble4).NumDf;
            double f24DenomDf = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble2, tableDouble4).DenomDf;
            double f24pVal = Statystyki.CalculateFTestToCompareTwoVariances(tableDouble2, tableDouble4).PValue;

            Assert.AreEqual(f24, 6.6667);
            Assert.AreEqual(f24numDf, 3);
            Assert.AreEqual(f24DenomDf, 3);
            Assert.AreEqual(f24pVal, 0.15353);
        }
    }
}
