using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CUPID.Utilities;
using Newtonsoft.Json;

namespace CUPID {
    public partial class AppForm : Form {
        private bool active;
        private readonly Utilities.GlobalKeyboardHook gkh = new Utilities.GlobalKeyboardHook();
        private Rectangle bounds;
        private Graphics graphics;
        private bool drawnPP;
        private int initialX;
        private int initialY;
        private bool isDown;
        private Bitmap frozenBitmap;

        public AppForm() {
            InitializeComponent();
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
            FormBorderStyle = FormBorderStyle.None;
        }

        private void icon_Click(object sender, MouseEventArgs e) {
            // open settings
            WindowState = FormWindowState.Normal;
        }

        private void onAppLoad(object sender, EventArgs e) {
            // load keybind hooks
            gkh.HookedKeys.Add(Keys.PrintScreen);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.HookedKeys.Add(Keys.Enter);
            gkh.KeyDown += gkh_KeyDownAsync;
            gkh.KeyUp += gkh_KeyUp;

        }
        private void gkh_KeyDownAsync(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.PrintScreen:
                    if(!active)
                        TakePreScreenshot();
                    break;
                case Keys.Escape:
                    if(active) {
                        active = false;
                        TopMost = false;
                        Hide();
                        Opacity = 1;
                    }
                    break;
                case Keys.Enter:
                    if(active && drawnPP) {
                        TakeScreenShotAsync();
                    } else {
                        e.Handled = true;
                    }
                    break;
            }
            e.Handled = true;
        }
        private void TakePreScreenshot() {
            active = true;
            Brush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            //   if (bounds.Width == 0 || bounds.Height == 0) return;
            Rectangle bonk = Screen.GetBounds(Point.Empty);
            Bitmap bmp = new Bitmap(bonk.Width, bonk.Height, PixelFormat.Format32bppArgb);
            frozenBitmap = new Bitmap(bonk.Width, bonk.Height, PixelFormat.Format32bppArgb); // this is so we can just fuck you.
            Graphics fG = Graphics.FromImage(frozenBitmap);
            fG.CopyFromScreen(Point.Empty, Point.Empty, bonk.Size);
            fG.Save();
            // this is for the masking
            Graphics preGraphics = Graphics.FromImage(bmp);
            preGraphics.CopyFromScreen(Point.Empty, Point.Empty, bonk.Size);
            preGraphics.FillRectangle(brush, bonk.X, bonk.Y, bonk.Width, bonk.Height);
//            bmp.Save(@"D:/Videos/test.png", ImageFormat.Png);
            TopMost = true;
            WindowState = FormWindowState.Maximized;
            BackgroundImage = bmp;
            this.TopMost = true;
            Opacity = 1;
        }
        private void TakeScreenShotAsync() {
            if (!active) return;
            if (bounds.Width == 0 || bounds.Height == 0) return;
            Invalidate();
            Bitmap target = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gTar = Graphics.FromImage(target);
            gTar.SmoothingMode = SmoothingMode.AntiAlias;
            gTar.DrawImage(frozenBitmap, new Rectangle(0,0, target.Width, target.Height), bounds, GraphicsUnit.Pixel);
            //    target.Save(@"D:/Videos/test.png", ImageFormat.Png);
            //    BGraph.CopyFromScreen(bounds.Left, bounds.Top, 1, 1, fuckyou.Size, CopyPixelOperation.SourceCopy);
            // uncomment for higher quality output

            active = false;
            TopMost = false;
            Hide();
            Opacity = 1;
            uploadImageAsync(target);
        }
        private async void uploadImageAsync(Bitmap target) {
            //  WebClient webClient = new WebClient();
            HttpClient client = new HttpClient();
            HttpContent bytesContent = new ByteArrayContent(ImageToByte(target));
            HttpContent stringContent = new StringContent("TOKEN HERE");
            var formData = new MultipartFormDataContent();
            client.DefaultRequestHeaders.Add("User-Agent", "CUPID/V1.0");
            formData.Add(stringContent, "token");
            formData.Add(bytesContent, "image", "image");
            try {
                var response = await client.PostAsync("URL HERE", formData);
                if (!response.IsSuccessStatusCode) {
                    return;
                }
                var data = await response.Content.ReadAsByteArrayAsync();
                var fuck = ServerResponse.parse(data);
                var lv = new UploadBox { Link = fuck };
                lv.Show();
                active = false;
            } catch(HttpRequestException e) {
                MessageBox.Show("ERROR: " + e.Message);
                active = false;
            }
        }
        
        public static byte[] ImageToByte(Image img) {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private void gkh_KeyUp(object sender, KeyEventArgs e) {
            e.Handled = true;
        }
        private void onDrawDown(object sender, MouseEventArgs e) {
            if (!active) return;
            isDown = true;
            initialX = e.X;
            initialY = e.Y;
        }
        // draw rect
        private void onDrawMove(object sender, MouseEventArgs e) {
            if (!active || !isDown) return;
            drawnPP = false;
            Refresh();
            Pen drawPen = new Pen(Color.Navy, 1);
            bounds = new Rectangle(Math.Min(e.X, initialX), Math.Min(e.Y, initialY), Math.Abs(e.X - initialX), Math.Abs(e.Y - initialY));
            graphics = CreateGraphics();
            graphics.DrawRectangle(drawPen, bounds);
        }

        private void onDrawUp(object sender, MouseEventArgs e) {
            if (!active) return;
            isDown = false;
            drawnPP = true;
        }
        public static void UploadFilesToServer(Uri uri, Dictionary<string, string> data, string fileName, string fileContentType, byte[] fileData) {
            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.BeginGetRequestStream((result) => {
                try {
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                    using (Stream requestStream = request.EndGetRequestStream(result)) {
                        UploadImage.WriteMultipartForm(requestStream, boundary, data, fileName, fileContentType, fileData);
                    }
                    request.BeginGetResponse(a => {
                        try {
                            var response = request.EndGetResponse(a);
                            var responseStream = response.GetResponseStream();
                            using (var sr = new StreamReader(responseStream)) {
                                using (StreamReader streamReader = new StreamReader(response.GetResponseStream())) {
                                    string responseString = streamReader.ReadToEnd();
                                    // do shit here?
                                }
                            }
                        }
                        catch (Exception) {

                        }
                    }, null);
                }
                catch (Exception) {

                }
            }, httpWebRequest);
        }
    }
}
