using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Notifications
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        VkApi Vk;
        VkNet.Model.WallGetObject wallPosts;
        long? OwnerId;

        public Form1(VkApi vk)
        {
            Vk = vk;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Timer t = new Timer()
            {
                Interval = 5000
            };
            t.Tick += T_Tick;

            StreamReader fileResponseStream = new StreamReader(
                HttpWebRequest
                .CreateHttp($"https://api.vk.com/method/groups.getById?" +
                $"group_id={textBox1.Text.Split('/').Last()}&" +
                $"v=5.63")
                .GetResponseAsync()
                .Result
                .GetResponseStream()
                );

            JObject fileResponseJSON = JObject.Parse(fileResponseStream.ReadToEnd());
            OwnerId = long.Parse(fileResponseJSON["response"][0]["id"].ToString());

            wallPosts = Vk.Wall.Get(new VkNet.Model.RequestParams.WallGetParams()
            {
                Count = 2,
                OwnerId = -OwnerId
            });
            t.Start();
            label3.Visible = true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            var res = Vk.Wall.Get(new VkNet.Model.RequestParams.WallGetParams()
            {
                OwnerId=-OwnerId,
                Count = 2
            });           
            
            if (res.WallPosts[1].Date != wallPosts.WallPosts[1].Date)
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("notidicationsender@gmail.com", "we34n8xza"),
                    EnableSsl = true
                };
                client.Send("notidicationsender@gmail.com", textBox2.Text, "Новый пост в группе " + textBox1.Text, $"Ссылка на пост: https://vk.com/{textBox1.Text.Split('/').Last()}?w=wall" + res.WallPosts[0].ToString().Substring(4));
                wallPosts = res;
            }            
        }
    }
}
