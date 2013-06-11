using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace DavidLoginSystem.Common
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public class CheckCodeUtil
    {
        private static readonly string[] fonts = new string[] { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
        private static readonly Color[] colors = new Color[] { Color.Black, Color.Blue, Color.Brown, Color.Red, Color.Green, Color.Pink, Color.Orange };
        private static readonly char[] elements = new char[]{'0','1','2','3','4','5','6','7','8','9','A','B','D','E','F','G','H','I','J','C','K'
            ,'L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

        public static CheckCode GenerateNewCode()
        {
            CheckCode checkCode = new CheckCode();
            Random rd = new Random();
            //生成验证码
            for (int i = 0; i < 4; i++)
            {
                char temp = elements[rd.Next(elements.Length)];
                checkCode.Code += temp;
            }

            Bitmap bitMap = new Bitmap(100, 30);
            using (Graphics g = Graphics.FromImage(bitMap))
            {
                g.Clear(Color.White);
                //画噪线
                for (int i = 0; i < 4; i++)
                {
                    using (Pen pen = new Pen(colors[rd.Next(colors.Length)]))
                    {
                        g.DrawLine(pen, rd.Next(100), rd.Next(30), rd.Next(100), rd.Next(30));
                    }
                }

                //画字符
                for (int i = 0; i < checkCode.Code.Length; i++)
                {
                    using (Font font = new Font(fonts[rd.Next(fonts.Length)], 16))
                    {
                        using (Brush brush = new SolidBrush(colors[rd.Next(colors.Length)]))
                        {
                            //X，Y参数表示左上角的坐标点位置
                            g.DrawString(checkCode.Code[i].ToString(), font, brush, i * 20 + 5, 6);
                        }
                    }
                }

                //画噪点
                for (int i = 0; i < 50; i++)
                {
                    bitMap.SetPixel(rd.Next(bitMap.Width), rd.Next(bitMap.Height), colors[rd.Next(colors.Length)]);
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                //位图保存到IO流
                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //IO流写入buffer数组
                checkCode.Buffer = ms.ToArray();
            }

            return checkCode;
        }
    }

    /// <summary>
    /// 验证码类
    /// </summary>
    public class CheckCode
    {
        public CheckCode()
        {
            this.Code = string.Empty;
        }

        public string Code { get; set; }

        public byte[] Buffer { get; set; }
    }
}