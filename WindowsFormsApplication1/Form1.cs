
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using TM = System.Timers;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Collections;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private delegate void Delegate_FFT(string data);
        List<double> a1 = new List<double>();

        ArrayList WR = new ArrayList();
        List<double> a3 = new List<double>();
        List<double> a4 = new List<double>();
        public static double[][] DaubechiesCoefficients =
    { 
      new double[] {1.0/Math.Sqrt(2.0),1.0/Math.Sqrt(2.0)},
      new double[] {0.6830127/Math.Sqrt(2.0),1.1830127/Math.Sqrt(2.0),0.3169873/Math.Sqrt(2.0),-0.1830127/Math.Sqrt(2.0)},
      new double[] {0.47046721/Math.Sqrt(2.0),1.14111692/Math.Sqrt(2.0),0.650365/Math.Sqrt(2.0),-0.19093442/Math.Sqrt(2.0),-0.12083221/Math.Sqrt(2.0),0.0498175/Math.Sqrt(2.0)},
      new double[] {0.32580343/Math.Sqrt(2.0),1.01094572/Math.Sqrt(2.0),0.8922014/Math.Sqrt(2.0),-0.03957503/Math.Sqrt(2.0),
              -0.26450717/Math.Sqrt(2.0),0.0436163/Math.Sqrt(2.0),0.0465036/Math.Sqrt(2.0),-0.01498699/Math.Sqrt(2.0)},
      new double[] {0.22641898/Math.Sqrt(2.0),0.85394354/Math.Sqrt(2.0),1.02432694/Math.Sqrt(2.0),0.19576696/Math.Sqrt(2.0),
              -0.34265671/Math.Sqrt(2.0),-0.04560113/Math.Sqrt(2.0),0.10970265/Math.Sqrt(2.0),-0.0088268/Math.Sqrt(2.0),
              -0.01779187/Math.Sqrt(2.0),4.72E-03/Math.Sqrt(2.0)}
    };


        Series series6 = new Series("FFT_mada");
        public Form1()
        {
            InitializeComponent();

            // チャート全体の背景色を設定
            chart1.BackColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Transparent;

            chart5.BackColor = Color.White;
            chart5.ChartAreas[0].BackColor = Color.Transparent;
            // チャート表示エリア周囲の余白をカットする
            chart1.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart1.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart1.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart1.ChartAreas[0].InnerPlotPosition.X = 2;
            chart1.ChartAreas[0].InnerPlotPosition.Y = 0;

            chart5.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart5.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart5.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart5.ChartAreas[0].InnerPlotPosition.X = 2;
            chart5.ChartAreas[0].InnerPlotPosition.Y = 0;


            // X,Y軸情報のセット関数を定義
            Action<Axis> setAxis = (axisInfo) =>
            {
                // 軸のメモリラベルのフォントサイズ上限値を制限
                axisInfo.LabelAutoFitMaxFontSize = 8;

                // 軸のメモリラベルの文字色をセット
                axisInfo.LabelStyle.ForeColor = Color.Black;

                // 軸タイトルの文字色をセット(今回はTitle未使用なので関係ないが...)
                axisInfo.TitleForeColor = Color.Black;

                // 軸の色をセット
                axisInfo.MajorGrid.Enabled = true;
                axisInfo.MajorGrid.LineColor = ColorTranslator.FromHtml("#000000");
                axisInfo.MinorGrid.Enabled = false;
                axisInfo.MinorGrid.LineColor = ColorTranslator.FromHtml("#000000");
            };

            // X,Y軸の表示方法を定義
            setAxis(chart1.ChartAreas[0].AxisY);
            setAxis(chart1.ChartAreas[0].AxisX);
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.Maximum = 5.5;    // 縦軸の最大値を5にする


            setAxis(chart5.ChartAreas[0].AxisY);
            setAxis(chart5.ChartAreas[0].AxisX);
            chart5.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart5.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            // 縦軸の最大値を5にする
            //グラフタイプ、色などの設定




            //ダミーデータ
            chart1.Series[0].Points.AddXY(100, 0);
            chart1.Series[0].Points.AddXY(0, 0);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string line;
            chart1.Series.Clear();
            Series newSeries1 = new Series("a1");
            newSeries1.ChartType = SeriesChartType.Line;
            newSeries1.BorderWidth = 2;
            newSeries1.Color = Color.Black;

            chart1.Series.Add(newSeries1);


            a1.Clear();
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            ofd.FileName = "default.html";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = @"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter =
                "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]ではじめに
            //「すべてのファイル」が選択されているようにする
            ofd.FilterIndex = 2;
            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                //選択されたファイル名を表示する
                Console.WriteLine(ofd.FileName);
                label1.Text = ofd.FileName;
            }
            try
            {

                // Read the file and display it line by line.
                System.IO.StreamReader file =
                    new System.IO.StreamReader(ofd.FileName);

                while ((line = file.ReadLine()) != null)
                {



                    chart1.Series[0].Points.AddXY(counter + 1, line);
                    counter++;

                    //スケール調整
                    chart1.ResetAutoValues();

                    // Invalidate chart
                    chart1.Invalidate();
                    double Volt = Convert.ToDouble(line);

                    a1.Add(Volt);

                } file.Close();

            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }

            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.
            System.Console.ReadLine();
            chart1.Series[0].IsVisibleInLegend = false;
            chart1.Series[0].IsValueShownAsLabel = false;
        }



        private void button5_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            int size = 4096;
            int bitsize = 12;
            double[] dftIn = new double[size];
            Series series5 = new Series("FFT_mada");

            for (int i = 0; i < size; i++)
            { dftIn[i] = a1[i]; }

            if (a1 != null)
            {

                //ＦＦＴ解析
                FFT t = new FFT(dftIn, size, bitsize, out reFFT, out imFFT);
                //チャートへの書き込み
                for (int i = 0; i < size / 4; i++)
                {
                    if (i > 0)
                    {
                        double x = (double)i * (100.0 / size);
                        double y2 = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);
                        series5.Points.AddXY(x, y2);
                        textBox1.AppendText(y2.ToString("#0.00") + Environment.NewLine);
                        textBox6.AppendText(x.ToString("#0.00") + Environment.NewLine);
                    }
                } chart5.Series.Add(series5);
            }
            // 折れ線グラフとして表示
            chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 保存できる拡張子のフィルタ
            saveFileDialog1.Filter = "テキスト(*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;     // キャンセルなら終了
            }
            else if (this.saveFileDialog1.FileName == "")
            {
                return;     // ファイル名未入力なら終了
            }

            // ファイル名取得
            string filePath = saveFileDialog1.FileName;

            //文字コード(ここでは、Shift JIS)
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");

            //TextBox1の内容を書き込む

            //ファイルの末尾にTextBox1の内容を書き加える
            System.IO.File.AppendAllText(filePath, textBox1.Text, enc);
            System.IO.File.AppendAllText(filePath + 1, textBox7.Text, enc);
            System.IO.File.AppendAllText(filePath + 2, textBox6.Text, enc);
            //ファイルの末尾にTextBox1の内容を書き加える
            textBox1.ResetText();
            textBox6.ResetText();
            textBox7.ResetText();

        }


        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox6.ResetText();
            textBox7.ResetText();
            WR.Clear();
            a1.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();

            Series series5 = new Series("FFT_mada");

            int size = a1.Count;
            double[] afx = new double[size];
            double ave = a1.Average();
            for (int i = 0; i < size - 1; i++)
            {

                double m = a1[i] - a1[i + 1];
                afx[i] = m;

                series5.Points.AddXY(i, afx[i]);


            } chart5.Series.Add(series5);

            Console.WriteLine(ave);


            series5.ChartType = SeriesChartType.Line;
            series5.BorderWidth = 2;
            series5.Color = Color.Black;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();

            Series series5 = new Series("FFT_mada");

            int size = a1.Count;
            double[] afx = new double[size];
            double[] pre = new double[size];
            pre[0] = 0;
            double ave = 0;
            int N = 10;
            int move = 0;
            for (int i = 0; i < size - N; i++)
            {
                if (i > 0)
                {
                    pre[i] = (pre[i - 1] - a1[i - 1]) + a1[i + (N - 1)];

                    afx[i] = pre[i] / N;

                }
                else
                {
                    for (int k = 0; k < N; k++)
                    {
                        ave = ave + a1[move + k];
                        Console.WriteLine(ave);
                    } afx[i] = ave / N;
                    pre[i] = ave - a1[i];
                }


                move = move + 1;

                a1[i] = afx[i];
                series5.Points.AddXY(i, afx[i]);
                textBox1.AppendText(afx[i].ToString("#0.00") + Environment.NewLine);
            } chart5.Series.Add(series5);
            series5.ChartType = SeriesChartType.Line;
            series5.BorderWidth = 2;
            series5.Color = Color.Black;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            int size = 256;
            int shift = 0;
            int bitsize = 8;
            int a = a1.Count - shift;
            double fr = 100.00;
            double rp = fr / size;

            double[] dftIn = new double[size];
            Series series5 = new Series("FFT_mada");
            while (a - size > size)
            {
                a = a1.Count - shift;
                textBox6.ResetText();
                textBox1.AppendText(shift.ToString() + ",");
                Console.WriteLine(shift);
                for (int i = 0; i < size; i++)
                { dftIn[i] = a1[i + shift]; }

                if (a1 != null)
                {

                    //ＦＦＴ解析
                    FFT t = new FFT(dftIn, size, bitsize, out reFFT, out imFFT);

                    //チャートへの書き込み
                    for (int i = 0; i < size / 2; i++)
                    {
                        if (i > 0)
                        {
                            double x = (double)i * (100.00 / size);
                            double y2 = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);

                            textBox1.AppendText(y2.ToString("#0.00") + ",");
                            textBox6.AppendText(x.ToString("#0.00") + Environment.NewLine);

                        }
                    }
                } textBox1.AppendText(Environment.NewLine);
                shift = shift + 64;

                textBox7.AppendText(shift.ToString("#0.00") + Environment.NewLine);
                // 折れ線グラフとして表示

            }

        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            int size = 1024;
            int shift = 0;
            int bitsize = 10;
            int a = a1.Count - shift;
            double fr = 1000.00;
            double rp = fr / size;

            double[] dftIn = new double[size];
            Series series5 = new Series("FFT_mada");
            while (a - size > size)
            {
                a = a1.Count - shift;
                textBox6.ResetText();
                textBox1.AppendText(shift.ToString() + ",");
                Console.WriteLine(shift);
                for (int i = 0; i < size; i++)
                { dftIn[i] = a1[i + shift]; }

                if (a1 != null)
                {

                    //ＦＦＴ解析
                    FFT t = new FFT(dftIn, size, bitsize, out reFFT, out imFFT);

                    //チャートへの書き込み
                    for (int i = 0; i < size / 2; i++)
                    {
                        if (i > 0)
                        {
                            double x = (double)i * (100.00 / size);
                            double y2 = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);

                            textBox1.AppendText(y2.ToString("#0.00") + ",");
                            textBox6.AppendText(x.ToString("#0.00") + Environment.NewLine);

                        }
                    }
                } textBox1.AppendText(Environment.NewLine);
                shift = shift + 1024;

                textBox7.AppendText(shift.ToString("#0.00") + Environment.NewLine);
                // 折れ線グラフとして表示

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int size = a1.Count;
            chart5.Series.Clear();
            chart5.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            double[] tmp;
            double[] tmpim;
            double[] dftIn = new double[size];
            Series series5 = new Series("FFT_mada");
            int bitsize = 10;
            for (int i = 0; size >= 1; ++i)
            {
                size = size / 2;
                bitsize = i;
            }
            /////bit数の繰り上げ
            int datasize = 1 << bitsize;
            size = size / 2;
            if (datasize < size)
            {
                bitsize = bitsize + 1;
            }
            size = datasize;
            double Average = a1.Average();
            for (int i = 0; i < size; i++)
            {
                dftIn[i] = (a1[i] - Average);
            }



            if (a1 != null)
            {

                //ＦＦＴ解析
                FFT t = new FFT(dftIn, size, bitsize, out reFFT, out imFFT);
                t.IFFT(reFFT, imFFT, bitsize, out tmp, out tmpim);

                //チャートへの書き込み
                for (int i = 0; i < size; i++)
                {
                    if (i > 0)
                    {
                        double y2 = tmp[i] ;
                        double x = reFFT[i] ;
                        double z= imFFT[i];
                        textBox1.AppendText(y2.ToString("#0.00") + Environment.NewLine);
                        Console.WriteLine(tmp[i]);
                    }
                } chart5.Series.Add(series5);
            }
            // 折れ線グラフとして表示
            chart5.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;

        }


        private void button4_Click(object sender, EventArgs e)
        {
            chart5.Series.Clear();
            chart5.Series.Clear();

            Series series5 = new Series("FFT_mada");

            int size = a1.Count;
            double[] aave = new double[size];
            double[] afx = new double[size];
            double k = 0;
            double ave = a1.Average();
            for (int i = 0; i < size - 1; i++)
            {

                double m = a1[i] - ave;
                aave[i] = m;

                textBox1.AppendText( aave[i].ToString("#0.0000000") + ",");

            }

            for (int i = 0; i < size - 1; i++)
            {

                k = aave[i] + k;
                afx[i] = k;


                series5.Points.AddXY(i, afx[i]);


            } chart5.Series.Add(series5);
         

            series5.ChartType = SeriesChartType.Line;
            series5.BorderWidth = 2;
            series5.Color = Color.Black;

        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            int size = a1.Count;
            double[] data = new double[size];
            double pi2 = (8.0 * Math.Atan(1.0));
            double N = (size - 1);                 //データ点数                    
            double M = 30;              //周波数の個数
            double FS = 100.0;              //サンプリングの周波数(HZ)
            double sigma = 5;             //係数のσ
            double limit = 0.01;            //ガボール減衰の最小値
            double[] result = new double[size];            //変換結果データ
            double[] result1 = new double[size];            //変換結果データ
            //直流成分除去
            double Average = a1.Average();
            for (int i = 0; i < size; i++)
            {
                data[i] = (a1[i] - Average);
            }

            //周波数のループ
            for (int m = 1; m < M; m++)
            {
                //結果の保存

                double f = 0.5 * m;   //周波数
                double a = 1.0000 / f;    //スケールのａ＝周期
                //データ点数分までのループ

                for (int p = 0; p < N; p++)
                {

                    double valr = 0.0;
                    double vali = 0.0;
                    //畳み込み積分のループ
                    for (int n = -p; n < N - p; n++)
                    {
                        int p1 = p + n;           //畳み込み乗算の位置
                        double t = n / FS / a;    //(t-b)/a
                        double alfa = pi2 * t;  //ωt
                        //ガボール減衰係数＝1 / (2 sqrt(π) σ) * exp(-t^2/σ^2)
                        double gabor = Math.Exp(-t * t / (sigma * sigma)) / (2 * Math.Sqrt(pi2) * sigma);
                        if (gabor >= limit && 0 <= p1 && p1 < N)
                        {
                            //畳み込み乗算：実数、虚数。
                            valr += data[p1] * gabor * Math.Cos(alfa);
                            vali += data[p1] * gabor * Math.Sin(alfa);


                            result[p] = Math.Sqrt(valr * valr + vali * vali) / Math.Sqrt(a);
                            result1[p] = valr;
                        }
                    }
                } for (int p = 0; p < N; p++)
                {

                    textBox1.AppendText(result[p].ToString("#0.0000000") + ",");

                } Console.WriteLine(m);
                textBox1.AppendText(Environment.NewLine);
            }
        

        }
           
        private void button7_Click(object sender, EventArgs e)
        {//////未完成
            int size = a1.Count;
            int k = 4;
            int J = 1;
            double[] data = new double[size];
            double pi2 = (8.0 * Math.Atan(1.0));
            double N = (size - 1);                 //データ点数                    
            double[] result = new double[size];            //変換結果データ
            double[] s1 = new double[size * 2];
            double[] w1 = new double[size];
            double[] p = new double[] { 0.482962913145, 0.836516303738, 0.224143868042, -0.129409522551 };
            double[] q = new double[size];
            double[] coef = new double[size];
            double[] coefr = new double[size + 1000];
            Series series5 = new Series("FFT_mada");

            int sup = k, s_len = size;



            /* ドベシイの数列 p_k (N=2) */

            //直流成分除去
            double Average = a1.Average();
            for (int i = 0; i < size; i++)
            {
                data[i] = (a1[i] - Average);
            }

            for (int i = 0; i < sup; i++)
            {  /* p_k から q_k を生成 */
                q[i] = Math.Pow(-1, i) * p[sup - i - 1];
            }
            for (int i = 0; i < size; i++)
            {
                coef[i] = data[i];
            }


            DWT DWT = new DWT(coef, size, p, q, out s1, out w1);



            /* 1次元高速ウェーブレット変換 */
            s_len = s_len / 2;
            for (int i = 0; i < s_len; i++)
            {
                coef[i] = s1[i];        /* スケーリング係数の保持 */
                coef[i + s_len] = w1[i];


            }

            for (int j = J; j > 0; j--)
            {
                IDWT IDWT = new IDWT(s1, w1, size, p, q, out coefr);

                /* 1次元高速ウェーブレット逆変換による */
                /* レベル j スケーリング係数の再構成 */

                for (int i = 0; i < s_len; i++)
                {
                    coef[i] = coefr[i];
                    /* 再構成されたスケーリング係数の保持 */
                }
            }
            double mse = 0.0;
            for (int i = 0; i < size; i++)
            {
                mse = (data[i] - coef[i]);
                textBox1.AppendText(coef[i].ToString("#0.0000000") + ",");
                series5.Points.AddXY(i, mse);
            } chart5.Series.Add(series5);
            series5.ChartType = SeriesChartType.Line;
            series5.BorderWidth = 2;
            series5.Color = Color.Black;
        }

        private void button8_Click(object sender, EventArgs e)
        {//////未完成

            int size = a1.Count;
            double[] data = new double[size];
            int totalLength = size;  //データ数（2のべき乗）
            int maxLevel = 3;   //分解レベル
            int N = 1;          //次数
            int kMax = 2 * N;
            int daubechiesIndex = N - 1;
            Series series5 = new Series("FFT_mada");
            chart5.Series.Add(series5);
            series5.ChartType = SeriesChartType.Line;
            series5.BorderWidth = 2;
            series5.Color = Color.Black;
            double[] inputSignal = new double[totalLength];
            double[] outputSignal = new double[totalLength];



            double Average = a1.Average();
            for (int i = 0; i < size; i++)
            {
                data[i] = (a1[i] - Average);
            }

            for (int i = 0; i < size; i++)
            {
                inputSignal[i] = data[i];
            }


            for (int level = 1; level <= maxLevel; level++)
            {
                int scale = 1;
                for (int i = 0; i < level; i++)
                {
                    scale *= 2;
                }

                //ダウンサンプリング
                for (int index = 0; index < totalLength / scale; index++)
                {
                    int setApproximationIndex = index;
                    int setDetailIndex = index + totalLength / scale;

                    outputSignal[setApproximationIndex] = 0.0;
                    outputSignal[setDetailIndex] = 0.0;

                    for (int k = 0; k < kMax; k++)
                    {
                        int getInputIndex = 2 * index + k;

                        if (getInputIndex >= 2 * totalLength / scale)
                        {
                            //配列外にデータにアクセスする際に折り返しを行う
                            int over = getInputIndex % (2 * totalLength / scale) + 1;
                            getInputIndex = (2 * totalLength / scale) - over;
                        }

                        //フィルタリング
                        outputSignal[setApproximationIndex] += inputSignal[getInputIndex] * DaubechiesCoefficients[daubechiesIndex][k];
                        outputSignal[setDetailIndex] += inputSignal[getInputIndex] * Math.Pow(-1.0, k) * DaubechiesCoefficients[daubechiesIndex][kMax - 1 - k];
                    }
                }

                //配列コピー
                Array.Copy(outputSignal, inputSignal, totalLength);

                //結果出力

                for (int signalIndex = 0; signalIndex < totalLength / scale; signalIndex++)
                {
                    series5.Points.AddXY(signalIndex, outputSignal[signalIndex]);
                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

           
                int size = a1.Count;
                int bitsize = 0;
          
         
            size = a1.Count;
            double[] data = new double[size];
            double dj = 0.1; //減衰係数
            double fmin = 0.001; //最少周波数                             
            double fmax = 100; //最大周波数                 
            double fs = 100.0;              //サンプリングの周波数(HZ)
            double[,] table;//周波数テーブル
            double[] scales = new double[size];//スケール
            double[] isignalr = new double[size];//実部信号
            double[] isignali = new double[size];//虚部信号
            double delta = 0;//周波数テーブル
            double[,] coefsR; //実部変換結果データ
            double[,] coefsIm;　//虚部変換結果データ　　　　　　
            double fourier_factor = 0;
            double sigma =6;



            if (size > 0)
            {
               bitsize= Bitsize(size);
               int datasize = 1 << bitsize;
               Console.WriteLine(datasize);

            for (int i = 0; i < size; i++)
            {
                double Average = a1.Average();
                data[i] = (a1[i] - Average);  //直流成分除去
            }

            CWT CWT = new CWT();


            CWT.setScale(fs, fmin, fmax, dj, size,sigma, out table, out delta, out scales);  //周波数テーブルセット

          
            CWT.CWTR(data, table, datasize, bitsize, out coefsR);//実部ウェーブレット
            CWT.ICWR(coefsR, delta, datasize, bitsize, out isignalr);//実部逆ウェーブレット
            CWT.CWTC(data, table, datasize, bitsize, out coefsIm);//虚部ウェーブレット
            CWT.ICWTC(coefsIm, delta, datasize, bitsize, out isignali);//虚部逆ウェーブレット
 
            double[,] result = new double[coefsR.GetLength(0), coefsR.GetLength(1)];
            for (int j = coefsR.GetLength(1)-1; j >0 ; --j)
            {

                ////////周波数計算
                  fourier_factor =(4 * Math.PI)/(sigma + Math.Sqrt(2 + sigma *sigma));

                  double f = (scales[j]* fourier_factor);

                  double ff = (1 / f)*fs;

                  WR.Add(ff + ","); 



                //ウェーブレット結果出力
                for (int k = 0; k < coefsR.GetLength(0); ++k)
                {
                    result[k, j] = Math.Sqrt((coefsR[k, j] * coefsR[k, j]) + ((coefsIm[k, j] * coefsIm[k, j]))) / Math.Sqrt(scales[j]); //データの正規化
              
                    int _fs =( coefsR.GetLength(1) -j) -1;
                    WR.Add(result[k, j] + ","); //結果の出力

                }
           
                WR.Add(Environment.NewLine);
        
            }


            for (int i = 0; i < datasize; i++)
            {
                //逆ウェーブレット結果出力

                double z = (isignalr[i] + isignali[i]) / 0.7784;

                textBox1.AppendText(isignalr[i].ToString("#0.000000") + Environment.NewLine);
                textBox6.AppendText(isignali[i].ToString("#0.000000") + Environment.NewLine);
                textBox7.AppendText(z.ToString("#0.000000") + Environment.NewLine);
            }
            }

        }

   

        private void button15_Click(object sender, EventArgs e)
        {
            //0.2秒毎に、周波数を0,8,16,24,0の順で
            //1秒間の正弦波のデータ作成。

            int N = 1024;
            int FS = 100;
            double[] data = new double[N];
            for (int i = 0; i < N; i++)
            {


                data[i] = 1 * Math.Sin(4 * (2 * Math.PI / FS) * i) + (2 * Math.Sin(30 * (2 * Math.PI / FS) * i)) + (3 * Math.Sin(1* (2 * Math.PI / FS) * i)) + (2 * Math.Sin(0.5 * (2 * Math.PI / FS) * i));

               
                a1.Add(data[i]);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            // 保存できる拡張子のフィルタ
            saveFileDialog1.Filter = "テキスト(*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;     // キャンセルなら終了
            }
            else if (this.saveFileDialog1.FileName == "")
            {
                return;     // ファイル名未入力なら終了
            }

            // ファイル名取得
            string filePath = saveFileDialog1.FileName;
            StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            //文字コード(ここでは、Shift JIS)
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");

  
       sw.WriteLine(String.Join("", WR.ToArray()));

　　　　sw.Dispose();


        }
        private static int Bitsize(int size)
        {
         
            int bitsize = 0;
          

            /////  bit数の計算
            for (int i = 0; size >= 1; ++i)
            {
                size = size / 2;
                bitsize = i;
            }

            /////bit数の繰り上げ
            int datasize = 1 << bitsize;
            size = size / 2;
            if (datasize < size)
            {
                bitsize = bitsize + 1;
            }

       

            return bitsize;

        }

    }
}
        


            
     
          
    