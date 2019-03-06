using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class HandPage : Page
    {
        ObservableCollection<Object> list = new ObservableCollection<Object>();
        Models.Menu menu;
        string uri;
        public HandPage()
        {
            menu = (Application.Current as App).menu;
            uri = "http://www.nogizaka46.com/event/20180403.php";
            this.InitializeComponent();
            Load(uri);
        }


        /// <summary>
        /// 加载网页，获取html
        /// 解析html
        /// 获取所需节点
        /// </summary>
        /// <param name="uri"></param>
        async void Load(string uri)
        {
            try
            {
                string html = "";
                HttpClient myWebClient = new HttpClient();
                Stream myStream = await myWebClient.GetStreamAsync(uri);
                StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                html = sr.ReadToEnd();
                //加载源代码，获取文档对象
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                //更加xpath获取总的对象，如果不为空，就继续选择dl标签
                var res = doc.DocumentNode.SelectSingleNode(@"//*[@id=""eventlist""]/ul");
                for (int i = 0; i < res.ChildNodes.Count / 2; i++)
                {
                    var temp = doc.DocumentNode.SelectSingleNode(@"//*[@id=""eventlist""]/ul/li[" + (i + 1) + "]/a/span[1]");
                    TextBlock text = new TextBlock();
                    text.Text = temp.InnerText;
                    // 保存握手会对应地址
                    text.AccessKey = temp.ParentNode.Attributes["href"].Value;
                    list.Add(text);
                }
            }
            // //*[@id="eventlist"]/ul/li[1]/a/span[1]
            // //*[@id="eventlist"]/ul/li[2]/a/span[1]
            // //*[@id="eventlist"]/ul/li[53]/a/span[1]

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
        /// 加载新页面，显示握手会信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            TextBlock temp = e.ClickedItem as TextBlock;
            this.Frame.Navigate(typeof(Schedule), temp.AccessKey);
        }
    }
}
