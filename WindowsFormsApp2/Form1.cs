using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using KAutoHelper;
using System.Threading;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string deviceID = string.Empty; //Nhập deviceID của thiết bị
            Thread th = new Thread(async() =>
            {
                try
                {
                    Bitmap bitmap = ADBHelper.ScreenShoot(deviceID, false, "Captcha.png"); //Hình ảnh captcha được chụp từ thiết bị
                    string result = BitmaptoBase64.BitmapToBase64(bitmap);//Chuyển định dạng của ảnh sang mã hóa Base64
                    string apiKey = textBox1.Text;//Api Key
                    string imageBase64 = result;//Đoạn mã base64 đã được vonvert
                    string typeJobId = "24";//Loại Job (tiktok captcha là loại kéo thả mang mã 24 tại web omocaptcha, vào web tham khảo api để biết rõ hơn)
                    string createJobResponse = await CaptchaApiClient.CreateCaptchaJob(apiKey, imageBase64, typeJobId);//Tạo job gửi đến trang wweb
                    int jobId = 0;
                    if (createJobResponse.Contains("\"job_id\""))
                    {
                        var responseObj = JObject.Parse(createJobResponse);
                        jobId = responseObj["job_id"].Value<int>();
                    }
                    string resultToado = await CaptchaApiClient.GetCaptchaJobResult(apiKey, jobId);//Tọa độ được trang web trả về
                    JObject responseObj1 = JObject.Parse(resultToado);
                    string toado = responseObj1["result"].ToString();
                    string[] coordinates = toado.Split('|');
                    //Tách chuỗi tọa độ
                    string x1 = coordinates[0];
                    string y1 = coordinates[1];
                    string x2 = coordinates[2];
                    string y2 = coordinates[3];
                    //Ép kiểu tọa độ sang dạng int
                    int x1Int, y1Int, x2Int, y2Int;
                    bool successX1 = int.TryParse(x1, out x1Int);
                    bool successY1 = int.TryParse(y1, out y1Int);
                    bool successX2 = int.TryParse(x2, out x2Int);
                    bool successY2 = int.TryParse(y2, out y2Int);
                    //Tiến hành kéo
                    if (successX1 && successY1 && successX2 && successY2)
                    {
                        ADBHelper.Swipe(deviceID, x1Int, y1Int, x2Int, y2Int, 2500);
                    }
                    else
                    {
                        MessageBox.Show("Invalid number format");
                    }
                }
                catch
                {
                    MessageBox.Show("Lỗi Khi Giải Captcha!");
                }
            });
            th.Start();
        }
    }
}
