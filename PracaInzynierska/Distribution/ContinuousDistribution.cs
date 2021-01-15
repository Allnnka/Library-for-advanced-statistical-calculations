using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaInzynierska.Distribution
{
    public static class ContinuousDistribution
    {
        public static double Gauss(double z)
        {
            // input = z-value (-inf to +inf)
            // output = p under Standard Normal curve from -inf to z
            // e.g., if z = 0.0, function returns 0.5000
            // ACM Algorithm #209
            double y; // 209 scratch variable
            double p; // result. called 'z' in 209
            double w; // 209 scratch variable ContinuousDistribution
            if (z == 0.0)
                p = 0.0;
            else
            {
                y = Math.Abs(z) / 2;
                if (y >= 3.0)
                {
                    p = 1.0;
                }
                else if (y < 1.0)
                {
                    w = y * y;
                    p = ((((((((0.000124818987 * w
                      - 0.001075204047) * w + 0.005198775019) * w
                      - 0.019198292004) * w + 0.059054035642) * w
                      - 0.151968751364) * w + 0.319152932694) * w
                      - 0.531923007300) * w + 0.797884560593) * y * 2.0;
                }
                else
                {
                    y = y - 2.0;
                    p = (((((((((((((-0.000045255659 * y
                      + 0.000152529290) * y - 0.000019538132) * y
                      - 0.000676904986) * y + 0.001390604284) * y
                      - 0.000794620820) * y - 0.002034254874) * y
                      + 0.006549791214) * y - 0.010557625006) * y
                      + 0.011630447319) * y - 0.009279453341) * y
                      + 0.005353579108) * y - 0.002141268741) * y
                      + 0.000535310849) * y + 0.999936657524;
                }
            }
            if (z > 0.0)
                return (p + 1.0) / 2;
            else
                return (1.0 - p) / 2;
        }
        public static double Student(double t, double df)
        {
            // for large integer df or double df
            // adapted from ACM algorithm 395
            // returns 2-tail p-value
            if (df <= 0) throw new ArgumentException("The degrees of freedom need to be positive.");

            double n = df; // to sync with ACM parameter name
            double a, b, y;
            t = t * t;
            y = t / n;
            b = y + 1.0;
            if (y > 1.0E-6) y = Math.Log(b);
            a = n - 0.5;
            b = 48.0 * a * a;
            y = a * y;
            y = (((((-0.4 * y - 3.3) * y - 24.0) * y - 85.5) /
              (0.8 * y * y + 100.0 + b) + y + 3.0) / b + 1.0) *
              Math.Sqrt(y);
            return 2.0 * Gauss(-Math.Abs(y)); // ACM algorithm 209
        }


        public static double Student(double t, int df)
        {
            // adapted from ACM algorithm 395
            // for small int df
            int n = df; // to sync with ACM parameter name
            double a, b, y, z;

            z = 1.0;
            t = t * t;
            y = t / n;
            b = 1.0 + y;

            if (n >= 20 && t < n || n > 200) // large df
            {
                double x = (double)n; // make df a double
                return Student(t, x); // double version
            }

            if (n < 20 && t < 4.0)
            {
                a = Math.Sqrt(y);
                y = Math.Sqrt(y);
                if (n == 1)
                    a = 0.0;
            }
            else
            {
                a = Math.Sqrt(b);
                y = a * n;
                for (int j = 2; a != z; j += 2)
                {
                    z = a;
                    y = y * (j - 1) / (b * j);
                    a = a + y / (n + j);
                }
                n = n + 2;
                z = y = 0.0;
                a = -a;
            }

            int sanityCt = 0;
            while (true && sanityCt < 10000)
            {
                ++sanityCt;
                n = n - 2;
                if (n > 1)
                {
                    a = (n - 1) / (b * n) * a + y;
                    continue;
                }

                if (n == 0)
                    a = a / Math.Sqrt(b);
                else // n == 1
                    a = (Math.Atan(y) + a / b) * 0.63661977236; // 2/Pi

                return z - a;
            }

            return -1.0; // error
        } // Student (int df)

        public static double NormalQuantile(double p, double mu, double sigma)
        {
            if (sigma < 0)
            {
                throw new ArgumentException("The sigma parameter must be positive.");
            }
            else if (sigma == 0)
            {
                return mu;
            }

            double r;
            double val;

            double q = p - 0.5;

            if (0.075 <= p && p <= 0.925)
            {
                r = 0.180625 - q * q;
                val = q * (((((((r * 2509.0809287301226727 + 33430.575583588128105) * r + 67265.770927008700853) * r
                    + 45921.953931549871457) * r + 13731.693765509461125) * r + 1971.5909503065514427) * r + 133.14166789178437745) * r
                    + 3.387132872796366608) / (((((((r * 5226.495278852854561 + 28729.085735721942674) * r + 39307.89580009271061) * r
                    + 21213.794301586595867) * r + 5394.1960214247511077) * r + 687.1870074920579083) * r + 42.313330701600911252) * r + 1);
            }
            else
            { /* closer than 0.075 from {0,1} boundary */
                /* r = min(p, 1-p) < 0.075 */
                if (q > 0)
                {
                    r = 1 - p;
                }
                else
                {
                    r = p;/* = R_DT_Iv(p) ^=  p */
                }

                r = Math.Sqrt(-Math.Log(r)); /* r = sqrt(-log(r))  <==>  min(p, 1-p) = exp( - r^2 ) */

                if (r <= 5.0)
                { /* <==> min(p,1-p) >= exp(-25) ~= 1.3888e-11 */
                    r += -1.6;
                    val = (((((((r * 7.7454501427834140764e-4 + 0.0227238449892691845833) * r + 0.24178072517745061177) * r
                        + 1.27045825245236838258) * r + 3.64784832476320460504) * r + 5.7694972214606914055) * r
                        + 4.6303378461565452959) * r + 1.42343711074968357734) / (((((((r * 1.05075007164441684324e-9 + 5.475938084995344946e-4) * r
                        + 0.0151986665636164571966) * r + 0.14810397642748007459) * r + 0.68976733498510000455) * r + 1.6763848301838038494) * r
                        + 2.05319162663775882187) * r + 1.0);
                }
                else
                { /* very close to  0 or 1 */
                    r += -5.0;
                    val = (((((((r * 2.01033439929228813265e-7 + 2.71155556874348757815e-5) * r + 0.0012426609473880784386) * r
                        + 0.026532189526576123093) * r + 0.29656057182850489123) * r + 1.7848265399172913358) * r + 5.4637849111641143699) * r
                        + 6.6579046435011037772) / (((((((r * 2.04426310338993978564e-15 + 1.4215117583164458887e-7) * r
                        + 1.8463183175100546818e-5) * r + 7.868691311456132591e-4) * r + 0.0148753612908506148525) * r
                        + 0.13692988092273580531) * r + 0.59983220655588793769) * r + 1.0);
                }

                if (q < 0.0)
                {
                    val = -val;
                }
                /* return (q >= 0.)? r : -r ;*/
            }
            return mu + sigma * val;
        }


        public static double LogGamma(double Z)
        {
            double S = 1.0 + 76.18009173 / Z - 86.50532033 / (Z + 1.0) + 24.01409822 / (Z + 2.0) - 1.231739516 / (Z + 3.0) + 0.00120858003 / (Z + 4.0) - 0.00000536382 / (Z + 5.0);
            double LG = (Z - 0.5) * Math.Log(Z + 4.5) - (Z + 4.5) + Math.Log(S * 2.50662827465);

            return LG;
        }

        //Internal function used by gammaCdf
        private static double GCdf(double x, double A)
        {
            // Good for X>A+1
            double A0 = 0;
            double B0 = 1;
            double A1 = 1;
            double B1 = x;
            double AOLD = 0;
            double N = 0;
            while (Math.Abs((A1 - AOLD) / A1) > .00001)
            {
                AOLD = A1;
                N = N + 1;
                A0 = A1 + (N - A) * A0;
                B0 = B1 + (N - A) * B0;
                A1 = x * A0 + N * A1;
                B1 = x * B0 + N * B1;
                A0 = A0 / B1;
                B0 = B0 / B1;
                A1 = A1 / B1;
                B1 = 1;
            }
            double Prob = Math.Exp(A * Math.Log(x) - x - LogGamma(A)) * A1;

            return 1.0 - Prob;
        }
        private static double gSer(double x, double A)
        {
            // Good for X<A+1.
            double T9 = 1 / A;
            double G = T9;
            double I = 1;
            while (T9 > G * 0.00001)
            {
                T9 = T9 * x / (A + I);
                G = G + T9;
                ++I;
            }
            G = G * Math.Exp(A * Math.Log(x) - x - LogGamma(A));

            return G;
        }
        public static double Gamma(double x, double a)
        {
            if (x < 0)
            {
                throw new ArgumentException("The x parameter must be positive.");
            }

            double GI;
            if (a > 200)
            {
                double z = (x - a) / Math.Sqrt(a);
                double y = Gauss(z);
                double b1 = 2 / Math.Sqrt(a);
                double phiz = 0.39894228 * Math.Exp(-z * z / 2);
                double w = y - b1 * (z * z - 1) * phiz / 6;  //Edgeworth1
                double b2 = 6 / a;
                int zXor4 = ((int)z) ^ 4;
                double u = 3 * b2 * (z * z - 3) + b1 * b1 * (zXor4 - 10 * z * z + 15);
                GI = w - phiz * z * u / 72;        //Edgeworth2
            }
            else if (x < a + 1)
            {
                GI = gSer(x, a);
            }
            else
            {
                GI = GCdf(x, a);
            }

            return GI;
        }
        public static double ChiSquareCdf(double x, int df)
        {
            if (df <= 0)
            {
                throw new ArgumentException("The degrees of freedom need to be positive.");
            }

            return Gamma(x / 2.0, df / 2.0);
        }

        //Internal function used by StudentCdf
        public static double Betinc(double x, double A, double B)
        {
            double A0 = 0.0;
            double B0 = 1.0;
            double A1 = 1.0;
            double B1 = 1.0;
            double M9 = 0.0;
            double A2 = 0.0;
            while (Math.Abs((A1 - A2) / A1) > 0.00001)
            {
                A2 = A1;
                double C9 = -(A + M9) * (A + B + M9) * x / (A + 2.0 * M9) / (A + 2.0 * M9 + 1.0);
                A0 = A1 + C9 * A0;
                B0 = B1 + C9 * B0;
                M9 = M9 + 1;
                C9 = M9 * (B - M9) * x / (A + 2.0 * M9 - 1.0) / (A + 2.0 * M9);
                A1 = A0 + C9 * A1;
                B1 = B0 + C9 * B1;
                A0 = A0 / B1;
                B0 = B0 / B1;
                A1 = A1 / B1;
                B1 = 1.0;
            }
            return A1 / A;
        }
        public static double Beta(double x, double a, double b)
        {
            if (x < 0 || a <= 0 || b <= 0)
            {
                throw new ArgumentException("All the parameters must be positive.");
            }

            double beta = 0.0;

            if (x == 0)
            {
                return beta;
            }
            else if (x >= 1)
            {
                beta = 1.0;
                return beta;
            }

            double S = a + b;

            double BT = Math.Exp(LogGamma(S) - LogGamma(b) - LogGamma(a) + a * Math.Log(x) + b * Math.Log(1 - x));
            if (x < (a + 1.0) / (S + 2.0))
            {
                beta = BT * Betinc(x, a, b);
            }
            else
            {
                beta = 1.0 - BT * Betinc(1.0 - x, b, a);
            }

            return beta;
        }

        //Calculates the probability from 0 to X under F Distribution
        public static double FCdf(double x, int d1, int d2)
        {
            if (x < 0 || d1 <= 0 || d2 <= 0)
                throw new ArgumentException("All the parameters must be positive.");
            double Z = (d1*x)/(d1*x+d2);
            double FCdf = Beta(Z, d1 / 2.0, d2 / 2.0);

            return 1.0-FCdf;
        }

    }
}
