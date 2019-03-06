using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class MemberPage : Page
    {
        MemberViewModel ViewModel { get; set; }
        public MemberPage()
        {
            ViewModel = (Application.Current as App).ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示成员信息（浏览器）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.Member;
            webView.Navigate(new Uri(item.detail));
            list.Visibility = Visibility.Collapsed;
            webView.Visibility = Visibility.Visible;
        }
    }
}
