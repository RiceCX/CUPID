using CUPID.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CUPID {
    public partial class UploadBox : Form {
        public ServerResponse Link;
        public DatabaseCache dbc;
        public UploadBox() {
            InitializeComponent();
        }

        private void UploadBox_Load(object sender, EventArgs e) {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dbc.updateTables(Link.url, Link.del_url, Link.thumb);
            URLText.Text = Link.url;
            PlaceLowerRight();
            TopMost = true;
        }

        private void PlaceLowerRight() {
            //Determine "rightmost" screen
            Screen rightmost = Screen.PrimaryScreen;

            Left = rightmost.WorkingArea.Right - this.Width;
            Top = rightmost.WorkingArea.Bottom - this.Height;
        }
        private void openBtn_Click(object sender, EventArgs e) {
            Process.Start(Link.url);
        }
        private void copyBtn_Click(object sender, EventArgs e) {
            Clipboard.SetText(Link.url);
            Close();
        }

        private async void delete_Click(object sender, EventArgs e) {
            var client = new HttpClient();
            await client.GetAsync(Link.del_url);
            Close();
        }
    }
}
