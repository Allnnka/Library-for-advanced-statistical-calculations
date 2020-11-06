using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PracaInzynierska.Statystyka;


namespace TestAnalizaWyników
{
    [TestClass]
    public class TestStatystyka
    {
        readonly double[] tablicaDouble1 = { 1, 2, 3, 4, 5 };
        readonly double[] tablicaDouble2 = { 1, 2, 3, 4 };
        readonly double[] tablicaDouble3 = { 1, 1, 1, 2, 2 };
        readonly double[] tablicaDouble4 = { 1, 1, 1, 2 };

        readonly int[] tablicaInt1 = { 1, 2, 3, 4, 5 };
        readonly int[] tablicaInt2 = { 1, 2, 3, 4 };
        readonly int[] tablicaInt3 = { 1, 1, 1, 2, 2 };
        readonly int[] tablicaInt4 = { 1, 1, 1, 2 };

        [TestMethod]
        public void TestObliczMedianęDouble()
        {
            double mediana1 = Statystyki.ObliczMedianę(tablicaDouble1);
            double mediana2 = Statystyki.ObliczMedianę(tablicaDouble2);
            double mediana3 = Statystyki.ObliczMedianę(tablicaDouble3);
            double mediana4 = Statystyki.ObliczMedianę(tablicaDouble4);

            Assert.AreEqual(3, mediana1);
            Assert.AreEqual(2.5, mediana2);
            Assert.AreEqual(1, mediana3);
            Assert.AreEqual(1, mediana4);
        }

