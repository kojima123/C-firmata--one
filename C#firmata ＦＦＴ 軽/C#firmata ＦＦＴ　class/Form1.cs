
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
using Firmata.NET;using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
namespace sample_0007
{
    public partial class Form1 : Form

    {
        // 波形サイズ
        int size = 1024;
        // FFT用データ
        private double[] reFFT;
        private double[] imFFT;
        private int pointIndex, pointIndex1, pointIndex2, pointIndex3 = 0;
        List<double> prelist = new List<double>();
        //arduino設定
        Arduino arduino = new Arduino("COM4", 57600);
        //デリゲート設定
        private delegate void Delegate_a0(string data);
        private delegate void Delegate_FFT(string data);
        public Form1()
        {
        
            InitializeComponent();

            // チャート初期化
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            // create a line chart series
            Series newSeries = new Series("a0");
            Series newSeries2 = new Series("a1");
            Series newSeries3 = new Series("a2");
            Series newSeries4 = new Series("a3");
            // チャート全体の背景色を設定
            chart1.BackColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Transparent;
            chart2.BackColor = Color.White; ;
            chart2.ChartAreas[0].BackColor = Color.Transparent;
            chart3.BackColor = Color.White;
            chart3.ChartAreas[0].BackColor = Color.Transparent;
            chart4.BackColor = Color.White;
            chart4.ChartAreas[0].BackColor = Color.Transparent;
            chart5.BackColor = Color.White;
            chart5.ChartAreas[0].BackColor = Color.Transparent;
            // チャート表示エリア周囲の余白をカットする
            chart1.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart1.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart1.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart1.ChartAreas[0].InnerPlotPosition.X = 2;
            chart1.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart2.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart2.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart2.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart2.ChartAreas[0].InnerPlotPosition.X = 2;
            chart2.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart3.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart3.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart3.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart3.ChartAreas[0].InnerPlotPosition.X = 2;
            chart3.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart4.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart4.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart4.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart4.ChartAreas[0].InnerPlotPosition.X = 2;
            chart4.ChartAreas[0].InnerPlotPosition.Y = 0;
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
            chart2.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart2.ChartAreas[0].AxisY);
            setAxis(chart2.ChartAreas[0].AxisX);
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.Maximum = 5.5;    // 縦軸の最大値を5にする
            chart2.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart3.ChartAreas[0].AxisY);
            setAxis(chart3.ChartAreas[0].AxisX);
            chart3.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.Maximum = 5.5;    // 縦軸の最大値を5にする
            chart4.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart4.ChartAreas[0].AxisY);
            setAxis(chart4.ChartAreas[0].AxisX);
            chart4.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisY.Maximum = 5.5;    // 縦軸の最大値を5にする
            chart4.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart5.ChartAreas[0].AxisY);
            setAxis(chart5.ChartAreas[0].AxisX);
            chart5.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart5.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
             // 縦軸の最大値を5にする
        
         //凡例の削除
          
