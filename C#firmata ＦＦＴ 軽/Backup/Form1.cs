/****************************************************************************
 *	Copylight(C) 2011 Kanazawa-soft-design,LLC.All Rights Reserved.
 ****************************************************************************/
/*!
 *	@file	Form1.cs
 *
 *	@brief	シリアルポート通信プログラム.
 *
 *	@author	金澤 宣明
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace sample_0007
{
	public partial class Form1 : Form
	{
		/****************************************************************************/
		/*!
		 *	@brief	ボーレート格納用のクラス定義.
		 */
		private class BuadRateItem : Object
		{
			private string	m_name  = "";
			private int		m_value = 0;

			// 表示名称
			public string NAME
			{
				set { m_name = value; }
				get { return m_name;  }
			}

			// ボーレート設定値.
			public int BAUDRATE
			{
				set { m_value = value; }
				get { return m_value;  }
			}

			// コンボボックス表示用の文字列取得関数.
			public override string  ToString()
			{
				return m_name;
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	制御プロトコル格納用のクラス定義.
		 */
		private class HandShakeItem : Object
		{
			private string		m_name  = "";
			private Handshake	m_value = Handshake.None;

			// 表示名称
			public string NAME
			{
				set { m_name = value; }
				get { return m_name;  }
			}

			// 制御プロトコル設定値.
			public Handshake HANDSHAKE
			{
				set { m_value = value; }
				get { return m_value;  }
			}

			// コンボボックス表示用の文字列取得関数.
			public override string  ToString()
			{
				return m_name;
			}
		}

		private delegate void Delegate_RcvDataToTextBox( string data );

		/****************************************************************************/
		/*!
		 *	@brief	コンストラクタ.
		 *
		 *	@param	なし.
		 *
		 *	@retval	なし.
		 */
		public Form1()
		{
			InitializeComponent();
		}

		/****************************************************************************/
		/*!
		 *	@brief	ダイアログの初期処理.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void Form1_Load(object sender, EventArgs e)
		{
			//! 利用可能なシリアルポート名の配列を取得する.
			string[] PortList = SerialPort.GetPortNames();

			cmbPortName.Items.Clear();

			//! シリアルポート名をコンボボックスにセットする.
			foreach( string PortName in PortList ){
				cmbPortName.Items.Add( PortName );
			}
			if( cmbPortName.Items.Count > 0 ){
				cmbPortName.SelectedIndex = 0;
			}

			cmbBaudRate.Items.Clear();

			// ボーレート選択コンボボックスに選択項目をセットする.
			BuadRateItem baud;
			baud          = new BuadRateItem();
			baud.NAME     = "4800bps";
			baud.BAUDRATE = 4800;
			cmbBaudRate.Items.Add( baud );

			baud          = new BuadRateItem();
			baud.NAME     = "9600bps";
			baud.BAUDRATE = 9600;
			cmbBaudRate.Items.Add( baud );

			baud          = new BuadRateItem();
			baud.NAME     = "19200bps";
			baud.BAUDRATE = 19200;
			cmbBaudRate.Items.Add( baud );

			baud          = new BuadRateItem();
			baud.NAME     = "115200bps";
			baud.BAUDRATE = 115200;
			cmbBaudRate.Items.Add( baud );
			cmbBaudRate.SelectedIndex = 1;

			cmbHandShake.Items.Clear();

			// フロー制御選択コンボボックスに選択項目をセットする.
			HandShakeItem ctrl;
			ctrl           = new HandShakeItem();
			ctrl.NAME      = "なし";
			ctrl.HANDSHAKE = Handshake.None;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "XON/XOFF制御";
			ctrl.HANDSHAKE = Handshake.XOnXOff;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "RTS/CTS制御";
			ctrl.HANDSHAKE = Handshake.RequestToSend;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "XON/XOFF + RTS/CTS制御";
			ctrl.HANDSHAKE = Handshake.RequestToSendXOnXOff;
			cmbHandShake.Items.Add( ctrl );
			cmbHandShake.SelectedIndex = 0;

			// 送受信用のテキストボックスをクリアする.
			sndTextBox.Clear();
			rcvTextBox.Clear();
		}

		/****************************************************************************/
		/*!
		 *	@brief	ダイアログの終了処理.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//! シリアルポートをオープンしている場合、クローズする.
			if( serialPort1.IsOpen ){
				serialPort1.Close();
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	[終了]ボタンを押したときの処理.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void exitButton_Click(object sender, EventArgs e)
		{
			//! ダイアログをクローズする.
			Close();
		}

		/****************************************************************************/
		/*!
		 *	@brief	[接続]/[切断]ボタンを押したときにシリアルポートのオープン/クローズを行う.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void connectButton_Click(object sender, EventArgs e)
		{

			if( serialPort1.IsOpen == true ){
				//! シリアルポートをクローズする.
				serialPort1.Close();

				//! ボタンの表示を[切断]から[接続]に変える.
				connectButton.Text = "接続";
			}else{
				//! オープンするシリアルポートをコンボボックスから取り出す.
				serialPort1.PortName = cmbPortName.SelectedItem.ToString();

				//! ボーレートをコンボボックスから取り出す.
				BuadRateItem baud    = (BuadRateItem)cmbBaudRate.SelectedItem;
				serialPort1.BaudRate = baud.BAUDRATE;

				//! データビットをセットする. (データビット = 8ビット)
				serialPort1.DataBits = 8;

				//! パリティビットをセットする. (パリティビット = なし)
				serialPort1.Parity = Parity.None;

				//! ストップビットをセットする. (ストップビット = 1ビット)
				serialPort1.StopBits = StopBits.One;

				//! フロー制御をコンボボックスから取り出す.
				HandShakeItem ctrl    = (HandShakeItem)cmbHandShake.SelectedItem;
				serialPort1.Handshake = ctrl.HANDSHAKE;

				//! 文字コードをセットする.
				serialPort1.Encoding = Encoding.Unicode;

				try {
					//! シリアルポートをオープンする.
					serialPort1.Open();

					//! ボタンの表示を[接続]から[切断]に変える.
					connectButton.Text = "切断";
				}
				catch ( Exception ex ){
					MessageBox.Show(ex.Message);
				}
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	[送信]ボタンを押して、データ送信を行う.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void sndButton_Click(object sender, EventArgs e)
		{
			//! シリアルポートをオープンしていない場合、処理を行わない.
			if( serialPort1.IsOpen == false ){
				return;
			}
			//! テキストボックスから、送信するテキストを取り出す.
			String data = sndTextBox.Text;

			//! 送信するテキストがない場合、データ送信は行わない.
			if( string.IsNullOrEmpty( data ) == true ){
				return;
			}

			try {
				//! シリアルポートからテキストを送信する.
				serialPort1.Write( data );

				//! 送信データを入力するテキストボックスをクリアする.
				sndTextBox.Clear();
			}
			catch ( Exception ex ){
				MessageBox.Show( ex.Message );
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	データ受信が発生したときのイベント処理.
		 *
		 *	@param	[in]	sender	イベントの送信元のオブジェクト.
		 *	@param	[in]	e		イベント情報.
		 *
		 *	@retval	なし.
		 */
		private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			//! シリアルポートをオープンしていない場合、処理を行わない.
			if( serialPort1.IsOpen == false ){
				return;
			}

			try {
				//! 受信データを読み込む.
				string data = serialPort1.ReadExisting();

				//! 受信したデータをテキストボックスに書き込む.
				Invoke( new Delegate_RcvDataToTextBox( RcvDataToTextBox ), new Object[] {data} );
			}
			catch ( Exception ex ){
				MessageBox.Show( ex.Message );
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	受信データをテキストボックスに書き込む.
		 *
		 *	@param	[in]	data	受信した文字列.
		 *
		 *	@retval	なし.
		 */
		private void RcvDataToTextBox( string data )
		{
			//! 受信データをテキストボックスの最後に追記する.
			if( data != null ){
				rcvTextBox.AppendText( data );
			}
		}
	}
}

/* End of file */
