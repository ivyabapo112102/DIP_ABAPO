using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ImageProcessing_Abapo
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap processedImage;
        private Bitmap imageB, imageA;
        private bool isRunning = false;
        private Func<Bitmap, Bitmap> selectedFilter;
        private VideoCaptureDevice videoSource;
        private FilterInfoCollection videoDevices;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    pictureBox1.Image = originalImage;
                }
            }
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            if (processedImage != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "BMP Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png|GIF Image|*.gif";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        processedImage.Save(saveFileDialog.FileName);
                    }
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Your code for pictureBox1 Click event
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Your code for pictureBox2 Click event
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Your code for Form1 Load event
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = comboBox1.SelectedItem.ToString();

            switch (selectedOption)
            {
                case "Copy":
                    CopyImage();
                    break;
                case "Greyscale":
                    ApplyGreyscale();
                    break;
                case "Color Inversion":
                    ApplyColorInversion();
                    break;
                case "Histogram":
                    ApplyHistogram();
                    break;
                case "Sepia":
                    ApplySepia();
                    break;
                default:
                    break;
            }
        }

        private void CopyImage()
        {
            if (originalImage != null)
            {
                processedImage = new Bitmap(originalImage);
                pictureBox2.Image = processedImage;
            }
        }

        private void ApplyGreyscale()
        {
            if (originalImage != null)
            {
                processedImage = ApplyFilter(originalImage, ColorToGrayscale);
                pictureBox2.Image = processedImage;
            }
        }

        private void ApplyColorInversion()
        {
            if (originalImage != null)
            {
                processedImage = ApplyFilter(originalImage, InvertColors);
                pictureBox2.Image = processedImage;
            }
        }

        private void ApplyHistogram()
        {
            if (originalImage != null)
            {
                processedImage = ApplyFilter(originalImage, AdjustContrast);
                pictureBox2.Image = processedImage;
            }
        }

        private void ApplySepia()
        {
            if (originalImage != null)
            {
                processedImage = ApplyFilter(originalImage, ApplySepia);
                pictureBox2.Image = processedImage;
            }
        }

        private Bitmap ApplyFilter(Bitmap image, Func<Color, Color> filter)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color originalColor = image.GetPixel(x, y);
                    Color processedColor = filter(originalColor);
                    result.SetPixel(x, y, processedColor);
                }
            }

            return result;
        }

        private Color ColorToGrayscale(Color color)
        {
            int average = (color.R + color.G + color.B) / 3;
            return Color.FromArgb(average, average, average);
        }

        private Color InvertColors(Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        private Color AdjustContrast(Color color)
        {
            int factor = 50; // Adjust this value to control the contrast
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, color.R + factor)),
                Math.Max(0, Math.Min(255, color.G + factor)),
                Math.Max(0, Math.Min(255, color.B + factor))
            );
        }

        private Color ApplySepia(Color color)
        {
            int intensity = (int)(0.3 * color.R + 0.59 * color.G + 0.11 * color.B);
            int sepiaR = Math.Min(255, intensity + 40);
            int sepiaG = Math.Min(255, intensity + 20);
            int sepiaB = Math.Min(255, intensity);

            return Color.FromArgb(sepiaR, sepiaG, sepiaB);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int maxWidth = Math.Max(imageB.Width, imageA.Width);
            int maxHeight = Math.Max(imageB.Height, imageA.Height);

            Bitmap resizedLoadImage = ResizeImage(imageB, maxWidth, maxHeight);
            Bitmap resizedBackgroundImage = ResizeImage(imageA, maxWidth, maxHeight);

            Bitmap processed = new Bitmap(maxWidth, maxHeight);

            for (int x = 0; x < maxWidth; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    Color pixelA = resizedLoadImage.GetPixel(x, y);
                    Color pixelB = resizedBackgroundImage.GetPixel(x, y);

                    int greenThreshold = 100;
                    if (pixelA.G > greenThreshold && pixelA.G > pixelA.R && pixelA.G > pixelA.B)
                    {
                        processed.SetPixel(x, y, pixelB);
                    }
                    else
                    {
                        processed.SetPixel(x, y, pixelA);
                    }
                }
            }


            private Bitmap ResizeImage(Image image, int width, int height)
            {
                Bitmap resizedImage = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.DrawImage(image, 0, 0, width, height);
                }
                return resizedImage;
            }


            private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
            {
                imageB = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = imageB;
            }

            private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
            {
                imageA = new Bitmap(openFileDialog2.FileName);
                pictureBox2.Image = imageA;
            }


            private void StartWebcam()
            {
                try
                {
                    if (videoSource == null)
                    {
                        videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                        if (videoDevices.Count > 0)
                        {
                            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                            videoSource.NewFrame += VideoSource_NewFrame;

                            videoSource.Start();
                            isRunning = true;
                        }
                        else
                        {
                            MessageBox.Show("No devices found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in starting webcam: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
            {
                pictureBox1.BeginInvoke((MethodInvoker)delegate
                {
                    pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                });

                if (selectedFilter != null)
                {
                    pictureBox2.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Image = selectedFilter((Bitmap)eventArgs.Frame.Clone());
                    });
                }
                else
                {
                    pictureBox2.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
                    });
                }
            }
            private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (selectedFilter != null)
            {
                Bitmap webcamFrame = new Bitmap(eventArgs.Frame);
                pictureBox1.Image = selectedFilter(webcamFrame);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWebcam();
        }

            private void button4_Click(object sender, EventArgs e)
            {
                StartWebcam();
                isRunning = true;
            }

            private void StopWebcam()
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= VideoSource_NewFrame;
                videoSource = null;
            }

            private void button5_Click(object sender, EventArgs e)
            {
                if (isRunning)
                {
                    StopWebcam();
                    isRunning = false;
                }

                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
            }

            private Bitmap ApplyFilter(Bitmap loaded)
        {
            return ApplyGrayscaleFilter(loaded);
        }

    }
}
