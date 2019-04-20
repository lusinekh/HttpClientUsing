using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadProgram
{
    public partial class Form1 : Form
    {
        static int Count = 1;
        string DownloadFileUrl;
        string DestinationFilePath;
        string NewPath;
        public Form1()
        {
            InitializeComponent();
        }
        private async void Download_Click(object sender, EventArgs e)
        {
            try
            {
                DownloadFileUrl = TextUri.Text;
                DestinationFilePath = Path.GetFullPath($"NewFile{Count}.zip");
                Count++;
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (Exception)
            {
                throw;
            }

            using (var client = new HttpClientDownloadWithProgress(DownloadFileUrl, DestinationFilePath))
            {
                client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
                {
                    TextProgress.Text = $"{progressPercentage}% ({totalBytesDownloaded}/{totalFileSize})";
                    progressBar1.Value = (int)progressPercentage;
                };
                try
                {
                    await client.StartDownload();
                }

                catch (Exception ex)
                {
                    TextProgress.Text = ex.Message;

                }
            }
        }
    }
}
