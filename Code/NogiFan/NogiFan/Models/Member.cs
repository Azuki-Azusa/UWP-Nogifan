using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using SQLite.Net.Attributes;
using Windows.UI.Xaml;

namespace NogiFan.Models
{
    public class Member
    {
        
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [MaxLength(64)]
        public string name { get; set; }
        public string toBlog { get; set; }
        public string toProfile { get; set; }
        public string eng { get; set; }
        public string image { get; set; }
        public string birthday { get; set; }
        public string blogPath { get; set; }
        public string detail { get; set; }
        public Member()
        {
            toBlog = (Application.Current as App).menu.toBlog;
            toProfile = (Application.Current as App).menu.toProfile;
        }
        public void change()
        {
            toBlog = (Application.Current as App).menu.toBlog;
            toProfile = (Application.Current as App).menu.toProfile;
        }
        public Member(string n, string e, string bir, string img, string blog, string de)
        {
            toBlog = (Application.Current as App).menu.toBlog;
            toProfile = (Application.Current as App).menu.toProfile;
            name = n;
            eng = e;
            image = img;
            blogPath = blog;
            detail = de;
            birthday = bir;
        }
    }
}
