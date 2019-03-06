using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace NogiFan
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int n = 1;
        Models.Menu menu;
        public MainPage()
        {
            menu = (Application.Current as App).menu;
            this.InitializeComponent();
        }

        /// <summary>
        /// 点击菜单，显示对应页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            To.Visibility = Visibility.Visible;
            About.Visibility = Visibility.Collapsed;
            var temp = e.ClickedItem as Grid;
            if (temp.Name.Equals("MemberPage"))
            {
                To.Navigate(typeof(MemberPage));
            }
            else if (temp.Name.Equals("SongPage"))
            {
                To.Navigate(typeof(SongPage));
            }
            else if (temp.Name.Equals("BlogPage"))
            {
                To.Navigate(typeof(BlogPage));
            }
            else if (temp.Name.Equals("OshimenPage"))
            {
                To.Navigate(typeof(OshimenPage));
            }
            else if (temp.Name.Equals("NewsPage"))
            {
                To.Navigate(typeof(ExchangePage));
            }
            else if (temp.Name.Equals("ShopPage"))
            {
                await Launcher.LaunchUriAsync(new Uri("https://www.nogizaka46shop.com/"));
            }
            else if (temp.Name.Equals("HandPage"))
            {
                To.Navigate(typeof(HandPage));
            }
        }

        /// <summary>
        /// 语言转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).menu.Change();
            for (int i = 0; i < (Application.Current as App).ViewModel.getItems.Count; i++)
            {
                (Application.Current as App).ViewModel.getItems[i].change();
            }
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// 播放/暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (BGM.CurrentState == MediaElementState.Playing)
            {
                BGM.Pause();
                play.Icon = new SymbolIcon(Symbol.Play);
                play.Label = "Play";
            }
            else
            {
                BGM.Play();
                play.Icon = new SymbolIcon(Symbol.Pause);
                play.Label = "Pause";
            }
        }

        /// <summary>
        /// 音乐播放结束的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BGM_MediaEnded(object sender, RoutedEventArgs e)
        {
            BGM.Stop();
            Uri uri;
            if (n == 1)
            {
                uri = new Uri("ms-appx:///Assets/BGM2.mp3");
                n = 2;
            }
            else if (n == 2)
            {
                uri = new Uri("ms-appx:///Assets/BGM3.mp3");
                n = 3;
            }
            else if (n == 3)
            {
                uri = new Uri("ms-appx:///Assets/BGM4.mp3");
                n = 4;
            }
            else
            {
                uri = new Uri("ms-appx:///Assets/BGM1.mp3");
                n = 1;
            }
            BGM.Source = uri;
            BGM.Play();
            BGM.AutoPlay = true;
        }

        /// <summary>
        /// 切歌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            BGM.Stop();
            Uri uri;
            if (n == 1)
            {
                uri = new Uri("ms-appx:///Assets/BGM2.mp3");
                n = 2;
            }
            else if (n == 2)
            {
                uri = new Uri("ms-appx:///Assets/BGM3.mp3");
                n = 3;
            }
            else if (n == 3)
            {
                uri = new Uri("ms-appx:///Assets/BGM4.mp3");
                n = 4;
            }
            else
            {
                uri = new Uri("ms-appx:///Assets/BGM1.mp3");
                n = 1;
            }
            BGM.Source = uri;
            play.Icon = new SymbolIcon(Symbol.Pause);
            play.Label = "Pause";
            BGM.Play();
            BGM.AutoPlay = true;
        }

        /// <summary>
        /// 显示官网（浏览器）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.nogizaka46.com/"));
        }

        /// <summary>
        /// 显示应用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                To.Visibility = Visibility.Collapsed;
                string filename = "Assets/README.xml";
                XmlDocument document = new XmlDocument();
                document.LoadXml(System.IO.File.ReadAllText(filename));
                StringBuilder show = new StringBuilder();
                XmlNodeList head;
                if (menu.language == true) head = document.GetElementsByTagName("CHN");
                else head = document.GetElementsByTagName("JPN");
                var child = head[0].ChildNodes;
                for (int i = 0; i < child.Count; i++)
                {
                    var childs = child[i].ChildNodes;
                    for (int j = 0; j < childs.Count; j++)
                    {
                        show.Append(childs[j].InnerText);
                    }
                    show.Append('\n');
                }
                About.Text = show.ToString();
                About.Visibility = Visibility.Visible;

            }
            catch (Exception ee)
            {
                var dialog = new MessageDialog(ee.Message, "Error");

                dialog.Commands.Add(new UICommand(menu.OK, cmd => { }, commandId: 0));

                await dialog.ShowAsync();
            }
        }
    }
}
