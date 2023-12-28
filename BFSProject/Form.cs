using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BFSProject
{
    public partial class Form : System.Windows.Forms.Form
    {
        List<List<int>> Adjacent;
        int Vertices; // so dinh

        GraphicsTools g; // khai bao 1 graphicsTools
        List<Point> lstPointVertices; // danh sach toa do cac dinh
        string FileName;
        public const int PicturePadding = 50; // picture padding
        public Form()
        {
            InitializeComponent();
            cboFrom.Size = new Size(172, 35);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // disable button findpath va button save image
            this.btnFindPath.Enabled = false;
            this.btnSaveImage.Enabled = false;
            this.btnGrapConnected.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFindPath_Click(object sender, EventArgs e)
        {
            BFS bfs = new BFS(this.Adjacent);

            int start = this.cboFrom.SelectedIndex;
            int end = this.cboTo.SelectedIndex;

            // reset picGraphics va txtResult
            this.picGraphics.Image = this.g.Reset(this.Adjacent, this.lstPointVertices);
            this.txtResult.Clear();

            if (start == end)
            {
                MessageBox.Show("Các đỉnh bị trùng lặp. Vui lòng chọn lại!", "Lỗi đỉnh đã chọn");
                return;
            }
            List<int> res = bfs.findPathbyBfs(this.Vertices, start, end);

            if (res == null)
            {
                string text = "Không thể tìm thấy bất kỳ đường đi nào từ {0} đến {1}.";
                MessageBox.Show(string.Format(text, start + 1, end + 1), "Tìm đường đi");
                return;
            }
            else
            {

                int index;
                this.txtResult.Text = "";

                // reset bit map

                // xuat ket qua ra text box
                for (index = 0; index < res.Count - 1; ++index)
                    this.txtResult.Text += (1 + res[index]).ToString() + " ---> ";
                this.txtResult.Text += (1 + res[index]).ToString();

                // ve duong di len bitmap
                this.picGraphics.Image = this.g.DrawPath(res, this.lstPointVertices);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Document File(*.txt)|*.txt|All File(*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // enable button findpath va button save image
                    this.btnFindPath.Enabled = true;
                    this.btnSaveImage.Enabled = true;
                    this.btnGrapConnected.Enabled = true;

                    // reset control values
                    this.cboFrom.Items.Clear();
                    this.cboTo.Items.Clear();
                    this.lvMatrix.Items.Clear();
                    this.lvMatrix.Columns.Clear();
                    this.txtResult.Clear();
                    this.picGraphics.Image = null;
                    //
                    Matrix m = new Matrix();
                    //
                    this.FileName = ofd.FileName;
                    this.Adjacent = m.LoadFile(this.FileName, this.lvMatrix, this.cboFrom, this.cboTo);
                    this.cboFrom.SelectedIndex = 0;
                    this.cboTo.SelectedIndex = 1;

                    this.Vertices = m.Vertices;

                    this.g = new GraphicsTools(this.picGraphics.Width, this.picGraphics.Height);// khoi tao graphics tool

                    // lay danh sach toa do cac dinh
                    this.lstPointVertices = new Vector2D(this.Vertices, this.picGraphics.Width - Form.PicturePadding,
                                                this.picGraphics.Height - Form.PicturePadding).getRandomPoint();

                    // tao bitmap do thi voi danh sach ke va danh sach toa cac dinh

                    this.picGraphics.Image = this.g.CreateGraphics(this.Adjacent, this.lstPointVertices);

                }
                catch (Exception EX)
                {

                            MessageBox.Show("Vui lòng kiểm tra lại dữ liệu file", "Thông báo lỗi",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            ofd.Dispose();
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveImgDialog = new SaveFileDialog();
            saveImgDialog.DefaultExt = "png";
            saveImgDialog.Filter = "Bitmap Image (*.png)|*.png|All File (*.*)|*.*";
            saveImgDialog.AddExtension = true;
            saveImgDialog.RestoreDirectory = true;
            saveImgDialog.Title = "Lưu đồ thị thành công";
            string initFileName = System.IO.Path.GetFileNameWithoutExtension(this.FileName);
            saveImgDialog.FileName = initFileName;

            try
            {
                if (saveImgDialog.ShowDialog() == DialogResult.OK)
                {
                    string imgPath = saveImgDialog.FileName;

                    if (imgPath != null)
                    {
                        if (this.picGraphics.Image != null)
                        {
                            this.picGraphics.Image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            saveImgDialog.Dispose();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnFindPath1_Click(object sender, EventArgs e)
        {

            BFS bfs = new BFS(this.Adjacent);

            if (bfs.IsGraphConnected())
            {
                MessageBox.Show("Đồ thị có liên thông.");
            }
            else
            {
                MessageBox.Show("Đồ thị không liên thông.");
            }
        }
    }
}
