using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    public sealed partial class OshimenPage : Page
    {
        ObservableCollection<string> name;
        ObservableCollection<string> name2;
        Models.Menu menu;
        MemberViewModel ViewModel { get; set; }
        public OshimenPage()
        {
            name2 = new ObservableCollection<string>();
            name = new ObservableCollection<string>();
            menu = (Application.Current as App).menu;
            ViewModel = (Application.Current as App).ViewModel;
            Initialization();
            this.InitializeComponent();
        }

        /// <summary>
        /// 更新ComboBox
        /// </summary>
        private void Initialization()
        {
            name.Clear();
            name2.Clear();
            for (int i = 0; i < ViewModel.getItems.Count; i ++)
            {
                name.Add(ViewModel.getItems[i].name);
            }
            for (int i = 0; i < ViewModel.getoshi.Count; i ++)
            {
                name2.Add(ViewModel.getoshi[i].name);
            }
            for (int i = 0; i < name.Count; i ++)
            {
                for (int j = 0; j < name2.Count; j ++)
                {
                    if (name[i].Equals(name2[j]))
                    {
                        name.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 点击博客主页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void blog_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            var item = (Models.Member)s.DataContext;
            await Launcher.LaunchUriAsync(new Uri(item.blogPath));
        }

        /// <summary>
        /// 点击成员主页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void detail_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            var item = (Models.Member)s.DataContext;
            await Launcher.LaunchUriAsync(new Uri(item.detail));
        }

        /// <summary>
        /// 添加喜欢的成员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.getoshi.Count == 3)
            {
                var dialog = new MessageDialog(menu.message, "Error");

                dialog.Commands.Add(new UICommand(menu.OK, cmd => { }, commandId: 0));
                var result = await dialog.ShowAsync();
                return;
            }
            ViewModel.AddOshi(addBox.SelectedItem.ToString());
            Initialization();
        }

        /// <summary>
        /// 移除喜欢的成员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.getoshi.Count == 0) return;
            ViewModel.RemoveOshi(removeBox.SelectedItem.ToString());
            Initialization();
        }

        /// <summary>
        /// 加载成员最新的博客
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Models.Member;
            this.Frame.Navigate(typeof(OneBlog), item.blogPath);
        }
    }
}
