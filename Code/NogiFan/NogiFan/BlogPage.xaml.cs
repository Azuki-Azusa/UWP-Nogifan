using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace NogiFan
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlogPage : Page
    {
        Models.Menu menu;
        MemberViewModel ViewModel { get; set; }
        public BlogPage()
        {
            menu = (Application.Current as App).menu;
            ViewModel = (Application.Current as App).ViewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// 显示成员博客（浏览器）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.Member;
            webView.Navigate(new Uri(item.blogPath));
            list.Visibility = Visibility.Collapsed;
            webView.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示所有成员的博客（浏览器）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllBlog_Click(object sender, RoutedEventArgs e)
        {
            webView.Navigate(new Uri("http://blog.nogizaka46.com/"));
            list.Visibility = Visibility.Collapsed;
            webView.Visibility = Visibility.Visible;
        }
    }
}
