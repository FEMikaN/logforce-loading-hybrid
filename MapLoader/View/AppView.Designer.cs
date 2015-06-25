namespace FifthElement.MapLoader.View
{
    partial class AppView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppView));
            this.CurrentPackageProgressBar = new System.Windows.Forms.ProgressBar();
            this.PackageStatusText = new System.Windows.Forms.Label();
            this.FileStatusText = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.PackageListPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.StartButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentPackageProgressBar
            // 
            this.CurrentPackageProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentPackageProgressBar.Location = new System.Drawing.Point(12, 311);
            this.CurrentPackageProgressBar.Name = "CurrentPackageProgressBar";
            this.CurrentPackageProgressBar.Size = new System.Drawing.Size(463, 23);
            this.CurrentPackageProgressBar.TabIndex = 0;
            // 
            // PackageStatusText
            // 
            this.PackageStatusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PackageStatusText.AutoSize = true;
            this.PackageStatusText.Location = new System.Drawing.Point(13, 341);
            this.PackageStatusText.Name = "PackageStatusText";
            this.PackageStatusText.Size = new System.Drawing.Size(196, 13);
            this.PackageStatusText.TabIndex = 1;
            this.PackageStatusText.Text = "Käynnistä karttalataus painamalla Aloita.";
            // 
            // FileStatusText
            // 
            this.FileStatusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FileStatusText.Location = new System.Drawing.Point(273, 341);
            this.FileStatusText.Name = "FileStatusText";
            this.FileStatusText.Size = new System.Drawing.Size(202, 13);
            this.FileStatusText.TabIndex = 2;
            this.FileStatusText.Text = "...";
            this.FileStatusText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(361, 371);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(114, 30);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Sulje";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // PackageListPanel
            // 
            this.PackageListPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackageListPanel.BackColor = System.Drawing.SystemColors.Control;
            this.PackageListPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.PackageListPanel.Location = new System.Drawing.Point(12, 12);
            this.PackageListPanel.Name = "PackageListPanel";
            this.PackageListPanel.Size = new System.Drawing.Size(463, 293);
            this.PackageListPanel.TabIndex = 4;
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartButton.Location = new System.Drawing.Point(241, 371);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(114, 30);
            this.StartButton.TabIndex = 5;
            this.StartButton.Text = "Aloita";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.OnStartButtonClick);
            // 
            // AppView
            // 
            this.AcceptButton = this.StartButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(487, 413);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.PackageListPanel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.FileStatusText);
            this.Controls.Add(this.PackageStatusText);
            this.Controls.Add(this.CurrentPackageProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AppView";
            this.Text = "Kotka Karttalataus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar CurrentPackageProgressBar;
        private System.Windows.Forms.Label PackageStatusText;
        private System.Windows.Forms.Label FileStatusText;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.FlowLayoutPanel PackageListPanel;
        private System.Windows.Forms.Button StartButton;
    }
}