namespace FifthElement.LogforceLoadingHybrid
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.ShowDevTools = new System.Windows.Forms.Button();
            this.ReloadPage = new System.Windows.Forms.Button();
            this.CopyHref = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ShowDevTools
            // 
            this.ShowDevTools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowDevTools.Location = new System.Drawing.Point(975, 708);
            this.ShowDevTools.Name = "ShowDevTools";
            this.ShowDevTools.Size = new System.Drawing.Size(34, 23);
            this.ShowDevTools.TabIndex = 0;
            this.ShowDevTools.Text = "F12";
            this.ShowDevTools.UseVisualStyleBackColor = true;
            this.ShowDevTools.Visible = false;
            this.ShowDevTools.Click += new System.EventHandler(this.OnShowDevToolsClicked);
            // 
            // ReloadPage
            // 
            this.ReloadPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReloadPage.Location = new System.Drawing.Point(942, 708);
            this.ReloadPage.Name = "ReloadPage";
            this.ReloadPage.Size = new System.Drawing.Size(34, 23);
            this.ReloadPage.TabIndex = 1;
            this.ReloadPage.Text = "F5";
            this.ReloadPage.UseVisualStyleBackColor = true;
            this.ReloadPage.Visible = false;
            this.ReloadPage.Click += new System.EventHandler(this.OnReloadPageClicked);
            // 
            // CopyHref
            // 
            this.CopyHref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyHref.Location = new System.Drawing.Point(861, 708);
            this.CopyHref.Name = "CopyHref";
            this.CopyHref.Size = new System.Drawing.Size(75, 23);
            this.CopyHref.TabIndex = 2;
            this.CopyHref.Text = "Kopioi osoite";
            this.CopyHref.UseVisualStyleBackColor = true;
            this.CopyHref.Visible = false;
            this.CopyHref.Click += new System.EventHandler(this.CopyHref_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.CopyHref);
            this.Controls.Add(this.ReloadPage);
            this.Controls.Add(this.ShowDevTools);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Kotka";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ShowDevTools;
        private System.Windows.Forms.Button ReloadPage;
        private System.Windows.Forms.Button CopyHref;
    }
}