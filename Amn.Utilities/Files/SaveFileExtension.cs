using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Files
{
    public static class SaveFileExtension
    {

        #region ذخیره فایل در یک آدرس مشخص
        /// <summary>
        /// ذخیره فایل در یک آدرس مشخص
        /// </summary>
        /// <param name="file">فایل</param>
        /// <param name="SavePath">
        /// مسیر فایل درون پوشه روت. مثلا:
        /// /Uploads/Admin/
        /// </param>
        /// <param name="FileName">نام به همراه اکستنشن</param>
        /// <param name="type">تایپ فایل</param>
        /// <param name="ProjectPhysicalPath">آدرس فیزیکی روت پروژه (بیرون wwwroot(</param>
        /// <returns>نام نهایی فایل ذخیره شده</returns>
        public static async Task<SaveFileResult> SaveFile(this IFormFile file, string SavePath, string FileName = null, FileFormat? type = null, string ProjectPhysicalPath = null)
        {
            try
            {
                if (file == null)
                    return new SaveFileResult(false, "فایل موجود نیست!");

                if (SavePath == null)
                    return new SaveFileResult(false, "مسیر ذخیره فایل موجود نیست!");

                var fileType = GetFormat(file.FileName);
                if (fileType == FileFormat.Other)
                    return new SaveFileResult(false, "فقط بارگذاری فایلهای pdf, word, apk, excel و ویدیو و تصویر مجاز می باشد.");

                if (!file.IsValidFile(type))
                    return new SaveFileResult(false, "فرمت فایل بارگذاری شده صحیح نیست!");

                if (FileName == null)
                {
                    var ext = file.GetExtension();
                    FileName = Guid.NewGuid().ToString() + ext;
                }

                var myPath = ProjectPhysicalPath ?? Directory.GetCurrentDirectory();

                string _Path = Path.Combine(myPath, "wwwroot" + SavePath + FileName);

                var i = 1;
                string BaseName = FileName;
                while (File.Exists(_Path))
                {
                    FileName = i + "-" + BaseName;
                    _Path = Path.Combine(myPath, "wwwroot" + SavePath + FileName);
                    i++;
                }

                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new SaveFileResult(true, "ذخیره فایل با موفقیت انجام شد.", FileName);
            }
            catch (Exception ex)
            {
                return new SaveFileResult(false, "ذخیره فایل با خطا همراه بوده است.");
            }
        }
        #endregion



        #region گرفتن فرمت فایل آپلود شده
        public static FileFormat GetFormat(string FileName)
        {
            var splited = FileName.Split('.');
            if (splited.Length > 0)
            {
                string extansion = "." + splited[splited.Length - 1].ToLower();
                if (extansion.IsImage())
                    return FileFormat.Image;
                if (extansion.IsPdf())
                    return FileFormat.Pdf;
                if (extansion.IsWord())
                    return FileFormat.Docx;
                if (extansion.IsExcel())
                    return FileFormat.Excel;
                if (extansion.IsApk())
                    return FileFormat.Apk;
            }
            return FileFormat.Other;
        }
        #endregion



        #region ذخیره عکس
        /// <summary>
        /// ذخیره عکس
        /// </summary>
        /// <param name="Pic">فایل عکس</param>
        /// <param name="ThumbPath">آدرس عکس کوچک</param>
        /// <param name="LargePath">آدرس عکس بزرگ</param>
        /// <param name="ThumbWidth">اندازه عرض کوچک</param>
        /// <param name="ThumbHeight">اندازه ارتفاع کوچک</param>
        /// <param name="LargeWidth">اندازه عرض بزرگ</param>
        /// <param name="LargeHeight">اندازه ارتفاع بزرگ</param>
        /// <param name="FileName">نام فایل به همراه پسوند</param>
        /// <returns></returns>
        public static async Task<SaveFileResult> SavePicture(this IFormFile Pic, string ThumbPath = null, string LargePath = null, int? ThumbWidth = null, int? ThumbHeight = null, int? LargeWidth = null, int? LargeHeight = null, string FileName = null)
        {
            try
            {
                if (Pic == null)
                    return new SaveFileResult(false, "عکس موجود نیست!");

                if (!Pic.IsImage())
                    return new SaveFileResult(false, "فرمت فایل بارگذاری شده صحیح نیست!");

                if (ThumbPath == null && LargePath == null)
                    return new SaveFileResult(false, "آدرس ذخیره عکس وارد نشده است!");

                if (FileName == null)
                {
                    var ext = Pic.GetExtension();
                    FileName = Guid.NewGuid().ToString() + ext;
                }

                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + (ThumbPath ?? LargePath) + FileName);

                var i = 1;
                string BaseName = FileName;
                while (File.Exists(_Path))
                {
                    FileName = i + "-" + BaseName;
                    _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + ThumbPath + FileName);
                    i++;
                }


                // اگر عکس با اندازه اصلی باید ذخیره شود
                if (ThumbWidth == null && ThumbHeight == null && LargeWidth == null && LargeHeight == null)
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        await Pic.CopyToAsync(stream);
                    }

                // برای ذخیره فایل در اندازه thumb
                if (ThumbPath != null)
                {
                    string _ThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + ThumbPath + FileName);
                    using (var stream = new FileStream(_ThumbPath, FileMode.Create))
                    {
                        await Pic.CopyToAsync(stream);
                    }

                    if (ThumbWidth != null && ThumbHeight != null)
                        await ImageResizer.CropImage(_ThumbPath, _ThumbPath, ThumbWidth.Value, ThumbHeight.Value, 100);
                }

                // برای ذخیره فایل در اندازه large
                if (LargePath != null)
                {
                    string _LargePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + LargePath + FileName);
                    using (var stream = new FileStream(_LargePath, FileMode.Create))
                    {
                        await Pic.CopyToAsync(stream);
                    }

                    if (LargeWidth != null && LargeHeight != null)
                        await ImageResizer.CropImage(_LargePath, _LargePath, LargeWidth.Value, LargeHeight.Value, 100);
                }

                return new SaveFileResult(true, "ذخیره عکس با موفقیت انجام شد.", FileName);
            }
            catch (Exception ex)
            {
                return new SaveFileResult(false, "ذخیره عکس با خطا همراه بوده است!", ex.Message);
            }

        }
        #endregion



        #region حذف عکس
        /// <summary>
        /// حذف عکس
        /// </summary>
        /// <param name="FileName">نام فایل</param>
        /// <param name="ThumbPath">آدرس عکس کوچک</param>
        /// <param name="LargePath">آدرس عکس بزرگ</param>
        /// <returns></returns>
        public static bool DeletePicOrFile(this string FileName, string ThumbPath = null, string LargePath = null)
        {
            try
            {
                if (ThumbPath == null && LargePath == null) return false;

                if (ThumbPath != null)
                {
                    string _ThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + ThumbPath + FileName);
                    if (File.Exists(_ThumbPath))
                    {
                        File.Delete(_ThumbPath);
                    }
                }

                if (LargePath != null)
                {
                    string _LargePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + LargePath + FileName);
                    if (File.Exists(_LargePath))
                    {
                        File.Delete(_LargePath);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
