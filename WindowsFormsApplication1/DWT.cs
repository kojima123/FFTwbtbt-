using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//////未完成
namespace WindowsFormsApplication1
{
    class DWT
    {
        int k = 4;
        public double[] data;
        double pi2 = (8.0 * Math.Atan(1.0));


        public double[] w1;
        private double[] s1;



        int size;
        private double[] p;
        private double[] q;

        private double[] coefr;


        public DWT(double[] coef, int size, double[] p, double[] q, out double[] s1_2, out double[] w1_2)
        {
            // TODO: Complete member initialization
            this.data = coef;
            this.size = size;
            this.p = p;
            this.q = q;
            s1 = new double[size];
            w1 = new double[size];
            coefr = new double[888888];

            int index = 0;
            int sup = k, s_len = size;

            for (k = 0; k < size / 2; k++)
            {
                s1[k] = 0.0;
                w1[k] = 0.0;
                for (int n = 0; n < sup; n++)
                {
                    index = (n + 2 * k) % size;
                    s1[k] += p[n] * data[index];
                    w1[k] += q[n] * data[index];

                }
            }
            s1_2 = s1;
            w1_2 = w1;

           

            }


        }
}
