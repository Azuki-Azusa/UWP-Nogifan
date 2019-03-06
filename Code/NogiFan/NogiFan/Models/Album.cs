using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NogiFan.Models
{
    public class Album
    {
        public string num;
        public string name;
        public string path;
        public List<song> songs;
        public Album(string Num, string n, string p, List<song> list)
        {
            num = Num;
            name = n;
            path = p;
            songs = list;
        }
    }
    public class song
    {
        public string name;
        public string link;
        public string image;
        public song(string n, string l, string i)
        {
            name = n;
            link = l;
            image = i;
        }
    }
}
