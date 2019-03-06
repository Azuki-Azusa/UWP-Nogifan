using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NogiFan.Models
{
    public class Menu
    {
        public bool language;
        public string member;
        public string song;
        public string blog;
        public string oshimen;
        public string news;
        public string shop;
        public string add;
        public string remove;
        public string allBlog;
        public string toBlog;
        public string toProfile;
        public string message;
        public string OK;
        public string NO;
        public string tran;
        public string API;
        public string BU1;
        public string BU2;
        public string date;
        public string author;
        public string official;
        public string hand;
        public string place;
        public string sche;

        public Menu()
        {
            Chinese();
        }
        public void Change()
        {
            if (language)
            {
                Japanese();
            }
            else
            {
                Chinese();
            }
        }
        void Chinese()
        {
            member = "成员";
            song = "歌曲";
            blog = "博客";
            oshimen = "推";
            news = "汇率";
            shop = "WebShop";
            add = "添加";
            remove = "移除";
            allBlog = "全部";
            toBlog = "博客主页";
            toProfile = "成员介绍";
            message = "DD是犯罪，推不能超过三人！";
            OK = "确认";
            NO = "取消";
            tran = "日语";
            API = "调用API失败，原因是：";
            BU1 = "交换";
            BU2 = "计算";
            date = "日期";
            author = "作者";
            official = "官网";
            hand = "握手会";
            place = "会场";
            sche = "日程";
            language = true;
        }
        void Japanese()
        {
            member = "メンバー";
            song = "ソング";
            blog = "ブログ";
            oshimen = "推しメン";
            news = "為替レート";
            shop = "ウェブショップ";
            add = "追加";
            remove = "削除";
            allBlog = "すべて";
            toBlog = "ブログへ";
            toProfile = "紹介へ";
            message = "DDは犯罪です、三人以上はできません！";
            OK = "はい";
            NO = "いいえ";
            tran = "中国語";
            API = "APIの呼び出しに失敗しました、理由は：";
            BU1 = "交換";
            BU2 = "計算";
            date = "日付";
            author = "著者";
            official = "公式サイト";
            hand = "握手会";
            place = "会場";
            sche = "日程";
            language = false;
        }
    }
}
