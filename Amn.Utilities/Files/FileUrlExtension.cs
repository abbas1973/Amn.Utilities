using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Files
{
    public static class FileUrlExtension
    {

        private const string DOWNLOAD_URL_BY_ID = "/Api/v1/MediaFiles/Download/{0}?IsThumb={3}";
        private const string STREAM_URL_BY_ID = "/Api/v1/MediaFiles/GetStream/{0}?IsThumb={3}";

        private const string DOWNLOAD_URL_BY_NAME = "/Api/v1/MediaFiles/Download?FileName={0}&Group={1}&IsPic={2}&IsThumb={3}";
        private const string STREAM_URL_BY_NAME = "/Api/v1/MediaFiles/GetStream?FileName={0}&Group={1}&IsPic={2}&IsThumb={3}";

        private const string DELETE_FILE_URL = "/Api/v1/MediaFiles/DeleteFile?FileName={0}&Group={1}";
        private const string DELETE_PIC_URL = "/Api/v1/MediaFiles/DeletePic?FileName={0}&Group={1}";



        #region گرفتن آدرس دانلود فایل
        /// <summary>
        /// گرفتن آدرس دانلود با آیدی
        /// </summary>
        /// <param name="id">آیدی مدیا فایل</param>
        /// <param name="isThumb">تصویر کوچک مد نظر است؟</param>
        /// <returns></returns>
        public static string GetDownloadUrl(this long id, bool isThumb)
        {
            return string.Format(DOWNLOAD_URL_BY_ID, id, isThumb.ToString());
        }


        /// <summary>
        /// گرفتن آدرس دانلود با نام فایل
        /// </summary>
        /// <param name="fileName">نام فایل</param>
        /// <param name="Group">گروه بندی فایل</param>
        /// <param name="isPic">تصویر است؟</param>
        /// <param name="isThumb">تصویر کوچک مد نظر است؟</param>
        /// <returns></returns>
        public static string GetDownloadUrl(this string fileName, string group, bool isPic, bool isThumb)
        {
            return string.Format(DOWNLOAD_URL_BY_NAME, fileName, group, isPic.ToString(), isThumb.ToString());
        }
        #endregion




        #region گرفتن آدرس استریم فایل
        /// <summary>
        /// گرفتن آدرس استریم با آیدی
        /// </summary>
        /// <param name="id">آیدی مدیا فایل</param>
        /// <param name="isThumb">تصویر کوچک مد نظر است؟</param>
        /// <returns></returns>
        public static string GetStreamUrl(this long id, bool isThumb)
        {
            return string.Format(STREAM_URL_BY_ID, id, isThumb.ToString());
        }


        /// <summary>
        /// گرفتن آدرس استریم با نام فایل
        /// </summary>
        /// <param name="fileName">نام فایل</param>
        /// <param name="Group">گروه بندی فایل</param>
        /// <param name="isPic">تصویر است؟</param>
        /// <param name="isThumb">تصویر کوچک مد نظر است؟</param>
        /// <returns></returns>
        public static string GetStreamUrl(this string fileName, string group, bool isPic, bool isThumb)
        {
            return string.Format(STREAM_URL_BY_NAME, fileName, group, isPic.ToString(), isThumb.ToString());
        }
        #endregion




        #region گرفتن آدرس حذف فیزیکی فایل
        public static string GetDeleteUrl(this string fileName, string group, bool isPic)
        {
            if (isPic)
                return string.Format(DELETE_PIC_URL, fileName, group);
            else
                return string.Format(DELETE_FILE_URL, fileName, group);
        }
        #endregion


    }
}
