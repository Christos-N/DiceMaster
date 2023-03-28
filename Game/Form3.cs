using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form3 : Form
    {
        Form1.User formUser = new Form1.User();
        List<Form1.User> users = new List<Form1.User>();
        IFormatter formatter = new BinaryFormatter();
        Random random;
        int countdown = 16; //Στην πραγματικότητα είναι 15 δευτερόλεπτα
        int randomImage;    //Ο ακέραιος που θα "διαλέξει" την εικόνα(ζάρι)
        int sum;            //Σκορ
        int lev;            //δυσκολία(level)
        int max = 0;        //Για την εύρεση του μεγίστου
        string name;        //Για να ενημέρωσω το όνομα του χρήστη που έκανε top score
        int count = 0;      //Για να ελέγξω πόσες φορές κλίκαρε στο panel (για τα επίπεδα 2,3)
        public Form3(int level, Form1.User user)
        {
            InitializeComponent();
            Stream stream = new FileStream("users.txt", FileMode.Open, FileAccess.Read);
            users = (List<Form1.User>)formatter.Deserialize(stream);
            stream.Close();
            switch (level)
            {
                case 1:
                    label1.Text = "Level 1 (Beginner)";
                    label5.Text = user.Highscore1.ToString();
                    max = users.First().Highscore1;
                    foreach (Form1.User us in users)    //Εύρεση του μεγαλύτερου highscore (level 1)
                    {
                        if (us.Highscore1 >= max)
                        {
                            max = us.Highscore1;
                            name = us.Name;
                        }
                    }
                    break;
                case 2:
                    label1.Text = "Level 2 (Intermediate)";
                    label5.Text = user.Highscore2.ToString();
                    foreach (Form1.User us in users)    //Εύρεση του μεγαλύτερου highscore (level 2)
                    {
                        if (us.Highscore2 > max)
                        {
                            max = us.Highscore2;
                            name = us.Name;
                        }
                    }
                    break;
                case 3:
                    countdown = 31; //Το χρονόμετρο έχει interval 500ms οπότε διπλασιάζω το countdown(τα πραγματικά δευτερόλεπτα που μένουν στον χρήστη) και προσθέτω το 1 για να προλάβει ο χρήστης να πατήσει το πρώτο ζάρι
                    label1.Text = "Level 3 (Legend)";
                    label5.Text = user.Highscore3.ToString();
                    foreach (Form1.User us in users)    //Εύρεση του μεγαλύτερου highscore (level 3)
                    {
                        if (us.Highscore3 > max)
                        {
                            max = us.Highscore3;
                            name = us.Name;
                        }
                    }
                    break;
            }
            label11.Text = name;            //Ενημερώνω τα labels
            label14.Text = max.ToString();
            lev = level;
            label9.Text = user.Name;
            if (lev == 1 || lev == 2)
                label7.Text = (countdown - 1).ToString();//15   Έχουν "διαφορετικό" countdown επειδή το timer του level 3 έχει 500ms interval
            else label7.Text = (countdown/2).ToString();//15    Και στις 3 περιπτώσεις πρέπει να δείξει 15 στον χρόνο που απομένει στον χρήστη   
            formUser = user;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            random = new Random();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            label10.Visible = false;
            pictureBox1.Enabled = true;
            switch (lev)    //Ανάλογα την περίπτωση ενεργοποιώ το κατάλληλο timer
            {
                case 1:
                    if (timer1.Enabled)
                        timer1.Enabled = false;
                    else
                        timer1.Enabled = true;
                    break;
                case 2:
                    if (timer2.Enabled)
                        timer2.Enabled = false;
                    else
                        timer2.Enabled = true;
                    break;
                case 3:
                    if (timer3.Enabled)
                        timer3.Enabled = false;
                    else
                        timer3.Enabled = true;
                    break;
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sum += randomImage;
            label3.Text = sum.ToString();
            if (lev == 3 && pictureBox1.Width>=50)  //Στο level 3 μειώνω το μέγεθος του ζαριού τις πρώτες 2 φορές που θα το κλικάρει
            {
                pictureBox1.Width /= 2;
                pictureBox1.Height /= 2;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)    //level 1
        {
            randomImage = random.Next(1, 7);        //Τυχαίος 1 έως 6
            pictureBox1.ImageLocation = "images/" + randomImage.ToString() + ".png";    //Οι φωτογραφίες έχουν ονομαστεί 1.png, 2.png κλπ
            int x1, y1;
            x1 = random.Next(panel1.Width - pictureBox1.Width);     //Για να μην βγει εκτός ορίου
            y1 = random.Next(panel1.Height - pictureBox1.Height);   //>>
            pictureBox1.Location = new Point(x1, y1);
            countdown--;
            label7.Text = countdown.ToString();
            if (countdown == 0)     //Τέλος του γύρου
            {
                randomImage = 0;    //Αν θέλει να ξαναπαίξει ο χρήστης, στο πρώτο κλικ(πριν ξεκινήσει ο χρόνος) δεν πρέπει να προστίθενται πόντοι
                timer1.Enabled = false;
                pictureBox1.Enabled = false;
                if (sum > formUser.Highscore1)  //Έλεγχος για προσωπικό highscore
                {
                    MessageBox.Show("Congratulations, you just made a new highscore!");
                    foreach (Form1.User user in users)  //Έλεγχος για top score
                    {
                        if (user.Name == formUser.Name)
                        {
                            user.Highscore1 = sum;
                            formUser = user;
                        }
                    }
                    label5.Text = formUser.Highscore1.ToString();
                    if (formUser.Highscore1 > max) {    //Αν έκανε top score, αλλάζω τα αντίστοιχα label για top user, top score
                        label14.Text = sum.ToString();
                        label11.Text = formUser.Name;
                    }
                    Stream stream = new FileStream("users.txt", FileMode.Open, FileAccess.Write);
                    formatter.Serialize(stream, users);
                    stream.Close();
                }

                label10.Text = "   Score: " + sum.ToString() + Environment.NewLine + "PLAY AGAIN!";   //Το START! το μετατρέπω σε PLAY AGAIN! γράφοντας και το σκορ 
                label10.Location = new Point(350, 244); //Να φαίνεται στο κέντρο
                label10.Visible = true; //Το εμφανίζω
                pictureBox1.Location = new Point(20, 20);   //Τοποθετώ το ζάρι πάνω αριστερά του panel
                countdown = 16; //Ξανά
                sum = 0;    //από
                label3.Text = sum.ToString();   //την αρχή
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = false; //Σταματάω τα χρονόμετρα για να μην αποθηκευτεί το σκορ
            timer2.Enabled = false;
            timer3.Enabled = false;
            this.Hide();
            Form2 form2 = new Form2(formUser);
            form2.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            randomImage = random.Next(1, 7);        //Τυχαίος 1 έως 6
            pictureBox1.ImageLocation = "images/" + randomImage.ToString() + ".png";    //Οι φωτογραφίες έχουν ονομαστεί 1.png, 2.png κλπ
            int x1, y1;
            x1 = random.Next(panel1.Width - pictureBox1.Width);     //Για να μην βγει εκτός ορίου
            y1 = random.Next(panel1.Height - pictureBox1.Height);   //>>
            pictureBox1.Location = new Point(x1, y1);
            countdown--;
            label7.Text = countdown.ToString();
            switch (randomImage)    //Μικραίνω τις διαστάσεις των 4,5,6
            {
                case 4:
                    pictureBox1.Width = 50;
                    pictureBox1.Height = 50;
                    break;
                case 5:
                    pictureBox1.Width = 40;
                    pictureBox1.Height = 40;
                    break;
                case 6:
                    pictureBox1.Width = 30;
                    pictureBox1.Height = 30;
                    break;
                default:
                    pictureBox1.Width = 60;
                    pictureBox1.Height = 60;
                    break;
            }
            if (countdown == 0) //τέλος γύρου
            {
                randomImage = 0;    //Αν θέλει να ξαναπαίξει ο χρήστης, στο πρώτο κλικ(πριν ξεκινήσει ο χρόνος) δεν πρέπει να προστίθενται πόντοι
                timer2.Enabled = false;
                pictureBox1.Enabled = false;
                if (count == 0)     //Αν δεν έχει κλικάρει στο panel
                    sum *= 2;       //Διπλασιάζω το σκορ του
                if (sum > formUser.Highscore2)  //Έλεγχος για προσωπικό highscore
                {
                    MessageBox.Show("Congratulations, you just made a new highscore!");
                    foreach (Form1.User user in users)  //Έλεγχος για top score
                    {
                        if (user.Name == formUser.Name)
                        {
                            user.Highscore2 = sum;
                            formUser = user;
                        }
                    }
                    label5.Text = formUser.Highscore2.ToString();
                    if (formUser.Highscore2 > max)
                    {    //Αν έκανε top score, αλλάζω τα αντίστοιχα label για top user, top score
                        label14.Text = sum.ToString();
                        label11.Text = formUser.Name;
                    }
                    Stream stream = new FileStream("users.txt", FileMode.Open, FileAccess.Write);
                    formatter.Serialize(stream, users);
                    stream.Close();
                }

                label10.Text = "   Score: " + sum.ToString() + Environment.NewLine + "PLAY AGAIN!";   //Το START! το μετατρέπω σε PLAY AGAIN! γράφοντας και το σκορ 
                label10.Location = new Point(350, 244); //Να φαίνεται στο κέντρο
                label10.Visible = true; //Το εμφανίζω
                pictureBox1.Location = new Point(20, 20);   //Τοποθετώ το ζάρι πάνω αριστερά του panel
                countdown = 16; //Ξανά
                sum = 0;    //από
                count = 0;  //την
                label3.Text = sum.ToString();   //αρχή
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            randomImage = random.Next(1, 7);        //Τυχαίος 1 έως 6
            pictureBox1.ImageLocation = "images/" + randomImage.ToString() + ".png";    //Οι φωτογραφίες έχουν ονομαστεί 1.png, 2.png κλπ
            int x1, y1;
            x1 = random.Next(panel1.Width - pictureBox1.Width);     //Για να μην βγει εκτός ορίου
            y1 = random.Next(panel1.Height - pictureBox1.Height);   //>>
            pictureBox1.Location = new Point(x1, y1);
            countdown--;
            label7.Text = (countdown/2).ToString(); //To countdown αλλάζει ανά μισό δευτερόλεπτο
            if (countdown == 0)
            {
                randomImage = 0;    //Αν θέλει να ξαναπαίξει ο χρήστης, στο πρώτο κλικ(πριν ξεκινήσει ο χρόνος) δεν πρέπει να προστίθενται πόντοι
                timer3.Enabled = false;
                pictureBox1.Enabled = false;
                if (count == 0)     //Αν δεν έχει κλικάρει στο panel
                    sum *= 2;       //Διπλασιάζω το σκορ του
                if (sum > formUser.Highscore3)  //Έλεγχος για προσωπικό highscore
                {
                    MessageBox.Show("Congratulations, you just made a new highscore!");
                    foreach (Form1.User user in users)
                    {
                        if (user.Name == formUser.Name) //Έλεγχος για top score
                        {
                            user.Highscore3 = sum;
                            formUser = user;
                        }
                    }
                    label5.Text = formUser.Highscore3.ToString();
                    if (formUser.Highscore3 > max)
                    {    //Αν έκανε top score, αλλάζω τα αντίστοιχα label για top user, top score
                        label14.Text = sum.ToString();
                        label11.Text = formUser.Name;
                    }
                    Stream stream = new FileStream("users.txt", FileMode.Open, FileAccess.Write);
                    formatter.Serialize(stream, users);
                    stream.Close();
                }

                label10.Text = "   Score: " + sum.ToString() + Environment.NewLine + "PLAY AGAIN!";   //Το START! το μετατρέπω σε PLAY AGAIN! γράφοντας και το σκορ
                label10.Location = new Point(350, 244); //Να φαίνεται στο κέντρο
                label10.Visible = true; //Το εμφανίζω
                pictureBox1.Location = new Point(20, 20);   //Τοποθετώ το ζάρι πάνω αριστερά του panel
                countdown = 31; //Ξανά
                sum = 0;    //από
                count = 0;  //την
                label3.Text = sum.ToString();   //αρχή
                pictureBox1.Width = 100;
                pictureBox1.Height = 100;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if ((lev == 2 || lev == 3)) //Όποτε ο χρήστης κλικάρει στο panel του αφαιρώ το νούμερο του ζαριού εκτός αν βγει αρνητικός αριθμός, τότε γίνεται 0 το σκορ
            {
                if (sum - randomImage < 0)
                    sum = 0;
                else
                    sum -= randomImage;
                label3.Text = sum.ToString();
                count++;    //Αυξάνω το count για να μην διπλασιαστεί το σκορ
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("");
            switch (lev)    //Κάνω sort ανάλογα το κάθε level και το αντίστοιχο highscore του κάθε χρήστη και γράφω τα αντίστοιχα username με τα highscores στο StringBuilder sb
            {
                case 1:
                    users = users.OrderByDescending(x => x.Highscore1).ToList();
                    foreach (var item in users)
                    {
                        sb.Append(item.Name + ": " + item.Highscore1.ToString() + Environment.NewLine);
                    }
                    break;
                case 2:
                    users = users.OrderByDescending(x => x.Highscore2).ToList();
                    foreach (var item in users)
                    {
                        sb.Append(item.Name + ": " + item.Highscore2.ToString() + Environment.NewLine);
                    }
                    break;
                case 3:
                    users = users.OrderByDescending(x => x.Highscore3).ToList();
                    foreach (var item in users)
                    {
                        sb.Append(item.Name + ": " + item.Highscore3.ToString() + Environment.NewLine);
                    }
                    break;
            }
            Leaderboard leaderboard = new Leaderboard(sb);
            leaderboard.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HowToPlay howToPlay = new HowToPlay();  //Οδηγίες
            howToPlay.Show();
        }
    }
}
