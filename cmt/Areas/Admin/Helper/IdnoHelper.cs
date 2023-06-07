using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace cmt.Areas.Admin.Helper
{
    public class IdnoHelper
    {
        /// 判斷身分證號及統一證號是否正確，並判斷性別及國籍
        ///
        /// 國籍
        /// 本署核發之外來人口統一證號編碼，共計10碼，前2碼使用英文字母，
        ///第1碼為區域碼（同國民身分證註1）
        ///第2碼為性別碼(註 2)、3至10碼為阿拉伯數字，其中第3至9碼為流水號、第10碼為檢查號碼。
        ///註1：英文字母代表直轄市、縣、市別：
        /// 台北市 A、台中市 B、基隆市 C、台南市 D、高雄市 E
        /// 新北市 F、宜蘭縣 G、桃園縣 H、嘉義市 I、新竹縣 J
        /// 苗栗縣 K、原台中縣 L、南投縣 M、彰化縣 N、新竹市 O
        /// 雲林縣 P、嘉義縣 Q、原台南縣 R、原高雄縣 S、屏東縣 T
        /// 花蓮縣 U、台東縣 V、金門縣 W、澎湖縣 X、連江縣 Z
        /// 註2：
        /// 臺灣地區無戶籍國民、大陸地區人民、港澳居民：
        /// 男性使用A、女性使用B
        ///外國人：
        /// 男性使用C、女性使用D
        /// </summary>
        /// <param name="str"></param>
        public static bool CheckIdno(String str)
        {
            string sex = "";
            string nationality = "";
            if (str == null || string.IsNullOrWhiteSpace(str) || str.Length != 10)
            {
                return false;
            }
            char[] pidCharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            str = str.ToUpper(); // 轉換大寫
            char[] strArr = str.ToCharArray(); // 字串轉成char陣列
            int verifyNum = 0;

            string pat = @"[A-Z]{1}[1-2]{1}[0-9]{8}";
            // Instantiate the regular expression object.
            Regex rTaiwan = new Regex(pat, RegexOptions.IgnoreCase);
            // Match the regular expression pattern against a text string.
            Match mTaiwan = rTaiwan.Match(str);
            // 檢查身分證字號
            return mTaiwan.Success;
        }
    }
}