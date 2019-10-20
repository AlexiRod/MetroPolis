namespace Metropolis
{
    partial class SetupForm
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
            this.ImpLines = new System.Windows.Forms.Button();
            this.ImpStations = new System.Windows.Forms.Button();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.ExpStations = new System.Windows.Forms.Button();
            this.EditModeCBox = new System.Windows.Forms.CheckBox();
            this.ImpStationsExpBt = new System.Windows.Forms.Button();
            this.St2LangRes = new System.Windows.Forms.Button();
            this.LangResImp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ImpLines
            // 
            this.ImpLines.Location = new System.Drawing.Point(12, 12);
            this.ImpLines.Name = "ImpLines";
            this.ImpLines.Size = new System.Drawing.Size(99, 23);
            this.ImpLines.TabIndex = 0;
            this.ImpLines.Text = "Импорт линий";
            this.ImpLines.UseVisualStyleBackColor = true;
            this.ImpLines.Click += new System.EventHandler(this.ImpLines_Click);
            // 
            // ImpStations
            // 
            this.ImpStations.Location = new System.Drawing.Point(12, 42);
            this.ImpStations.Name = "ImpStations";
            this.ImpStations.Size = new System.Drawing.Size(98, 23);
            this.ImpStations.TabIndex = 1;
            this.ImpStations.Text = "Импорт станций";
            this.ImpStations.UseVisualStyleBackColor = true;
            this.ImpStations.Click += new System.EventHandler(this.ImpStations_click);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.Location = new System.Drawing.Point(75, 406);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(0, 13);
            this.ProgressLabel.TabIndex = 2;
            // 
            // ExpStations
            // 
            this.ExpStations.Location = new System.Drawing.Point(12, 71);
            this.ExpStations.Name = "ExpStations";
            this.ExpStations.Size = new System.Drawing.Size(98, 25);
            this.ExpStations.TabIndex = 3;
            this.ExpStations.Text = "Эксп.станций";
            this.ExpStations.UseVisualStyleBackColor = true;
            this.ExpStations.Click += new System.EventHandler(this.ExpStations_Click_1);
            // 
            // EditModeCBox
            // 
            this.EditModeCBox.AutoSize = true;
            this.EditModeCBox.Location = new System.Drawing.Point(148, 18);
            this.EditModeCBox.Name = "EditModeCBox";
            this.EditModeCBox.Size = new System.Drawing.Size(108, 17);
            this.EditModeCBox.TabIndex = 4;
            this.EditModeCBox.Text = "Редактор карты";
            this.EditModeCBox.UseVisualStyleBackColor = true;
            this.EditModeCBox.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // ImpStationsExpBt
            // 
            this.ImpStationsExpBt.Location = new System.Drawing.Point(12, 102);
            this.ImpStationsExpBt.Name = "ImpStationsExpBt";
            this.ImpStationsExpBt.Size = new System.Drawing.Size(98, 35);
            this.ImpStationsExpBt.TabIndex = 5;
            this.ImpStationsExpBt.Text = "Расш.имп станций";
            this.ImpStationsExpBt.UseVisualStyleBackColor = true;
            this.ImpStationsExpBt.Click += new System.EventHandler(this.ImpStationsExpBt_Click);
            // 
            // St2LangRes
            // 
            this.St2LangRes.Location = new System.Drawing.Point(207, 341);
            this.St2LangRes.Name = "St2LangRes";
            this.St2LangRes.Size = new System.Drawing.Size(98, 38);
            this.St2LangRes.TabIndex = 6;
            this.St2LangRes.Text = "Cтанции в яз ресурсы";
            this.St2LangRes.UseVisualStyleBackColor = true;
            this.St2LangRes.Click += new System.EventHandler(this.St2LangRes_Click);
            // 
            // LangResImp
            // 
            this.LangResImp.Location = new System.Drawing.Point(207, 297);
            this.LangResImp.Name = "LangResImp";
            this.LangResImp.Size = new System.Drawing.Size(98, 38);
            this.LangResImp.TabIndex = 7;
            this.LangResImp.Text = "Расш.имп яз ресурсов";
            this.LangResImp.UseVisualStyleBackColor = true;
            this.LangResImp.Click += new System.EventHandler(this.LangResImp_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LangResImp);
            this.Controls.Add(this.St2LangRes);
            this.Controls.Add(this.ImpStationsExpBt);
            this.Controls.Add(this.EditModeCBox);
            this.Controls.Add(this.ExpStations);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.ImpStations);
            this.Controls.Add(this.ImpLines);
            this.Name = "SetupForm";
            this.Text = "Н А С Т Р О  Й К А";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ImpLines;
        private System.Windows.Forms.Button ImpStations;
        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.Button ExpStations;
        private System.Windows.Forms.CheckBox EditModeCBox;
        private System.Windows.Forms.Button ImpStationsExpBt;
        private System.Windows.Forms.Button St2LangRes;
        private System.Windows.Forms.Button LangResImp;
    }
}