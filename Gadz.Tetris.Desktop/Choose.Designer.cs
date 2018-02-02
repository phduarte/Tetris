namespace Gadz.Tetris.Desktop {
    partial class Choose {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnStart = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblChoose = new System.Windows.Forms.Label();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            this.cmbEstilo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbSoundKey = new System.Windows.Forms.Label();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.Lime;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(115, 190);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(126, 39);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "COMEÇAR";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(315, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(29, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblChoose
            // 
            this.lblChoose.BackColor = System.Drawing.Color.Transparent;
            this.lblChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChoose.Location = new System.Drawing.Point(114, 118);
            this.lblChoose.Name = "lblChoose";
            this.lblChoose.Size = new System.Drawing.Size(129, 23);
            this.lblChoose.TabIndex = 4;
            this.lblChoose.Text = "Escolha o estilo:";
            this.lblChoose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lnkHelp
            // 
            this.lnkHelp.AutoSize = true;
            this.lnkHelp.BackColor = System.Drawing.Color.Transparent;
            this.lnkHelp.Location = new System.Drawing.Point(255, 448);
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.Size = new System.Drawing.Size(34, 13);
            this.lnkHelp.TabIndex = 5;
            this.lnkHelp.TabStop = true;
            this.lnkHelp.Text = "Ajuda";
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lnkAbout
            // 
            this.lnkAbout.AutoSize = true;
            this.lnkAbout.BackColor = System.Drawing.Color.Transparent;
            this.lnkAbout.Location = new System.Drawing.Point(304, 448);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(35, 13);
            this.lnkAbout.TabIndex = 6;
            this.lnkAbout.TabStop = true;
            this.lnkAbout.Text = "Sobre";
            this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // cmbEstilo
            // 
            this.cmbEstilo.FormattingEnabled = true;
            this.cmbEstilo.Items.AddRange(new object[] {
            "BRICK GAME",
            "TETRIS"});
            this.cmbEstilo.Location = new System.Drawing.Point(96, 153);
            this.cmbEstilo.Name = "cmbEstilo";
            this.cmbEstilo.Size = new System.Drawing.Size(164, 21);
            this.cmbEstilo.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Malgun Gothic Semilight", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 45);
            this.label1.TabIndex = 8;
            this.label1.Text = "TETRIS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSoundKey
            // 
            this.lbSoundKey.AutoSize = true;
            this.lbSoundKey.BackColor = System.Drawing.Color.Transparent;
            this.lbSoundKey.ForeColor = System.Drawing.Color.Gray;
            this.lbSoundKey.Location = new System.Drawing.Point(74, 370);
            this.lbSoundKey.Name = "lbSoundKey";
            this.lbSoundKey.Size = new System.Drawing.Size(208, 13);
            this.lbSoundKey.TabIndex = 9;
            this.lbSoundKey.Text = "Aperte SHIFT para ligar ou desligar o SOM";
            this.lbSoundKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkSound
            // 
            this.chkSound.BackColor = System.Drawing.Color.Transparent;
            this.chkSound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSound.Checked = true;
            this.chkSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSound.Location = new System.Drawing.Point(128, 343);
            this.chkSound.Name = "chkSound";
            this.chkSound.Size = new System.Drawing.Size(95, 24);
            this.chkSound.TabIndex = 10;
            this.chkSound.Text = "Som ligado";
            this.chkSound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSound.UseVisualStyleBackColor = false;
            this.chkSound.CheckedChanged += new System.EventHandler(this.chkSound_CheckedChanged);
            // 
            // Choose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Gadz.Tetris.Desktop.Properties.Resources.BACKGROUND_TETRIS;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(356, 479);
            this.Controls.Add(this.chkSound);
            this.Controls.Add(this.lbSoundKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEstilo);
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.lblChoose);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "Choose";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.Choose_Load);
            this.Shown += new System.EventHandler(this.Choose_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Inicio_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblChoose;
        private System.Windows.Forms.LinkLabel lnkHelp;
        private System.Windows.Forms.LinkLabel lnkAbout;
        private System.Windows.Forms.ComboBox cmbEstilo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbSoundKey;
        private System.Windows.Forms.CheckBox chkSound;
    }
}