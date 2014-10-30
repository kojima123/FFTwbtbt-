using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class IDWT
    { int k2 = 4;
        private double[] s1;
        private double[] w1;
        private int size;
        private double[] p;
        private double[] q;
        private double[] coefr;

        public IDWT(double[] s1, double[] w1, int size, double[] p, double[] q, out double[] coefr_2)
        {
            {
                // TODO: Complete member initialization
                this.s1 = s1;
                this.w1 = w1;
                this.size = size;
                this.p = p;
                this.q = q;
                coefr = new double[size*2];
               
                int sup = k2, s_len = size;
                int ofs = Math.Max(2000, size);  /* index が負にならないための補正値 */
                for (int n = 0; n < size; n++)
                {
                    coefr[n + 1] = 0.0;
                    coefr[2 * n] = 0.0;


                    for (int k = 0; (k < sup / 2); k++)
                    {
                        int ksku = (2 * k + 1);
                        int index = (n - k + ofs) % size;
                        coefr[2 * n + 1] += p[2 * k + 1] * s1[index] + q[2 * k + 1] * w1[index];
                        coefr[2 * n] += p[2 * k] * s1[index] + q[2 * k] * w1[index];


                    }

                }

                coefr_2 = coefr;
               
            }
        }

   
    }
}