        [TestMethod]
        public void TestObliczMedianęInt()
        {
            double mediana1 = Statystyki.ObliczMedianę(tablicaInt1);
            double mediana2 = Statystyki.ObliczMedianę(tablicaInt2);
            double mediana3 = Statystyki.ObliczMedianę(tablicaInt3);
            double mediana4 = Statystyki.ObliczMedianę(tablicaInt4);

            Assert.AreEqual(3, mediana1);
            Assert.AreEqual(2.5, mediana2);
            Assert.AreEqual(1, mediana3);
            Assert.AreEqual(1, mediana4);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPróbkiDouble()
        {
            double odchylenieStandardowe1 = Statystyki.CalculateStandardDeviation(tablicaDouble1, Statystyki.StandardDeviationType.Próbki);
            double odchylenieStandardowe2 = Statystyki.CalculateStandardDeviation(tablicaDouble2, Statystyki.StandardDeviationType.Próbki);
            double odchylenieStandardowe3 = Statystyki.CalculateStandardDeviation(tablicaDouble3, Statystyki.StandardDeviationType.Próbki);
            double odchylenieStandardowe4 = Statystyki.CalculateStandardDeviation(tablicaDouble4, Statystyki.StandardDeviationType.Próbki);

            //wartości wzorcowe z eksela =ODCH.STANDARD.PRÓBKI(A1:A5)
            double dopuszczalnyBłąd = 0.000001;
            Assert.AreEqual(1.58113883, odchylenieStandardowe1, dopuszczalnyBłąd);
            Assert.AreEqual(1.290994449, odchylenieStandardowe2, dopuszczalnyBłąd);
            Assert.AreEqual(0.547722558, odchylenieStandardowe3, dopuszczalnyBłąd);
            Assert.AreEqual(0.5, odchylenieStandardowe4, dopuszczalnyBłąd);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPopulacjiDouble()
        {
            double odchylenieStandardowe1 = Statystyki.CalculateStandardDeviation(tablicaDouble1, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe2 = Statystyki.CalculateStandardDeviation(tablicaDouble2, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe3 = Statystyki.CalculateStandardDeviation(tablicaDouble3, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe4 = Statystyki.CalculateStandardDeviation(tablicaDouble4, Statystyki.StandardDeviationType.Populacji);

            //wartości wzorcowe z eksela =ODCH.STANDARD.POPUL.A(A1:A5)
            double dopuszczalnyBłąd = 0.000001;            
            Assert.AreEqual(1.414213562, odchylenieStandardowe1, dopuszczalnyBłąd);
            Assert.AreEqual(1.118033989, odchylenieStandardowe2, dopuszczalnyBłąd);
            Assert.AreEqual(0.489897949, odchylenieStandardowe3, dopuszczalnyBłąd);
            Assert.AreEqual(0.433012702, odchylenieStandardowe4, dopuszczalnyBłąd);
        }

        [TestMethod]
        public void TestCalculateStandardDeviationPopulacjiInt()
        {
            double odchylenieStandardowe1 = Statystyki.CalculateStandardDeviation(tablicaInt1, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe2 = Statystyki.CalculateStandardDeviation(tablicaInt2, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe3 = Statystyki.CalculateStandardDeviation(tablicaInt3, Statystyki.StandardDeviationType.Populacji);
            double odchylenieStandardowe4 = Statystyki.CalculateStandardDeviation(tablicaInt4, Statystyki.StandardDeviationType.Populacji);

            //wartości wzorcowe z eksela =ODCH.STANDARD.POPUL.A(A1:A5)
            double dopuszczalnyBłąd = 0.000001;
            Assert.AreEqual(1.414213562, odchylenieStandardowe1, dopuszczalnyBłąd);
            Assert.AreEqual(1.118033989, odchylenieStandardowe2, dopuszczalnyBłąd);
            Assert.AreEqual(0.489897949, odchylenieStandardowe3, dopuszczalnyBłąd);
            Assert.AreEqual(0.433012702, odchylenieStandardowe4, dopuszczalnyBłąd);
        }

        [TestMethod]
        public void TestObliczDominante()
        {
            List<double> moda3 = Statystyki.ObliczModę(tablicaDouble3);
            List<double> moda4 = Statystyki.ObliczModę(tablicaDouble4);


            Assert.ThrowsException<Exception>(() => Statystyki.ObliczModę(tablicaDouble1));
            Assert.ThrowsException<Exception>(() => Statystyki.ObliczModę(tablicaDouble2));
            CollectionAssert.AreEqual(new List<double>() {1}, moda3);
            CollectionAssert.AreEqual(new List<double>() {1}, moda4);

        }

        [TestMethod]
        public void TestPearsonsCorrelationCoefficient()
        {
            double cor1 = Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble1, tablicaDouble3).PearsonsCorrelationCoefficient;
            double cor2 = Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).PearsonsCorrelationCoefficient;

            double cor1PValue = Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble1, tablicaDouble3).NormalDistributionPValue;
            double cor2PValue = Statystyki.CalculatePearsonsCorrelationCoefficient(tablicaDouble2, tablicaDouble4).NormalDistributionPValue;

            Assert.AreEqual(0.8660254, cor1);
            Assert.AreEqual(0.7745967, cor2);

            Assert.AreEqual(0.05767, cor1PValue);
            Assert.AreEqual(0.2254, cor2PValue);
        }

        [TestMethod]
        public void TestCalculateSingleSampleStudentsTTest()
        {
            double t1 = Statystyki.CalculateStudentsTTest(tablicaDouble1).TStudentTest;
            double t2 = Statystyki.CalculateStudentsTTest(tablicaDouble2).TStudentTest;
            double t3 = Statystyki.CalculateStudentsTTest(tablicaDouble3).TStudentTest;
            double t4 = Statystyki.CalculateStudentsTTest(tablicaDouble4).TStudentTest;

            Assert.AreEqual(4.2426, t1);
            Assert.AreEqual(3.873, t2);
            Assert.AreEqual(5.7155, t3);
            Assert.AreEqual(5, t4);

            double t1PValue = Statystyki.CalculateStudentsTTest(tablicaDouble1).PValueForStudentTest;
            double t2PValue = Statystyki.CalculateStudentsTTest(tablicaDouble2).PValueForStudentTest;
            double t3PValue = Statystyki.CalculateStudentsTTest(tablicaDouble3).PValueForStudentTest;
            double t4PValue = Statystyki.CalculateStudentsTTest(tablicaDouble4).PValueForStudentTest;
            Assert.AreEqual(0.01324, t1PValue);
            Assert.AreEqual(0.03047, t2PValue);
            Assert.AreEqual(0.00464, t3PValue);
            Assert.AreEqual(0.01539, t4PValue);

            double t1mu = Statystyki.CalculateStudentsTTest(tablicaDouble1, 3).TStudentTest;
            double t2mu = Statystyki.CalculateStudentsTTest(tablicaDouble2, 3).TStudentTest;
            double t3mu = Statystyki.CalculateStudentsTTest(tablicaDouble3, 2).TStudentTest;
            double t4mu = Statystyki.CalculateStudentsTTest(tablicaDouble4, 2).TStudentTest;


            Assert.AreEqual(0, t1mu);
            Assert.AreEqual(-0.7746, t2mu);
            Assert.AreEqual(-2.4495, t3mu);
            Assert.AreEqual(-3, t4mu);

            double t1muPValue = Statystyki.CalculateStudentsTTest(tablicaDouble1, 3).PValueForStudentTest;
            double t2muPValue = Statystyki.CalculateStudentsTTest(tablicaDouble2, 3).PValueForStudentTest;
            double t3muPvalue = Statystyki.CalculateStudentsTTest(tablicaDouble3, 2).PValueForStudentTest;
            double t4muPValue = Statystyki.CalculateStudentsTTest(tablicaDouble4, 2).PValueForStudentTest;

            Assert.AreEqual(1, t1muPValue);
            Assert.AreEqual(0.49503, t2muPValue);
            Assert.AreEqual(0.07048, t3muPvalue);
            Assert.AreEqual(0.05767, t4muPValue);

        }

        [TestMethod]
        public void TestCalculateStudentsTtestForIndependentGroups()
        {
            double t1t2 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble2).TStudentTest;
            double df12= Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble2).DegreesOfFreedom;
            double p12 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble2).PValueForStudentTest;

            double t2t3 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble3).TStudentTest;
            double df23 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble3).DegreesOfFreedom;
            double p23 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble3).PValueForStudentTest;

