using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;

namespace UkeyTech.OA.Equity.Pub
{

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        RAR = 8297,
        jpg = 255216,
        exe = 7790,
        xml = 6063,
        html = 6033,
        aspx = 239187,
        cs = 117115,
        js = 119105,
        txt = 210187,
        sql = 255254
    }
    public class ImageClass
    {

        #region
        #endregion

        #region 切片
        /// <summary>
        /// 切片
        /// </summary>
        /// <param name="originalImagePath">原图片路径</param>
        /// <param name="sectionFilePath">切片后图片保存文件夹路径</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fileExt"></param>
        public static string Section(string originalImagePath, string sectionFilePath, int width, int height, string fileExt)
        {
            string result = null;
            Image originalImage = Image.FromFile(originalImagePath);
            Bitmap bitmap = new Bitmap(originalImagePath);
            double maxRow = Math.Ceiling((double)bitmap.Height / height);
            double maxColumn = Math.Ceiling((double)bitmap.Width / width);
            //最大放大级别
            int maxZoom = (int)(maxRow > maxColumn ? Math.Log(maxRow,2) : Math.Log(maxColumn, 2));

            //原图实际大小
            int sWidth = originalImage.Width;
            int sHeight = originalImage.Height;
            result = "[";
            for (var z = 0; z <= maxZoom; z++)
            {
                int row = (int)Math.Pow(2, z);

                int towidth = width * row;
                int toheight = height * row;
                
                int x = 0; //缩略图在画布上的X放向起始点  
                int y = 0; //缩略图在画布上的Y放向起始点  
                int dw = 0;
                int dh = 0;

                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                {
                    //宽比高大，以宽为准  
                    dw = originalImage.Width * towidth / originalImage.Width;
                    dh = originalImage.Height * toheight / originalImage.Width;
                    x = 0;
                    y = (toheight - dh) / 2;
                }
                else
                {
                    //高比宽大，以高为准  
                    dw = originalImage.Width * towidth / originalImage.Height;
                    dh = originalImage.Height * toheight / originalImage.Height;
                    x = (towidth - dw) / 2;
                    y = 0;
                }


                #region

                //switch (mode)
                //{
                //    case "HW":  //指定高宽缩放（可能变形）                
                //        break;
                //    case "W":   //指定宽，高按比例                    
                //        toheight = originalImage.Height * width / originalImage.Width;
                //        break;
                //    case "H":   //指定高，宽按比例
                //        towidth = originalImage.Width * height / originalImage.Height;
                //        break;
                //    case "Cut": //指定高宽裁减（不变形）                
                //        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                //        {
                //            oh = originalImage.Height;
                //            ow = originalImage.Height * towidth / toheight;
                //            y = 0;
                //            x = (originalImage.Width - ow) / 2;
                //        }
                //        else
                //        {
                //            ow = originalImage.Width;
                //            oh = originalImage.Width * height / towidth;
                //            x = 0;
                //            y = (originalImage.Height - oh) / 2;
                //        }
                //        break;
                //    default:
                //        break;
                //}

                #endregion


                //新建一个bmp图片
                Bitmap bmp = new Bitmap(towidth, toheight);
                //新建一个画板
                Graphics g = Graphics.FromImage(bmp);
                //设置高质量插值法
                g.InterpolationMode = InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = SmoothingMode.HighQuality;
                //消除锯齿 
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //清空画布并以透明背景色填充
                //g.Clear(Color.Transparent);
                g.Clear(Color.White);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(x, y, dw, dh),new Rectangle(0, 0, sWidth, sHeight), GraphicsUnit.Pixel);

         
                for (int m = 0; m < row; m++)
                {
                    for (int n = 0; n < row; n++)
                    {
                        //切片后的文件名
                        string filename = z.ToString() + "_" + n.ToString() + "_" + m.ToString() + "" + fileExt;
                        //新建一个切片bmp图片
                        Bitmap sectionBmp = new Bitmap(width, height);
                        for (int offsetX = 0; offsetX < width; offsetX++)
                        {
                            for (int offsetY = 0; offsetY < height; offsetY++)
                            {
                                if (((n * width + offsetX) < bmp.Width) && ((m * height + offsetY) < bmp.Height))
                                {
                                    //给图片指定的像素添加指定的颜色
                                    sectionBmp.SetPixel(offsetX, offsetY,
                                        bmp.GetPixel((int)(n * width + offsetX), (int)(m * height + offsetY)));
                                }
                            }
                        }
                        Graphics sectionG = Graphics.FromImage(sectionBmp);
                        ImageFormat format = ImageFormat.Png;

                      
                        //以jpg 格式保存缩略图
                        if (!Directory.Exists(sectionFilePath))
                        {
                            Directory.CreateDirectory(sectionFilePath); 
                                
                        }
                        sectionBmp.Save(sectionFilePath + "//" + filename, format);

                        result += "{";
                        result += "\"SourceImagePath\":\""+ filename+"\",";
                        result += "\"Z\":\"" + z + "\",";
                        result += "\"X\":\"" + n + "\",";
                        result += "\"Y\":\"" + m + "\"";
                        result += "}";
                        if (!(z == maxZoom && n == row - 1 && m == row - 1))
                        {
                            result += ",";
                        }
                    }
                }

            }
            result += "]";
            return result;

        }
        #endregion

    }
}
