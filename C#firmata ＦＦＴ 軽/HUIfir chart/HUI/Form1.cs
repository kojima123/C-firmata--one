using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Firmata.NET;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Kinect;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Speech.AudioFormat;  // Microsoft.Speechを参照設定する
using Microsoft.Speech.Recognition;  // 標準的なインストール場所は、
// C:\Program Files\Microsoft SDKs\Speech\v11.0\Assembly\Microsoft.Speech.dll

using Firmata.NET;
namespace HUI
{
    public partial class Form1 : Form
    {
        Arduino arduino = new Arduino("COM5", 57600);
        private int pointIndex, pointIndex1, pointIndex2, pointIndex3 = 0;

        int time, time1, time2 = 0, prevolte, prevolte1, prevolte2 = 1;
        private delegate void Delegate_a0(string data);
        // Kinectセンサー
        KinectSensor sensor = null;

        // 音声認識エンジン
        private SpeechRecognitionEngine speechEngine = null;

        // ジョイントの座標を保存するためのディクショナリー
        Dictionary<JointType, JointInfo> JointMappings =
          new Dictionary<JointType, JointInfo>();

        // 円を描くための変数
        bool fDrawCircle = false;
        List<Point> Circles = new List<Point>();
        // 四角形を描くための変数
        bool fDrawRect = false;
        List<Point> Rects = new List<Point>();

        // コンストラクタ。クラスのインスタンスが作成されるときに実行される
        public Form1()
        {// チャート初期化
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
            chart1.Series[0].Points.AddXY(100, 0);
            chart1.Series[0].Points.AddXY(0, 0);
            chart2.Series[0].Points.AddXY(100, 0);
            chart2.Series[0].Points.AddXY(0, 0);
            chart3.Series[0].Points.AddXY(100, 0);
            chart3.Series[0].Points.AddXY(0, 0);
            chart4.Series[0].Points.AddXY(100, 0);
            chart4.Series[0].Points.AddXY(0, 0);
        





            InitializeComponent();

            try
            {
                // Kinectが接続されているかどうかを確認する
                if (KinectSensor.KinectSensors.Count == 0)
                {
                    string msg = "Kinect for Windowsセンサーが" +
                      "接続されていません。";
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // フォームがロードされるときに実行される
        private void Form1_Load(object sender, EventArgs e)
        {
            // Kinectが接続されているかどうかを確認する
            if (KinectSensor.KinectSensors.Count == 0)
            {
                this.Close();
                return;
            }

            // ウィンドウの有効なサイズを設定する
            this.ClientSize = new Size(640,
              480);
            // ピクチャーボックスのサイズと位置を設定する
            pictureBoxSkltn.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxSkltn.Top = menuStrip1.Height;
            pictureBoxSkltn.Left = 5;
            pictureBoxSkltn.Size = new Size(640, 480);
            // ステータスバーの準備
            statusStrip1.Items.Add("Start");

            // Kinect センサーの状態変更を通知するようにする
            KinectSensor.KinectSensors.StatusChanged +=
              new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            // センサーを利用可能にする
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }
            if (sensor == null)
            {
                string msg = "Kinect for Windowsセンサーが" +
                  "利用できません。";
                MessageBox.Show(msg);
                return;
            }

            StartKinect(sensor);

            // 音声認識を利用できるようにする
            StartRecognizer();
        }

        // 音声認識を利用できるようにする
        private void StartRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();

            if (null != ri)
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);

                InitSpeechEngine(ri);

                speechEngine.SpeechRecognized += SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += SpeechRejected;

                speechEngine.SetInputToAudioStream(
                  sensor.AudioSource.Start(),
                  new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                string msg = "音声認識機能が使用できません。";
                MessageBox.Show(msg);
            }
        }

        private void StartKinect(KinectSensor kinect)
        {
            sensor = kinect;
            // カメラを有効にして、フレーム更新イベントを登録する
            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable();
            sensor.AllFramesReady +=
              new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);

            // Kinectの動作を開始する
            sensor.Start();
        }

