using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ExchangePage : Page
    {
        string a = "JPY";
        string b = "RATE";
        string c = "CNY";
        string Message = "Rate Loading...";
        double DoubleRate;
        string t3;
        Models.Menu menu;

        public ExchangePage()
        {
            menu = (Application.Current as App).menu;
            this.InitializeComponent();
            B.Text = "0.0600";
            queryAsync();
        }

        /// <summary>
        /// 读取API获取xml
        /// 解析xml
        /// 将xml所需内容（汇率）保存
        /// </summary>
        async void queryAsync()
        {
           
            try
            {
                HttpClient client = new HttpClient();
                String result = await client.GetStringAsync("http://api.k780.com/?app=finance.rate_cnyquot&curno=JPY&bankno=BOC&appkey=33648&sign=3a615f9a79dedf64ec2874658b06b267&format=xml");
                XmlDocument document = new XmlDocument();
                document.LoadXml(result);
                XmlNodeList list = document.GetElementsByTagName("success");
                if (list[0].InnerText.Equals("1"))
                {
                    // API返回成功
                    list = document.GetElementsByTagName("middle");
                    string rate = list[0].InnerText;
                    DoubleRate = double.Parse(rate);
                    DoubleRate = DoubleRate / 100;
                    if (a.Equals("CNY"))
                    {
                        double temp = 1 / DoubleRate;
                        B.Text = "" + temp;
                    }
                    else
                    {
                        B.Text = "" + DoubleRate;
                    }
                    Message = "";
                    msg.Text = "";
                }
                else
                {
                    // API返回失败
                    list = document.GetElementsByTagName("msg");
                    DoubleRate = 0.0600;
                    Message = menu.API + list[0].InnerText;
                    B.Text = "" + DoubleRate;
                    msg.Text = Message;
                }
            }
            
            catch(Exception ee)
            {
                var dialog = new MessageDialog(ee.Message, "Error");

                dialog.Commands.Add(new UICommand(menu.OK, cmd => { }, commandId: 0));

                await dialog.ShowAsync();
            }
        }

        /// <summary>
        /// 交换中日货币输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exchange_Click(object sender, RoutedEventArgs e)
        {
            if (a.Equals("JPY"))
            {
                B.Text = "" + 1 / DoubleRate;
                a = "CNY";
                c = "JPY";
            }
            else
            {
                B.Text = "" + DoubleRate;
                a = "JPY";
                c = "CNY";
            }
            tb1.Text = a;
            tb2.Text = c;
        }

        /// <summary>
        /// 计算兑换结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            string t1 = A.Text;
            string t2 = B.Text;
            bool flag = false;
            if (t1.Length == 0 || t2.Length == 0)
            {
                Message = "Error Input";
                msg.Text = Message;
                return;
            }
            for (int i = 0; i < t1.Length; i ++)
            {
                if (t1[i] == '.' && flag == false)
                {
                    flag = true;
                    continue;
                }
                if (t1[i] == '.' && flag == true || t1[i] < '0' || t1[i] > '9')
                {
                    Message = "Error Input!";
                    msg.Text = Message;
                    return;
                }
            }
            flag = false;
            for (int i = 0; i < t2.Length; i++)
            {
                if (t2[i] == '.' && flag == false)
                {
                    flag = true;
                    continue;
                }
                if (t2[i] == '.' && flag == true || t2[i] < '0' || t2[i] > '9')
                {
                    Message = "Error Input!";
                    msg.Text = Message;
                    return;
                }
            }
            t3 = "" + double.Parse(t1) * double.Parse(t2);
            C.Text = t3;
        }
    }
}
