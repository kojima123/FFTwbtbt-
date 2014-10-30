using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class DFT
    {private double[] data;
        private int size;
        private int bitsize;

      

        public DFT()
        {
            // TODO: Complete member initialization
        }



        internal static void dft(double[] data, int size, out double[] reDFT, out double[] imDFT)
        {




            double[] tmpReal = new double[size];
            double[] tmpImag = new double[size];
            double[] reDFT1 = new double[size];
            double[] imDFT1 = new double[size];
            double[] DFT = new double[size];

            for (int i = 0; i < size; i++)
            {

                double d = 2.0 * Math.PI * i / size;

                for (int j = 0; j < size; j++)
                {

                    double phase = d * j;

                    tmpReal[i] += data[j] * Math.Cos(phase);
                    tmpImag[i] -= data[j] * Math.Sin(phase);


                }
            }


            for (int i = 0; i < size; i++)
            {
                reDFT1[i] = tmpReal[i];
                imDFT1[i] = tmpImag[i];

            }
            reDFT = reDFT1;
            imDFT = imDFT1;


        }  

    internal static void idft(double[] reDFT, double[] imDFT, int size, out double[] isignal)
    {
        double[] real = new double[size];
        double[] imag = new double[size];
        double[] _isingnal = new double[size];
        double theta = 8.0 * Math.Atan(1.0) / size;
        for (int n = 0; n < size; n++)
        {
            for (int k = 0; k < size; k++)
            {
                real[n] += reDFT[k] * Math.Cos(theta * k * n) - imDFT[k] * Math.Sin(theta * k * n);	//実数部の計算
                imag[n] += reDFT[k] * Math.Sin(theta * k * n) + imDFT[k] * Math.Cos(theta * k * n);	//虚数部の計算
               
            }
           
            real[n] /= size;
            imag[n] /= size;

            _isingnal[n] = Math.Sqrt(Math.Pow(real[n], 2) + Math.Pow(imag[n], 2));	//絶対値の計

            
        }
        isignal = real;
    }

    
    }


    }
