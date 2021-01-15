using PracaInzynierska.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PracaInzynierska.Statystyka.Statystyki;

namespace PracaInzynierska
{
    public static class ANOVA
    {
        public struct AnovaResult
        {
            public double TestValue;
            public double Dfwg;
            public double Dfbg;

            public double MsWG;
            public double MsBG; 
            public double SsWG;
            public double SsBG;
            public double PValue;
        }
        public static AnovaResult OneWayAnalysisOfVariance(params IEnumerable<double>[] args)
        {
            int r = args.Length;
            int ni = args.FirstOrDefault().Count();
            int n = r*ni;
            foreach (IEnumerable<double> list in args)
            {
                if (ni != list.Count()) throw new SizeOutOfRangeException();
            }

            List<double> meanInEachGroup = new List<double>();
            double ssWG = 0;
            int nmean = 0;
            foreach (IEnumerable<double> list in args)
            {
                meanInEachGroup.Add(list.Average());
                for (int i = 0; i < ni; i++)
                {
                    ssWG += (list.ElementAt(i) - meanInEachGroup.ElementAt(nmean))* (list.ElementAt(i) - meanInEachGroup.ElementAt(nmean));
                }
                nmean++;
                
            }
            double overallMean = meanInEachGroup.Sum() / meanInEachGroup.Count();
            double ssBG = 0;
            foreach(double item in meanInEachGroup)
            {
                ssBG += ni * (item - overallMean) * (item - overallMean);
            }
            double dfBG = r - 1;
            double msBG = ssBG / dfBG;
            double dfWG = r*(ni-1);
            double msWG = ssWG / dfWG;
            double statistic = msBG / msWG;
            double p = ContinuousDistribution.FCdf(statistic, (int)dfBG, (int)dfWG);

            return new AnovaResult
            {
                TestValue = Math.Round(statistic,3),
                Dfbg=dfBG,
                Dfwg=dfWG,
                MsBG=msBG,
                MsWG=msWG,
                SsBG=ssBG,
                SsWG=ssWG,
                PValue=p
            };
        }



    }
}
