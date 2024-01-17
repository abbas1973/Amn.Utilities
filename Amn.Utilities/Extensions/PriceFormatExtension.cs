using System;

namespace Amn.Utilities
{
    public static class PriceFormatExtension
    {


        #region تبدیل به فرمت قیمت
        #region string
        /// <summary>
        /// تبدیل به فرمت قیمت
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string ToPriceFormat(this string price, bool convertToTooman = false)
        {
            if (string.IsNullOrEmpty(price))
                return null;
            var amount = Convert.ToInt64(price);
            return amount.ToPriceFormat(convertToTooman);
        }
        #endregion


        #region int
        public static string ToPriceFormat(this int? price, bool convertToTooman = false)
        {
            if (price == null)
                return null;
            return ((int)price).ToPriceFormat(convertToTooman);
        }
        public static string ToPriceFormat(this int price, bool convertToTooman = false)
        {
            if (convertToTooman)
                price = price / 10;
            return price.ToString("N0");
        }
        #endregion


        #region long
        public static string ToPriceFormat(this long? price, bool convertToTooman = false)
        {
            if (price == null)
                return null;
            return ((long)price).ToPriceFormat(convertToTooman);
        }

        public static string ToPriceFormat(this long price, bool convertToTooman = false)
        {
            if (convertToTooman)
                price = price / 10;
            return ((long)price).ToPriceFormat(convertToTooman);
        }
        #endregion


        #region decimal
        public static string ToPriceFormat(this decimal? price, bool convertToTooman = false)
        {
            if (price == null)
                return null;
            return ((decimal)price).ToPriceFormat(convertToTooman);
        }

        public static string ToPriceFormat(this decimal price, bool convertToTooman = false)
        {
            if (convertToTooman)
                price = price / 10;
            return String.Format("{0:#,##0.##}", price);
        }
        #endregion



        #region double
        public static string ToPriceFormat(this double? price, bool convertToTooman = false)
        {
            if (price == null)
                return null;
            return ((decimal)price).ToPriceFormat(convertToTooman);
        }

        public static string ToPriceFormat(this double price, bool convertToTooman = false)
        {
            if (convertToTooman)
                price = price / 10;
            return String.Format("{0:#,##0.##}", price);
        }
        #endregion



        #region float
        public static string ToPriceFormat(this float? price, bool convertToTooman = false)
        {
            if (price == null)
                return null;
            return ((decimal)price).ToPriceFormat(convertToTooman);
        }

        public static string ToPriceFormat(this float price, bool convertToTooman = false)
        {
            if (convertToTooman)
                price = price / 10;
            return String.Format("{0:#,##0.##}", price);
        }
        #endregion
        #endregion



    }
}
