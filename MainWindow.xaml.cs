using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace downloadPresentations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private int? getSlidesCount(string url)
        {
            WebRequest web;
            for (int i = 1; i < 1000; i++)
            {
                try
                {
                    web = WebRequest.Create(new Uri(url + i));
                    using (WebResponse response = web.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                //textUrl.Text = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebExceptionStatus status = ex.Status;
                    if (status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;
                        if ((int)response.StatusCode == 404)
                        {
                            return i;
                        }
                    }
                }
            }
            return null;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            url = textUrl.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                WebClient web = new WebClient();
                int? n = getSlidesCount(url);
                for (int i = 1; i < n; i++)
                {
                    web.DownloadFile(new Uri(url + i), AppDomain.CurrentDomain.BaseDirectory + i + ".svg");
                    debugText.Text += $"{i}.svg - downloaded" + "\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
