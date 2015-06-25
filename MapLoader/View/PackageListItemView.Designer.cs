namespace FifthElement.MapLoader.View
{
    partial class PackageListItemView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StatusIndicator = new System.Windows.Forms.PictureBox();
            this.PackageName = new System.Windows.Forms.Label();
            this.PackageSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.StatusIndicator)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusIndicator
            // 
            this.StatusIndicator.Location = new System.Drawing.Point(9, 2);
            this.StatusIndicator.Name = "StatusIndicator";
            this.StatusIndicator.Size = new System.Drawing.Size(16, 16);
            this.StatusIndicator.TabIndex = 0;
            this.StatusIndicator.TabStop = false;
            // 
            // PackageName
            // 
            this.PackageName.AutoSize = true;
            this.PackageName.Location = new System.Drawing.Point(32, 4);
            this.PackageName.Name = "PackageName";
            this.PackageName.Size = new System.Drawing.Size(78, 13);
            this.PackageName.TabIndex = 1;
            this.PackageName.Text = "PackageName";
            // 
            // PackageSize
            // 
            this.PackageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PackageSize.Location = new System.Drawing.Point(365, 4);
            this.PackageSize.Name = "PackageSize";
            this.PackageSize.Size = new System.Drawing.Size(97, 13);
            this.PackageSize.TabIndex = 2;
            this.PackageSize.Text = "Tarkastetaan...";
            this.PackageSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PackageListItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.PackageSize);
            this.Controls.Add(this.PackageName);
            this.Controls.Add(this.StatusIndicator);
            this.Name = "PackageListItemView";
            this.Size = new System.Drawing.Size(465, 21);
            ((System.ComponentModel.ISupportInitialize)(this.StatusIndicator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox StatusIndicator;
        private System.Windows.Forms.Label PackageName;
        private System.Windows.Forms.Label PackageSize;
    }
}
