using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Kinect;
using System.Drawing.Imaging;
using Microsoft.Speech.AudioFormat;  // Microsoft.Speechを参照設定する
using Microsoft.Speech.Recognition;  // 標準的なインストール場所は、
// C:\Program Files\Microsoft SDKs\Speech\v11.0\Assembly\Microsoft.Speech.dll

namespace HUI
{
  public partial class Form2 : Form
  {
    KinectSensor sensor = null;
    // このフォームの音声認識エンジン
    private SpeechRecognitionEngine speechEngine = null;

    public Form2()
    {
      InitializeComponent();
    }

    private void Form2_Load(object sender, EventArgs e)
    {
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

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

      RecognizerInfo ri = GetKinectRecognizer();

      if (null != ri)
      {
        this.speechEngine = new SpeechRecognitionEngine(ri.Id);

        InitSpeechEngine(ri);

        speechEngine.SpeechRecognized += SpeechRecognized;

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
      speechEngine.SpeechRecognized += this.SpeechRecognized;

      richTextBox1.LoadFile("../../help.rtf");
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
      directions.Add(new SemanticResultValue("とじろ", "CLOSEDLG"));

      var gb = new GrammarBuilder { Culture = ri.Culture };
      gb.Append(directions);
      var g = new Grammar(gb);
      speechEngine.LoadGrammar(g);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
          case "CLOSEDLG":
            this.Close();
            break;
        }
      }
    }

    private void Form2_FormClosing(object sender, FormClosingEventArgs e)
    {
      speechEngine.SpeechRecognized -= this.SpeechRecognized;
      speechEngine.RecognizeAsyncStop();
    }

    private void richTextBox1_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
