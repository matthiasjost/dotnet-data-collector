using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Core
{
    public class Channel
    {
        public string name { get; set; } = "";
        public string url { get; set; } = "";
    }

    public class Feed
    {
        public string type { get; set; } = "";
        public string url { get; set; } = "";
    }

    public class CreatorCardDto
    {
        public string name { get; set; } = "";
        public string country { get; set; } = "";
        public string slogan { get; set; } = "";
        public string bio { get; set; } = "";
        public string tags { get; set; } = "";
        public Socials socials { get; set; } = new Socials();
        public List<Channel> channels { get; set; } = new List<Channel>();
        public List<Feed> feeds { get; set; } = new List<Feed>();

        public CreatorCardDto()
        {
        }
    }

    public class Socials
    {
        public string youtube { get; set; } = "";
        public string linkedin { get; set; } = "";
        public string twitter { get; set; } = "";
        public string github { get; set; } = "";
        public string mastodon { get; set; } = "";
    }
}
