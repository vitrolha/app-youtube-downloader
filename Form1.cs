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
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Playlists;
using YoutubeExplode.Videos.Streams;
using System.Text.RegularExpressions;

namespace YoutubeDownloaderTesteForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Pesquisar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        async void Pesquisar()
        {
            limparLista();

            var youtube = new YoutubeClient();
            if (checkBox1.Checked)
            {
                var video = await youtube.Videos.GetAsync(textBox1.Text);
                ListViewItem item = new ListViewItem();
                listView1.Items.Add(Convert.ToString(video));
            }
            if (checkBox2.Checked)
            {
                listaPLaylist();
            }
            
        }

        async void listaPLaylist()
        {
            var youtube = new YoutubeClient();
            var playlist = textBox1.Text;
            var videos = await youtube.Playlists.GetVideosAsync(playlist);

            //string[] pr = new string[videos.Count];
            //pr[0] = col_videos.Text;

            ListViewItem item= new ListViewItem();
            foreach(var video in videos)
            {
                listView1.Items.Add(Convert.ToString(video));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = label2.Text;
            DownloadMp3(path);
        }

        async void DownloadMp3(string path)
        {
            var youtube = new YoutubeClient();


            if (checkBox1.Checked)
            {
                var video = await youtube.Videos.GetAsync(textBox1.Text);
                //var title = video.Title.Replace("\""," ");
                Regex pattern = new Regex("[;,?*\\'\" ]");
                var title = pattern.Replace(video.Title, " ");
                await youtube.Videos.DownloadAsync(textBox1.Text, $"{path}" + "\\" + $"{title}.mp3");                
            }

            if (checkBox2.Checked)
            {               
                var playlist = textBox1.Text;
                var videos = await youtube.Playlists.GetVideosAsync(playlist);
                foreach (var videoPlaylist in videos)
                {
                    //var title = videoPlaylist.Title.Replace("\""," ");
                    Regex pattern = new Regex("[;,?*\\'\" ]");
                    var title = pattern.Replace(videoPlaylist.Title, " ");
                    var urlvideo = videoPlaylist.Url;
                    await youtube.Videos.DownloadAsync(urlvideo, $"{path}" + "\\" + $"{title}.mp3");
                }
                MessageBox.Show("Finalizado!");
            }
        }

        private void limparLista()
        {
            listView1.Items.Clear();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label2.Text = selecinarCaminho();
        }

        private string selecinarCaminho()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            return folderBrowserDialog.SelectedPath;
        }

    }
}
