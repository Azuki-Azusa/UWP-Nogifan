using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace NogiFan
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Schedule : Page
    {
        ObservableCollection<Object> place = new ObservableCollection<Object>();
        ObservableCollection<Object> date = new ObservableCollection<Object>();
        string uri;
        Models.Menu menu;
        public Schedule()
        {
            menu = (Application.Current as App).menu;
            this.InitializeComponent();
            TextBlock A = new TextBlock();
            TextBlock B = new TextBlock();
            A.Text = menu.place;
            B.Text = menu.sche;
            place.Add(A);
            date.Add(B);
        }

        /// <summary>
        /// 获取握手会页面url
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uri = (string)e.Parameter;
            uri = "http://www.nogizaka46.com/event/" + uri;
            Load(uri);
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 加载网页
        /// 解析html
        /// 添加数据到页面
        /// </summary>
        /// <param name="uri"></param>
        async void Load(string uri)
        {
            try {
                string html = "";
                HttpClient myWebClient = new HttpClient();
                Stream myStream = await myWebClient.GetStreamAsync(uri);
                StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                html = sr.ReadToEnd();
                //加载源代码，获取文档对象
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                //更加xpath获取总的对象，如果不为空，就继续选择dl标签
                var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[2]/div[3]/div[1]/div[1]/table[1]");
                for (int i = 0; i < res.ChildNodes.Count / 2; i++)
                {
                    var temp = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[2]/div[3]/div[1]/div[1]/table[1]/tr[" + (i + 1) + "]/th[1]/font[1]");
                    TextBlock text = new TextBlock();
                    text.Text = temp.InnerText;
                    date.Add(text);
                    var temp2 = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[2]/div[3]/div[1]/div[1]/table[1]/tr[" + (i + 1) + "]/td[1]/font[1]");
                    TextBlock text2 = new TextBlock();
                    text2.Text = temp2.InnerText;
                    place.Add(text2);
                }
            }

            catch (Exception ee)
            {
                string message = ee.Message + "\nA browser will be opened to show the blog net.";
                var dialog = new MessageDialog(message, "Error");

                dialog.Commands.Add(new UICommand(menu.OK, cmd => { }, menu.OK));
                dialog.Commands.Add(new UICommand(menu.NO, cmd => { }, menu.NO));

                //设置默认按钮，不设置的话默认的确认按钮是第一个按钮
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                //获取返回值
                var result = await dialog.ShowAsync();
                if (result.Id as string == menu.OK)
                {
                    await Launcher.LaunchUriAsync(new Uri(uri));
                }
                this.Frame.GoBack();
                return;
            }
        }

        /// <summary>
        /// 复制到剪贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            TextBlock temp = e.ClickedItem as TextBlock;
            DataPackage dp = new DataPackage();
            dp.SetText(temp.Text);
            Clipboard.SetContent(dp);
        }
    }
}
