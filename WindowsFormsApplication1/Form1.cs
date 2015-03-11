
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
        public static double[][] DaubechiesCoefficients ={  };
        public Form1()
        {　　 InitializeComponent();
            // チャート全体の背景色を設定
            Readchart.BackColor = Color.White;
            Readchart.ChartAreas[0].BackColor = Color.Transparent;
            Rsultchart.BackColor = Color.White;
            Rsultchart.ChartAreas[0].BackColor = Color.Transparent;
            // チャート表示エリア周囲の余白をカットする
            Readchart.ChartAreas[0].InnerPlotPosition.Auto = false;
            Readchart.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            Readchart.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            Readchart.ChartAreas[0].InnerPlotPosition.X = 2;
            Readchart.ChartAreas[0].InnerPlotPosition.Y = 0;
            Rsultchart.ChartAreas[0].InnerPlotPosition.Auto = false;
            Rsultchart.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            Rsultchart.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            Rsultchart.ChartAreas[0].InnerPlotPosition.X = 2;
            Rsultchart.ChartAreas[0].InnerPlotPosition.Y = 0;
            // X,Y軸情報のセット関数を定義
            Action<Axis> setAxis = (axisInfo) =>
            {　// 軸のメモリラベルのフォントサイズ上限値を制限
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
            setAxis(Readchart.ChartAreas[0].AxisY);
            setAxis(Readchart.ChartAreas[0].AxisX);
            Readchart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            Readchart.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            Readchart.ChartAreas[0].AxisY.Maximum = 5.5;    // 縦軸の最大値を5にする
            setAxis(Rsultchart.ChartAreas[0].AxisY);
            setAxis(Rsultchart.ChartAreas[0].AxisX);
            Rsultchart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            Rsultchart.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            //ダミーデータ
            Readchart.Series[0].Points.AddXY(100, 0);
            Readchart.Series[0].Points.AddXY(0, 0);
        }
        private void open_Click(object sender, EventArgs e)
        {　 int counter = 0;
            string line;
            Readchart.Series.Clear();
            Series newSeries1 = new Series("a1");
            newSeries1.ChartType = SeriesChartType.Line;
            newSeries1.BorderWidth = 2;
            newSeries1.Color = Color.Black;
            Readchart.Series.Add(newSeries1);
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
            {　// データをラインごとに読み込み
                System.IO.StreamReader file =
                    new System.IO.StreamReader(ofd.FileName);
                while ((line = file.ReadLine()) != null)
                {　 Readchart.Series[0].Points.AddXY(counter + 1, line);
                    counter++;
                    //スケール調整
                    Readchart.ResetAutoValues();
                    // チャートのインターバル
                    Readchart.Invalidate();
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
            Readchart.Series[0].IsVisibleInLegend = false;
            Readchart.Series[0].IsValueShownAsLabel = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {   Rsultchart.Series.Clear();
            Rsultchart.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            int size = 4096;
            int bitsize = 12;
            double[] dftIn = new double[size];
            Series series = new Series("FFT_mada");

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
                        series.Points.AddXY(x, y2);
                        Result1.AppendText(y2.ToString("#0.00") + Environment.NewLine);
                        Result2.AppendText(x.ToString("#0.00") + Environment.NewLine);
                    }
                } Rsultchart.Series.Add(series);
            }
            // 折れ線グラフとして表示
            Rsultchart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
        }
        private void save_Click(object sender, EventArgs e)
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
            System.IO.File.AppendAllText(filePath, Result1.Text, enc);
            System.IO.File.AppendAllText(filePath + 1, Result3.Text, enc);
            System.IO.File.AppendAllText(filePath + 2, Result2.Text, enc);
            //ファイルの末尾にTextBox1の内容を書き加える
            Result1.ResetText();
            Result2.ResetText();
            Result3.ResetText();
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            Result1.ResetText();
            Result2.ResetText();
            Result3.ResetText();
            WR.Clear();
            a1.Clear();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void FFT_Click(object sender, EventArgs e)
        {
            int size = a1.Count;
            Rsultchart.Series.Clear();
            Rsultchart.Series.Clear();
            double[] reFFT;
            double[] imFFT;
            double[] tmp;
            double[] tmpim;
            double[] dftIn = new double[size];
            Series series = new Series("FFT_mada");
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
                for (int i = 0; i < size / 2; i++)
                {
                    if (i > 0)
                    {
                        double x = (double)i * (100.00 / size);
                        double y2 = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);
                        Result1.AppendText(y2.ToString("#0.00") + ",");
                        Result2.AppendText(x.ToString("#0.00") + Environment.NewLine);
                    }
                } Rsultchart.Series.Add(series);
            }
            // 折れ線グラフとして表示
            Rsultchart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
        }
        private void　gabor_Click_1(object sender, EventArgs e)
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
            {　//結果の保存
                double f = 0.5 * m;   //周波数
                double a = 1.0000 / f;    //スケールのａ＝周期
                //データ点数分までのループ

                for (int p = 0; p < N; p++)
                {   double valr = 0.0;
                    double vali = 0.0;
                    //畳み込み積分のループ
                    for (int n = -p; n < N - p; n++)
                    {   int p1 = p + n;           //畳み込み乗算の位置
                        double t = n / FS / a;    //(t-b)/a
                        double alfa = pi2 * t;  //ωt
                        //ガボール減衰係数＝1 / (2 sqrt(π) σ) * exp(-t^2/σ^2)
                        double gabor = Math.Exp(-t * t / (sigma * sigma)) / (2 * Math.Sqrt(pi2) * sigma);
                        if (gabor >= limit && 0 <= p1 && p1 < N)
                        {   //畳み込み乗算：実数、虚数。
                            valr += data[p1] * gabor * Math.Cos(alfa);
                            vali += data[p1] * gabor * Math.Sin(alfa);
                            result[p] = Math.Sqrt(valr * valr + vali * vali) / Math.Sqrt(a);
                            result1[p] = valr;
                        }
                    }
                } for (int p = 0; p < N; p++)
                {
                    Result1.AppendText(result[p].ToString("#0.0000000") + ",");
                } Console.WriteLine(m);
                Result1.AppendText(Environment.NewLine);
            }
        }
        private void csvsave_Click(object sender, EventArgs e)
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
             WR.Clear();
        }
        private static int Bitsize(int size)
        {   int bitsize = 0;
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
        private void morlet1_Click(object sender, EventArgs e)
        {
            int size = a1.Count;
            int bitsize = 0;
            size = a1.Count;
            double[] data = new double[size];
            double Cd = 0.7784; //復元係数σ５＝0.9484 　６＝0.7784　7＝0.8886 ８＝0.7794　１０＝0.4579
            double dj = 0.1; //目盛間隔
            double fmin = 0.01; //最少周波数                             
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
                bitsize = Bitsize(size);
                int datasize = 1 << bitsize;
                Console.WriteLine(datasize);
                for (int i = 0; i < size; i++)
                {
                    double Average = a1.Average();
                    data[i] = (a1[i] - Average);  //直流成分除去
                }
                CWT CWT = new CWT();
                CWT.setScale(fs, fmin, fmax, dj, size, sigma, out table, out delta, out scales);  //周波数テーブル
                CWT.CWTR(data, table, datasize, bitsize, out coefsR);//実部ウェーブレット
                CWT.ICWR(coefsR, delta, datasize, bitsize, out isignalr);//実部逆ウェーブレット
                CWT.CWTC(data, table, datasize, bitsize, out coefsIm);//虚部ウェーブレット
                CWT.ICWTC(coefsIm, delta, datasize, bitsize, out isignali);//虚部逆ウェーブレット
                double Fa = 0;
                double[,] result = new double[coefsR.GetLength(0), coefsR.GetLength(1)];

                {
                    for (int i = 0; i < coefsR.GetLength(0) + 1; ++i)
                    { WR.Add(i + ","); } //結果の出力

                }
                WR.Add(Environment.NewLine);
                for (int j = coefsR.GetLength(1) - 1; j > 0; --j)
                {   Fa = 0;
                    ////////周波数計算
                    fourier_factor = (4 * Math.PI) / (sigma + Math.Sqrt(2 + sigma * sigma));
                    double f = (scales[j] * fourier_factor);
                    double ff = (1 / f) * fs;
                    WR.Add(ff + ",");

                    //ウェーブレット結果出力
                    for (int k = 0; k < coefsR.GetLength(0); ++k)
                    { result[k, j] = ((coefsR[k, j] * coefsR[k, j]) + (coefsIm[k, j] * coefsIm[k, j])) / Math.Sqrt(scales[j]); //データの正規化
                        Fa += result[k, j];
                        WR.Add(result[k, j] + ","); //結果の出力
                    }
                    WR.Add(Environment.NewLine);
                    Result1.AppendText(Fa.ToString("#0.000000") + Environment.NewLine);
                }
                a1.Clear();
                for (int i = 0; i < datasize; i++)
                {
                    //逆ウェーブレット結果出力
                    double z = (isignalr[i] + isignali[i]) / Cd;
                    a1.Add(z);
                    Result3.AppendText(z.ToString("#0.000000") + Environment.NewLine);
                }
            }
        }
    }
}
        


            
     
          
    