            double t1t3 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3).TStudentTest;
            double df13 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3).DegreesOfFreedom;
            double p13 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3).PValueForStudentTest;

            double t2t4 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4).TStudentTest;
            double df24 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4).DegreesOfFreedom;
            double p24 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4).PValueForStudentTest;

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
            double t1t3 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3,true).TStudentTest;
            double df13 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3, true).DegreesOfFreedom;
            double p13 = Statystyki.CalculateStudentsTTest(tablicaDouble1, tablicaDouble3, true).PValueForStudentTest;

            double t2t4 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4,true).TStudentTest;
            double df24 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4, true).DegreesOfFreedom;
            double p24 = Statystyki.CalculateStudentsTTest(tablicaDouble2, tablicaDouble4, true).PValueForStudentTest;


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
            double w1 = Statystyki.CalculateWilcoxonTest(tablicaDouble1).WValue;
            double w1PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble1).PValue;

            double w1mu = Statystyki.CalculateWilcoxonTest(tablicaDouble1,2).WValue;
            double w1muPValue = Statystyki.CalculateWilcoxonTest(tablicaDouble1, 2).PValue;

            double w2 = Statystyki.CalculateWilcoxonTest(tablicaDouble2).WValue;
            double w2PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble2).PValue;


            double w3 = Statystyki.CalculateWilcoxonTest(tablicaDouble3).WValue;
            double w3PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble3).PValue;

            double w4 = Statystyki.CalculateWilcoxonTest(tablicaDouble4).WValue;
            double w4PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble4).PValue;

            double w4mu = Statystyki.CalculateWilcoxonTest(tablicaDouble4,1).WValue;
            double w4muPvalue = Statystyki.CalculateWilcoxonTest(tablicaDouble4, 1).PValue;



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
            double w13 = Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble3).WValue;
            double w13PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble3).PValue;

            double w24 = Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).WValue;
            double w24PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble2, tablicaDouble4).PValue;


            Assert.AreEqual(10, w13);
            Assert.AreEqual(0.0975,w13PValue);
            //wilcox.test(d2,d4, paired=TRUE) in RStudio
            Assert.AreEqual(6, w24);
            Assert.AreEqual(w24PValue,0.1736);

            double w14PValue = Statystyki.CalculateWilcoxonTest(tablicaDouble1, tablicaDouble4).PValue;
            //Assert.AreEqual(0.09481, w14PValue);
        }
        [TestMethod]
        public void TestKolmogorovSmirnovTests()
        {
            double d1 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble1).TestValue;
            double d1PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble1).PValue;

            double d2 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble2).TestValue;
            double d2PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble2).PValue;

            double d3 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble3).TestValue;
            double d3PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble3).PValue;

            double d4 = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble4).TestValue;
            double d4PVal = Statystyki.CalculateKolmogorovSmirnovTestForNormality(tablicaDouble4).PValue;

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
            double w1 = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble1).TestValue;
            double w1PVal = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble1).PValue;

            double w2 = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble2).TestValue;
            double w2PVal = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble2).PValue;

            double w3 = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble3).TestValue;
            double w3PVal = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble3).PValue;

            double w4 = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble4).TestValue;
            double w4PVal = Statystyki.CalculateShapiroWilkTestForNormality(tablicaDouble4).PValue;


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
            double ch13 = Statystyki.CalculateKruskalaWalisaTest(tablicaDouble1,tablicaDouble3).TestValue;


            Assert.AreEqual(3, ch13);
        }
    }
}