　　　　//グラフタイプ、色などの設定
            newSeries.ChartType = SeriesChartType.Line;
            newSeries.BorderWidth = 2;
            newSeries.Color = Color.Black;
            newSeries2.ChartType = SeriesChartType.Line;
            newSeries2.BorderWidth = 2;
            newSeries2.Color = Color.Black;
            newSeries3.ChartType = SeriesChartType.Line;
            newSeries3.BorderWidth = 2;
            newSeries3.Color = Color.Black;
            newSeries4.ChartType = SeriesChartType.Line;
            newSeries4.BorderWidth = 2;
            newSeries4.Color = Color.Black;
           newSeries4.ChartType = SeriesChartType.Line;
            newSeries4.BorderWidth = 2;
            newSeries4.Color = Color.Black;
            //チャートにデータをセット
                      chart1.Series.Add(newSeries);
                      chart2.Series.Add(newSeries2);
                      chart3.Series.Add(newSeries3);
                      chart4.Series.Add(newSeries4);
            //ダミーデータ
                      chart1.Series[0].Points.AddXY(100 ,0);
                      chart1.Series[0].Points.AddXY(0 , 0);
                      chart2.Series[0].Points.AddXY(100,0);
                      chart2.Series[0].Points.AddXY(0, 0);
                      chart3.Series[0].Points.AddXY(100, 0);
                      chart3.Series[0].Points.AddXY(0, 0);
                      chart4.Series[0].Points.AddXY(100, 0);
                      chart4.Series[0].Points.AddXY(0, 0);
        
        }
        private void Form1_Load(object sender, EventArgs e)
        {


        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            //! ダイアログをクローズする.
            Close();
        }
           //各ボタン設定
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = ("停止");

                {//スレッドの作成と、メソッドa0の実行
                    Thread threadA = new Thread(
                   new ThreadStart(a0)); // （1）
                    threadA.Start(); // （2）
                }
            }
            else
            {
                checkBox1.Text = ("開始");
          
            }
           
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.Text = ("停止");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a1)); // （1）
                    threadA.Start(); // （2）
                }
            }
            else
            {
                checkBox1.Text = ("開始");

            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.Text = ("停止");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a2)); // （1）
                    threadA.Start(); // （2）
                }
            }
            else
            {
                checkBox3.Text = ("開始");

            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox4.Text = ("停止");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a3)); // （1）
                    threadA.Start(); // （2）
                }
            }
            else
            {
                checkBox4.Text = ("開始");

            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox5.Text = ("停止");

                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
            }
            else
            {
                checkBox5.Text = ("開始");
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
            }
        }
        private void plota0(string data)
        {
            if (data != null)
            {
                ////電圧に変化
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                textBox1.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
               
                // チャートにデータを追加

                chart1.Series[0].Points.AddXY(pointIndex + 1, Volt);
                ++pointIndex;
                
                //スケール調整
                chart1.ResetAutoValues();

                {
                    // チャート送り設定
                    while (chart1.Series[0].Points.Count > 100)
                    {
                        chart1.Series[0].Points.RemoveAt(0);
                    }
                }

                // Invalidate chart
                chart1.Invalidate();
            }
        }
        private void plota1(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                textBox2.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
                // チャートにデータを追加
                chart2.Series[0].Points.AddXY(pointIndex1 + 1, Volt);
                ++pointIndex1;
                //スケール調整
                chart2.ResetAutoValues();

                {
                    // チャート送り設定
                    while (chart2.Series[0].Points.Count > 100)
                    {
                        chart2.Series[0].Points.RemoveAt(0);
                    }
                }

                // Invalidate chart
                chart2.Invalidate();
            }
        }
        private void plota2(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                textBox3.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
                // チャートにデータを追加
                chart3.Series[0].Points.AddXY(pointIndex2 + 1, Volt);
                ++pointIndex2;
                //スケール調整
                chart3.ResetAutoValues();
                {
                    // チャート送り設定
                    while (chart3.Series[0].Points.Count > 100)
                    {
                        chart3.Series[0].Points.RemoveAt(0);
                    }
                }
                // Invalidate chart
                chart3.Invalidate();
            }
        }
        private void plota3(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                textBox4.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
                // チャートにデータを追加
                chart4.Series[0].Points.AddXY(pointIndex3+ 1, Volt);
                ++pointIndex3;
                //スケール調整
                chart4.ResetAutoValues();
                {
                    // チャート送り設定
                    while (chart4.Series[0].Points.Count > 100)
                    {
                        chart4.Series[0].Points.RemoveAt(0);
                    }
                }
             
                // Invalidate chart
                chart4.Invalidate();
             
            }
            }
        private void a0()
        {
            while (checkBox1.Checked == true)
            {
                //arduinoからanalogread(0)の取得
                int analog0 = arduino.analogRead(0);
                string data = Convert.ToString(analog0);
                //デリゲートでメソッドplotaoへ送る
                Invoke(new Delegate_a0(plota0), new Object[] { data });
                //10ms休止；1ms単位で設定
                Thread.Sleep(100);
                if (checkBox1.Checked == false) break;
            }
        }
        private void ffta0()
        {
            while (checkBox6.Checked == true)
            {

                int analog0 = arduino.analogRead(0);
                string data = Convert.ToString(analog0);
                Invoke(new Delegate_FFT(plot), new Object[] { data });
                Thread.Sleep(10);
                if (checkBox6.Checked == false) break;
            }
        }
        private void a1()
        {
            while (checkBox2.Checked == true)
            {

                int analog1 = arduino.analogRead(1);
                string data = Convert.ToString(analog1);
                Invoke(new Delegate_a0(plota1), new Object[] { data });

                Thread.Sleep(10);
                if (checkBox2.Checked == false) break;
            }
        }
        private void a2()
        {
            while (checkBox3.Checked == true)
            {
                int analog2 = arduino.analogRead(2);
                string data = Convert.ToString(analog2);
                Invoke(new Delegate_a0(plota2), new Object[] { data });

                Thread.Sleep(10);
               if (checkBox3.Checked == false) break;
            }
        }
        private void a3()
        {
            while (checkBox4.Checked == true)
            {
                int analog3 = arduino.analogRead(3);
                string data = Convert.ToString(analog3);
                Invoke(new Delegate_a0(plota3), new Object[] { data });

                Thread.Sleep(10);
                if (checkBox4.Checked == false) break;
            }
        }
        private void plot(string datas)
        {
            Series series5 = new Series("FFT_mada");
            if (datas != null)
            {
                //FFＴ用の配列作成
                double[] windata = new double[size];
                double[] dftIn = new double[size];
                double[] dftInIm = new double[size];
                double[] Re = new double[size];
               
                int data;

                //取得データを電圧値に変換
                data = Convert.ToInt32(datas);
                double Volt = (data * 5.0) / 1024.0;
                Console.WriteLine(Volt);
                //prelistに追加
                prelist.Add(Volt);
                //prelistの要素数を取得
                int n = prelist.Count;
                Console.WriteLine(n);
              //prelistの要素数とsizeが同じ時に処理を開始：ここでは1024個
                if (n == size)
                {
                    //チャートの初期化
                    chart5.Series.Clear();
                    series5.Points.Clear();

                    //prelistを配列dftInにコピー
                    dftIn = prelist.ToArray();
                    //prelistの要素を削除
                    prelist.Clear();
                    //ＦＦＴ解析
                    FFT t = new FFT(dftIn, size, out reFFT, out imFFT);
                    Console.WriteLine(reFFT);

                  //チャートへの書き込み
                    for (int i = 0; i < size/2; i++)
                    {
                        if (i > 0)
                        { double x = (double)i * (180.0 / size);
                            double y2 = Math.Sqrt(reFFT[i] * reFFT[i] + imFFT[i] * imFFT[i]);
                            series5.Points.AddXY(x, y2);
                        }
                    } chart5.Series.Add(series5);
                } 
 }
 }
        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            {
                // 保存できる拡張子のフィルタ
                saveFileDialog1.Filter = "テキスト(*.txt)|*.log";
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
                //ファイルの末尾にTextBox1の内容を書き加える
                System.IO.File.AppendAllText(filePath+1, textBox2.Text, enc);
                System.IO.File.AppendAllText(filePath +("2.txt"), textBox3.Text, enc);
                System.IO.File.AppendAllText(filePath + 3, textBox4.Text, enc);
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
     if (checkBox6.Checked)
            {
                checkBox6.Text = ("停止");

                {
                    Thread threadB = new Thread(
                   new ThreadStart(ffta0)); // （1）
                    threadB.Start(); // （2）
                }
            }
            else
            {
                checkBox6.Text = ("開始");

            }
        
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox2.ResetText();
            textBox3.ResetText();
            textBox4.ResetText();
        }
    }}

/* End of file */
