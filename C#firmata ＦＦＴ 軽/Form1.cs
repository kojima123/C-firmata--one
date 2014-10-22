
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


        // �g�`�T�C�Y
        int size = 512;

        private int pointIndex, pointIndex1, pointIndex2, pointIndex3, Count, Count1, Count2, Count3 = 0;
        double preVolt = 0;
        //arduino�ݒ�

        //�f���Q�[�g�ݒ�
        private delegate void Delegate_a0(string data);
        public Form1()
        {

            InitializeComponent();

            // �`���[�g������
            chart1.Series.Clear();

            Series newSeries = new Series("a0");
            Series newSeries2 = new Series("a1");
            Series newSeries3 = new Series("a2");
            Series newSeries4 = new Series("a3");
            // �`���[�g�S�̂̔w�i�F��ݒ�
            chart1.BackColor = Color.White;
            chart1.ChartAreas[0].BackColor = Color.Transparent;


            // �`���[�g�\���G���A���̗͂]�����J�b�g����
            chart1.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart1.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart1.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(�����̃��������x���󎚕��̗]�T��݂���)
            chart1.ChartAreas[0].InnerPlotPosition.X = 2;
            chart1.ChartAreas[0].InnerPlotPosition.Y = 0;

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


            //�}��̍폜

            //�O���t�^�C�v�A�F�Ȃǂ̐ݒ�
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

            //�_�~�[�f�[�^
            chart1.Series[0].Points.AddXY(100, 0);
            chart1.Series[0].Points.AddXY(0, 0);


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] PortList = SerialPort.GetPortNames();

            cmbPortName.Items.Clear();
            //! �V���A���|�[�g�����R���{�{�b�N�X�ɃZ�b�g����.
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
            //! �_�C�A���O���N���[�Y����.
            arduino.Close();
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


        private void plota0(string data)
        {
            if (data != null)
            {
                ////�d���ɕω�
                int data00;
                data00 = Convert.ToInt32(data);




                double g = (-2.2744 * data00) + 1468.2;


                double Volt = (data00 * 5.0) / 4024.0;
                textBox1.AppendText(Volt.ToString("#0.00") + Environment.NewLine);
                Console.WriteLine(data00);
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


        private void a0()
        {


            while (checkBox1.Checked == true)
            {
                //arduino����analogread(0)�̎擾
                int analog2 = arduino.analogRead(2);
                string data = Convert.ToString(analog2);
                //�f���Q�[�g�Ń��\�b�hplotao�֑���
                Invoke(new Delegate_a0(plota0), new Object[] { data });
                //10ms�x�~�G1ms�P�ʂŐݒ�
                Thread.Sleep(1);
                if (checkBox1.Checked == false) break;
            }
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
                    checkBox5.Text = ("��~");

                    string PortName = cmbPortName.SelectedItem.ToString();


                    arduino.ArduinoOpen(PortName, 57600); 
                  
                }
                else
                {
                    checkBox5.Text = ("�J�n");
                    arduino.Close();
                }

            }
            //! �I�[�v������V���A���|�[�g���R���{�{�b�N�X������o��.
            

            //! �{�^���̕\����[�ڑ�]����[�ؒf]�ɕς���.
        }
    }
}




/* End of file */
