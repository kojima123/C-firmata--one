namespace HUI
{
  partial class Form1
  {
    /// <summary>
    /// 必要なデザイナー変数です。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows フォーム デザイナーで生成されたコード

    /// <summary>
    /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
    /// コード エディターで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
        System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
        System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
        System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
        System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
        System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
        System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
        System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
        System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
        System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
        System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
        System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
        System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
        this.fileExit = new System.Windows.Forms.ToolStripMenuItem();
        this.pictureBoxSkltn = new System.Windows.Forms.PictureBox();
        this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.textBox3 = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.a2Box = new System.Windows.Forms.TextBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.a1Box = new System.Windows.Forms.TextBox();
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.grpRecv = new System.Windows.Forms.GroupBox();
        this.a0Box = new System.Windows.Forms.TextBox();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.a3Box = new System.Windows.Forms.TextBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.checkBox2 = new System.Windows.Forms.CheckBox();
        this.checkBox3 = new System.Windows.Forms.CheckBox();
        this.checkBox4 = new System.Windows.Forms.CheckBox();
        this.checkBox5 = new System.Windows.Forms.CheckBox();
        this.button3 = new System.Windows.Forms.Button();
        this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
        this.chart4 = new System.Windows.Forms.DataVisualization.Charting.Chart();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSkltn)).BeginInit();
        this.menuStrip1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.grpRecv.SuspendLayout();
        this.groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart4)).BeginInit();
        this.SuspendLayout();
        // 
        // fileExit
        // 
        this.fileExit.Name = "fileExit";
        this.fileExit.Size = new System.Drawing.Size(118, 22);
        this.fileExit.Text = "終了(&X)";
        this.fileExit.Click += new System.EventHandler(this.fileExit_Click);
        // 
        // pictureBoxSkltn
        // 
        this.pictureBoxSkltn.Location = new System.Drawing.Point(26, 105);
        this.pictureBoxSkltn.Margin = new System.Windows.Forms.Padding(2);
        this.pictureBoxSkltn.Name = "pictureBoxSkltn";
        this.pictureBoxSkltn.Size = new System.Drawing.Size(75, 40);
        this.pictureBoxSkltn.TabIndex = 4;
        this.pictureBoxSkltn.TabStop = false;
        // 
        // fileMenu
        // 
        this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileExit});
        this.fileMenu.Name = "fileMenu";
        this.fileMenu.Size = new System.Drawing.Size(85, 22);
        this.fileMenu.Text = "ファイル(&F)";
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
        this.menuStrip1.Size = new System.Drawing.Size(1695, 26);
        this.menuStrip1.TabIndex = 5;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // statusStrip1
        // 
        this.statusStrip1.Location = new System.Drawing.Point(0, 722);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
        this.statusStrip1.Size = new System.Drawing.Size(1695, 22);
        this.statusStrip1.TabIndex = 7;
        this.statusStrip1.Text = "statusStrip1";
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(1430, 118);
        this.textBox1.Multiline = true;
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(58, 309);
        this.textBox1.TabIndex = 8;
        // 
        // textBox3
        // 
        this.textBox3.Location = new System.Drawing.Point(1552, 118);
        this.textBox3.Multiline = true;
        this.textBox3.Name = "textBox3";
        this.textBox3.Size = new System.Drawing.Size(55, 309);
        this.textBox3.TabIndex = 7;
        // 
        // textBox2
        // 
        this.textBox2.Location = new System.Drawing.Point(1491, 118);
        this.textBox2.Multiline = true;
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(58, 309);
        this.textBox2.TabIndex = 17;
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.a2Box);
        this.groupBox2.Location = new System.Drawing.Point(1236, 385);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(170, 118);
        this.groupBox2.TabIndex = 25;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "a2";
        // 
        // a2Box
        // 
        this.a2Box.Location = new System.Drawing.Point(0, 18);
        this.a2Box.Multiline = true;
        this.a2Box.Name = "a2Box";
        this.a2Box.Size = new System.Drawing.Size(159, 89);
        this.a2Box.TabIndex = 2;
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.a1Box);
        this.groupBox1.Location = new System.Drawing.Point(1236, 243);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(170, 116);
        this.groupBox1.TabIndex = 21;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "a1";
        // 
        // a1Box
        // 
        this.a1Box.Location = new System.Drawing.Point(6, 18);
        this.a1Box.Multiline = true;
        this.a1Box.Name = "a1Box";
        this.a1Box.Size = new System.Drawing.Size(159, 89);
        this.a1Box.TabIndex = 2;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(1247, 34);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(112, 24);
        this.button1.TabIndex = 20;
        this.button1.Text = "終了";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(1247, 64);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(112, 24);
        this.button2.TabIndex = 19;
        this.button2.Text = "接続";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // grpRecv
        // 
        this.grpRecv.Controls.Add(this.a0Box);
        this.grpRecv.Location = new System.Drawing.Point(1236, 106);
        this.grpRecv.Name = "grpRecv";
        this.grpRecv.Size = new System.Drawing.Size(170, 115);
        this.grpRecv.TabIndex = 18;
        this.grpRecv.TabStop = false;
        this.grpRecv.Text = "a0";
        // 
        // a0Box
        // 
        this.a0Box.Location = new System.Drawing.Point(6, 20);
        this.a0Box.Multiline = true;
        this.a0Box.Name = "a0Box";
        this.a0Box.Size = new System.Drawing.Size(153, 81);
        this.a0Box.TabIndex = 7;
        // 
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.a3Box);
        this.groupBox3.Location = new System.Drawing.Point(1236, 532);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(170, 100);
        this.groupBox3.TabIndex = 28;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "a3";
        // 
        // a3Box
        // 
        this.a3Box.Location = new System.Drawing.Point(0, 18);
        this.a3Box.Multiline = true;
        this.a3Box.Name = "a3Box";
        this.a3Box.Size = new System.Drawing.Size(159, 89);
        this.a3Box.TabIndex = 2;
        // 
        // checkBox1
        // 
        this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(1236, 221);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(39, 22);
        this.checkBox1.TabIndex = 29;
        this.checkBox1.Text = "開始";
        this.checkBox1.UseVisualStyleBackColor = true;
        // 
        // checkBox2
        // 
        this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
        this.checkBox2.AutoSize = true;
        this.checkBox2.Location = new System.Drawing.Point(1236, 365);
        this.checkBox2.Name = "checkBox2";
        this.checkBox2.Size = new System.Drawing.Size(39, 22);
        this.checkBox2.TabIndex = 30;
        this.checkBox2.Text = "開始";
        this.checkBox2.UseVisualStyleBackColor = true;
        // 
        // checkBox3
        // 
        this.checkBox3.Appearance = System.Windows.Forms.Appearance.Button;
        this.checkBox3.AutoSize = true;
        this.checkBox3.Location = new System.Drawing.Point(1236, 509);
        this.checkBox3.Name = "checkBox3";
        this.checkBox3.Size = new System.Drawing.Size(39, 22);
        this.checkBox3.TabIndex = 31;
        this.checkBox3.Text = "開始";
        this.checkBox3.UseVisualStyleBackColor = true;
        // 
        // checkBox4
        // 
        this.checkBox4.Appearance = System.Windows.Forms.Appearance.Button;
        this.checkBox4.AutoSize = true;
        this.checkBox4.Location = new System.Drawing.Point(1236, 645);
        this.checkBox4.Name = "checkBox4";
        this.checkBox4.Size = new System.Drawing.Size(39, 22);
        this.checkBox4.TabIndex = 31;
        this.checkBox4.Text = "開始";
        this.checkBox4.UseVisualStyleBackColor = true;
        // 
        // checkBox5
        // 
        this.checkBox5.AutoSize = true;
        this.checkBox5.Location = new System.Drawing.Point(753, 69);
        this.checkBox5.Name = "checkBox5";
        this.checkBox5.Size = new System.Drawing.Size(78, 16);
        this.checkBox5.TabIndex = 32;
        this.checkBox5.Text = "a0-a3開始";
        this.checkBox5.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(956, 64);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 33;
        this.button3.Text = "保存";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // chart1
        // 
        chartArea1.Name = "ChartArea1";
        this.chart1.ChartAreas.Add(chartArea1);
        legend1.Name = "Legend1";
        this.chart1.Legends.Add(legend1);
        this.chart1.Location = new System.Drawing.Point(681, 100);
        this.chart1.Name = "chart1";
        series1.ChartArea = "ChartArea1";
        series1.Legend = "Legend1";
        series1.Name = "Series1";
        this.chart1.Series.Add(series1);
        this.chart1.Size = new System.Drawing.Size(520, 143);
        this.chart1.TabIndex = 34;
        this.chart1.Text = "chart1";
        // 
        // chart2
        // 
        chartArea2.Name = "ChartArea1";
        this.chart2.ChartAreas.Add(chartArea2);
        legend2.Name = "Legend1";
        this.chart2.Legends.Add(legend2);
        this.chart2.Location = new System.Drawing.Point(681, 249);
        this.chart2.Name = "chart2";
        series2.ChartArea = "ChartArea1";
        series2.Legend = "Legend1";
        series2.Name = "Series1";
        this.chart2.Series.Add(series2);
        this.chart2.Size = new System.Drawing.Size(520, 143);
        this.chart2.TabIndex = 35;
        this.chart2.Text = "chart2";
        // 
        // chart3
        // 
        chartArea3.Name = "ChartArea1";
        this.chart3.ChartAreas.Add(chartArea3);
        legend3.Name = "Legend1";
        this.chart3.Legends.Add(legend3);
        this.chart3.Location = new System.Drawing.Point(681, 398);
        this.chart3.Name = "chart3";
        series3.ChartArea = "ChartArea1";
        series3.Legend = "Legend1";
        series3.Name = "Series1";
        this.chart3.Series.Add(series3);
        this.chart3.Size = new System.Drawing.Size(520, 143);
        this.chart3.TabIndex = 36;
        this.chart3.Text = "chart3";
        // 
        // chart4
        // 
        chartArea4.Name = "ChartArea1";
        this.chart4.ChartAreas.Add(chartArea4);
        legend4.Name = "Legend1";
        this.chart4.Legends.Add(legend4);
        this.chart4.Location = new System.Drawing.Point(681, 547);
        this.chart4.Name = "chart4";
        series4.ChartArea = "ChartArea1";
        series4.Legend = "Legend1";
        series4.Name = "Series1";
        this.chart4.Series.Add(series4);
        this.chart4.Size = new System.Drawing.Size(520, 143);
        this.chart4.TabIndex = 37;
        this.chart4.Text = "chart4";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1695, 744);
        this.Controls.Add(this.chart4);
        this.Controls.Add(this.chart3);
        this.Controls.Add(this.chart2);
        this.Controls.Add(this.chart1);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.checkBox5);
        this.Controls.Add(this.checkBox4);
        this.Controls.Add(this.checkBox3);
        this.Controls.Add(this.checkBox2);
        this.Controls.Add(this.checkBox1);
        this.Controls.Add(this.groupBox3);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.grpRecv);
        this.Controls.Add(this.textBox2);
        this.Controls.Add(this.textBox3);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.statusStrip1);
        this.Controls.Add(this.pictureBoxSkltn);
        this.Controls.Add(this.menuStrip1);
        this.KeyPreview = true;
        this.Margin = new System.Windows.Forms.Padding(2);
        this.Name = "Form1";
        this.Text = "HUI";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.Load += new System.EventHandler(this.Form1_Load);
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSkltn)).EndInit();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.grpRecv.ResumeLayout(false);
        this.grpRecv.PerformLayout();
        this.groupBox3.ResumeLayout(false);
        this.groupBox3.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.chart4)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStripMenuItem fileExit;
    private System.Windows.Forms.PictureBox pictureBoxSkltn;
    private System.Windows.Forms.ToolStripMenuItem fileMenu;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TextBox a2Box;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox a1Box;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.GroupBox grpRecv;
    private System.Windows.Forms.TextBox a0Box;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox a3Box;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.CheckBox checkBox2;
    private System.Windows.Forms.CheckBox checkBox3;
    private System.Windows.Forms.CheckBox checkBox4;
    private System.Windows.Forms.CheckBox checkBox5;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart4;
  }
}

