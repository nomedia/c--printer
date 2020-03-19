using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace TestTSPL
{
    public partial class FrmClient : Form
    {
        public FrmClient()
        {
            InitializeComponent();
        }

        //定义回调
        private delegate void SetTextCallBack(string strValue);
        //声明
        private SetTextCallBack setCallBack;

        //定义接收服务端发送消息的回调
        private delegate void ReceiveMsgCallBack(string strMsg);
        //声明
        private ReceiveMsgCallBack receiveCallBack;

        //创建连接的Socket
        Socket socketSend;
        //创建接收客户端发送消息的线程
        Thread threadReceive;
        private String txt_IP;
        private Label label1;
        private String txt_Port;
        private Label txt_Log;
        private Button connect;
        private Label txt_Msg;
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connect_Click(object sender, EventArgs e)
        {
            try
            {

              //  IPHostEntry hostInfo = Dns.GetHostEntry("47.100.46.2");
           //     IPAddress ipAddress = hostInfo.AddressList[0];

             //   txt_IP = "superpao.aiweber.com/socket";
             //   txt_Port = "80";
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse("47.100.46.2");
                socketSend.Connect(ip, 2120);
                //实例化回调
                setCallBack = new SetTextCallBack(SetValue);
                receiveCallBack = new ReceiveMsgCallBack(SetValue);
                // this.Label.Invoke(setCallBack, "连接成功");

                this.label1.Text = "receive  successful"+ receiveCallBack.ToString();
            //    MessageBox.Show("lianjieco:" + setCallBack.ToString());
                //开启一个新的线程不停的接收服务器发送消息的线程
                threadReceive = new Thread(new ThreadStart(Receive));
                //设置为后台线程
                threadReceive.IsBackground = false;
                threadReceive.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接服务端出错:" + ex.ToString());
            }
        }

        /// <summary>
        /// 接口服务器发送的消息
        /// </summary>
        private void Receive()
        {

            byte[] buffer = new byte[2048];
            //实际接收到的字节数
            int r = socketSend.Receive(buffer);


            MessageBox.Show("ieshoduaoxinxi :" + r);


            //this.label1.Text="Receive start :";
            try
            {
                while (true)
                {
               //     byte[] buffer = new byte[2048];
                    //实际接收到的字节数
                //    int r = socketSend.Receive(buffer);


             //    MessageBox.Show("ieshoduaoxinxi :"+r);
                 //   this.label1.Text = "Lab3234234el";



                    if (r == 0)
                    {
                        break;
                    }
                    else
                    {
                        //判断发送的数据的类型
                        if (buffer[0] == 0)//表示发送的是文字消息
                        {
                            string str = Encoding.Default.GetString(buffer, 1, r - 1);
                            MessageBox.Show("接收服务端发送的消息出错:" + str);

                            this.txt_Log.Invoke(receiveCallBack, "接收远程服务器:" + socketSend.RemoteEndPoint + "发送的消息:" + str);
                        }
                       
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("接收服务端发送的消息出错:" + ex.ToString());
            }
        }


        private void SetValue(string strValue)
        {
          //  this.txt_Log.AppendText(strValue + "\r \n");
        }

        /// <summary>
        /// 客户端给服务器发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                string strMsg = this.txt_Msg.Text.Trim();
                byte[] buffer = new byte[2048];
                buffer = Encoding.Default.GetBytes(strMsg);
                int receive = socketSend.Send(buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送消息出错:" + ex.Message);
            }
        }

        private void FrmClient_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CloseConnect_Click(object sender, EventArgs e)
        {
            //关闭socket
            socketSend.Close();
            //终止线程
            threadReceive.Abort();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.connect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1dd";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(288, 94);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(83, 34);
            this.connect.TabIndex = 1;
            this.connect.Text = "button1";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // FrmClient
            // 
            this.ClientSize = new System.Drawing.Size(549, 465);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.label1);
            this.Name = "FrmClient";
            this.Load += new System.EventHandler(this.FrmClient_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FrmClient_Load_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}