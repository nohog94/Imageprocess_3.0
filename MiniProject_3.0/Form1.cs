using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenCvSharp;
//using Emgu.CV;
//using Emgu.CV.Structure;

namespace MiniProject_1._5
{
    public partial class Form1 : Form
    {
        protected ChromeDriverService _driverService = null;
        protected ChromeOptions _options = null;
        protected ChromeDriver _driver = null;

        public Form1()
        {
            InitializeComponent();
        }
        /// //////////////////
        /// 전역 변수부
        static byte[,,] inImage = null, outImage = null;
        static int inH, inW, outH, outW;
        static string fileName;
        static Bitmap paper, paper2, bitmap; // 그림을 콕콕 찍을 종이
        static int upload_flag = 0; // 파일을 열면 1로 설정
        // 0 : bright, 1 : dark, 2 : reverse, 3: black white, 4 : gamma, 5: filter, 6: move, 7: upsidedown, 8: rotate, 9: zoom in, 10: zoom out
        static int[] change_flag = Enumerable.Repeat<int>(0, 11).ToArray<int>();       
        double[,] mask = new double[5, 5];
        OpenCvSharp.Mat inCvImage, outCvImage;
        String[] people = new string[] { "Elton John", "IU", "Bruno Mars", "Baek Yelin"};
        

        const int RGB = 3, RR = 0, GG = 1, BB = 2;

        private void 로컬ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openImage();
        }

