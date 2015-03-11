
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Linq;
using System.Text;


namespace WindowsFormsApplication1
{
    class CWT
    {
        private double fs;
        private double fmin;
        private double fmax;
        private double dj;

        List<double> _scales = new List<double>();
        DFT DFT = new DFT();

        public CWT(double fs, double fmin, double fmax, double dj)
        {
            this.fs = fs;
            this.fmin = fmin;
            this.fmax = fmax;
            this.dj = dj;
        }

        public CWT()
        {

        }

        public void setScale(double fs, double fmin, double fmax, double dj, int N, double sigma, out double[,] table, out double delta, out double[] scales)
        ///マザーウェーブレットスケールセット
        {
            //σの設定
            double flc = 0,
                  fuc = 0,
                  _delta = 0,
                  a = Math.Pow(2.0, dj);

            flc = 0.4;
            fuc = 1.2;
            //////マザーウェーブレット設定（モレット）


            double jMin = (Math.Ceiling(Math.Log(fs * flc / fmax) / Math.Log(2.0) / dj)),

            jMax = (Math.Ceiling(Math.Log(fs * fuc / fmin) / Math.Log(2.0) / dj)),
            J = jMax - jMin + 1;
            ///マザーウェーブレットスケールセット  
            for (int j = 0; j < J; ++j)
            {

                _scales.Add(Math.Pow(a, (j + jMin)));
            }
            int Js = _scales.Count;

            double[,] tables = new double[Js, N];
            double[] s = new double[Js];

            for (int j = 0; j < J; ++j)
            {

                s[j] = _scales[j];

            }

            setTable(_scales, N, Js, sigma, out tables, out _delta);

            table = tables;
            delta = _delta;
            scales = s;

        }
        private void setTable(List<double> scales, int N, int Js, double omega, out double[,] tables, out double delta)
        {  //////マザーウェーブレット計算
            int J = Js;
            double[] expnt = new double[N];
            double[] tmp = new double[N];
            double[,] table = new double[J, N];
            double c = 0;
            for (int j = 0; j < J; ++j)
            {
                // 基本周波数
               c = (2 * Math.PI * scales[j] / N);
    
                for (int k = 0; k < N; ++k)
                {
                    if (k <= N / 2)
                        expnt[k] = c * k;
                    else
                        expnt[k] = c * (k - N);

                    expnt[k] = (-0.5) * ((expnt[k] - omega) * (expnt[k] - omega));
                    tmp[k] = Math.Pow(Math.PI, 0.25) * (Math.Sqrt(4 * Math.PI * c)) * Math.Exp(expnt[k]);

                    table[j, k] = tmp[k];
                }
            }
            delta = constDelta(table, N, J);//デルタ関数計算

            tables = table;
        }
        public double constDelta(double[,] table, int N, int J)
        {

            double sum;
            double C = 0;

            for (int j = 0; j < J; ++j)
            {
                sum = 0;
                for (int k = 0; k < N; ++k)
                {
                    sum += table[j, k];
                }
                C += (sum / Math.Sqrt(_scales[j]));

            }
            return C;
        }
        public void CWTC(double[] data, double[,] table, int size, int bitsize, out double[,] _coefsIm)
        {///虚部ウェーブレット変換
            {
                int N = size;
                int J = _scales.Count;
                double[,] coefsIm = new double[N, J];
                double[] tmp = new double[N];
                double[] tmpim = new double[N];
                double[] reDFT = new double[N];
                double[] imDFT = new double[N];
                double[] sigDFT = new double[N];
                double[] tmpDFT = new double[N];
                double[] tmpiDFT = new double[N];

                FFT t = new FFT(data, size, bitsize, out reDFT, out imDFT);///FFT

                for (int j = 0; j < J; ++j)
                {
                    // 虚部ウェーブレット変換
                    for (int k = 0; k < N; ++k)
                    {
                        tmpDFT[k] = reDFT[k] * table[j, k];
                        tmpiDFT[k] = imDFT[k] * table[j, k];
                    }
                    t.IFFT(tmpDFT, tmpiDFT, bitsize, out tmp, out tmpim);//IFFT
                    for (int i = 0; i < N; ++i)
                    {
                        coefsIm[i, j] = tmpim[i];
                    }
                }
                _coefsIm = coefsIm;
            }
        }
        public void ICWTC(double[,] coefsIm, double delta, int size, int bitsize, out double[] _signal)
        {
            int N = size;
            int J = _scales.Count;
            double[] signal = new double[N];

            //虚部逆ウェーブレット
            for (int i = 0; i < N; ++i)
            {

                for (int j = 0; j < J; ++j)
                    {
                        signal[i] += (coefsIm[i, j]) / Math.Sqrt(_scales[j]);
                    }
            }
                for (int i = 0; i < N; ++i)
                {
                    signal[i] = signal[i] /( N / delta);

                }

                _signal = signal;

            
        }
        public void CWTR(double[] data, double[,] table, int size, int bitsize, out double[,] _coefsR)
        {///実部ウェーブレット変換

            int N = size;
            int J = _scales.Count;


            double[,] coefsR = new double[N, J];
            double[] tmp = new double[N];
            double[] tmpim = new double[N];
            double[] reDFT = new double[N];
            double[] imDFT = new double[N];
            double[] sigDFT = new double[N];
            double[] tmpDFT = new double[N];
            double[] tmpiDFT = new double[N];

            FFT t = new FFT(data, size, bitsize, out reDFT, out imDFT);//FFT

            //実部ウェーブレット変換
            for (int j = 0; j < J; ++j)
            {
                for (int k = 0; k < N; ++k)
                {
                    tmpDFT[k] = reDFT[k] * table[j, k];
                    tmpiDFT[k] = imDFT[k] * table[j, k];

                }
                t.IFFT(tmpDFT, tmpiDFT, bitsize, out tmp, out tmpim);//IFFT
                for (int i = 0; i < N; ++i)
                {
                    coefsR[i, j] = tmp[i];
                }
            }

            _coefsR = coefsR;


        }
        public void ICWR(double[,] coefsR, double delta, int size, int bitsize, out double[] _signal)
        {
            int N = size;
            int J = _scales.Count;

            double[] signal = new double[N];

            // 実部ウェーブレット変換
       
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < J; ++j)
                    {
                        signal[i] += (coefsR[i, j]) / Math.Sqrt(_scales[j]);
   
                    }
                }
                for (int i = 0; i < N; ++i)
                {
                    signal[i] = signal[i] /(N/ delta);

                } 
            _signal = signal;
        }
    }

}



