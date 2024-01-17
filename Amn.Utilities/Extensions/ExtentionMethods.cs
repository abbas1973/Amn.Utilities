using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amn.Utilities
{
    public static class ExtentionMethods
    {
        #region گرفتن تمام فرزندان در یک ساختار درختی در قالب یک لیست
        /// <summary>
        /// گرفتن تمام فرزندان در یک ساختار درختی در قالب یک لیست
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="source"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T, R>(this IEnumerable<T> source, Func<T, R> recursion) where R : IEnumerable<T>
        {
            return source.SelectMany(x => (recursion(x) != null && recursion(x).Any()) ? recursion(x).Flatten(recursion) : null)
                         .Where(x => x != null);
        } 
        #endregion



        #region محاسبه سن با تاریخ تولد
        /// <summary>
        /// محاسبه سن با تاریخ تولد
        /// </summary>
        /// <param name="BirthDay"></param>
        /// <returns></returns>
        public static string GetFullAge(this DateTime BirthDay)
        {
            var today = DateTime.Today;
            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (BirthDay.Year * 100 + BirthDay.Month) * 100 + BirthDay.Day;

            var year = (a - b) / 10000;
            var month = ((a - b) % 10000) / 100;
            var day = (a - b) % 100;

            string age = year + " سال و " + month + " ماه و " + day + " روز ";
            return age;
        }
        #endregion




        #region گرفتن فرمت شماره کارت
        /// <summary>
        /// گرفتن فرمت شماره کارت
        /// <para>
        /// بین هر 4 کاراکتر یک خط تیره میگذارد
        /// </para>
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        public static string GetCardIdFormat(this string CardId)
        {
            string InCardId = CardId;
            string OutCardId = "";

            while (InCardId.Length > 4)
            {
                OutCardId = InCardId.Substring(InCardId.Length - 4) + "-" + OutCardId;
                InCardId = InCardId.Substring(0, InCardId.Length - 4);
            }
            OutCardId = InCardId + "-" + OutCardId;
            return OutCardId.Substring(0, OutCardId.Length - 1);
        }
        #endregion




        #region گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// <summary>
        /// گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static PersianDateTime? ToPersianDateTime(this DateTime? Date)
        {
            if (Date == null)
                return null;
            return ((DateTime)Date).ToPersianDateTime();
        }

        /// <summary>
        /// گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static PersianDateTime ToPersianDateTime(this DateTime Date)
        {
            var pd = new PersianDateTime(Date) { EnglishNumber = true };
            pd.EnglishNumber = true;
            return pd;
        }
        #endregion



        #region آیا درخواست از نوع اجکس است؟
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        #endregion


        #region برای چک کرددن اینکه یک آیتم درون لیستی از آیتم ها موجود است یا خیر
        /// <summary>
        /// برای چک کرددن اینکه یک آیتم درون لیستی از آیتم ها موجود است یا خیر
        /// <para>
        /// if (1.In(1, 2))
        /// </para>
        /// </summary>
        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }
        #endregion

    }
}