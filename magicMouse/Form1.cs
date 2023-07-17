using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace magicMouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

    //
    //GLOBAL VARIABLES
    //
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        List<int[]> positionList = new List<int[]>();
        bool isOn = false;
        bool permission = false;
        bool taskDone = false;
        Thread thMimick;        
        
        int wholeDuration = 0;
        int delay = 0;
        int duration= 0;

    //
    //MOVE THE MOUSE FUNCTION
    //
        public void  mouseMove(int x,int y) 
        {
            Point newPositionPoint = new Point(x, y);
            Cursor.Position = newPositionPoint;
        }

    //
    //CLICK WITH THE MOUSE FUNCTION
    //
        public void mouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    
    //
    //MIMICK WHAT'S BEEN RECORDED FUNCTION
    //
        public void mimick()  
        {
            taskDone = false;
            if (positionList.Count > 0 && isOn == false)
            {    
                if (permission == true)
                {
                    double iterations = wholeDuration / duration / delay;
                    for (int i = 0; i < Math.Round(iterations); i++)
                    {
                        for (int jˇlistItem = 0; jˇlistItem < duration; jˇlistItem++)
                        {
             
                            if (permission == true)
                            {
                                Thread.Sleep(delay);
                                mouseMove(positionList[jˇlistItem][0], positionList[jˇlistItem][1]);
                                mouseClick();
                            }

                        }

                    }

                }
                
            }
            taskDone = true;
        }

    //
    //BUTTON REC
    //
        private void button1_Click(object sender, EventArgs e)
        {
            if (isOn == false)
            {
                isOn = true;
                button2.BackColor = Color.Coral;
                label3.Text = "1. Hit NumPad0 at desired locations";
            }
            else if (isOn == true)
            {
                isOn = false;
                button2.BackColor = Color.White;
                label3.Text = "1. Click on 'Record'";
            }
          
        }
  
    //
    //BUTTON START
    //
        private void button3_Click(object sender, EventArgs e)
        {
            permission = true;

            try
            {
                wholeDuration = Convert.ToInt32(textBox1.Text) * 1000; //textbox1
                if (wholeDuration < 10000) { textBox1.Text = "Must be >10"; }
            }
            catch (Exception)
            {
                permission = false;
                textBox1.Text = "Invalid input";
                
            }

            duration = positionList.Count;

            try
            {
                double temp = Math.Ceiling(Convert.ToDouble(textBox2.Text) * 1000);
                delay = (int)temp; //textbox2
                if (delay <= 0) { textBox2.Text = "Must be >0"; }
            }
            catch (Exception)
            {
                permission = false;
                textBox2.Text = "Invalid input";
            }

            thMimick = new Thread(mimick);//.Start()
            thMimick.Start();
            abortListener();
        }

    //   
    //COLLECTING POSITIONS while focusing 'Record'
    //
        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isOn == true && e.KeyCode == Keys.NumPad0)
            {
                if (positionList.Count==0)
                {
                    button4.BackColor = Color.MistyRose;
                }
                //vmelyikButton.PerformClick(); but we perform a chunk of code instead
                int currentX = Cursor.Position.X;
                int currentY = Cursor.Position.Y;

                int[] coordinates = new int[2];
                coordinates[0] = currentX;
                coordinates[1] = currentY;
                positionList.Add(coordinates);
            }
         
        }

    //
    //ABORT MIMICKING ON GLOBAL KEYDOWN
    //
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 num);

        public void abortListener()
        {         
            while (true)
            {   
                Thread.Sleep(5);
                
                int keyState = GetAsyncKeyState(83);
                if (keyState == 32769)
                {
                    permission = false;
                    break;
                }
                if (taskDone == true)
                {
                    break;
                }
            }
        }

    //
    //BUTTON RESET MEMORY
    //
        private void button4_Click(object sender, EventArgs e)
        {
            positionList.Clear();
            button4.BackColor = Color.White;
        }

    //
    //REMOVE TEXT EVENT
    //
        public void removeText(object sender, EventArgs e)
        {
            if (textBox1.Focused == true) {
                
                if (textBox1.Text == "Invalid input" || textBox1.Text == "Must be >10") { textBox1.Text = ""; }      
            }

            if (textBox2.Focused == true) { 
            
                if (textBox2.Text == "Invalid input" || textBox2.Text == "Must be >0") { textBox2.Text = ""; }  
            }
                
        }


    }//class ends

}//namespace ends
