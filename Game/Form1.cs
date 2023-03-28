using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Game
{

    public partial class Form1 : Form
    {
        
        List<User> users = new List<User>();
        IFormatter formatter = new BinaryFormatter();   //Για το serialize, deserialize
        Stream stream;                                  //>>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                stream = new FileStream("users.txt", FileMode.OpenOrCreate, FileAccess.Read);   //Deserialize(αν γίνεται)
                users = (List<User>)formatter.Deserialize(stream);
            }catch (Exception) {}
            finally
            {
                stream.Close();
            }
            
        }
        [Serializable]
        public class User
        {
            public User() {}
            public User(string name,string email,string password, string country)
            {
                Name = name;
                Email = email;
                Password = password;
                Country = country;
                Highscore1 = 0;
                Highscore2 = 0;
                Highscore3 = 0;
            }

            public String Name { get; set; }
            public String Email { get; set; }
            public String Password { get; set; }
            public String Country { get; set; }
            public int Highscore1 { get; set; }
            public int Highscore2 { get; set; }
            public int Highscore3 { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;  //Άμα παραμείνει false θα γίνει η εγγραφή του χρήστη
            foreach  (User user in users)   //Ελέγχω αν υπάρχουν λογαριασμοί με το ίδιο username ή email
            {
                if (textBox3.Text == user.Name)
                {
                    MessageBox.Show("This username already exists.");
                    flag = true;
                    break;
                }
                if (textBox5.Text == user.Email)
                {
                    MessageBox.Show("There is already an account registered to this email.");
                    flag = true;
                    break;
                }
            }
            if (!flag && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                User newuser = new User(textBox3.Text, textBox5.Text, textBox4.Text,textBox6.Text); //Φτιάχνω το νέο χρήστη
                users.Add(newuser);     //και τον βάζω στη λίστα
                Stream stream = new FileStream("users.txt", FileMode.OpenOrCreate, FileAccess.Write);   //και μετά
                formatter.Serialize(stream, users);     //βάζω την ανανεωμένη λίστα στο αρχείο
                stream.Close();
                MessageBox.Show("You have succesfully created your account!");
                this.Hide();
                Form form2 = new Form2(newuser);
                form2.Show();
            }
            else if (!flag)
                MessageBox.Show("Please fill all fields.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;  //Για να ελέγξω αν υπάρχει συνδυασμός username-password και να εμφανίσω το κατάλληλο μήνυμα
            foreach (User user in users)
            {
                if (user.Name == textBox1.Text && user.Password == textBox2.Text)
                {
                    flag = true;
                    MessageBox.Show("Successful login!");
                    this.Hide();
                    Form form2 = new Form2(user);
                    form2.Show();
                    break;  
                }
            }
            if (!flag)
                MessageBox.Show("No account with this username/password combination was found.");
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)    //Με το χτύπημα του enter στο textbox1 "πατιέται" το Login
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)    //Με το χτύπημα του enter στο textbox2 "πατιέται" το Login
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)    //Με το χτύπημα του enter στο textbox3,4,5,6 "πατιέται" το Sign up
        {
            if (e.KeyCode == Keys.Enter)
                button2.PerformClick();
        }
    }
}
