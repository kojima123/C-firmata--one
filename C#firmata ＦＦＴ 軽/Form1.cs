
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
using Firmata.NET;
using Sharpduino;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
namespace sample_0007
{



    public partial class Form1 : Form
    {
        Arduino arduino = new Arduino();


        // 波形サイズ
        int size = 512;

        private int pointIndex, pointIndex1, pointIndex2, pointIndex3, Count, Count1, Count2, Count3 = 0;
        double preVolt = 0;
        //arduino設定

        //デリゲート設定
        private delegate void Delegate_a0(string data);
        public Form1()
        {

            InitializeComponent();

            // チャート初期化
            chart1.Series.Clear();

            Series newSeries = new Series("a0");
            Series newSeries2 = new Series("a1");
            Series newSeries3 = new Series("a2");
            Series newSeries4 = new Series("a3");
            // チャート全体の背景色を設定
            chart1.BackColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Transparent;


            // チャート表示エリア周囲の余白をカットする
            chart1.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart1.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart1.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart1.ChartAreas[0].InnerPlotPosition.X = 2;
            chart1.ChartAreas[0].InnerPlotPosition.Y = 0;

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

            //ダミーデータ
            chart1.Series[0].Points.AddXY(100, 0);
            chart1.Series[0].Points.AddXY(0, 0);


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] PortList = SerialPort.GetPortNames();

            cmbPortName.Items.Clear();
            //! シリアルポート名をコンボボックスにセットする.
            foreach (string PortName in PortList)
            {
                cmbPortName.Items.Add(PortName);
            }
            if (cmbPortName.Items.Count > 0)
            {
                cmbPortName.SelectedIndex = 0;
            }




        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            //! ダイアログをクローズする.
            arduino.Close();
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


        private void plota0(string data)
        {
            if (data != null)
            {
                ////電圧に変化
                int data00;
                data00 = Convert.ToInt32(data);




                double g = (-2.2744 * data00) + 1468.2;


                double Volt = (data00 * 5.0) / 4024.0;
                textBox1.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
                Console.WriteLine(data00);
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


        private void a0()
        {


            while (checkBox1.Checked == true)
            {
                //arduinoからanalogread(0)の取得
                int analog2 = arduino.analogRead(2);
                string data = Convert.ToString(analog2);
                //デリゲートでメソッドplotaoへ送る
                Invoke(new Delegate_a0(plota0), new Object[] { data });
                //10ms休止；1ms単位で設定
                Thread.Sleep(1);
                if (checkBox1.Checked == false) break;
            }
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

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.ResetText();

        }

      

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (checkBox5.Checked)
                {
                    checkBox5.Text = ("停止");

                    string PortName = cmbPortName.SelectedItem.ToString();


                    arduino.ArduinoOpen(PortName, 57600); 
                  
                }
                else
                {
                    checkBox5.Text = ("開始");
                    arduino.Close();
                }

            }
            //! オープンするシリアルポートをコンボボックスから取り出す.
            

            //! ボタンの表示を[接続]から[切断]に変える.
        }
    }
}




/* End of file */
