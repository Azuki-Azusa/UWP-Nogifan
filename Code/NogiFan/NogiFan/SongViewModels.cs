using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;

namespace NogiFan
{
    public class SongViewModels
    {
        private ObservableCollection<Models.Album> allItems;
        public ObservableCollection<Models.Album> GetItems { get { return this.allItems; } }

        public SongViewModels()
        {
            allItems = new ObservableCollection<Models.Album>();
            Ini();
        }

        /// <summary>
        /// 读取xml文件
        /// 加载歌曲信息
        /// </summary>
        async private void Ini()
        {
            try
            {
                const int numOfSingle = 22;
                for (int i = numOfSingle; i > 14; i--)
                {
                    string filename = "Assets/Song/S" + i + ".xml";
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(System.IO.File.ReadAllText(filename));
                    XmlNodeList number = document.GetElementsByTagName("number");
                    XmlNodeList title = document.GetElementsByTagName("title");
                    XmlNodeList image = document.GetElementsByTagName("image");
                    XmlNodeList song = document.GetElementsByTagName("song");
                    XmlNodeList link = document.GetElementsByTagName("link");
                    List<Models.song> songs = new List<Models.song>();
                    for (int j = 0; j < song.Count; j++)
                    {
                        Models.song temp = new Models.song(song[j].InnerText, link[j].InnerText, image[0].InnerText);
                        songs.Add(temp);
                    }
                    Models.Album temp2 = new Models.Album(number[0].InnerText, title[0].InnerText, image[0].InnerText, songs);
                    allItems.Add(temp2);
                }
            }

            catch(Exception e)
            {
                var dialog = new MessageDialog(e.Message, "Error");

                dialog.Commands.Add(new UICommand("Yes", cmd => { }, commandId: 0));

                await dialog.ShowAsync();
            }
        }
    }
}
