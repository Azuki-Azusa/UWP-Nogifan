using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Popups;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace NogiFan
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class OneBlog : Page
    {
        ObservableCollection<Object> list = new ObservableCollection<Object>();
        Models.Menu menu;
        string uri;
        public OneBlog()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uri = (string)e.Parameter;
            Load(uri);
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 根据url获取html
        /// 解析html并将信息添加到ListView中
        /// </summary>
        /// <param name="u"></param>
        async void Load(string u)
        {
            try
            {
                menu = (Application.Current as App).menu;
                string html = "";
                HttpClient myWebClient = new HttpClient();
                Stream myStream = await myWebClient.GetStreamAsync(u);
                StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                html = sr.ReadToEnd();
                //加载源代码，获取文档对象
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                //获取最新博客对应的根节点
                var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]");
                //var res = doc.DocumentNode;

                // 添加题目
                LoadTitle(res);
                // 添加日期
                LoadDate(res);
                // 添加作者
                LoadAuthor(res);
                // 添加正文
                await LoadArticleAsync(res.SelectSingleNode(@"div[3]"));
            }
            
            // 异常时弹窗并返回
            catch(Exception ee)
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
                    await Launcher.LaunchUriAsync(new Uri(u));
                }
                this.Frame.GoBack();
                return;
            }
        }

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="res"></param>
        void LoadTitle(HtmlNode res)
        {
            TextBlock title = new TextBlock();
            title.FontSize = 24;
            title.TextWrapping = TextWrapping.Wrap;
            title.Text += res.SelectSingleNode(@"h1[1]/span[2]/span[2]/a[1]").InnerText;
            list.Add(title);
        }

        /// <summary>
        /// 添加日期
        /// </summary>
        /// <param name="res"></param>
        void LoadDate(HtmlNode res)
        {
            TextBlock date = new TextBlock();
            date.HorizontalAlignment = HorizontalAlignment.Center;
            date.FontSize = 16;
            date.Text += menu.date;
            date.Text += ": ";
            date.Text += res.SelectSingleNode(@"h1[1]/span[1]/span[1]").InnerText;
            date.Text += "/";
            date.Text += res.SelectSingleNode(@"h1[1]/span[1]/span[2]/span[1]").InnerText;
            date.Text += " ";
            date.Text += res.SelectSingleNode(@"h1[1]/span[1]/span[2]/span[2]").InnerText;
            list.Add(date);
        }

        /// <summary>
        /// 添加作者
        /// </summary>
        /// <param name="res"></param>
        void LoadAuthor(HtmlNode res)
        {
            TextBlock author = new TextBlock();
            author.HorizontalAlignment = HorizontalAlignment.Center;
            author.FontSize = 16;
            author.Text += res.SelectSingleNode(@"h1[1]/span[2]/span[1]").InnerText;
            list.Add(author);
        }

        /// <summary>
        /// 添加正文
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        async System.Threading.Tasks.Task LoadArticleAsync(HtmlNode res)
        {
            var tests = res.ChildNodes;
            for (int i = 0; i < tests.Count; i ++)
            {
                /* 节点名为img时
                 * 添加Image到ListView中
                 */
                if (tests[i].Name.Equals("img"))
                {
                    string link;
                    Image temp = new Image();
                    var uri = tests[i].Attributes["src"].Value;
                    if (tests[i].ParentNode.ChildAttributes("href").Count() == 0)
                    {
                        link = uri;
                    }
                    else
                    {
                        link = tests[i].ParentNode.Attributes["href"].Value;
                        if (link.Contains("img1.php?id"))
                        {
                            HttpClient myWebClient = new HttpClient();
                            Stream myStream = await myWebClient.GetStreamAsync(link);
                            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                            string html = sr.ReadToEnd();
                            //加载源代码，获取文档对象
                            var doc = new HtmlDocument();
                            doc.LoadHtml(html);
                            //更加xpath获取总的对象，如果不为空，就继续选择dl标签
                            var img = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[1]/img[1]");
                            if (!img.Attributes["src"].Value.Equals("/img/expired.gif"))
                            {
                                link = img.Attributes["src"].Value;
                            }
                        }
                    }
                    BitmapImage bitmapImage = new BitmapImage(new Uri(uri));
                    temp.Source = bitmapImage;
                    temp.MaxWidth = 300;
                    temp.MaxHeight = 300;
                    temp.HorizontalAlignment = HorizontalAlignment.Left;
                    temp.AccessKey = link;
                    list.Add(temp);
                }
                /* 节点名不是img时
                 * 作为文本添加
                 */
                else if (!tests[i].HasChildNodes)
                {
                    if (tests[i].Name.Equals("br"))
                    {
                        if (i != 0 && !tests[i - 1].Name.Equals("br")) continue;
                    }
                    TextBlock temp = new TextBlock();
                    temp.FontSize = 16;
                    temp.TextWrapping = TextWrapping.Wrap;
                    temp.Text = tests[i].InnerText;
                    while (temp.Text.Contains("&nbsp;"))
                    {
                        temp.Text = temp.Text.Replace("&nbsp;", " ");
                    }
                    list.Add(temp);
                }
                /* 当html有嵌套时
                 * 递归读取其子节点
                 */
                else
                {
                    await LoadArticleAsync(tests[i]);
                }
            }
            /*
            var test = res.SelectSingleNode(@"div[3]");
            var tests = test.ChildNodes;
            for (int i = 0; i < tests.Count; i ++)
            {
                if (tests[i].HasChildNodes)
                {
                    if (tests[i].LastChild.HasChildNodes)
                    {
                        string link;
                        Image temp = new Image();
                        link = tests[i].LastChild.Attributes["href"].Value;
                        var att = tests[i].LastChild.FirstChild.Attributes["src"].Value;
                        BitmapImage bitmapImage = new BitmapImage(new Uri(att));
                        temp.Source = bitmapImage;
                        temp.MaxWidth = 300;
                        temp.MaxHeight = 300;
                        temp.HorizontalAlignment = HorizontalAlignment.Left;
                        list.Add(temp);
                        temp.AccessKey = link;
                    }
                    else
                    {
                        TextBlock temp = new TextBlock();
                        temp.FontSize = 20;
                        temp.TextWrapping = TextWrapping.Wrap;
                        temp.Text = tests[i].FirstChild.InnerText;
                        while (temp.Text.Contains("&nbsp;"))
                        {
                            temp.Text = temp.Text.Replace("&nbsp;", " ");
                        }
                        list.Add(temp);
                    }
                }
                else
                {
                    TextBlock temp = new TextBlock();
                    temp.FontSize = 20;
                    temp.Text = tests[i].InnerText;
                    list.Add(temp);
                }
            }
            */
        }

        /// <summary>
        /// 点击博客数据
        /// 复制文本或打开原图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 判断节点类型
            TextBlock control = e.ClickedItem as TextBlock;
            if (control != null)
            {
                // 复制文本
                TextBlock temp = e.ClickedItem as TextBlock;
                DataPackage dp = new DataPackage();
                dp.SetText(temp.Text);
                Clipboard.SetContent(dp);
            }
            else
            {
                // 打开浏览器查看图源
                try
                {
                    Image image = e.ClickedItem as Image;
                    string templink = image.AccessKey.Replace("img2.php?sec_key", "img1.php?id");
                    await Launcher.LaunchUriAsync(new Uri(templink));
                    // await Launcher.LaunchUriAsync(new Uri(image.AccessKey));

                    /*HttpClient myWebClient = new HttpClient();
                    Stream myStream = await myWebClient.GetStreamAsync(image.AccessKey);
                    StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                    string html = sr.ReadToEnd();
                    //加载源代码，获取文档对象
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    //更加xpath获取总的对象，如果不为空，就继续选择dl标签
                    var img = doc.DocumentNode.SelectSingleNode(@"/html/body/img");
                    if (img != null)
                    {
                        byte[] temp = await myWebClient.GetByteArrayAsync(image.AccessKey);

                        var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                        folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                        folderPicker.FileTypeFilter.Add("*");

                        Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();

                        StorageFile file = await folder.CreateFileAsync("pic.jpg");
                        await FileIO.WriteBytesAsync(file, temp);
                    }
                    else await Launcher.LaunchUriAsync(new Uri(image.AccessKey));*/

                }
                catch(Exception ee)
                {
                    string message = ee.Message;
                    var dialog = new MessageDialog(message, "Error");

                    dialog.Commands.Add(new UICommand(menu.OK, cmd => { }, menu.OK));
                    await dialog.ShowAsync();
                    return;
                }
            }
        }

        /* 
         * year/month:      /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/h1[1]/span[1]/span[1]/#text[1]
         * day:             /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/h1[1]/span[1]/span[2]/span[1]/#text[1]
         * author:          /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/h1[1]/span[2]/span[1]/#text[1]
         * title:           /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/h1[1]/span[2]/span[2]/a[1]
         * article:         /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]
         * first sentence:  /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/#text[1]
         * second sentence: /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[XX]/#text[1]
         * first image:     /html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[3]/div[1]/div[1]/div[3]/div[XX]/a[1]/img[1]/@src[1]
         */
                }
}
