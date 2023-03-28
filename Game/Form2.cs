using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form2 : Form
    {
        Form1.User formUser = new Form1.User(); 
        public Form2(Form1.User user)
        {
            InitializeComponent();
            label1.Text = "Good to see you, " + user.Name + "!";    //"Ενημερώνω" τα κατάλληλα labels
            label5.Text = user.Highscore1.ToString();
            label6.Text = user.Highscore2.ToString();
            label7.Text = user.Highscore3.ToString();
            label9.Text = user.Email;
            label11.Text = user.Country;
            formUser = user;    //Για να ξέρω ποιος χρήστης συνδέθηκε
        }

        private void Form2_Leave(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            Application.Exit(); //Για να κλείσει και την πρώτη φόρμα
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)       //Ανάλογα με την δυσκολία καλείται ο constructor της Form3 (διαφορετικό πρώτο όρισμα)
            {
                this.Hide();
                Form3 form3 = new Form3(1,formUser);
                form3.Show();
            }
            else if (radioButton2.Checked)
            {
                this.Hide();
                Form3 form3 = new Form3(2,formUser);
                form3.Show();
            }
            else
            {
                this.Hide();
                Form3 form3 = new Form3(3,formUser);
                form3.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HowToPlay howToPlay = new HowToPlay();  //Οδηγίες
            howToPlay.Show();
        }
    }
}
