using Base.Services;
using DbCmd.Models;

namespace DbCmd.Services
{
    //project service
#pragma warning disable CA2211 // 非常數欄位不應可見
    public static class _Xp
    {
        //public const string SiteVer = "20201228f";     //for my.js/css
        //public static string MyVer = _Date.NowSecStr(); //for my.js/css
        //public const string LibVer = "20250501";       //for lib.js/css

        //public const string RoleAll = "_All";       //角色Id:所有人員, 與XpRole.Id一致
        //public const string RoleHrMgr = "HrMgr";    //角色Id:Hr主管, 與XpRole.Id一致

        //public static string NoImagePath = _Fun.DirRoot + "/wwwroot/image/noImage.jpg";

        //dir
        public static string DirTpl = _Fun.DirRoot + "_template/";
        //public static string DirUpload = _Fun.DirRoot + "_upload/";

        public static string DirBaseUpload = _Fun.Dir("_upload");
        //public static string DirHrInsGov = DirUpload("HrInsGov");

        public static MyConfigDto Config = null!;

        /*
        public static string GetTplPath(string fileName, bool hasLocale)
        {
            return $"{DirTpl}{(hasLocale ? _Locale.GetLocale() : "")}/{fileName}";
        }
        */

        private static string DirUpload(string subDir, bool sep = true)
        {
            return DirBaseUpload + subDir + (sep ? _Fun.DirSep : "");
        }

    }//class
#pragma warning restore CA2211 // 非常數欄位不應可見
}