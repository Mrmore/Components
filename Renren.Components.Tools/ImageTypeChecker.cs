using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Thanks for this link
// http://www.cnblogs.com/Yjianyong/archive/2012/01/05/2313230.html

namespace Renren.Components.Tools
{
    /// <summary>
    /// 实现通过文件头2个字节判断图片的格式。
    /// </summary>
    static public class ImageTypeCheck
    {
        public static ImageType TryCheckImageType(this Stream stream)
        {
            return CheckImageType(stream);
        }

        public static string ToFileExtension(this ImageType type)
        {
            if (type == ImageType.BMP) { return ".bmp"; }
            else if (type == ImageType.GIF) { return ".gif"; }
            else if (type == ImageType.JPG) { return ".jpeg"; }
            else if (type == ImageType.None) { return ".none"; }
            else if (type == ImageType.PCX) { return ".pcx"; }
            else if (type == ImageType.PNG) { return ".png"; }
            else if (type == ImageType.PSD) { return ".psd"; }
            else if (type == ImageType.RAS) { return ".ras"; }
            else if (type == ImageType.SGI) { return ".sgi"; }
            else if (type == ImageType.TIFF) { return ".tiff"; }
            else return ".unkown";
        }
        
        static ImageTypeCheck()
        {
            _imageTag = InitImageTag();
        }
        private static SortedDictionary<int, ImageType> _imageTag ;

        public static readonly string ErrType = ImageType.None.ToString();

        private static SortedDictionary<int, ImageType> InitImageTag()
        {
            SortedDictionary<int, ImageType> list = new SortedDictionary<int, ImageType>();

            list.Add((int)ImageType.BMP, ImageType.BMP);
            list.Add((int)ImageType.JPG, ImageType.JPG);
            list.Add((int)ImageType.GIF, ImageType.GIF);
            list.Add((int)ImageType.PCX, ImageType.PCX);
            list.Add((int)ImageType.PNG, ImageType.PNG);
            list.Add((int)ImageType.PSD, ImageType.PSD);
            list.Add((int)ImageType.RAS, ImageType.RAS);
            list.Add((int)ImageType.SGI, ImageType.SGI);
            list.Add((int)ImageType.TIFF, ImageType.TIFF);
            return list;

        }

        /// <summary>  
        /// 通过文件头判断图像文件的类型  
        /// </summary>  
        /// <param name="stream"></param>  
        /// <returns></returns>  
        public static string CheckImageTypeStream(Stream stream)
        {
            return CheckImageType(stream).ToString();
        }

        /// <summary>  
        /// 通过文件头判断图像文件的类型  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public static ImageType CheckImageType(Stream stream)
        {
            byte[] buf = new byte[2];
            try
            {
                //using (MemoryStream sr = new MemoryStream())
                {
                    var oldPos = stream.Position;
                    stream.Seek(0, SeekOrigin.Begin);
                    int i = stream.Read(buf, 0, buf.Length);
                    stream.Seek(oldPos, SeekOrigin.Begin);

                    if (i != buf.Length)
                    {
                        return ImageType.None;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.ToString());
                return ImageType.None;
            }

            return CheckImageType(buf);
        }

        /// <summary>  
        /// 通过文件的前两个自己判断图像类型  
        /// </summary>  
        /// <param name="buf">至少2个字节</param>  
        /// <returns></returns>  
        public static ImageType CheckImageType(byte[] buf)
        {
            if (buf == null || buf.Length < 2)
            {
                return ImageType.None;
            }

            int key = (buf[1] << 8) + buf[0];
            ImageType s;  
            if (_imageTag.TryGetValue(key, out s))
            {
                return s;
            }  
            return ImageType.None;
        }

    }

    /// <summary>  
    /// 图像文件的类型  
    /// </summary>  
    public enum ImageType
    {
        None = 0,
        BMP = 0x4D42,
        JPG = 0xD8FF,
        GIF = 0x4947,
        PCX = 0x050A,
        PNG = 0x5089,
        PSD = 0x4238,
        RAS = 0xA659,
        SGI = 0xDA01,
        TIFF = 0x4949
    }
}

