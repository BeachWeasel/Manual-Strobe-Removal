namespace Manual_Strobe_Removal
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
            this.ListedTB = new System.Windows.Forms.TextBox();
            this.ImportBTN = new System.Windows.Forms.Button();
            this.ProcBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListedTB
            // 
            this.ListedTB.Location = new System.Drawing.Point(12, 41);
            this.ListedTB.Multiline = true;
            this.ListedTB.Name = "ListedTB";
            this.ListedTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ListedTB.Size = new System.Drawing.Size(206, 298);
            this.ListedTB.TabIndex = 0;
            // 
            // ImportBTN
            // 
            this.ImportBTN.Location = new System.Drawing.Point(12, 12);
            this.ImportBTN.Name = "ImportBTN";
            this.ImportBTN.Size = new System.Drawing.Size(206, 23);
            this.ImportBTN.TabIndex = 1;
            this.ImportBTN.Text = "Import";
            this.ImportBTN.UseVisualStyleBackColor = true;
            this.ImportBTN.Click += new System.EventHandler(this.ImportBTN_Click);
            // 
            // ProcBTN
            // 
            this.ProcBTN.Location = new System.Drawing.Point(12, 353);
            this.ProcBTN.Name = "ProcBTN";
            this.ProcBTN.Size = new System.Drawing.Size(206, 23);
            this.ProcBTN.TabIndex = 2;
            this.ProcBTN.Text = "Process";
            this.ProcBTN.UseVisualStyleBackColor = true;
            this.ProcBTN.Click += new System.EventHandler(this.ProcBTN_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 388);
            this.Controls.Add(this.ProcBTN);
            this.Controls.Add(this.ImportBTN);
            this.Controls.Add(this.ListedTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "MSR";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ListedTB;
        private System.Windows.Forms.Button ImportBTN;
        private System.Windows.Forms.Button ProcBTN;
    }
}

