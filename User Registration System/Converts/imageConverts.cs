using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using static System.Net.Mime.MediaTypeNames;

public class ImageConvertor
{
    /// <summary>
    /// تغییر اندازه تصویر و ذخیره آن در مسیر مشخص
    /// </summary>
    /// <param name="inputPath">مسیر فایل تصویر ورودی</param>
    /// <param name="outputPath">مسیر فایل تصویر خروجی</param>
    /// <param name="size">اندازه جدید (پیکسل)</param>
    /// <param name="encoder">انکودر تصویر خروجی (پیش‌فرض: JpegEncoder)</param>
    public void Image_resize(string inputPath, string outputPath, int size, IImageEncoder encoder = null)
    {
        if (encoder == null)
        {
            encoder = new JpegEncoder(); // فرمت پیش‌فرض
        }

        if (!System.IO.File.Exists(inputPath))
        {
            throw new FileNotFoundException($"فایل تصویر در مسیر مورد نظر یافت نشد: {inputPath}");
        }

        // بارگذاری تصویر از مسیر ورودی
        using (var image = SixLabors.ImageSharp.Image.Load(inputPath))
        {
            int newWidth, newHeight;

            // محاسبه اندازه جدید با حفظ نسبت ابعاد
            if (image.Width > image.Height)
            {
                newWidth = size;
                newHeight = (int)(image.Height * ((float)size / image.Width));
            }
            else
            {
                newHeight = size;
                newWidth = (int)(image.Width * ((float)size / image.Height));
            }

            // تغییر اندازه تصویر
            image.Mutate(x => x.Resize(newWidth, newHeight));

            // ذخیره تصویر تغییر اندازه داده شده
            image.Save(outputPath, encoder);
        }
    }

    /// <summary>
    /// تغییر اندازه تصویر از طریق استریم و بازگرداندن استریم خروجی
    /// </summary>
    /// <param name="inputStream">استریم تصویر ورودی</param>
    /// <param name="size">اندازه جدید (پیکسل)</param>
    /// <param name="encoder">انکودر تصویر خروجی (پیش‌فرض: JpegEncoder)</param>
    /// <returns>استریم تصویر تغییر اندازه داده شده</returns>
    public Stream Image_resize(Stream inputStream, int size, IImageEncoder encoder = null)
    {
        if (encoder == null)
        {
            encoder = new JpegEncoder(); // فرمت پیش‌فرض
        }

        // بارگذاری تصویر از استریم ورودی
        using (var image = SixLabors.ImageSharp.Image.Load(inputStream))
        {
            int newWidth, newHeight;

            // محاسبه اندازه جدید با حفظ نسبت ابعاد
            if (image.Width > image.Height)
            {
                newWidth = size;
                newHeight = (int)(image.Height * ((float)size / image.Width));
            }
            else
            {
                newHeight = size;
                newWidth = (int)(image.Width * ((float)size / image.Height));
            }

            // تغییر اندازه تصویر
            image.Mutate(x => x.Resize(newWidth, newHeight));

            // ذخیره تصویر تغییر اندازه داده شده در یک استریم
            var outputStream = new MemoryStream();
            image.Save(outputStream, encoder);
            outputStream.Position = 0; // بازنشانی موقعیت استریم برای خواندن
            return outputStream;
        }
    }
}
