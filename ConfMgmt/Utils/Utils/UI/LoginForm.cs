using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class LoginForm : Form
    {
        private Dictionary<string, string> _userTable;
        public LoginForm()
        {
            InitializeComponent();
            _userTable = new Dictionary<string, string>
            {
                ["admin"] = "abc@123",
                ["gary.zhang"] = "abc@123",
                ["arthur.liang"] = "abc@123",
                ["bo.jiang"] = "abc@123",
                ["hui.chen"] = "abc@123"
            };

        }

        public bool LoginResult { private set; get; }
        public string UserId {set; get; }
        public string UserPassword {set; get; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Please Ensure Fill In the ID or Password!");
                return;
            }
            if (_userTable.ContainsKey(textBox1.Text) && _userTable[textBox1.Text] == textBox2.Text)
            {
                UserId = textBox1.Text;
                UserPassword = textBox2.Text;
                LoginResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(@"user ID is not exist or password wrong!");
                return;
            }

           
         
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserId = "";
            UserPassword = "";
            LoginResult = false;
            Close();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = UserId;
            textBox2.Text = UserPassword;
        }
    }
}
