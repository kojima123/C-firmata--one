
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
        // �g�`�T�C�Y
        int size = 1024;
        // FFT�p�f�[�^
        private double[] reFFT;
        private double[] imFFT;
        private int pointIndex, pointIndex1, pointIndex2, pointIndex3 = 0;
        List<double> prelist = new List<double>();
        //arduino�ݒ�
        Arduino arduino = new Arduino("COM4", 57600);
        //�f���Q�[�g�ݒ�
        private delegate void Delegate_a0(string data);
        private delegate void Delegate_FFT(string data);
        public Form1()
        {
        
            InitializeComponent();

            // �`���[�g������
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            // create a line chart series
            Series newSeries = new Series("a0");
            Series newSeries2 = new Series("a1");
            Series newSeries3 = new Series("a2");
            Series newSeries4 = new Series("a3");
            // �`���[�g�S�̂̔w�i�F��ݒ�
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
            // �`���[�g�\���G���A���̗͂]�����J�b�g����
            chart1.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart1.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart1.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart1.ChartAreas[0].InnerPlotPosition.X = 2;
            chart1.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart2.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart2.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart2.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart2.ChartAreas[0].InnerPlotPosition.X = 2;
            chart2.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart3.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart3.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart3.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart3.ChartAreas[0].InnerPlotPosition.X = 2;
            chart3.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart4.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart4.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart4.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart4.ChartAreas[0].InnerPlotPosition.X = 2;
            chart4.ChartAreas[0].InnerPlotPosition.Y = 0;
            chart5.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart5.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart5.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart5.ChartAreas[0].InnerPlotPosition.X = 2;
            chart5.ChartAreas[0].InnerPlotPosition.Y = 0;
            // X,Y�����̃Z�b�g�֐����`
            Action<Axis> setAxis = (axisInfo) =>
            {
                // ���̃��������x���̃t�H���g�T�C�Y����l�𐧌�
                axisInfo.LabelAutoFitMaxFontSize = 8;

                // ���̃��������x���̕����F���Z�b�g
                axisInfo.LabelStyle.ForeColor = Color.Black;

                // ���^�C�g���̕����F���Z�b�g(�����Title���g�p�Ȃ̂Ŋ֌W�Ȃ���...)
                axisInfo.TitleForeColor = Color.Black;

                // ���̐F���Z�b�g
                axisInfo.MajorGrid.Enabled = true;
                axisInfo.MajorGrid.LineColor = ColorTranslator.FromHtml("#000000");
                axisInfo.MinorGrid.Enabled = false;
                axisInfo.MinorGrid.LineColor = ColorTranslator.FromHtml("#000000");
            };

            // X,Y���̕\�����@���`
            setAxis(chart1.ChartAreas[0].AxisY);
            setAxis(chart1.ChartAreas[0].AxisX);
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.Maximum = 5.5;    // �c���̍ő�l��5�ɂ���
            chart2.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart2.ChartAreas[0].AxisY);
            setAxis(chart2.ChartAreas[0].AxisX);
            chart2.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.Maximum = 5.5;    // �c���̍ő�l��5�ɂ���
            chart2.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart3.ChartAreas[0].AxisY);
            setAxis(chart3.ChartAreas[0].AxisX);
            chart3.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.Maximum = 5.5;    // �c���̍ő�l��5�ɂ���
            chart4.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart4.ChartAreas[0].AxisY);
            setAxis(chart4.ChartAreas[0].AxisX);
            chart4.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart4.ChartAreas[0].AxisY.Maximum = 5.5;    // �c���̍ő�l��5�ɂ���
            chart4.AntiAliasing = AntiAliasingStyles.None;
            setAxis(chart5.ChartAreas[0].AxisY);
            setAxis(chart5.ChartAreas[0].AxisX);
            chart5.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart5.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
             // �c���̍ő�l��5�ɂ���
        
         //�}��̍폜
          
