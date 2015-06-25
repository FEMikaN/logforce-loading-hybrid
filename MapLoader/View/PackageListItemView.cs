using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FifthElement.MapLoader.View
{
    public partial class PackageListItemView : UserControl
    {
        private readonly ToolTip _tip;

        public PackageListItemView()
        {
            InitializeComponent();

            Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _tip = new ToolTip {AutoPopDelay = 5000, InitialDelay = 200, ReshowDelay = 500, ShowAlways = true};
        }

        public void SetStatusIndicatorImage(Image indicator)
        {
            this.StatusIndicator.Image = indicator;
        }

        public void SetPackageName(string displayName)
        {
            this.PackageName.Text = displayName;
        }

        public void SetPackageSize(string packageSize)
        {
            this.PackageSize.Text = packageSize;
        }

        public void SetActive(bool active)
        {
            var font = new Font(this.PackageName.Font, active ? FontStyle.Bold : FontStyle.Regular);
            this.PackageName.Font = font;
            this.PackageSize.Font = font;
        }

        public void SetTooltip(string text)
        {
            _tip.SetToolTip(this.StatusIndicator, text);    
        }
    }
}
