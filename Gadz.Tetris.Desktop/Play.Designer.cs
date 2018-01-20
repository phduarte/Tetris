namespace Gadz.Tetris.Desktop {
    
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
            this.board = new System.Windows.Forms.Panel();
            this.lbNivel = new System.Windows.Forms.Label();
            this.lbLinhas = new System.Windows.Forms.Label();
            this.lbPontos = new System.Windows.Forms.Label();
            this.painelProximo = new System.Windows.Forms.Panel();
            this.lbTempo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbVelocidade = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // board
            // 
            this.board.BackColor = System.Drawing.Color.Transparent;
            this.board.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.board.ForeColor = System.Drawing.Color.Black;
            this.board.Location = new System.Drawing.Point(0, 0);
            this.board.Name = "board";
            this.board.Size = new System.Drawing.Size(220, 440);
            this.board.TabIndex = 0;
            // 
            // lbNivel
            // 
            this.lbNivel.BackColor = System.Drawing.Color.Transparent;
            this.lbNivel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNivel.ForeColor = System.Drawing.Color.Black;
            this.lbNivel.Location = new System.Drawing.Point(236, 266);
            this.lbNivel.Name = "lbNivel";
            this.lbNivel.Size = new System.Drawing.Size(86, 30);
            this.lbNivel.TabIndex = 0;
            this.lbNivel.Text = "0";
            this.lbNivel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLinhas
            // 
            this.lbLinhas.BackColor = System.Drawing.Color.Transparent;
            this.lbLinhas.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLinhas.ForeColor = System.Drawing.Color.Black;
            this.lbLinhas.Location = new System.Drawing.Point(236, 87);
            this.lbLinhas.Name = "lbLinhas";
            this.lbLinhas.Size = new System.Drawing.Size(86, 30);
            this.lbLinhas.TabIndex = 1;
            this.lbLinhas.Text = "0";
            this.lbLinhas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPontos
            // 
            this.lbPontos.BackColor = System.Drawing.Color.Transparent;
            this.lbPontos.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPontos.ForeColor = System.Drawing.Color.Black;
            this.lbPontos.Location = new System.Drawing.Point(236, 24);
            this.lbPontos.Name = "lbPontos";
            this.lbPontos.Size = new System.Drawing.Size(86, 30);
            this.lbPontos.TabIndex = 2;
            this.lbPontos.Text = "0";
            this.lbPontos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // painelProximo
            // 
            this.painelProximo.BackColor = System.Drawing.Color.Transparent;
            this.painelProximo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.painelProximo.ForeColor = System.Drawing.Color.Black;
            this.painelProximo.Location = new System.Drawing.Point(234, 154);
            this.painelProximo.Name = "painelProximo";
            this.painelProximo.Size = new System.Drawing.Size(88, 88);
            this.painelProximo.TabIndex = 0;
            // 
            // lbTempo
            // 
            this.lbTempo.AutoSize = true;
            this.lbTempo.BackColor = System.Drawing.Color.Transparent;
            this.lbTempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTempo.ForeColor = System.Drawing.Color.Black;
            this.lbTempo.Location = new System.Drawing.Point(229, 406);
            this.lbTempo.Name = "lbTempo";
            this.lbTempo.Size = new System.Drawing.Size(96, 25);
            this.lbTempo.TabIndex = 2;
            this.lbTempo.Text = "00:00:00";
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
            this.lbVelocidade.BackColor = System.Drawing.Color.Transparent;
            this.lbVelocidade.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVelocidade.ForeColor = System.Drawing.Color.Black;
            this.lbVelocidade.Location = new System.Drawing.Point(235, 328);
            this.lbVelocidade.Name = "lbVelocidade";
            this.lbVelocidade.Size = new System.Drawing.Size(86, 30);
            this.lbVelocidade.TabIndex = 8;
            this.lbVelocidade.Text = "0";
            this.lbVelocidade.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.Controls.Add(this.lbVelocidade);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbNivel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbLinhas);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbTempo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbPontos);
            this.Controls.Add(this.painelProximo);
            this.Controls.Add(this.board);
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

        private System.Windows.Forms.Panel board;
        private System.Windows.Forms.Panel painelProximo;
        private System.Windows.Forms.Label lbNivel;
        private System.Windows.Forms.Label lbLinhas;
        private System.Windows.Forms.Label lbPontos;
        private System.Windows.Forms.Label lbTempo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbVelocidade;
        private System.Windows.Forms.Label label8;
    }
}