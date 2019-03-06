using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace NogiFan
{
    public class MemberViewModel
    {
        private ObservableCollection<Models.Member> allItems = new ObservableCollection<Models.Member>();
        private ObservableCollection<Models.Member> oshi = new ObservableCollection<Models.Member>();

        public ObservableCollection<Models.Member> getItems { get { return this.allItems; } }
        public ObservableCollection<Models.Member> getoshi { get { return this.oshi; } }

        /// <summary>
        /// 添加喜欢的成员
        /// </summary>
        /// <param name="name"></param>
        public void AddOshi(string name)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].name.Equals(name))
                {
                    oshi.Add(allItems[i]);
                    TileUpdate((Application.Current as App).ViewModel.getoshi.Count - 1);
                    (Application.Current as App).conn.Insert(allItems[i]);
                    return;
                }
            }
        }
        /// <summary>
        /// 从数据库中添加喜欢的成员
        /// </summary>
        /// <param name="name"></param>
        public void Re(string name)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].name.Equals(name))
                {
                    oshi.Add(allItems[i]);
                    TileUpdate((Application.Current as App).ViewModel.getoshi.Count - 1);
                    return;
                }
            }
        }
        /// <summary>
        /// 移除喜欢的成员
        /// </summary>
        /// <param name="name"></param>
        public void RemoveOshi(string name)
        {

            for (int i = 0; i < oshi.Count; i ++)
            {
                if (oshi[i].name.Equals(name))
                {
                    oshi.RemoveAt(i);
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                    for (int j = 0; j < (Application.Current as App).ViewModel.getoshi.Count; j++)
                    {
                        TileUpdate(j);
                    }
                    (Application.Current as App).conn.Execute("delete from Member where name = ?", name);
                    return;
                }
            }
        }

        /// <summary>
        /// 语言转换
        /// </summary>
        public void Tran()
        {
            for (int i = 0; i < oshi.Count; i ++)
            {
                oshi[i].toBlog = (Application.Current as App).menu.toBlog;
                oshi[i].toProfile = (Application.Current as App).menu.toProfile;
            }
        }

        /// <summary>
        /// 磁贴更新
        /// </summary>
        /// <param name="i"></param>
        static public void TileUpdate(int i)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(System.IO.File.ReadAllText("Tile.xml"));
            XmlNodeList textElements = document.GetElementsByTagName("text");
            XmlNodeList image = document.GetElementsByTagName("image");
            int count = (Application.Current as App).ViewModel.getoshi.Count;
            if (count == 0) return;
            textElements[0].InnerText = (Application.Current as App).ViewModel.getoshi[i].name;
            textElements[1].InnerText = (Application.Current as App).ViewModel.getoshi[i].eng;
            textElements[2].InnerText = (Application.Current as App).ViewModel.getoshi[i].name;
            textElements[3].InnerText = (Application.Current as App).ViewModel.getoshi[i].eng;
            textElements[4].InnerText = (Application.Current as App).ViewModel.getoshi[i].name;
            textElements[5].InnerText = (Application.Current as App).ViewModel.getoshi[i].eng;
            ((XmlElement)image[0]).SetAttribute("src", (Application.Current as App).ViewModel.getoshi[i].image);
            ((XmlElement)image[1]).SetAttribute("src", (Application.Current as App).ViewModel.getoshi[i].image);
            ((XmlElement)image[2]).SetAttribute("src", (Application.Current as App).ViewModel.getoshi[i].image);
            var tileNotification = new TileNotification(document);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
        }

        /// <summary>
        /// 构造函数
        /// 添加成员列表
        /// </summary>
        public MemberViewModel()
        {

            allItems.Add(new Models.Member("秋元 真夏", "あきもと まなつ", "1993年8月20日",
                "http://img.nogizaka46.com/www/member/img/akimotomanatsu_prof.jpg",
                "http://blog.nogizaka46.com/manatsu.akimoto/",
                "http://www.nogizaka46.com/member/detail/akimotomanatsu.php"));
            allItems.Add(new Models.Member("生田 絵梨花", "いくた えりか", "1997年1月22日",
                "http://img.nogizaka46.com/www/member/img/ikutaerika_prof.jpg",
                "http://blog.nogizaka46.com/erika.ikuta/",
                "http://www.nogizaka46.com/member/detail/ikutaerika.php"));
            allItems.Add(new Models.Member("伊藤 かりん", "いとう かりん", "1993年5月26日",
                "http://img.nogizaka46.com/www/member/img/itoukarin_prof.jpg",
                "http://blog.nogizaka46.com/karin.itou/",
                "http://www.nogizaka46.com/member/detail/itoukarin.php"));
            allItems.Add(new Models.Member("伊藤 純奈", "いとう じゅんな", "1998年11月30日",
                "http://img.nogizaka46.com/www/member/img/itoujunna_prof.jpg",
                "http://blog.nogizaka46.com/junna.itou/",
                "http://www.nogizaka46.com/member/detail/itoujunna.php"));
            allItems.Add(new Models.Member("井上 小百合", "いのうえ さゆり", "1994年12月14日",
                "http://img.nogizaka46.com/www/member/img/inouesayuri_prof.jpg",
                "http://blog.nogizaka46.com/sayuri.inoue/",
                "http://www.nogizaka46.com/member/detail/inouesayuri.php"));
            allItems.Add(new Models.Member("衛藤 美彩", "えとう みさ", "1993年1月4日",
                "http://img.nogizaka46.com/www/member/img/etoumisa_prof.jpg",
                "http://blog.nogizaka46.com/misa.eto/",
                "http://www.nogizaka46.com/member/detail/etoumisa.php"));
            allItems.Add(new Models.Member("川後 陽菜", "かわご ひな", "1998年3月22日",
                "http://img.nogizaka46.com/www/member/img/kawagohina_prof.jpg",
                "http://blog.nogizaka46.com/hina.kawago/",
                "http://www.nogizaka46.com/member/detail/kawagohina.php"));
            allItems.Add(new Models.Member("北野 日奈子", "きたの ひなこ", "1996年7月17日",
                "http://img.nogizaka46.com/www/member/img/kitanohinako_prof.jpg",
                "http://blog.nogizaka46.com/hinako.kitano/",
                "http://www.nogizaka46.com/member/detail/kitanohinako.php"));
            allItems.Add(new Models.Member("齋藤 飛鳥", "さいとう あすか", "1998年8月10日",
                "http://img.nogizaka46.com/www/member/img/saitouasuka_prof.jpg",
                "http://blog.nogizaka46.com/asuka.saito/",
                "http://www.nogizaka46.com/member/detail/saitouasuka.php"));
            allItems.Add(new Models.Member("斎藤 ちはる", "さいとう ちはる", "1997年2月17日",
                "http://img.nogizaka46.com/www/member/img/saitouchiharu_prof.jpg",
                "http://blog.nogizaka46.com/chiharu.saito/",
                "http://www.nogizaka46.com/member/detail/saitouchiharu.php"));
            allItems.Add(new Models.Member("斉藤 優里", "さいとう ゆうり", "1993年7月20日",
                "http://img.nogizaka46.com/www/member/img/saitouyuuri_prof.jpg",
                "http://blog.nogizaka46.com/yuuri.saito/",
                "http://www.nogizaka46.com/member/detail/saitouyuuri.php"));
            allItems.Add(new Models.Member("相楽 伊織", "さがら いおり", "1997年11月26日",
                "http://img.nogizaka46.com/www/member/img/sagaraiori_prof.jpg",
                "http://blog.nogizaka46.com/iori.sagara/",
                "http://www.nogizaka46.com/member/detail/sagaraiori.php"));
            allItems.Add(new Models.Member("桜井 玲香", "さくらい れいか", "1994年5月16日",
                "http://img.nogizaka46.com/www/member/img/sakuraireika_prof.jpg",
                "http://blog.nogizaka46.com/reika.sakurai/",
                "http://www.nogizaka46.com/member/detail/sakuraireika.php"));
            allItems.Add(new Models.Member("佐々木 琴子", "ささき ことこ", "1998年8月28日",
                "http://img.nogizaka46.com/www/member/img/sasakikotoko_prof.jpg",
                "http://blog.nogizaka46.com/kotoko.sasaki/",
                "http://www.nogizaka46.com/member/detail/sasakikotoko.php"));
            allItems.Add(new Models.Member("白石 麻衣", "しらいし まい", "1992年8月20日",
                "http://img.nogizaka46.com/www/member/img/shiraishimai_prof.jpg",
                "http://blog.nogizaka46.com/mai.shiraishi/",
                "http://www.nogizaka46.com/member/detail/shiraishimai.php"));
            allItems.Add(new Models.Member("新内 眞衣", "しんうち まい", "1992年1月22日",
                "http://img.nogizaka46.com/www/member/img/shinuchimai_prof.jpg",
                "http://blog.nogizaka46.com/mai.shinuchi/",
                "http://www.nogizaka46.com/member/detail/shinuchimai.php"));
            allItems.Add(new Models.Member("鈴木 絢音", "すずき あやね", "1999年3月5日",
                "http://img.nogizaka46.com/www/member/img/suzukiayane_prof.jpg",
                "http://blog.nogizaka46.com/ayane.suzuki/",
                "http://www.nogizaka46.com/member/detail/suzukiayane.php"));
            allItems.Add(new Models.Member("高山 一実", "たかやま かずみ", "1994年2月8日",
                "http://img.nogizaka46.com/www/member/img/takayamakazumi_prof.jpg",
                "http://blog.nogizaka46.com/kazumi.takayama/",
                "http://www.nogizaka46.com/member/detail/takayamakazumi.php"));
            allItems.Add(new Models.Member("寺田 蘭世", "てらだ らんぜ", "1998年9月23日",
                "http://img.nogizaka46.com/www/member/img/teradaranze_prof.jpg",
                "http://blog.nogizaka46.com/ranze.terada/",
                "http://www.nogizaka46.com/member/detail/teradaranze.php"));
            allItems.Add(new Models.Member("中田 花奈", "なかだ かな", "1994年8月6日",
                "http://img.nogizaka46.com/www/member/img/nakadakana_prof.jpg",
                "http://blog.nogizaka46.com/kana.nakada/",
                "http://www.nogizaka46.com/member/detail/nakadakana.php"));
            allItems.Add(new Models.Member("西野 七瀬", "にしの ななせ", "1994年5月25日",
                "http://img.nogizaka46.com/www/member/img/nishinonanase_prof.jpg",
                "http://blog.nogizaka46.com/nanase.nishino/",
                "http://www.nogizaka46.com/member/detail/nishinonanase.php"));
            allItems.Add(new Models.Member("能條 愛未", "のうじょう あみ", "1994年10月18日",
                "http://img.nogizaka46.com/www/member/img/noujouami_prof.jpg",
                "http://blog.nogizaka46.com/ami.noujo/",
                "http://www.nogizaka46.com/member/detail/noujouami.php"));
            allItems.Add(new Models.Member("樋口 日奈", "ひぐち ひな", "1998年1月31日",
                "http://img.nogizaka46.com/www/member/img/higuchihina_prof.jpg",
                "http://blog.nogizaka46.com/hina.higuchi/",
                "http://www.nogizaka46.com/member/detail/higuchihina.php"));
            allItems.Add(new Models.Member("星野 みなみ", "ほしの みなみ", "1998年2月6日",
                "http://img.nogizaka46.com/www/member/img/hoshinominami_prof.jpg",
                "http://blog.nogizaka46.com/minami.hoshino/",
                "http://www.nogizaka46.com/member/detail/hoshinominami.php"));
            allItems.Add(new Models.Member("堀 未央奈", "ほり みおな", "1996年10月15日",
                "http://img.nogizaka46.com/www/member/img/horimiona_prof.jpg",
                "http://blog.nogizaka46.com/miona.hori/",
                "http://www.nogizaka46.com/member/detail/horimiona.php"));
            allItems.Add(new Models.Member("松村 沙友理", "まつむら さゆり", "1992年8月27日",
                "http://img.nogizaka46.com/www/member/img/matsumurasayuri_prof.jpg",
                "http://blog.nogizaka46.com/sayuri.matsumura/",
                "http://www.nogizaka46.com/member/detail/matsumurasayuri.php"));
            allItems.Add(new Models.Member("山崎 怜奈", "やまざき れな", "1997年5月21日",
                "http://img.nogizaka46.com/www/member/img/yamazakirena_prof.jpg",
                "http://blog.nogizaka46.com/rena.yamazaki/",
                "http://www.nogizaka46.com/member/detail/yamazakirena.php"));
            allItems.Add(new Models.Member("若月 佑美", "わかつき ゆみ", "1994年6月27日",
                "http://img.nogizaka46.com/www/member/img/wakatsukiyumi_prof.jpg",
                "http://blog.nogizaka46.com/yumi.wakatsuki/",
                "http://www.nogizaka46.com/member/detail/wakatsukiyumi.php"));
            allItems.Add(new Models.Member("渡辺 みり愛", "わたなべ みりあ", "1999年11月1日",
                "http://img.nogizaka46.com/www/member/img/watanabemiria_prof.jpg",
                "http://blog.nogizaka46.com/miria.watanabe/",
                "http://www.nogizaka46.com/member/detail/watanabemiria.php"));
            allItems.Add(new Models.Member("和田 まあや", "わだ まあや", "1998年4月23日",
                "http://img.nogizaka46.com/www/member/img/wadamaaya_prof.jpg",
                "http://blog.nogizaka46.com/maaya.wada/",
                "http://www.nogizaka46.com/member/detail/wadamaaya.php"));
            allItems.Add(new Models.Member("伊藤 理々杏", "いとう りりあ", "2002年10月8日",
                "http://img.nogizaka46.com/www/member/img/itouriria_prof.jpg",
                "http://blog.nogizaka46.com/riria.itou/",
                "http://www.nogizaka46.com/member/detail/itouriria.php"));
            allItems.Add(new Models.Member("岩本 蓮加", "いわもと れんか", "2004年2月2日",
                "http://img.nogizaka46.com/www/member/img/iwamotorenka_prof.jpg",
                "http://blog.nogizaka46.com/renka.iwamoto/",
                "http://www.nogizaka46.com/member/detail/iwamotorenka.php"));
            allItems.Add(new Models.Member("梅澤 美波", "うめざわ みなみ", "1999年1月6日",
                "http://img.nogizaka46.com/www/member/img/umezawaminami_prof.jpg",
                "http://blog.nogizaka46.com/minami.umezawa/",
                "http://www.nogizaka46.com/member/detail/umezawaminami.php"));
            allItems.Add(new Models.Member("大園 桃子", "おおぞの ももこ", "1999年9月13日",
                "http://img.nogizaka46.com/www/member/img/oozonomomoko_prof.jpg",
                "http://blog.nogizaka46.com/momoko.oozono/",
                "http://www.nogizaka46.com/member/detail/oozonomomoko.php"));
            allItems.Add(new Models.Member("久保 史緒里", "くぼ しおり", "2001年7月14日",
                "http://img.nogizaka46.com/www/member/img/kuboshiori_prof.jpg",
                "http://blog.nogizaka46.com/shiori.kubo/",
                "http://www.nogizaka46.com/member/detail/kuboshiori.php"));
            allItems.Add(new Models.Member("阪口 珠美", "さかぐち たまみ", "2001年11月10日",
                "http://img.nogizaka46.com/www/member/img/sakaguchitamami_prof.jpg",
                "http://blog.nogizaka46.com/tamami.sakaguchi/",
                "http://www.nogizaka46.com/member/detail/sakaguchitamami.php"));
            allItems.Add(new Models.Member("佐藤 楓", "さとう かえで", "1998年3月23日",
                "http://img.nogizaka46.com/www/member/img/satoukaede_prof.jpg",
                "http://blog.nogizaka46.com/kaede.satou/",
                "http://www.nogizaka46.com/member/detail/satoukaede.php"));
            allItems.Add(new Models.Member("中村 麗乃", "なかむら れの", "2001年9月27日",
                "http://img.nogizaka46.com/www/member/img/nakamurareno_prof.jpg",
                "http://blog.nogizaka46.com/reno.nakamura/",
                "http://www.nogizaka46.com/member/detail/nakamurareno.php"));
            allItems.Add(new Models.Member("向井 葉月", "むかい はづき", "1999年8月23日",
                "http://img.nogizaka46.com/www/member/img/mukaihazuki_prof.jpg",
                "http://blog.nogizaka46.com/hazuki.mukai/",
                "http://www.nogizaka46.com/member/detail/mukaihazuki.php"));
            allItems.Add(new Models.Member("山下 美月", "やました みづき", "1999年7月26日",
                "http://img.nogizaka46.com/www/member/img/yamashitamizuki_prof.jpg",
                "http://blog.nogizaka46.com/mizuki.yamashita/",
                "http://www.nogizaka46.com/member/detail/yamashitamizuki.php"));
            allItems.Add(new Models.Member("吉田 綾乃クリスティー", "よしだ あやのくりすてぃー", "1995年9月6日",
                "http://img.nogizaka46.com/www/member/img/yoshidaayanochristie_prof.jpg",
                "http://blog.nogizaka46.com/ayanochristie.yoshida/",
                "http://www.nogizaka46.com/member/detail/yoshidaayanochristie.php"));
            allItems.Add(new Models.Member("与田 祐希", "よだ ゆうき", "2000年5月5日",
                "http://img.nogizaka46.com/www/member/img/yodayuuki_prof.jpg",
                "http://blog.nogizaka46.com/yuuki.yoda/",
                "http://www.nogizaka46.com/member/detail/yodayuuki.php"));
        }

        
    }
}
