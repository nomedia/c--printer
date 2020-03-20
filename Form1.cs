using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.IO;

using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestTSPL
{
 
 
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// 下载网络图片

        ///

        /// 网络地址

        /// 本地保存路径

        public void DownloadImage(string url, string path)

        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ServicePoint.Expect100Continue = false;

            req.Method = "GET";

            req.KeepAlive = true;

            req.ContentType = "image/*";

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            System.IO.Stream stream = null;

            try

            {

                // 以字符流的方式读取HTTP响应

                stream = rsp.GetResponseStream();

                Image.FromStream(stream).Save(path);

            }

            finally

            {

                // 释放资源

                if (stream != null) stream.Close();

                if (rsp != null) rsp.Close();

            }

        }



        ///
        /// Get请求
        /// 
        /// 
        /// 字符串
        public static string GetHttpResponse(string url, int Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        //实例化一个timer  
 System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public int stared=0;

        public int printed = 0;
        private void StopTimeBtn_Click(object sender, EventArgs e)//停止计时  
        {
            //计时开始  
       myTimer.Stop();
        }
        //回调函数  
        private void Callback(object sender, EventArgs e)
        {
            //获取系统时间 20:16:16  
          label1.Text ="时间："+ DateTime.Now.ToLongTimeString().ToString();

           startPrinter();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (stared == 0) {
                this.button1.Text = "关闭";
                stared = 1;
                //给timer挂起事件  
                myTimer.Tick += new EventHandler(Callback);
            //使timer可用  
            myTimer.Enabled = true;
            //设置时间间隔，以毫秒为单位  
            myTimer.Interval = 1000;//1s  
            }
            else
            {
                myTimer.Stop();
                this.button1.Text = "开始";
                stared = 0;
            }

        }
        private void startPrinter()
        {
            string json = GetHttpResponse("http://superpao.aiweber.com/api/printer", 30000);
           // MessageBox.Show( rs);


            JavaScriptSerializer js = new JavaScriptSerializer();
          //  Person[] persons = js.Deserialize<Person[]>(json);


            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                Console.WriteLine("{0} {1} {2} {3}\n", item.fee, item.payed, item.created_at, item.id, item.id);
                //   printer(item.send_address.nickname,item.receive_address.nickname,item.receive_address.sname,item.num_big,item.num_small,item.fee, item.fee,item.created_at,item.id,item.id);

                string ji = item.send_address.nickname;
                string mdd = item.receive_address.city_name;
                string da = item.num_big;
                string xiao = item.num_small;
                string yun = item.fee;
                string wei = item.fee;
                string time = item.created_at;
                string code = item.id;
                string dang = item.id;

                string shou = item.receive_address.nickname;


                printer(ji,shou,mdd,da,xiao,yun,wei,time,code,dang);


            }
            //  MessageBox.Show(str.ToString(), "TestTSPL", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            return;

        }

       private void printer(string ji,string shou,string mdd,string da,string xiao,string yun,string wei,string time,string code,string dang)

          
        {


            printed++;

            int result = 0;
            byte [] msg=new byte [512];
            tspl_dll dll = new tspl_dll();

            result=tspl_dll.PrinterCreator( ref dll.printer,"R42");

            if (0 == result)
            {
                if ((result = tspl_dll.PortOpen(dll.printer, "USB")) == 0)
                {
                    int s = 0;
                    result = tspl_dll.TSPL_GetPrinterStatus(dll.printer, ref s);

                    if (9 == s)
                    {
                        MessageBox.Show("head opened and out of ribbon", "TestTSPL", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                        return;
                    }
                    else if (12 == s)
                    {
                        MessageBox.Show("Out of ribbon and out of paper", "TestTSPL", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                        return;
                    }
                    else if (32 == s)
                    {
                     //   MessageBox.Show("Printing", "TestTSPL", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                      //  return;
                    }
                    //打印机自测  如果需要请先去掉注释
              //     tspl_dll.TSPL_SelfTest(dll.printer);

                    tspl_dll.TSPL_ClearBuffer(dll.printer);
                    tspl_dll.TSPL_Setup(dll.printer, 100, 60, 4,   10, 1,1, 0);
                  //  result = tspl_dll.TSPL_BitMap(dll.printer, 0, 0, 0, @"md\1000004975.jpg");

                    //TEST
                    //打印汉字实例  字体应选择9
                    Encoding gbk = Encoding.GetEncoding("GBK");
                   
             
                    byte[] data = gbk.GetBytes("畅翔(秀新)托运部");
           
                    result = tspl_dll.TSPL_Text(dll.printer, 20, 430, 8, 3, 2,2, data);

                    data = gbk.GetBytes("地址：温州紧固件市场1排45号");

                    result = tspl_dll.TSPL_Text(dll.printer, 80, 430, 9, 3, 1,1, data);
                    data = gbk.GetBytes("手机微信：18958861800");

                    result = tspl_dll.TSPL_Text(dll.printer, 110, 430, 9, 3, 1, 1, data);
                    data = gbk.GetBytes("电话：0577-86529984 86528109");

                    result = tspl_dll.TSPL_Text(dll.printer, 140, 430, 9, 3, 1, 1, data);

                   result = tspl_dll.TSPL_Reverse(dll.printer, 180, 0, 5,500);



                    data = gbk.GetBytes("寄："+ji);

                    result = tspl_dll.TSPL_Text(dll.printer, 210, 430, 9, 3, 2, 2, data);

                    data = gbk.GetBytes("收："+shou);

                    result = tspl_dll.TSPL_Text(dll.printer, 270, 430, 9, 3, 2, 2, data);

                    data = gbk.GetBytes("目地："+mdd);

                    result = tspl_dll.TSPL_Text(dll.printer, 330, 430, 9, 3, 2, 2, data);

                    data = gbk.GetBytes("大件:"+da);

                    result = tspl_dll.TSPL_Text(dll.printer, 390, 430, 9, 3, 2, 2, data);


                    data = gbk.GetBytes("小件:" + xiao);

                    result = tspl_dll.TSPL_Text(dll.printer, 390, 230, 9, 3, 2, 2, data);


                    data = gbk.GetBytes("运费："+xiao);

                    result = tspl_dll.TSPL_Text(dll.printer, 450, 430, 9, 3, 2, 2, data);


                    string status = "未付";

                    if (wei == "1")
                    {
                        status = "已付";

                    }

                    data = gbk.GetBytes(status);

                    result = tspl_dll.TSPL_Text(dll.printer, 450, 230, 9, 3, 2, 2, data);




                    //    result = tspl_dll.TSPL_Text(dll.printer, 200, 350, 9, 3, 1, 1, data);
                    //QRCODE

                    string url = "http://superpao.aiweber.com/record/"+code;
                    result = tspl_dll.TSPL_QrCode(dll.printer, 530, 150, 3,5, 0, 5, 0, 5, "\""+ url + "\"");

                    data = gbk.GetBytes("单号："+dang);

                    result = tspl_dll.TSPL_Text(dll.printer, 730, 430, 9, 3, 1, 1, data);

                    data = gbk.GetBytes(time);

                    result = tspl_dll.TSPL_Text(dll.printer, 760, 430, 9, 3, 1, 1, data);
                    //BITMAP
                    //    result = tspl_dll.TSPL_BitMap(dll.printer, 0, 0, 0, @"md\1000004975.jpg");
                    //  

                    //DirectIO 使用示例
                    //byte[] str=new byte[]{0x53,0x45,0x4c,0x46,0x54,0x45,0x53,0x54,0x0d,0x0a};
                    //result = tspl_dll.DirectIO(dll.printer, str, 10, str, 0,ref result);

                    result = tspl_dll.TSPL_Print(dll.printer, 1, 1);

                    result = tspl_dll.PortClose(dll.printer);
                    result = tspl_dll.PrinterDestroy(dll.printer);

                }
                else
                {
                    result = tspl_dll.FormatError(result, 1, msg, 0, 512);
                    MessageBox.Show(ASCIIEncoding.Default.GetString(msg));
                }
            }
            else
            {
                result = tspl_dll.FormatError(result, 1, msg, 0, 512);
                MessageBox.Show(ASCIIEncoding.Default.GetString(msg));
            }
        }

        public class tspl_dll
        {
            [DllImport("TSPL_SDK")]
            public static extern int FormatError(int error_no, int langid, byte[] buf, int pos, int bufSize);
            [DllImport("TSPL_SDK")]
            public static extern int PrinterCreator(ref IntPtr printer, string model);

            [DllImport("TSPL_SDK")]
            public static extern IntPtr PrinterCreatorS(string model);

            [DllImport("TSPL_SDK")]
            public static extern int PortOpen(IntPtr printer, string portSetting);

            [DllImport("TSPL_SDK")]
            public static extern int PortClose(IntPtr printer);

            [DllImport("TSPL_SDK")]
            public static extern int PrinterDestroy(IntPtr printer);
            [DllImport("TSPL_SDK")]
            public static extern int DirectIO(IntPtr printer, byte[] writeData, int writenum, byte[] readData, int readNum, ref int readedNum);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_SelfTest(IntPtr printer);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_BitMap(IntPtr printer, int xPos, int yPos, int mode, string fileName);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Setup(IntPtr printer, int labelWidth, int labelHeight, int speed, int density, int type, int gap, int offset);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_ClearBuffer(IntPtr printer);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_BarCode(IntPtr printer, int xPos, int yPos, int codeType, int height, int readable, int rotation, int narrow, int wide, string data);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_QrCode(IntPtr printer, int xPos, int yPos, int eccLevel, int width, int mode, int rotation, int model, int mask, string data);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Text(IntPtr printer, int xPos, int yPos, int font, int rotation, int xMultiplication, int yMultiplication, byte[] data);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_FormFeed(IntPtr printer);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Box(IntPtr printer, int x_start, int y_start, int x_end, int y_end, int thinckness);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_SetTear(IntPtr printer, int on);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_SetRibbon(IntPtr printer, int ribbon);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Offset(IntPtr printer, int distance);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Direction(IntPtr printer, int direction);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Feed(IntPtr printer, int len, int xPos, int yPos);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Home(IntPtr printer);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_Print(IntPtr printer, int num, int copies);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_GetPrinterStatus(IntPtr printer, ref int status);

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_SetCodePage(IntPtr printer, int codepage);

			[DllImport("TSPL_SDK")]
            public static extern int TSPL_Reverse(IntPtr printer, int xPos, int yPos, int width, int height );

            [DllImport("TSPL_SDK")]
            public static extern int TSPL_GapDetect(IntPtr printer, int paperLength, int gapLength);


            public IntPtr printer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
