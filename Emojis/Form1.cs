using Emojis.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using WindowsInput.Native;
using WindowsInput;
using System.Net;
using System.IO;

namespace Emojis
{
    public partial class Form1 : Form
    {

        public static List<string> _DevicesName = new List<string>();
        private RawInput _rawinput;
        private string Chrome="",Firefox="";

        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        
      
        
        public Form1()
        {
            InitializeComponent();
            Notify.Visible = false;


            //-----------------------

            _rawinput = new RawInput(Handle, false);

            _rawinput.AddMessageFilter();   // Adding a message filter will cause keypresses to be handled
            Win32.DeviceAudit();            // Writes a file DeviceAudit.txt to the current directory

            _rawinput.KeyPressed += OnKeyPressed;

            //-----------------------
            

        }
        

       // private int cpt=1;


        public event KeyEventHandler KeyUp;
        KeyEventArgs kArg = new KeyEventArgs(Keys.Delete);
        private  void OnKeyPressed(object sender, RawInputEventArg e)
        {

            try { 
           if (e.KeyPressEvent.VKeyName == "BACK")
                return;
            if (Settings.Default.DeviceName == "")
                return;
            

            if (e.KeyPressEvent.KeyPressState.Equals("BREAK"))
            {
                //cpt = 0;
                
                //Check the Device name first !!!!
                if (!e.KeyPressEvent.Name.Contains(Settings.Default.DeviceName) && !Settings.Default.DeviceName.Contains(e.KeyPressEvent.Name))
                 return;
                String ActiveProcess =  ActiveApp.getActiveProccess();
                InputSimulator cmd = new InputSimulator();
                cmd.Keyboard.KeyPress(VirtualKeyCode.BACK);
                Console.WriteLine(e.KeyPressEvent.VKeyName);
                switch (ActiveProcess)
                {
                    case "chrome":
                            String UrlChrome;
                            lock (Chrome)
                            {
                                UrlChrome = Chrome;
                            }

                            if (UrlChrome.Trim() == "")
                            return;
                        if (UrlChrome.Contains("facebook") || UrlChrome.Contains("messenger"))
                            if(Emoji.FB(e.KeyPressEvent.VKeyName)!=null)
                                cmd.Keyboard.TextEntry(Emoji.FB(e.KeyPressEvent.VKeyName));
                        if (UrlChrome.Contains("twitter"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Twitter(e.KeyPressEvent.VKeyName));
                        if (UrlChrome.Contains("instagram"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Instagram(e.KeyPressEvent.VKeyName));
                        if (UrlChrome.Contains("mail.google"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Gmail(e.KeyPressEvent.VKeyName));
                        if (UrlChrome.Contains("outlook"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Outlook(e.KeyPressEvent.VKeyName));
                        

                        
                        break;
                    case "firefox":
                            // Console.WriteLine("=================FIREFOX=============== ");
                            //int ID0 = ActiveApp.getActiveProcessID();
                            String UrlFirefox;
                            lock (Firefox)
                            {
                                UrlFirefox = Firefox;
                            }
                            
                            //Console.WriteLine("URl : "+UrlFirefox);
                        if (UrlFirefox.Trim() == "")
                            return;
                        if (UrlFirefox.Contains("facebook") || UrlFirefox.Contains("messenger"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.FB(e.KeyPressEvent.VKeyName));
                        if (UrlFirefox.Contains("twitter"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Twitter(e.KeyPressEvent.VKeyName));
                        if (UrlFirefox.Contains("instagram"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Instagram(e.KeyPressEvent.VKeyName));
                        if (UrlFirefox.Contains("mail.google"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Gmail(e.KeyPressEvent.VKeyName));
                        if (UrlFirefox.Contains("outlook"))
                            if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                                cmd.Keyboard.TextEntry(Emoji.Outlook(e.KeyPressEvent.VKeyName));
                        
                        break;
                   /* case "snapchat":
                        if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                            cmd.Keyboard.TextEntry(Emoji.Snapchat(e.KeyPressEvent.VKeyName));
                        break;
                    */
                    case "whatsapp":
                        if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                            cmd.Keyboard.TextEntry(Emoji.Whatsapp(e.KeyPressEvent.VKeyName));
                        break;
                    case "Telegram":
                        if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                            cmd.Keyboard.TextEntry(Emoji.Telegram(e.KeyPressEvent.VKeyName));
                        break;
                    case "outlook":
                        if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                            cmd.Keyboard.TextEntry(Emoji.Outlook(e.KeyPressEvent.VKeyName));
                        break;
                    case "Skype":
                        if (Emoji.FB(e.KeyPressEvent.VKeyName) != null)
                            cmd.Keyboard.TextEntry(Emoji.Skype(e.KeyPressEvent.VKeyName));
                        break;
                   

                    

                }
                Console.WriteLine(ActiveProcess);
               // Console.WriteLine(API.getChromeUrl());
               // Console.WriteLine(e.KeyPressEvent.VKeyName);

                Thread.Sleep(333);

            }


                // _rawinput = new RawInput(Handle, false);
            }
            catch (Exception z)
            {
                Debug.Fail(z.Message);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            int cpt=0;
            SelectedDevice.AddItem("Select your device...");
            if (Settings.Default.DeviceName == "")  
                SelectedDevice.selectedIndex = 0;

            
            foreach (String Device in _DevicesName)
            {
                cpt++;
                if (Device.Equals(Settings.Default.DeviceName))
                {
                    SelectedDevice.AddItem(Device);
                    SelectedDevice.selectedIndex = cpt;
                    
                } else
                SelectedDevice.AddItem(Device);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (SelectedDevice.selectedIndex == 0)
                MessageBox.Show("Choose a device, Please.","No Device",MessageBoxButtons.OK,MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Your device has been saved, Thank you.", "Device Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Settings.Default.DeviceName = SelectedDevice.selectedValue;
                Settings.Default.Save();
                
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            SelectedDevice.Clear();
            SelectedDevice.AddItem("Select your device...");
            SelectedDevice.selectedIndex = 0;
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rawinput.DestroyHandle();
        }
        
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            this.Hide();
            Notify.Visible = true;
        }

        private void Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Notify.Visible = false;
            this.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static HttpListener _listener;

        
        private async void timer1_Tick(object sender, EventArgs e)
        {
            try { 
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:60024/");
            _listener.Start();
            
            HttpListenerContext context = await  _listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            
            //Answer getCommand/get post data/do whatever
            string response;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                response = reader.ReadToEnd();
            }
               // Console.WriteLine(response);
                if (!response.StartsWith("#"))
                    Chrome = response;
                else
                    Firefox = response;


                _listener.Close();
            
            }
            catch (Exception z) { }
            finally { _listener.Close(); GC.Collect(); }
        }
        
    }
}
