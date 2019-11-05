namespace Metropolis
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.cbShowInfrastructure = new System.Windows.Forms.CheckBox();
            this.lblShowInfrastructure = new System.Windows.Forms.Label();
            this.lblSet = new System.Windows.Forms.Label();
            this.pSet = new System.Windows.Forms.Panel();
            this.lblMyColor = new System.Windows.Forms.Label();
            this.lblDefColor = new System.Windows.Forms.Label();
            this.cbMyColor = new System.Windows.Forms.ComboBox();
            this.cbDefColor = new System.Windows.Forms.ComboBox();
            this.cbTime = new System.Windows.Forms.ComboBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.pSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbShowInfrastructure
            // 
            this.cbShowInfrastructure.AutoSize = true;
            this.cbShowInfrastructure.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowInfrastructure.Location = new System.Drawing.Point(126, 152);
            this.cbShowInfrastructure.Name = "cbShowInfrastructure";
            this.cbShowInfrastructure.Size = new System.Drawing.Size(22, 21);
            this.cbShowInfrastructure.TabIndex = 1;
            this.cbShowInfrastructure.UseVisualStyleBackColor = true;
            this.cbShowInfrastructure.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // lblShowInfrastructure
            // 
            this.lblShowInfrastructure.AutoSize = true;
            this.lblShowInfrastructure.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold);
            this.lblShowInfrastructure.Location = new System.Drawing.Point(154, 122);
            this.lblShowInfrastructure.Name = "lblShowInfrastructure";
            this.lblShowInfrastructure.Size = new System.Drawing.Size(821, 84);
            this.lblShowInfrastructure.TabIndex = 2;
            this.lblShowInfrastructure.Text = "Показывать информацию о инфраструктуре \r\nи маршрутах в исторической справке";
            this.lblShowInfrastructure.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblSet
            // 
            this.lblSet.AutoSize = true;
            this.lblSet.Font = new System.Drawing.Font("Lucida Fax", 16F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet.Location = new System.Drawing.Point(291, 29);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(463, 38);
            this.lblSet.TabIndex = 3;
            this.lblSet.Text = "Настройки и персонализация";
            // 
            // pSet
            // 
            this.pSet.Controls.Add(this.lblSet);
            this.pSet.Dock = System.Windows.Forms.DockStyle.Top;
            this.pSet.Location = new System.Drawing.Point(0, 0);
            this.pSet.Name = "pSet";
            this.pSet.Size = new System.Drawing.Size(1059, 88);
            this.pSet.TabIndex = 4;
            // 
            // lblMyColor
            // 
            this.lblMyColor.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold);
            this.lblMyColor.Location = new System.Drawing.Point(114, 237);
            this.lblMyColor.Name = "lblMyColor";
            this.lblMyColor.Size = new System.Drawing.Size(640, 42);
            this.lblMyColor.TabIndex = 6;
            this.lblMyColor.Text = "Основной цвет выделения:";
            this.lblMyColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDefColor
            // 
            this.lblDefColor.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold);
            this.lblDefColor.Location = new System.Drawing.Point(107, 297);
            this.lblDefColor.Name = "lblDefColor";
            this.lblDefColor.Size = new System.Drawing.Size(647, 42);
            this.lblDefColor.TabIndex = 7;
            this.lblDefColor.Text = "Дополнительный цвет выделения:";
            this.lblDefColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMyColor
            // 
            this.cbMyColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMyColor.Font = new System.Drawing.Font("High Tower Text", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMyColor.FormattingEnabled = true;
            this.cbMyColor.Items.AddRange(new object[] {
            "Blue",
            "Red",
            "Green",
            "Purple",
            "Pink",
            "AliceBlue",
            "Orange",
            "Lime",
            "WhiteSmoke"});
            this.cbMyColor.Location = new System.Drawing.Point(760, 240);
            this.cbMyColor.Name = "cbMyColor";
            this.cbMyColor.Size = new System.Drawing.Size(235, 34);
            this.cbMyColor.TabIndex = 5;
            this.cbMyColor.SelectedIndexChanged += new System.EventHandler(this.cbMyColor_SelectedIndexChanged);
            // 
            // cbDefColor
            // 
            this.cbDefColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefColor.Font = new System.Drawing.Font("High Tower Text", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDefColor.FormattingEnabled = true;
            this.cbDefColor.Items.AddRange(new object[] {
            "Blue",
            "Red",
            "Green",
            "Pink",
            "White",
            "Gainsboro",
            "AliceBlue",
            "Orange",
            "WhiteSmoke"});
            this.cbDefColor.Location = new System.Drawing.Point(760, 300);
            this.cbDefColor.Name = "cbDefColor";
            this.cbDefColor.Size = new System.Drawing.Size(235, 34);
            this.cbDefColor.TabIndex = 8;
            this.cbDefColor.SelectedIndexChanged += new System.EventHandler(this.cbDefColor_SelectedIndexChanged);
            // 
            // cbTime
            // 
            this.cbTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTime.Font = new System.Drawing.Font("Lucida Sans", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTime.FormattingEnabled = true;
            this.cbTime.Items.AddRange(new object[] {
            "Не менять фото на фоне",
            "15 секунд",
            "30 секунд",
            "60 секунд",
            "90 секунд",
            "120 секунд",
            "180 секунд"});
            this.cbTime.Location = new System.Drawing.Point(760, 360);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(235, 33);
            this.cbTime.TabIndex = 9;
            this.cbTime.SelectedIndexChanged += new System.EventHandler(this.cbTime_SelectedIndexChanged);
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold);
            this.lblTime.Location = new System.Drawing.Point(115, 354);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(640, 42);
            this.lblTime.TabIndex = 10;
            this.lblTime.Text = "Время на смену фото на фоне:";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 535);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.cbTime);
            this.Controls.Add(this.cbDefColor);
            this.Controls.Add(this.lblDefColor);
            this.Controls.Add(this.lblMyColor);
            this.Controls.Add(this.cbMyColor);
            this.Controls.Add(this.pSet);
            this.Controls.Add(this.lblShowInfrastructure);
            this.Controls.Add(this.cbShowInfrastructure);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.pSet.ResumeLayout(false);
            this.pSet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbShowInfrastructure;
        private System.Windows.Forms.Label lblShowInfrastructure;
        private System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.Panel pSet;
        private System.Windows.Forms.Label lblMyColor;
        private System.Windows.Forms.Label lblDefColor;
        private System.Windows.Forms.ComboBox cbMyColor;
        private System.Windows.Forms.ComboBox cbDefColor;
        private System.Windows.Forms.ComboBox cbTime;
        private System.Windows.Forms.Label lblTime;
    }
}