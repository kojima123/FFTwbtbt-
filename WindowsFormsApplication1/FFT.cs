using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class FFT
    {
        private double[] dftIn;
        private int size;

        public double[] reFFT;
        public double[] imFFT;




        public FFT(double[] dftIn, int size, int bitsize, out double[] reFFT_2, out double[] imFFT_2)
        {
            // TODO: Complete member initialization
            this.dftIn = dftIn;
            this.size = size;
            int dataSize = 1 << bitsize;
            size = 1 << bitsize;
            int _size = (int)Math.Pow(2, bitsize);
            {
          
                double[] G = new double[dataSize];

                double[] windata = new double[dataSize];
                double[] dftInIm = new double[dataSize];
                // TODO: Complete member initialization
            
              
                double Average = dftIn.Average();



                if (_size > size)
                {
                   int resize = _size - size;

                    for (int i = _size - resize; i < _size; i++)
                    {
                       
                        dftIn[i] = (0);
                     
                    }
                }
                for (int i = 0; i < size; i++)
                {

                    dftInIm[i] = dftIn[i];

                }


                Windowing(G, out windata);
                FFTmat(dftIn, dftInIm, out reFFT, out imFFT, bitsize);
  
                reFFT_2 = reFFT;
                imFFT_2 = imFFT;
            }
        }

        // 窓関数
        public void Windowing(double[] data, out double[] windata)
        {
            int size = data.Length;
            windata = new double[size];

            for (int i = 0; i < size; i++)
            {
                double winValue = 0;
                winValue = 0.5 - 0.5 * Math.Cos(2 * Math.PI * i / (size - 1));
                // 各々の窓関数

                // 窓関数を掛け算
                windata[i] = data[i] * winValue;
            }

        }

        // FFT
        public static void FFTmat(double[] inputRe, double[] inputIm, out double[] outputRe, out double[] outputIm, int bitSize)
        {
            int dataSize = 1 << bitSize;
            int[] reverseBitArray = BitScrollArray(dataSize);

            outputRe = new double[dataSize];
            outputIm = new double[dataSize];

            // バタフライ演算のための置き換え
            for (int i = 0; i < dataSize; i++)
            {
                outputRe[i] = inputRe[reverseBitArray[i]];
                outputIm[i] = inputIm[reverseBitArray[i]];
            }

            // バタフライ演算
            for (int stage = 1; stage <= bitSize; stage++)
            {
                int butterflyDistance = 1 << stage;
                int numType = butterflyDistance >> 1;
                int butterflySize = butterflyDistance >> 1;

                double wRe = 1.0;
                double wIm = 0.0;
                double uRe = System.Math.Cos(System.Math.PI / butterflySize);
                double uIm = -System.Math.Sin(System.Math.PI / butterflySize);

                for (int type = 0; type < numType; type++)
                {
                    for (int j = type; j < dataSize; j += butterflyDistance)
                    {
                        int jp = j + butterflySize;
                        double tempRe = outputRe[jp] * wRe - outputIm[jp] * wIm;
                        double tempIm = outputRe[jp] * wIm + outputIm[jp] * wRe;
                        outputRe[jp] = outputRe[j] - tempRe;
                        outputIm[jp] = outputIm[j] - tempIm;
                        outputRe[j] += tempRe;
                        outputIm[j] += tempIm;
                    }
                    double tempWRe = wRe * uRe - wIm * uIm;
                    double tempWIm = wRe * uIm + wIm * uRe;
                    wRe = tempWRe;
                    wIm = tempWIm;
                }
            }
        }

        // ビットを左右反転した配列を返す
        private static int[] BitScrollArray(int arraySize)
        {
            int[] reBitArray = new int[arraySize];
            int arraySizeHarf = arraySize >> 1;

            reBitArray[0] = 0;
            for (int i = 1; i < arraySize; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                    reBitArray[j + i] = reBitArray[j] + arraySizeHarf;
                arraySizeHarf >>= 1;
            }
            return reBitArray;
        }
 
       public void ifft(double[] reDFT, double[] imDFT, int bitSize, int size ,out double[] tmp, out double[] tmpim)
        {
            var theta = 2 * Math.PI / size;

   //scrambler
           int i=0 ;
           int mh,m ;
           double wr , wi ;
            double xr = 0;
            double xi = 0;
            wr = Math.Cos(theta * i);
            wi = Math.Sin(theta * i);
            mh = size;
     for (m = size; (m = mh << 1) <= size; mh = m) {

         Console.WriteLine(mh);
        
         int mq = mh >> 1;

        for (int jr = 0; jr < size; jr += m) {
        
                int kr =jr + mh;
                xr = reDFT[jr] - reDFT[kr];
                xi = imDFT[jr] - imDFT[kr];
                reDFT[jr] += reDFT[kr];
                imDFT[jr] += imDFT[kr];
             reDFT[kr] = xr;
             imDFT[kr] = xi;
            }
        for (i = 1; i < mq; i++) {
             int kr,ir,ji,jr,ki;
              for (int j = 0; j < size; j += m) {
                jr = j + i;
                ji = j + mh - i;
                kr = j + mh + i;
                ki = j + m - i;
                xr = reDFT[jr] - reDFT[ji];
                xi = imDFT[ki] + imDFT[kr];
                reDFT[jr] = reDFT[jr] + reDFT[ji];
                imDFT[ji] = imDFT[ki] - imDFT[kr];
           reDFT[kr] = wr * xr + wi * xi;
       imDFT[kr] = wr * xi - wi * xr;
                   
                  
            }
          }
        }

     tmp = reDFT;
     tmpim = imDFT;
            }
       public static void FFT2(double[] inRe,double[] inIm, int bitSize,out double[] outputRe,out double[] outputIm)
       {


           int dataSize = 1 << bitSize;
           int[] reverseBitArray = BitScrollArray(dataSize);

           double[] inputRe = new double[dataSize];
           double[] inputIm = new double[dataSize];

           outputRe = new double[dataSize];
           outputIm = new double[dataSize];
           int resize = inRe.GetLength(0);
           int imsize = inIm.GetLength(0);

           for (int i = 0; i < resize; i++)
           {
               inputRe[i] = inRe[i];
               inputIm[i] = inIm[i];
           }



           if (dataSize > resize)
           {
               double size = dataSize - resize;

               for (double i = dataSize - resize; i < dataSize; i++)
               {
                   int k = (int)i;
                   inputRe[k] = (0);
                   inputIm[k] = (0);
               }
           }
           // バタフライ演算のための置き換え
           for (int i = 0; i < dataSize; i++)
           {
               outputRe[i] = inputRe[reverseBitArray[i]];
               outputIm[i] = inputIm[reverseBitArray[i]];
           }

           // バタフライ演算
           for (int stage = 1; stage <= bitSize; stage++)
           {
               int butterflyDistance = 1 << stage;
               int numType = butterflyDistance >> 1;
               int butterflySize = butterflyDistance >> 1;

               double wRe = 1.0;
               double wIm = 0.0;
               double uRe =
                   System.Math.Cos(System.Math.PI / butterflySize);
               double uIm =
                   -System.Math.Sin(System.Math.PI / butterflySize);

               for (int type = 0; type < numType; type++)
               {
                   for (int j = type; j < dataSize; j += butterflyDistance)
                   {
                       int jp = j + butterflySize;
                       double tempRe =
                           outputRe[jp] * wRe - outputIm[jp] * wIm;
                       double tempIm =
                           outputRe[jp] * wIm + outputIm[jp] * wRe;
                       outputRe[jp] = outputRe[j] - tempRe;
                       outputIm[jp] = outputIm[j] - tempIm;
                       outputRe[j] += tempRe;
                       outputIm[j] += tempIm;
                   }
                   double tempWRe = wRe * uRe - wIm * uIm;
                   double tempWIm = wRe * uIm + wIm * uRe;
                   wRe = tempWRe;
                   wIm = tempWIm;
               }
           }
       }

       /// <summary>
       /// 1次元IFFT
       /// </summary>
       public  void IFFT(double[] inputRe,double[] inputIm, int bitSize, out double[] outputRe, out double[] outputIm
           )
       {
           int dataSize = 1 << bitSize;
           outputRe = new double[dataSize];
           outputIm = new double[dataSize];
          double[] newinputRe = new double[dataSize];
          for (int i = 0; i < dataSize; i++)
           {
               inputIm[i] = -inputIm[i];
           }
          for (int i = dataSize; i < dataSize; i++)
           {
               inputRe[i]= 0;
               inputIm[i]= 0;
           }
           FFT2(inputRe, inputIm, bitSize, out outputRe, out outputIm);
           for (int i = 0; i < dataSize; i++)
           {
               outputRe[i] /= (double)dataSize;
               outputIm[i] /= (double)(-dataSize);
   
               
           }
       }
 
        }
    }

