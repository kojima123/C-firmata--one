/****************************************************************************
 *	Copylight(C) 2011 Kanazawa-soft-design,LLC.All Rights Reserved.
 ****************************************************************************/
/*!
 *	@file	Form1.cs
 *
 *	@brief	�V���A���|�[�g�ʐM�v���O����.
 *
 *	@author	���V �閾
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
		 *	@brief	�{�[���[�g�i�[�p�̃N���X��`.
		 */
		private class BuadRateItem : Object
		{
			private string	m_name  = "";
			private int		m_value = 0;

			// �\������
			public string NAME
			{
				set { m_name = value; }
				get { return m_name;  }
			}

			// �{�[���[�g�ݒ�l.
			public int BAUDRATE
			{
				set { m_value = value; }
				get { return m_value;  }
			}

			// �R���{�{�b�N�X�\���p�̕�����擾�֐�.
			public override string  ToString()
			{
				return m_name;
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	����v���g�R���i�[�p�̃N���X��`.
		 */
		private class HandShakeItem : Object
		{
			private string		m_name  = "";
			private Handshake	m_value = Handshake.None;

			// �\������
			public string NAME
			{
				set { m_name = value; }
				get { return m_name;  }
			}

			// ����v���g�R���ݒ�l.
			public Handshake HANDSHAKE
			{
				set { m_value = value; }
				get { return m_value;  }
			}

			// �R���{�{�b�N�X�\���p�̕�����擾�֐�.
			public override string  ToString()
			{
				return m_name;
			}
		}

		private delegate void Delegate_RcvDataToTextBox( string data );

		/****************************************************************************/
		/*!
		 *	@brief	�R���X�g���N�^.
		 *
		 *	@param	�Ȃ�.
		 *
		 *	@retval	�Ȃ�.
		 */
		public Form1()
		{
			InitializeComponent();
		}

		/****************************************************************************/
		/*!
		 *	@brief	�_�C�A���O�̏�������.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void Form1_Load(object sender, EventArgs e)
		{
			//! ���p�\�ȃV���A���|�[�g���̔z����擾����.
			string[] PortList = SerialPort.GetPortNames();

			cmbPortName.Items.Clear();

			//! �V���A���|�[�g�����R���{�{�b�N�X�ɃZ�b�g����.
			foreach( string PortName in PortList ){
				cmbPortName.Items.Add( PortName );
			}
			if( cmbPortName.Items.Count > 0 ){
				cmbPortName.SelectedIndex = 0;
			}

			cmbBaudRate.Items.Clear();

			// �{�[���[�g�I���R���{�{�b�N�X�ɑI�����ڂ��Z�b�g����.
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

			// �t���[����I���R���{�{�b�N�X�ɑI�����ڂ��Z�b�g����.
			HandShakeItem ctrl;
			ctrl           = new HandShakeItem();
			ctrl.NAME      = "�Ȃ�";
			ctrl.HANDSHAKE = Handshake.None;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "XON/XOFF����";
			ctrl.HANDSHAKE = Handshake.XOnXOff;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "RTS/CTS����";
			ctrl.HANDSHAKE = Handshake.RequestToSend;
			cmbHandShake.Items.Add( ctrl );

			ctrl           = new HandShakeItem();
			ctrl.NAME      = "XON/XOFF + RTS/CTS����";
			ctrl.HANDSHAKE = Handshake.RequestToSendXOnXOff;
			cmbHandShake.Items.Add( ctrl );
			cmbHandShake.SelectedIndex = 0;

			// ����M�p�̃e�L�X�g�{�b�N�X���N���A����.
			sndTextBox.Clear();
			rcvTextBox.Clear();
		}

		/****************************************************************************/
		/*!
		 *	@brief	�_�C�A���O�̏I������.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			//! �V���A���|�[�g���I�[�v�����Ă���ꍇ�A�N���[�Y����.
			if( serialPort1.IsOpen ){
				serialPort1.Close();
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	[�I��]�{�^�����������Ƃ��̏���.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void exitButton_Click(object sender, EventArgs e)
		{
			//! �_�C�A���O���N���[�Y����.
			Close();
		}

		/****************************************************************************/
		/*!
		 *	@brief	[�ڑ�]/[�ؒf]�{�^�����������Ƃ��ɃV���A���|�[�g�̃I�[�v��/�N���[�Y���s��.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void connectButton_Click(object sender, EventArgs e)
		{

			if( serialPort1.IsOpen == true ){
				//! �V���A���|�[�g���N���[�Y����.
				serialPort1.Close();

				//! �{�^���̕\����[�ؒf]����[�ڑ�]�ɕς���.
				connectButton.Text = "�ڑ�";
			}else{
				//! �I�[�v������V���A���|�[�g���R���{�{�b�N�X������o��.
				serialPort1.PortName = cmbPortName.SelectedItem.ToString();

				//! �{�[���[�g���R���{�{�b�N�X������o��.
				BuadRateItem baud    = (BuadRateItem)cmbBaudRate.SelectedItem;
				serialPort1.BaudRate = baud.BAUDRATE;

				//! �f�[�^�r�b�g���Z�b�g����. (�f�[�^�r�b�g = 8�r�b�g)
				serialPort1.DataBits = 8;

				//! �p���e�B�r�b�g���Z�b�g����. (�p���e�B�r�b�g = �Ȃ�)
				serialPort1.Parity = Parity.None;

				//! �X�g�b�v�r�b�g���Z�b�g����. (�X�g�b�v�r�b�g = 1�r�b�g)
				serialPort1.StopBits = StopBits.One;

				//! �t���[������R���{�{�b�N�X������o��.
				HandShakeItem ctrl    = (HandShakeItem)cmbHandShake.SelectedItem;
				serialPort1.Handshake = ctrl.HANDSHAKE;

				//! �����R�[�h���Z�b�g����.
				serialPort1.Encoding = Encoding.Unicode;

				try {
					//! �V���A���|�[�g���I�[�v������.
					serialPort1.Open();

					//! �{�^���̕\����[�ڑ�]����[�ؒf]�ɕς���.
					connectButton.Text = "�ؒf";
				}
				catch ( Exception ex ){
					MessageBox.Show(ex.Message);
				}
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	[���M]�{�^���������āA�f�[�^���M���s��.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void sndButton_Click(object sender, EventArgs e)
		{
			//! �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
			if( serialPort1.IsOpen == false ){
				return;
			}
			//! �e�L�X�g�{�b�N�X����A���M����e�L�X�g�����o��.
			String data = sndTextBox.Text;

			//! ���M����e�L�X�g���Ȃ��ꍇ�A�f�[�^���M�͍s��Ȃ�.
			if( string.IsNullOrEmpty( data ) == true ){
				return;
			}

			try {
				//! �V���A���|�[�g����e�L�X�g�𑗐M����.
				serialPort1.Write( data );

				//! ���M�f�[�^����͂���e�L�X�g�{�b�N�X���N���A����.
				sndTextBox.Clear();
			}
			catch ( Exception ex ){
				MessageBox.Show( ex.Message );
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	�f�[�^��M�����������Ƃ��̃C�x���g����.
		 *
		 *	@param	[in]	sender	�C�x���g�̑��M���̃I�u�W�F�N�g.
		 *	@param	[in]	e		�C�x���g���.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			//! �V���A���|�[�g���I�[�v�����Ă��Ȃ��ꍇ�A�������s��Ȃ�.
			if( serialPort1.IsOpen == false ){
				return;
			}

			try {
				//! ��M�f�[�^��ǂݍ���.
				string data = serialPort1.ReadExisting();

				//! ��M�����f�[�^���e�L�X�g�{�b�N�X�ɏ�������.
				Invoke( new Delegate_RcvDataToTextBox( RcvDataToTextBox ), new Object[] {data} );
			}
			catch ( Exception ex ){
				MessageBox.Show( ex.Message );
			}
		}

		/****************************************************************************/
		/*!
		 *	@brief	��M�f�[�^���e�L�X�g�{�b�N�X�ɏ�������.
		 *
		 *	@param	[in]	data	��M����������.
		 *
		 *	@retval	�Ȃ�.
		 */
		private void RcvDataToTextBox( string data )
		{
			//! ��M�f�[�^���e�L�X�g�{�b�N�X�̍Ō�ɒǋL����.
			if( data != null ){
				rcvTextBox.AppendText( data );
			}
		}
	}
}

/* End of file */