�@�@�@�@//�O���t�^�C�v�A�F�Ȃǂ̐ݒ�
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
            //�`���[�g�Ƀf�[�^���Z�b�g
                      chart1.Series.Add(newSeries);
                      chart2.Series.Add(newSeries2);
                      chart3.Series.Add(newSeries3);
                      chart4.Series.Add(newSeries4);
            //�_�~�[�f�[�^
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
            //! �_�C�A���O���N���[�Y����.
            Close();
        }
           //�e�{�^���ݒ�
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = ("��~");

                {//�X���b�h�̍쐬�ƁA���\�b�ha0�̎��s
                    Thread threadA = new Thread(
                   new ThreadStart(a0)); // �i1�j
                    threadA.Start(); // �i2�j
                }
            }
            else
            {
                checkBox1.Text = ("�J�n");
          
            }
           
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.Text = ("��~");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a1)); // �i1�j
                    threadA.Start(); // �i2�j
                }
            }
            else
            {
                checkBox1.Text = ("�J�n");

            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.Text = ("��~");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a2)); // �i1�j
                    threadA.Start(); // �i2�j
                }
            }
            else
            {
                checkBox3.Text = ("�J�n");

            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox4.Text = ("��~");

                {
                    Thread threadA = new Thread(
                   new ThreadStart(a3)); // �i1�j
                    threadA.Start(); // �i2�j
                }
            }
            else
            {
                checkBox4.Text = ("�J�n");

            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox5.Text = ("��~");

                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
            }
            else
            {
                checkBox5.Text = ("�J�n");
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
                ////�d���ɕω�
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                textBox1.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
               
                // �`���[�g�Ƀf�[�^��ǉ�

                chart1.Series[0].Points.AddXY(pointIndex + 1, Volt);
                ++pointIndex;
                
                //�X�P�[������
                chart1.ResetAutoValues();

                {
                    // �`���[�g����ݒ�
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
                // �`���[�g�Ƀf�[�^��ǉ�
                chart2.Series[0].Points.AddXY(pointIndex1 + 1, Volt);
                ++pointIndex1;
                //�X�P�[������
                chart2.ResetAutoValues();

                {
                    // �`���[�g����ݒ�
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
                // �`���[�g�Ƀf�[�^��ǉ�
                chart3.Series[0].Points.AddXY(pointIndex2 + 1, Volt);
                ++pointIndex2;
                //�X�P�[������
                chart3.ResetAutoValues();
                {
                    // �`���[�g����ݒ�
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
                // �`���[�g�Ƀf�[�^��ǉ�
                chart4.Series[0].Points.AddXY(pointIndex3+ 1, Volt);
                ++pointIndex3;
                //�X�P�[������
                chart4.ResetAutoValues();
                {
                    // �`���[�g����ݒ�
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
                //arduino����analogread(0)�̎擾
                int analog0 = arduino.analogRead(0);
                string data = Convert.ToString(analog0);
                //�f���Q�[�g�Ń��\�b�hplotao�֑���
                Invoke(new Delegate_a0(plota0), new Object[] { data });
                //10ms�x�~�G1ms�P�ʂŐݒ�
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
                //FF�s�p�̔z��쐬
                double[] windata = new double[size];
                double[] dftIn = new double[size];
                double[] dftInIm = new double[size];
                double[] Re = new double[size];
               
                int data;

                //�擾�f�[�^��d���l�ɕϊ�
                data = Convert.ToInt32(datas);
                double Volt = (data * 5.0) / 1024.0;
                Console.WriteLine(Volt);
                //prelist�ɒǉ�
                prelist.Add(Volt);
                //prelist�̗v�f�����擾
                int n = prelist.Count;
                Console.WriteLine(n);
              //prelist�̗v�f����size���������ɏ������J�n�F�����ł�1024��
                if (n == size)
                {
                    //�`���[�g�̏�����
                    chart5.Series.Clear();
                    series5.Points.Clear();

                    //prelist��z��dftIn�ɃR�s�[
                    dftIn = prelist.ToArray();
                    //prelist�̗v�f���폜
                    prelist.Clear();
                    //�e�e�s���
                    FFT t = new FFT(dftIn, size, out reFFT, out imFFT);
                    Console.WriteLine(reFFT);

                  //�`���[�g�ւ̏�������
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
                // �ۑ��ł���g���q�̃t�B���^
                saveFileDialog1.Filter = "�e�L�X�g(*.txt)|*.log";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;     // �L�����Z���Ȃ�I��
                }
                else if (this.saveFileDialog1.FileName == "")
                {
                    return;     // �t�@�C���������͂Ȃ�I��
                }

                // �t�@�C�����擾
                string filePath = saveFileDialog1.FileName;

                //�����R�[�h(�����ł́AShift JIS)
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");

                //TextBox1�̓��e����������

                //�t�@�C���̖�����TextBox1�̓��e������������
                System.IO.File.AppendAllText(filePath, textBox1.Text, enc);
                //�t�@�C���̖�����TextBox1�̓��e������������
                System.IO.File.AppendAllText(filePath+1, textBox2.Text, enc);
                System.IO.File.AppendAllText(filePath +("2.txt"), textBox3.Text, enc);
                System.IO.File.AppendAllText(filePath + 3, textBox4.Text, enc);
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
     if (checkBox6.Checked)
            {
                checkBox6.Text = ("��~");

                {
                    Thread threadB = new Thread(
                   new ThreadStart(ffta0)); // �i1�j
                    threadB.Start(); // �i2�j
                }
            }
            else
            {
                checkBox6.Text = ("�J�n");

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
