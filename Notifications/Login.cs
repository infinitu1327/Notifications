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

namespace Notifications
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VkApi vk = new VkApi();            

            try
            {
                vk.Authorize(new ApiAuthParams()
                {
                    Login = textBox1.Text,
                    Password = textBox2.Text,
                    ApplicationId = 5591430,
                    Settings = VkNet.Enums.Filters.Settings.Wall
                });

                new Form1(vk).Show();
                Hide();
            }
            catch (Exception)
            {
                label3.Text = "Неправильный логин или пароль";
            }
        }
    }
}
