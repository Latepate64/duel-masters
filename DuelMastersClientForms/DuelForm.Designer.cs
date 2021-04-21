
namespace DuelMastersClientForms
{
    partial class DuelForm
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
            this.OpponentPictureBox = new System.Windows.Forms.PictureBox();
            this.PlayerHand = new System.Windows.Forms.FlowLayoutPanel();
            this.OpponentHand = new System.Windows.Forms.FlowLayoutPanel();
            this.PlayerBattleZone = new System.Windows.Forms.FlowLayoutPanel();
            this.OpponentBattleZone = new System.Windows.Forms.FlowLayoutPanel();
            this.CreateHandCardButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentPictureBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpponentPictureBox
            // 
            this.OpponentPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OpponentPictureBox.BackColor = System.Drawing.Color.Black;
            this.OpponentPictureBox.Location = new System.Drawing.Point(281, 136);
            this.OpponentPictureBox.Name = "OpponentPictureBox";
            this.OpponentPictureBox.Size = new System.Drawing.Size(143, 56);
            this.OpponentPictureBox.TabIndex = 1;
            this.OpponentPictureBox.TabStop = false;
            // 
            // PlayerHand
            // 
            this.PlayerHand.AutoScroll = true;
            this.PlayerHand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.PlayerHand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayerHand.Location = new System.Drawing.Point(3, 526);
            this.PlayerHand.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.PlayerHand.Name = "PlayerHand";
            this.PlayerHand.Size = new System.Drawing.Size(673, 127);
            this.PlayerHand.TabIndex = 2;
            // 
            // OpponentHand
            // 
            this.OpponentHand.AutoScroll = true;
            this.OpponentHand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.OpponentHand.Location = new System.Drawing.Point(3, 3);
            this.OpponentHand.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.OpponentHand.Name = "OpponentHand";
            this.OpponentHand.Size = new System.Drawing.Size(673, 127);
            this.OpponentHand.TabIndex = 3;
            // 
            // PlayerBattleZone
            // 
            this.PlayerBattleZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayerBattleZone.AutoScroll = true;
            this.PlayerBattleZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.PlayerBattleZone.Location = new System.Drawing.Point(3, 331);
            this.PlayerBattleZone.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.PlayerBattleZone.Name = "PlayerBattleZone";
            this.PlayerBattleZone.Size = new System.Drawing.Size(673, 127);
            this.PlayerBattleZone.TabIndex = 3;
            // 
            // OpponentBattleZone
            // 
            this.OpponentBattleZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OpponentBattleZone.AutoScroll = true;
            this.OpponentBattleZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.OpponentBattleZone.Location = new System.Drawing.Point(3, 198);
            this.OpponentBattleZone.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.OpponentBattleZone.Name = "OpponentBattleZone";
            this.OpponentBattleZone.Size = new System.Drawing.Size(673, 127);
            this.OpponentBattleZone.TabIndex = 4;
            // 
            // CreateHandCardButton
            // 
            this.CreateHandCardButton.Location = new System.Drawing.Point(82, 498);
            this.CreateHandCardButton.Name = "CreateHandCardButton";
            this.CreateHandCardButton.Size = new System.Drawing.Size(157, 23);
            this.CreateHandCardButton.TabIndex = 5;
            this.CreateHandCardButton.Text = "Create hand card";
            this.CreateHandCardButton.UseVisualStyleBackColor = true;
            this.CreateHandCardButton.Click += new System.EventHandler(this.CreateHandCardButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.OpponentHand);
            this.flowLayoutPanel1.Controls.Add(this.OpponentPictureBox);
            this.flowLayoutPanel1.Controls.Add(this.OpponentBattleZone);
            this.flowLayoutPanel1.Controls.Add(this.PlayerBattleZone);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox1);
            this.flowLayoutPanel1.Controls.Add(this.PlayerHand);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(294, 12);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(285, 3, 285, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(676, 659);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(281, 464);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 56);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(978, 12);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.Size = new System.Drawing.Size(274, 303);
            this.OutputTextBox.TabIndex = 7;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(978, 321);
            this.InputTextBox.Multiline = true;
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(274, 149);
            this.InputTextBox.TabIndex = 8;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(978, 476);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(274, 43);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // DuelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.CreateHandCardButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DuelForm";
            this.Text = "Duel";
            ((System.ComponentModel.ISupportInitialize)(this.OpponentPictureBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox OpponentPictureBox;
        private System.Windows.Forms.FlowLayoutPanel PlayerHand;
        private System.Windows.Forms.FlowLayoutPanel OpponentHand;
        private System.Windows.Forms.FlowLayoutPanel PlayerBattleZone;
        private System.Windows.Forms.FlowLayoutPanel OpponentBattleZone;
        private System.Windows.Forms.Button CreateHandCardButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox OutputTextBox;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button SendButton;
    }
}