namespace CUPID {
    partial class UploadBox {
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
            this.URLText = new System.Windows.Forms.TextBox();
            this.Open = new System.Windows.Forms.Button();
            this.Copy = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // URLText
            // 
            this.URLText.Location = new System.Drawing.Point(12, 55);
            this.URLText.Name = "URLText";
            this.URLText.Size = new System.Drawing.Size(361, 20);
            this.URLText.TabIndex = 0;
            // 
            // Open
            // 
            this.Open.AccessibleName = "Open";
            this.Open.Location = new System.Drawing.Point(217, 97);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(75, 23);
            this.Open.TabIndex = 1;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.openBtn_Click);
            // 
            // Copy
            // 
            this.Copy.Location = new System.Drawing.Point(298, 97);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(75, 23);
            this.Copy.TabIndex = 2;
            this.Copy.Text = "Copy";
            this.Copy.UseVisualStyleBackColor = true;
            this.Copy.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(136, 97);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 3;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // UploadBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 132);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.URLText);
            this.Name = "UploadBox";
            this.Text = "UploadBox";
            this.Load += new System.EventHandler(this.UploadBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox URLText;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button Copy;
        private System.Windows.Forms.Button delete;
    }
}