        // センサーの状態が変更された時の通知を受け取る
        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // デバイスが接続された
            if (e.Status == KinectStatus.Connected)
            {
                StartKinect(e.Sensor);
                statusStrip1.Items[0].Text = "Kinectが接続されました。";
            }
            // デバイスが切断された
            else if (e.Status == KinectStatus.Disconnected)
            {
                StopKinect();
                statusStrip1.Items[0].Text = "Kinectの接続がなくなりました。";
            }
            // 電源が供給されていない
            else if (e.Status == KinectStatus.NotPowered)
            {
                StopKinect();
                statusStrip1.Items[0].Text = "Kinectの電源が供給されていません。";
            }
            // サポートされていないデバイス
            else if (e.Status == KinectStatus.DeviceNotSupported)
            {
                statusStrip1.Items[0].Text =
                  "デバイス（おそらくKinect for Xbox 360）はサポートされません";
            }
            // USBの帯域が足りない
            else if (e.Status == KinectStatus.InsufficientBandwidth)
            {
                statusStrip1.Items[0].Text = "USBの帯域が足りません";
            }
        }

        private void StopKinect()
        {
            if (sensor != null)
            {
                sensor.AudioSource.Stop();
                // フレーム更新イベントを削除する
                sensor.AllFramesReady -= kinect_AllFramesReady;
                // Kinectセンサーを止めて破棄する
                sensor.AudioSource.Stop();
                sensor.Stop();
                sensor.Dispose();
                sensor = null;
            }
        }

        // 認識システムの情報を取得する
        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase)
                  && "ja-JP".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        // 認識エンジンを初期化する
        private void InitSpeechEngine(RecognizerInfo ri)
        {
            var directions = new Choices();
            directions.Add(new SemanticResultValue("おわり", "QUIT"));
            directions.Add(new SemanticResultValue("まる", "DRAWCIRCLE"));
            directions.Add(new SemanticResultValue("", "DRAWRECT"));
            directions.Add(new SemanticResultValue("けす", "CLEAR"));
            directions.Add(new SemanticResultValue("たすけて", "HELP"));

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(directions);
            var g = new Grammar(gb);
            speechEngine.LoadGrammar(g);
        }

        //　認識できたときのイベントハンドラ
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // 信頼性。0.3以下なら認識したとは判断しない
            const double ConfidenceThreshold = 0.3;

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "QUIT":
                        statusStrip1.Items[0].Text = "終了します。";
                        this.Close();
                        break;
                    case "HELP":
                        statusStrip1.Items[0].Text = "";
                        ShowHelp();
                        break;
                    case "DRAWRECT":
                        statusStrip1.Items[0].Text = "四角形を描きます。";
                        fDrawRect = true;
                        break;
                    case "DRAWCIRCLE":
                        statusStrip1.Items[0].Text = "円を描きます。";
                        fDrawCircle = true;
                        break;
                    case "CLEAR":
                        statusStrip1.Items[0].Text = "円と四角を消しました。";
                        Circles.Clear();
                        Rects.Clear();
                        break;
                }
            }
        }

        // ヘルプダイアログボックスを表示する
        private void ShowHelp()
        {
            // 音声認識を止める
            StopRecognizer();

            // ダイアログボックスを作成する
            Form2 dialog = new Form2();

            // ダイアログボックスを表示する
            dialog.ShowDialog();

            // 音声認識を利用できるようにする
            StartRecognizer();
        }

        //　認識できないときのイベントハンドラ
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.Beep();
            statusStrip1.Items[0].Text = "認識できません。";
        }

        // フレームデータの更新通知を受け取る
        void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (sensor == null)
                return;
            try
            {
                Bitmap bitmap = null;
                // RGBカメラのフレームデータを取得する
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    if (colorFrame != null)
                    {
                        // RGBカメラのピクセルデータを取得する
                        byte[] colorPixel = new byte[colorFrame.PixelDataLength];
                        colorFrame.CopyPixelDataTo(colorPixel);

                        PixelFormat frmt = PixelFormat.Format32bppRgb;
                        // ピクセルデータをビットマップに変換する
                        bitmap = new Bitmap(sensor.ColorStream.FrameWidth,
                          sensor.ColorStream.FrameHeight, frmt);

                        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        BitmapData data = bitmap.LockBits(rect, ImageLockMode.WriteOnly,
                                                frmt);
                        Marshal.Copy(colorPixel, 0, data.Scan0, colorPixel.Length);
                        bitmap.UnlockBits(data);
                    }
                }

                // スケルトン（骨格）フレームデータの更新通知を受け取る
                using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (bitmap != null)
                    {
                        Graphics g = Graphics.FromImage(bitmap);
                        Brush brush = Brushes.Aqua;
                        if (skeletonFrame != null && bitmap != null)
                        {
                            // スケルトンデータを取得する
                            Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                            skeletonFrame.CopySkeletonDataTo(skeletonData);

                            // プレーヤーごとのスケルトンを描画する
                            foreach (var skeleton in skeletonData)
                            {
                                JointMappings.Clear();
                                // ジョイント（関節）を描画する
                                foreach (Joint joint in skeleton.Joints)
                                {
                                    // ジョイントの座標をカラー座標に変換する
                                    ColorImagePoint point =
                                      sensor.CoordinateMapper.MapSkeletonPointToColorPoint(
                                        joint.Position,
                                        sensor.ColorStream.Format);

                                    JointMappings.Add(joint.JointType,
                                      new JointInfo(joint.TrackingState, point.X, point.Y));
                                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                                    {
                                        // 円を書く
                                        if (point.X > 0 && point.Y > 0)
                                            g.FillEllipse(brush, point.X, point.Y, 10, 10);
                                    }        // ニアモードの場合、スケルトン位置(Center hip)を描画する
                                    else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly)
                                    {
                                        // スケルトンの座標を描く
                                        if (point.X > 0 && point.Y > 0)
                                            g.FillEllipse(brush, point.X - 5, point.Y - 5, 10, 10);
                                    }


                                    // 右手
                                    if (joint.TrackingState != JointTrackingState.Inferred
                                      && joint.JointType == JointType.HandRight)
                                    {

                                        {
                                            if (point.X > 0 && point.Y > 0)
                                            {
                                                float x = skeleton.Joints[JointType.HandRight].Position.X;
                                                float y = skeleton.Joints[JointType.HandRight].Position.Y;
                                                float z = skeleton.Joints[JointType.HandRight].Position.Z;
                                                Circles.Add(new Point(point.X, point.Y));
                                                textBox1.AppendText(x.ToString() + Environment.NewLine);
                                                textBox2.AppendText(y.ToString() + Environment.NewLine);
                                                textBox3.AppendText(z.ToString() + Environment.NewLine);

                                            }

                                            fDrawCircle = false;
                                        }
                                    }

                                    // 左手
                                    if (joint.TrackingState != JointTrackingState.Inferred
                                      && joint.JointType == JointType.HandLeft)
                                    {
                                        if (fDrawRect)
                                        {
                                            if (point.X > 0 && point.Y > 0)
                                            {
                                                Rects.Add(new Point(point.X, point.Y));
                                                fDrawRect = false;
                                            }
                                        }
                                    }
                                }
                                DrawBones(g);
                            }
                        }
                    }
                }
                // ピクチャーボックスに表示する
                if (bitmap != null)
                {
                    Graphics g = Graphics.FromImage(bitmap);
                    DarwCircles(g);
                    DarwRects(g);
                    pictureBoxSkltn.Image = bitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 円を描く
        private void DarwCircles(Graphics g)
        {
            if (Circles.Count < 1)
                return;

            Brush brush = Brushes.Red;
            // Pen pen = Pens.Brown;  //DrawEllipse()を使う時

            foreach (var point in Circles)
            {
                g.FillEllipse(brush, point.X - 10, point.Y - 10, 20, 20);
                // 塗り潰さないならDrawEllipse()を使う
                // g.DrawEllipse(pen, point.X - 10, point.Y - 10, 20, 20);
            }
        }

        // 四角形を描く
        private void DarwRects(Graphics g)
        {
            if (Rects.Count < 1)
                return;

            Brush brush = Brushes.Brown;
            // Pen pen = Pens.Brown;  DrawRectangle()を使う時

            foreach (var point in Rects)
            {
                g.FillRectangle(brush, point.X - 10, point.Y - 10, 20, 20);
                // 塗り潰さないならDrawRectangle()を使う
                // g.DrawRectangle(pen, point.X - 10, point.Y - 10, 20, 20);
            }
        }

        // ジョイントを結ぶ線（ボーン、骨）を描く
        private void DrawBones(Graphics g)
        {
            // 頭と胴体を描く
            this.DrawBone(g, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(g, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(g, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(g, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(g, JointType.Spine, JointType.HipCenter);
            this.DrawBone(g, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(g, JointType.HipCenter, JointType.HipRight);

            // 左腕
            this.DrawBone(g, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(g, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(g, JointType.WristLeft, JointType.HandLeft);

            // 右腕
            this.DrawBone(g, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(g, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(g, JointType.WristRight, JointType.HandRight);

            // 左足
            this.DrawBone(g, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(g, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(g, JointType.AnkleLeft, JointType.FootLeft);

            // 右足
            this.DrawBone(g, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(g, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(g, JointType.AnkleRight, JointType.FootRight);
        }

        // 骨を描く
        private void DrawBone(Graphics g, JointType jointType1, JointType jointType2)
        {
            JointInfo j1, j2;

            // どちらかのジョイントが見つからなければ描かない
            if (!JointMappings.TryGetValue(jointType1, out j1) ||
                j1.state == JointTrackingState.NotTracked ||
                !JointMappings.TryGetValue(jointType2, out j2) ||
                j2.state == JointTrackingState.NotTracked)
            {
                return;
            }

            // 両方の点が推測である場合は描かない
            if (j1.state == JointTrackingState.Inferred &&
                j2.state == JointTrackingState.Inferred)
            {
                return;
            }

            // 線を描く
            g.DrawLine(Pens.Black, j1.x, j1.y, j2.x, j2.y);
        }

        // フォームが閉じられようとしているときに実行される
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Kinectの使用を停止する
            StopKinect();

            StopRecognizer();
        }

        private void StopRecognizer()
        {
            if (speechEngine != null)
            {
                speechEngine.SpeechRecognized -= SpeechRecognized;
                speechEngine.SpeechRecognitionRejected -= SpeechRejected;
                speechEngine.RecognizeAsyncStop();
            }
        }

        // [ファイル]-[終了]が選択されたときに実行される
        private void fileExit_Click(object sender, EventArgs e)
        {
            // フォームを閉じる＝アプリケーションを終了する
            this.Close();
        }

        // [F1]キーでヘルプダイアログボックスを表示する
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                ShowHelp();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //! ダイアログをクローズする.
            Close();
        }
 

       
      
        private void plota0(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
              a0Box.AppendText(Volt.ToString("#0.00") + Environment.NewLine);


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
               a1Box.AppendText(Volt.ToString("#0.00") + Environment.NewLine);

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
        private void plota2(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
               a2Box.AppendText(Volt.ToString("#0.00") + Environment.NewLine);

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
        private void plota3(string data)
        {
            if (data != null)
            {
                int data00;
                data00 = Convert.ToInt32(data);
                double Volt = (data00 * 5.0) / 1024.0;
                a3Box.AppendText(Volt.ToString("#0.00") + Environment.NewLine);

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
                int analog0 = arduino.analogRead(0);
                string data = Convert.ToString(analog0);
                //デリゲートでメソッドplotaoへ送る
                Invoke(new Delegate_a0(plota0), new Object[] { data });
                //10ms休止；1ms単位で設定
                Thread.Sleep(10);
                if (checkBox1.Checked == false) break;
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
         
                checkBox4.Checked = true;
            }
            else
            {
                checkBox5.Text = ("開始");
                checkBox1.Checked = false;
                checkBox2.Checked = false;
   
                checkBox4.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                //arduino
                //ファイルの末尾にTextBoxの内容を書き加える
                System.IO.File.AppendAllText(filePath, a0Box.Text, enc);
                System.IO.File.AppendAllText(filePath + ("1.txt"), a1Box.Text, enc);
                System.IO.File.AppendAllText(filePath + ("2.txt"), a2Box.Text, enc);
                System.IO.File.AppendAllText(filePath + ("3.txt"), a3Box.Text, enc);
                //kinect
                //ファイルの末尾にTextBoxの内容を書き加える
                System.IO.File.AppendAllText(filePath+("kinex.txt"), textBox1.Text, enc);
                System.IO.File.AppendAllText(filePath + ("kiney.txt"), textBox2.Text, enc);
                System.IO.File.AppendAllText(filePath + ("kinez.txt"), textBox3.Text, enc);
    
            }
        }



    }
  // ジョイント（関節）の状態と座標を保存するためのクラス
  public class JointInfo
  {
    public int x, y;  // 座標。Pointでもよいがパフォーマンス優先
    public JointTrackingState state; // 状態

    // JointInfoのコンストラクタ
    public JointInfo(JointTrackingState jts, int xx, int yy)
    {
      this.state = jts;
      this.x = xx;
      this.y = yy;
    }
  }


}
