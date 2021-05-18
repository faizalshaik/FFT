
namespace SharpSample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._btn_start = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this._btnRead = new System.Windows.Forms.Button();
            this._txt_fileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _btn_start
            // 
            this._btn_start.Location = new System.Drawing.Point(180, 39);
            this._btn_start.Name = "_btn_start";
            this._btn_start.Size = new System.Drawing.Size(77, 39);
            this._btn_start.TabIndex = 0;
            this._btn_start.Text = "button1";
            this._btn_start.UseVisualStyleBackColor = true;
            this._btn_start.Click += new System.EventHandler(this._btn_start_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(574, 21);
            this.progressBar1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // _btnRead
            // 
            this._btnRead.Location = new System.Drawing.Point(397, 171);
            this._btnRead.Name = "_btnRead";
            this._btnRead.Size = new System.Drawing.Size(96, 40);
            this._btnRead.TabIndex = 2;
            this._btnRead.Text = "Read and Chart";
            this._btnRead.UseVisualStyleBackColor = true;
            this._btnRead.Click += new System.EventHandler(this._btnRead_Click);
            // 
            // _txt_fileName
            // 
            this._txt_fileName.Location = new System.Drawing.Point(136, 144);
            this._txt_fileName.Name = "_txt_fileName";
            this._txt_fileName.Size = new System.Drawing.Size(316, 20);
            this._txt_fileName.TabIndex = 3;
            this._txt_fileName.Text = "d:\\outputFile.bin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Name";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(455, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 21);
            this.button1.TabIndex = 5;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 508);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._txt_fileName);
            this.Controls.Add(this._btnRead);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this._btn_start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button _btn_start;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button _btnRead;
        private System.Windows.Forms.TextBox _txt_fileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

