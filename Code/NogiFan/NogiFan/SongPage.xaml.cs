using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class SongPage : Page
    {
        SongViewModels SongList;
        public SongPage()
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            SongList = (Application.Current as App).SongList;
            this.InitializeComponent();
        }

        /// <summary>
        /// 宽屏右侧显示歌曲列表
        /// 窄屏跳转新页面显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void List_ItemClick(object sender, ItemClickEventArgs e)
        {
            Models.Album item = e.ClickedItem as Models.Album;
            if (Window.Current.Bounds.Width < 800)
            {
                (Application.Current as App).navi = true;
                this.Frame.Navigate(typeof(SongListPage), item.songs);
            }
            else
            {
                (Application.Current as App).navi = false;
                Songs.Navigate(typeof(SongListPage), item.songs);
            }
        }

        /// <summary>
        /// 导航返回 回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame rootFrame = this.Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                (Application.Current as App).navi = false;
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
    }
}
