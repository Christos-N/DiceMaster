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
    public partial class Leaderboard : Form
    {
        public Leaderboard(StringBuilder sb)    //Παίρνει ως όρισμα ένα StringBuilder (τα ονόματα με τα αντίστοιχα highscores)
        {
            InitializeComponent();
            label1.Text = sb.ToString();        //Και ενημερώνει το label ώστε να τα δει ο χρήστης
        }
    }
}
