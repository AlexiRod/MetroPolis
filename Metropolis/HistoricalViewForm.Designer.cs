namespace Metropolis
{
    partial class HistoricalViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoricalViewForm));
            this.cbYears = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnToRealSize = new System.Windows.Forms.Button();
            this.timerBack = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cbYears
            // 
            this.cbYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYears.FormattingEnabled = true;
            this.cbYears.Items.AddRange(new object[] {
            "1900-1929",
            "1930-1935",
            "1936",
            "1937-1939",
            "1940-1945",
            "1946-1953",
            "1954-1955",
            "1956",
            "1957",
            "1958-1961",
            "1962-1963",
            "1964-1969",
            "1970-1977",
            "1978",
            "1979",
            "1984-1988",
            "1989",
            "1990-1995",
            "1996-1999",
            "2000-2009",
            "2010-2013",
            "2014-2017",
            "2018-2019"});
            this.cbYears.Location = new System.Drawing.Point(233, 20);
            this.cbYears.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbYears.Name = "cbYears";
            this.cbYears.Size = new System.Drawing.Size(256, 38);
            this.cbYears.TabIndex = 0;
            this.cbYears.SelectedIndexChanged += new System.EventHandler(this.cbYears_SelectedIndexChanged);
            // 
            // lblYear
            // 
            this.lblYear.Location = new System.Drawing.Point(12, 20);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(213, 30);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "Выберите год:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 69);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(404, 666);
            this.textBox.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(422, 69);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(753, 555);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Location = new System.Drawing.Point(432, 77);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(60, 60);
            this.btnPrev.TabIndex = 4;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            this.btnPrev.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnPrev.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(498, 77);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(60, 60);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            this.btnNext.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnNext.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // btnToRealSize
            // 
            this.btnToRealSize.BackColor = System.Drawing.Color.Gainsboro;
            this.btnToRealSize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnToRealSize.Location = new System.Drawing.Point(509, 19);
            this.btnToRealSize.Name = "btnToRealSize";
            this.btnToRealSize.Size = new System.Drawing.Size(312, 38);
            this.btnToRealSize.TabIndex = 6;
            this.btnToRealSize.Text = "Сброс масштаба";
            this.btnToRealSize.UseVisualStyleBackColor = false;
            this.btnToRealSize.Click += new System.EventHandler(this.btnToRealSize_Click);
            this.btnToRealSize.MouseEnter += new System.EventHandler(this.btn_MouseEnter);
            this.btnToRealSize.MouseLeave += new System.EventHandler(this.btn_MouseLeave);
            // 
            // timerBack
            // 
            this.timerBack.Interval = 1000;
            this.timerBack.Tick += new System.EventHandler(this.timerBack_Tick);
            // 
            // HistoricalViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1624, 955);
            this.Controls.Add(this.btnToRealSize);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.cbYears);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "HistoricalViewForm";
            this.Text = "HistoricalViewForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.HistoricalViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbYears;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnToRealSize;
        private System.Windows.Forms.Timer timerBack;
    }
}