using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeModScan
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnOptionsReset_Click(object sender, EventArgs e)
        {
            tboptionDelay.Text = "3.5";
            tbOptionWindowSize.Text = "125";
            chbFastRecord.Checked = true;
        }

        private void btnOptionsCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
