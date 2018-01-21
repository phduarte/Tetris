﻿namespace Gadz.Tetris.Desktop {
    
    partial class Play {
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
            this.mainBoardPanel = new System.Windows.Forms.Panel();
            this.lbLevel = new System.Windows.Forms.Label();
            this.lbLines = new System.Windows.Forms.Label();
            this.lbPoints = new System.Windows.Forms.Label();
            this.nextBlockPanel = new System.Windows.Forms.Panel();
            this.lbTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // board
            // 
            this.mainBoardPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainBoardPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainBoardPanel.ForeColor = System.Drawing.Color.Black;
            this.mainBoardPanel.Location = new System.Drawing.Point(0, 0);
            this.mainBoardPanel.Name = "board";
            this.mainBoardPanel.Size = new System.Drawing.Size(220, 440);
            this.mainBoardPanel.TabIndex = 0;
            // 
            // lbNivel
            // 
            this.lbLevel.BackColor = System.Drawing.Color.Transparent;
            this.lbLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLevel.ForeColor = System.Drawing.Color.Black;
            this.lbLevel.Location = new System.Drawing.Point(236, 266);
            this.lbLevel.Name = "lbNivel";
            this.lbLevel.Size = new System.Drawing.Size(86, 30);
            this.lbLevel.TabIndex = 0;
            this.lbLevel.Text = "0";
            this.lbLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLinhas
            // 
            this.lbLines.BackColor = System.Drawing.Color.Transparent;
            this.lbLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLines.ForeColor = System.Drawing.Color.Black;
            this.lbLines.Location = new System.Drawing.Point(236, 87);
            this.lbLines.Name = "lbLinhas";
            this.lbLines.Size = new System.Drawing.Size(86, 30);
            this.lbLines.TabIndex = 1;
            this.lbLines.Text = "0";
            this.lbLines.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPontos
            // 
            this.lbPoints.BackColor = System.Drawing.Color.Transparent;
            this.lbPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPoints.ForeColor = System.Drawing.Color.Black;
            this.lbPoints.Location = new System.Drawing.Point(236, 24);
            this.lbPoints.Name = "lbPontos";
            this.lbPoints.Size = new System.Drawing.Size(86, 30);
            this.lbPoints.TabIndex = 2;
            this.lbPoints.Text = "0";
            this.lbPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // painelProximo
            // 
            this.nextBlockPanel.BackColor = System.Drawing.Color.Transparent;
            this.nextBlockPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.nextBlockPanel.ForeColor = System.Drawing.Color.Black;
            this.nextBlockPanel.Location = new System.Drawing.Point(234, 154);
            this.nextBlockPanel.Name = "painelProximo";
            this.nextBlockPanel.Size = new System.Drawing.Size(88, 88);
            this.nextBlockPanel.TabIndex = 0;
            // 
            // lbTempo
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.BackColor = System.Drawing.Color.Transparent;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.ForeColor = System.Drawing.Color.Black;
            this.lbTime.Location = new System.Drawing.Point(229, 406);
            this.lbTime.Name = "lbTempo";
            this.lbTime.Size = new System.Drawing.Size(96, 25);
            this.lbTime.TabIndex = 2;
            this.lbTime.Text = "00:00:00";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(226, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "SCORE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(226, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "TIME";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(226, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 21);
            this.label4.TabIndex = 6;
            this.label4.Text = "LINES";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(226, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 21);
            this.label5.TabIndex = 7;
            this.label5.Text = "LEVEL";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(226, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 21);
            this.label6.TabIndex = 9;
            this.label6.Text = "SPEED";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVelocidade
            // 
            this.lbSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lbSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSpeed.ForeColor = System.Drawing.Color.Black;
            this.lbSpeed.Location = new System.Drawing.Point(235, 328);
            this.lbSpeed.Name = "lbVelocidade";
            this.lbSpeed.Size = new System.Drawing.Size(86, 30);
            this.lbSpeed.TabIndex = 8;
            this.lbSpeed.Text = "0";
            this.lbSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(226, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 21);
            this.label8.TabIndex = 10;
            this.label8.Text = "NEXT";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Jogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(165)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(340, 440);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbSpeed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbLevel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbLines);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbPoints);
            this.Controls.Add(this.nextBlockPanel);
            this.Controls.Add(this.mainBoardPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Jogo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tetris";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Jogo_FormClosed);
            this.Load += new System.EventHandler(this.Jogo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Jogo_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mainBoardPanel;
        private System.Windows.Forms.Panel nextBlockPanel;
        private System.Windows.Forms.Label lbLevel;
        private System.Windows.Forms.Label lbLines;
        private System.Windows.Forms.Label lbPoints;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label label8;
    }
}