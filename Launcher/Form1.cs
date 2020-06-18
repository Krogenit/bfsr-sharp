using BattleForSpaceResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Form1 : Form
    {
        private bool fullscreen = false;
        private int width = 0, height = 0;

        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void onStartClick(object sender, EventArgs e)
        {
            this.Close();
            Thread theThread = new Thread(StartGame);
            theThread.Start();
        }
        public void StartGame()
        {
            Core game = new Core(width, height, fullscreen);
            game.Run();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (fullscreen)
            {
                fullscreen = false;
            }
            else
            {
                fullscreen = true;
            }
        }
        private void SelectItem(object sender, EventArgs e)
        {
            String s = listBox1.SelectedItem.ToString();
            if (s != "Auto")
            {
                char[] c = s.ToCharArray();
                width = Int32.Parse(new String(c, 0, 4));
                Console.WriteLine(width);
                height = Int32.Parse(new String(c, 5, c.Length - 5));
                Console.WriteLine(height);
            }
            else
            {
                width = 0; height = 0;
            }
        }
    }
}