        private void 검색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_random_Image();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < 20)
            {
                menuStrip1.Visible = true;
            }
            else if (e.Y > 20)
            {
                menuStrip1.Visible = false;
            }
        }

        private void 열기ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grayScale_cv();
        }

        private void hSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HSV_cv();
        }

        private void lABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LAB_cv();
        }

        private void 추가기능ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hLSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HLS_cv();
        }

        private void luvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Luv_cv();
        }

        private void xYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYZ_cv();
        }

        private void yUVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YUV_cv();
        }

        private void whoAreYouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WAY_cv();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            change();
        }

        private void btnBright_Click(object sender, EventArgs e)
        {
            change_flag[0] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0,255,255);
            btnBright.BackColor = Cyan;
        }

        private void btnDark_Click(object sender, EventArgs e)
        {
            change_flag[1] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnDark.BackColor = Cyan;
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            change_flag[2] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnReverse.BackColor = Cyan;
        }

        private void btnBlackWhite_Click(object sender, EventArgs e)
        {
            change_flag[3] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnBlackWhite.BackColor = Cyan;
        }

        private void btnGamma_Click(object sender, EventArgs e)
        {
            change_flag[4] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnGamma.BackColor = Cyan;
        }
        private void btnSharpen_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;
                // flag가 0이면 button 색 변환
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnSharpen.BackColor = Cyan;
                btnFilter.BackColor = Cyan;
                textBox7.Text = (0).ToString();
                textBox8.Text = (-1).ToString();
                textBox9.Text = (0).ToString();
                textBox12.Text = (-1).ToString();
                textBox13.Text = (5).ToString();
                textBox14.Text = (-1).ToString();
                textBox17.Text = (0).ToString();
                textBox18.Text = (-1).ToString();
                textBox19.Text = (0).ToString();
            }
        }
        private void btnBlur_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;
                // flag가 0이면 button 색 변환
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnBlur.BackColor = Cyan;
                btnFilter.BackColor = Cyan;
                textBox7.Text = (0.11).ToString();
                textBox8.Text = (0.11).ToString();
                textBox9.Text = (0.11).ToString();
                textBox12.Text = (0.11).ToString();
                textBox13.Text = (0.11).ToString();
                textBox14.Text = (0.11).ToString();
                textBox17.Text = (0.11).ToString();
                textBox18.Text = (0.11).ToString();
                textBox19.Text = (0.11).ToString();
            }
        }

        private void btnEdgeDetect_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;

                // flag가 0이면 button 색 변환
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnEdgeDetect.BackColor = Cyan;
                btnFilter.BackColor = Cyan;
                textBox7.Text = (0).ToString();
                textBox8.Text = (1).ToString();
                textBox9.Text = (0).ToString();
                textBox12.Text = (1).ToString();
                textBox13.Text = (-4).ToString();
                textBox14.Text = (1).ToString();
                textBox17.Text = (0).ToString();
                textBox18.Text = (1).ToString();
                textBox19.Text = (0).ToString();
            }
        }

        private void btnEdgeEnhance_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;

                // flag가 0이면 button 색 변환
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnEdgeEnhance.BackColor = Cyan;
                btnFilter.BackColor = Cyan;
                textBox7.Text = (0).ToString();
                textBox8.Text = (0).ToString();
                textBox9.Text = (0).ToString();
                textBox12.Text = (-1).ToString();
                textBox13.Text = (1).ToString();
                textBox14.Text = (0).ToString();
                textBox17.Text = (0).ToString();
                textBox18.Text = (0).ToString();
                textBox19.Text = (0).ToString();
            }
        }

        private void btnEmboss_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;

                // flag가 0이면 button 색 변환
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnEmboss.BackColor = Cyan;
                btnFilter.BackColor = Cyan;
                textBox7.Text = (-2).ToString();
                textBox8.Text = (-1).ToString();
                textBox9.Text = (0).ToString();
                textBox12.Text = (-1).ToString();
                textBox13.Text = (1).ToString();
                textBox14.Text = (1).ToString();
                textBox17.Text = (0).ToString();
                textBox18.Text = (1).ToString();
                textBox19.Text = (2).ToString();
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (change_flag[5] != 1)
            {
                change_flag[5] = 1;
                Color Cyan = Color.FromArgb(0, 255, 255);
                btnFilter.BackColor = Cyan;
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            change_flag[6] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnMove.BackColor = Cyan;
        }
        private void btnUpSideDown_Click(object sender, EventArgs e)
        {
            change_flag[7] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnUpSideDown.BackColor = Cyan;
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            change_flag[8] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnRotate.BackColor = Cyan;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            change_flag[9] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnZoomIn.BackColor = Cyan;
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            change_flag[10] = 1;

            // flag가 0이면 button 색 변환
            Color Cyan = Color.FromArgb(0, 255, 255);
            btnZoomOut.BackColor = Cyan;
        }

        /// //////////////////
        /// 공통 함수부
        /// 

        void change()
        {
            int temp = 0;
            for (int i = 0; i< change_flag.Length; i++)
            {
                temp += change_flag[i];
            }
            if (temp > 1)
            {
                MessageBox.Show("하나의 변환 기능만 선택 하세요");
                resetbutton();
                return;
            }
            if(upload_flag == 0)
            {
                MessageBox.Show("파일을 먼저 업로드하세요");
                resetbutton();
                return;
            }
            if (change_flag[0] == 1)
            {
                    string input_bright = Microsoft.VisualBasic.Interaction.InputBox("얼마나 밝게 하시겠습니까?", "입력", "0");
                    try
                    {
                        bright(int.Parse(input_bright));
                    }
                    catch
                    {
                        MessageBox.Show("올바른 형식으로 입력하세요 (정수)");
                    }
                
            }
            else if (change_flag[1] == 1)
            {
                string input_dark = Microsoft.VisualBasic.Interaction.InputBox("얼마나 어둡게 하시겠습니까?", "입력", "0");
                try
                {
                    dark(int.Parse(input_dark));
                }
                catch
                {
                    MessageBox.Show("올바른 형식으로 입력하세요 (정수)");
                }
            }
            else if (change_flag[2] == 1)
            {
                reverse();
            }
            else if (change_flag[3] == 1)
            {
                black_and_white();
            }
            else if (change_flag[4] == 1)
            {
                string input_gamma = Microsoft.VisualBasic.Interaction.InputBox("감마 값을 입력하세요(ex : 2.5)", "입력", "0");
                try
                {
                    gamma(double.Parse(input_gamma));
                }
                catch
                {
                    MessageBox.Show("올바른 형식으로 입력하세요 (실수)");
                }
            }
            else if (change_flag[5] == 1)
            {
                mask[0, 0] = double.Parse(textBox1.Text);
                mask[0, 1] = double.Parse(textBox2.Text);
                mask[0, 2] = double.Parse(textBox3.Text);
                mask[0, 3] = double.Parse(textBox4.Text);
                mask[0, 4] = double.Parse(textBox5.Text);
                mask[1, 0] = double.Parse(textBox6.Text);
                mask[1, 1] = double.Parse(textBox7.Text);
                mask[1, 2] = double.Parse(textBox8.Text);
                mask[1, 3] = double.Parse(textBox9.Text);
                mask[1, 4] = double.Parse(textBox10.Text);
                mask[2, 0] = double.Parse(textBox11.Text);
                mask[2, 1] = double.Parse(textBox12.Text);
                mask[2, 2] = double.Parse(textBox13.Text);
                mask[2, 3] = double.Parse(textBox14.Text);
                mask[2, 4] = double.Parse(textBox15.Text);
                mask[3, 0] = double.Parse(textBox16.Text);
                mask[3, 1] = double.Parse(textBox17.Text);
                mask[3, 2] = double.Parse(textBox18.Text);
                mask[3, 3] = double.Parse(textBox19.Text);
                mask[3, 4] = double.Parse(textBox20.Text);
                mask[4, 0] = double.Parse(textBox21.Text);
                mask[4, 1] = double.Parse(textBox22.Text);
                mask[4, 2] = double.Parse(textBox23.Text);
                mask[4, 3] = double.Parse(textBox24.Text);
                mask[4, 4] = double.Parse(textBox25.Text);
                filter_image(mask);
            }
            else if (change_flag[6] == 1)
            {
                string input_move = Microsoft.VisualBasic.Interaction.InputBox("얼마나 이동하시겠습니까? x,y", "입력", "0,0");
                try
                {
                    string[] xy_to_move = input_move.Split(',');
                    move(int.Parse(xy_to_move[0]), int.Parse(xy_to_move[1]));
                }
                catch
                {
                    MessageBox.Show("올바른 형식으로 입력하세요 x,y");
                }
            }
            else if (change_flag[7] == 1)
            {
                up_side_down();
            }
            else if (change_flag[8] == 1)
            {
                string angle = Microsoft.VisualBasic.Interaction.InputBox("회전 각도를 입력하세요", "입력", "0");
                try
                {
                    rotate(int.Parse(angle));
                }
                catch
                {
                    MessageBox.Show("올바른 형식으로 입력하세요, (정수)");
                }
            }
            else if (change_flag[9] == 1)
            {
                string scale_zoom_in = Microsoft.VisualBasic.Interaction.InputBox("몇 배 확대할 건지 입력하세요", "입력", "0");
                //try
                //{
                zoom_in(double.Parse(scale_zoom_in));
                //}
                //catch
                //{
                //    MessageBox.Show("올바른 형식으로 입력하세요, (실수)");
                //}
            }
            else if (change_flag[10] == 1)
            {
                string scale_zoom_out = Microsoft.VisualBasic.Interaction.InputBox("몇 배 축소할 건지 입력하세요", "입력", "0");
                try
                {
                    zoom_out(double.Parse(scale_zoom_out));
                }
                catch
                {
                    MessageBox.Show("올바른 형식으로 입력하세요, (실수)");
                }
            }
            resetbutton();
        }

        void openImage()
        {
            upload_flag = 1;
            OpenFileDialog ofd = new OpenFileDialog();  // 객체 생성
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif";
            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            fileName = ofd.FileName;
            // 파일 크기 알아내기 (?)
            bitmap = new Bitmap(fileName);
            // 중요! 입력이미지의 높이, 폭 알아내기
            inH = bitmap.Height;
            inW = bitmap.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c = bitmap.GetPixel(k, i);
                    inImage[RR, i, k] = c.R;
                    inImage[GG, i, k] = c.G;
                    inImage[BB, i, k] = c.B;
                }

            equal_image();
        }

        void open_random_Image()
        {
            upload_flag = 1;
            string keyword = Microsoft.VisualBasic.Interaction.InputBox("키워드를 입력하세요", "입력", "0");
            string resource_url = "https://www.pexels.com/search/";
            resource_url = resource_url + keyword;

            _driverService = ChromeDriverService.CreateDefaultService();
            _driverService.HideCommandPromptWindow = true;
            _options = new ChromeOptions();
            _options.AddArgument("disable-gpu");

            _driver = new ChromeDriver(_driverService, _options);
            _driver.Navigate().GoToUrl(resource_url);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var searchBox = _driver.FindElementByXPath("//img[@class='photo-item__img']");
            string attribute = searchBox.GetAttribute("srcset");
            string[] trimmed_attribute = attribute.Split(',');
            String url_address = trimmed_attribute[0];
            url_address = url_address.Substring(0, url_address.Length - 3);
            url_address = url_address + "&h=500";
            var request = (HttpWebRequest)WebRequest.Create(url_address);

            using (var stream = request.GetResponse().GetResponseStream())
            {
                using (var image = Image.FromStream(stream))
                {
                    bitmap = new Bitmap(image);
                    //use or return bitmap, image will automatically get disposed
                }
            }

            _driver.Close();

            inH = bitmap.Height;
            inW = bitmap.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c = bitmap.GetPixel(k, i);
                    inImage[RR, i, k] = c.R;
                    inImage[GG, i, k] = c.G;
                    inImage[BB, i, k] = c.B;
                }
            
            equal_image();
        }

        void saveImage()
        {
            if (outImage == null)
            {
                MessageBox.Show("파일을 먼저 불러오세요");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();      //파일 저장하는 위치선택창
            sfd.InitialDirectory = @"C:\images\RAW\saved_image";
            sfd.DefaultExt = ".jpg";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Bitmap img = new Bitmap(pictureBox1.Image);
            
            img.Save(sfd.FileName, ImageFormat.Jpeg);
            img.Dispose();
        }

        void displayImage()
        {
            // 벽, 게시판, 종이 크기 조절
            paper = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height); // 종이
            paper2 = new Bitmap(outW, outH);
            Color pen; // 펜(콕콕 찍을 용도)
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    byte r = outImage[RR, i, k]; // 잉크(색상값)
                    byte g = outImage[GG, i, k]; // 잉크(색상값)
                    byte b = outImage[BB, i, k]; // 잉크(색상값)
                    pen = Color.FromArgb(r, g, b); // 펜에 잉크 묻히기

                    try
                    {
                        paper.SetPixel(k + (pictureBox1.Size.Width - outW) / 2, i + (pictureBox1.Size.Height - outH) / 2, pen); // 종이에 콕 찍기
                        paper2.SetPixel(k, i,pen);
                    }
                    catch
                    {
                        MessageBox.Show("이미지 사이즈가 너무 큽니다.");
                        return;
                    }
                }
            paper2.Save("temp.jpg");
            // inCvImage로 불러오기
            inCvImage = Cv2.ImRead("temp.jpg");
            // 임시 저장 파일 삭제
            System.IO.File.Delete("temp.jpg");
            pictureBox1.Image = paper; // 게시판에 종이를 붙이기.
        }

        void Cv2ToOutImage()
        {
            // 출력 이미지 메모리 확보
            outH = outCvImage.Height;
            outW = outCvImage.Width;
            outImage = new byte[RGB, outH, outW];
            // OpenCV 이미지 --> 메모리 (로딩)
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    var c = outCvImage.At<Vec3b>(i, k);
                    outImage[RR, i, k] = c.Item2;
                    outImage[GG, i, k] = c.Item1;
                    outImage[BB, i, k] = c.Item0;
                }
            displayImage();
            equal_image2();
        }

        void resetbutton()
        {
            // flag가 0이면 button 색 변환
            Color background = Color.FromArgb(46, 51, 73);
            btnBright.BackColor = background;
            btnDark.BackColor = background;
            btnReverse.BackColor = background;
            btnBlackWhite.BackColor = background;
            btnGamma.BackColor = background;
            btnSharpen.BackColor = background;
            btnBlur.BackColor = background;
            btnEdgeDetect.BackColor = background;
            btnEdgeEnhance.BackColor = background;
            btnEmboss.BackColor = background;
            btnMove.BackColor = background;
            btnUpSideDown.BackColor = background;
            btnRotate.BackColor = background;
            btnZoomIn.BackColor = background;
            btnZoomOut.BackColor = background;
            btnFilter.BackColor = background;
            for (int i = 0; i< change_flag.Length;  i++)
            {
                change_flag[i] = 0;
            }
        }

        /// //////////////////
        /// 영상 처리 함수부
        void equal_image()
        {  
            if (inImage == null)
                return;
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            for(int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = inImage[rgb, i, k];
                    }
            displayImage();
        }

        void equal_image2()
        {
            inH = outH; inW = outW;
            inImage = new byte[RGB, inH, inW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        inImage[rgb, i, k] = outImage[rgb, i, k];
                    }
        }

        void bright(int color)
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((inImage[rgb, i, k] + color) > 255)
                            outImage[rgb, i, k] = 255;
                        else
                            outImage[rgb, i, k] = (byte)((int)inImage[rgb, i, k] + color);
                    }
            displayImage();
            equal_image2();
        }

        void dark(int color)
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((inImage[rgb, i, k] - color) < 0)
                            outImage[rgb, i, k] = 0;
                        else
                            outImage[rgb, i, k] = (byte)((int)inImage[rgb, i, k] - color);
                    }
            displayImage();
            equal_image2();
        }

        void grayScale_cv()
        {
            outCvImage = new Mat();

            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);

            Cv2ToOutImage();
        }

        void HSV_cv()
        {
            outCvImage = new Mat();
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2HSV);
            Cv2ToOutImage();
        }

        void LAB_cv()
        {
            outCvImage = new Mat();
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2Lab);
            Cv2ToOutImage();
        }

        void HLS_cv()
        {
            outCvImage = new Mat();
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2HLS);
            Cv2ToOutImage();
        }

        void Luv_cv()
        {
            outCvImage = new Mat();
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2Luv);
            Cv2ToOutImage();
        }

        void XYZ_cv()
        {
            outCvImage = new Mat();            
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2XYZ);           
            Cv2ToOutImage();
        }

        void YUV_cv()
        {
            outCvImage = new Mat();
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2YUV);
            Cv2ToOutImage();
        }

        void WAY_cv()
        {
            String path, img_path;
            CascadeClassifier faceCascade = new CascadeClassifier();
            faceCascade.Load("haar_face.xml");
            Mat img_array, gray, face_roi, pred_gray, pred_face_roi;
            int[] labels = new int[20];
            Mat[] features = new Mat[20];
            for(int i=0; i<people.Length; i++)
            {
                path = @"img\" + people[i];
                for (int j=0; j<5; j++)
                {
                    img_path = path + @"\" + i.ToString() + ".jpg";
                    img_array = Cv2.ImRead(img_path);
                    gray = new Mat();
                    Cv2.CvtColor(img_array, gray, ColorConversionCodes.BGR2GRAY);
                    Rect[] ret = faceCascade.DetectMultiScale(img_array, 1.1, 4);
                    face_roi = gray.AdjustROI(ret[0].Top, ret[0].Bottom, ret[0].Left, ret[0].Right);
                    labels[i*5+j] = i;
                    features[i*5+j] = face_roi;
                }
            }
            OpenCvSharp.Face.LBPHFaceRecognizer recognizer;
            recognizer = OpenCvSharp.Face.LBPHFaceRecognizer.Create();

            recognizer.Train(features, labels);
            pred_gray = new Mat();
            Cv2.CvtColor(inCvImage, pred_gray, ColorConversionCodes.BGR2GRAY);
            Rect[] ret2 = faceCascade.DetectMultiScale(pred_gray, 1.1, 4);
            pred_face_roi = pred_gray.AdjustROI(ret2[0].Top, ret2[0].Bottom, ret2[0].Left, ret2[0].Right);
            int ans = recognizer.Predict(pred_face_roi);
            MessageBox.Show(people[ans]+ " 입니다!, 아님 말고...");
        }

        void reverse()
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        outImage[rgb, i, j] = (byte)(255 - (int)inImage[rgb, i, j]);
                    }
                }
            displayImage();
            equal_image2();
        }


        void black_and_white()
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (inImage[rgb, i, k] > 128)
                            outImage[rgb, i, k] = 255;
                        else
                            outImage[rgb, i, k] = 0;
                    }
            displayImage();
            equal_image2();
        }

        void gamma(double gamma)
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        outImage[rgb, i, j] = (byte)((int)255 * Math.Pow(((double)inImage[rgb, i, j] / 255), gamma));
                        if (outImage[rgb, i, j] > 255)
                        {
                            outImage[rgb, i, j] = 255;
                        }
                    }
                }
            displayImage();
            equal_image2();
        }


        void filter_image(double[,] mask)
        {
            double sum = 0;
            byte[,,] temp_inImage = new byte[RGB, inH+4, inW+4];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH+4; i++)
                    for (int j = 0; j < inW+4; j++)
                    {
                        temp_inImage[rgb, i, j] = 0;
                    }
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int j = 0; j < inW; j++)
                    {
                        temp_inImage[rgb, i + 2, j + 2] = inImage[rgb, i, j];
                    }
                        
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        /* 기존 image와  filter에 대해서 convolution 수행 */
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 0; l < 5; l++)
                            {
                                /* 결과를 sum에 저장 */
                                sum += temp_inImage[rgb, i + k, j + l] * mask[k, l];
                            }
                        }
                        if (sum > 255)
                            outImage[rgb, i, j] = 255;
                        else if (sum < 0)
                            outImage[rgb, i, j] = 0;
                        else
                            outImage[rgb, i, j] = (byte)sum;
                        /* 다음 계산을 위해 sum 초기화 */
                        sum = 0;
                    }
                }
            displayImage();
            equal_image2();
        }

        void move(int move_x, int move_y)
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int j = 0; j < inW; j++)
                        /* outImage 초기화 수행 */
                        outImage[rgb, i, j] = 0;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        /* 경계를 벗어나지 않는 영역에 대해서만 outImage값 입력 */
                        if (i + move_y < inH && j + move_y < inW)
                            outImage[rgb, i + move_y, j + move_x] = inImage[rgb, i, j];
                        else
                            continue;
                    }
                }
            displayImage();
            equal_image2();
        }



        void up_side_down()
        {
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        outImage[rgb, i, j] = inImage[rgb, inH - 1 - i, j];
                    }
                }
            displayImage();
            equal_image2();
        }

        void rotate(int angle)
        {
            float temp_x, temp_y;
            int temp_after_rotate_x, temp_after_rotate_y;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int j = 0; j < inW; j++)
                        outImage[rgb, i, j] = 0;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        /* 회전변환을 위해 원점을 inW/2, inH/2로 가정 */
                        temp_x = j - inW / 2;
                        temp_y = inH / 2 - i;
                        /* 첨부한 그림 참조 */
                        temp_after_rotate_x = (int)(Math.Cos(angle * Math.PI / 180) * temp_x + Math.Sin(angle * Math.PI / 180) * temp_y);
                        temp_after_rotate_y = (int)(-Math.Sin(angle * Math.PI / 180) * temp_x + Math.Cos(angle * Math.PI / 180) * temp_y);
                        if (temp_after_rotate_y < inH / 2 && temp_after_rotate_y > -inH / 2 &&
                            temp_after_rotate_x < inW / 2 && temp_after_rotate_x > -inW / 2)
                            /* 영역을 벗어나지 않은 부분에 대해서만 값 입력 */
                            outImage[rgb, i, j] = inImage[rgb, inH / 2 - temp_after_rotate_y, temp_after_rotate_x + inW / 2];
                        else
                            continue;
                    }
                }
            displayImage();
            equal_image2();
        }

        void zoom_in(double scale)
        {
            outH = (int)(inH * scale); outW = (int)(inW * scale);
            if (outH > 500)
                outH = 500;
            if (outW > 500)
                outW = 500;
            outImage = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = (inImage[rgb, (byte)(i / scale), (byte)(k / scale)]);
                    }
            displayImage();
            equal_image2();
        }

        void zoom_out(double scale)
        {
            outH = (int)(inH / scale); outW = (int)(inW / scale);
            outImage = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = inImage[rgb, i * (byte)scale, k * (byte)scale];
                    }
            displayImage();
            equal_image2();
        }
    }
}
