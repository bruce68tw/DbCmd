using Base.Services;
using DbCmd.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DbCmd.Services
{
    /// <summary>
    /// 執行目錄 _sql 下的 sql
    /// </summary>
    public class RunSqlSvc
    {
        //目前小時0-23(24hour)
        private int _nowHour = DateTime.Now.Hour;
        private Db _db = null;

        public async Task RunA()
        {
            const string preLog = "RunSqlSvc: ";
            _Log.Info(preLog + "Start.");

            //sql目錄路徑
            var dirSql = _Str.AddDirSep(_Fun.DirRoot + "_sql");
            if (!Directory.Exists(dirSql))
            {
                _Log.Error($"{preLog}Directory not found : {dirSql}");
                return;
            }

            #region 讀取目錄清單(目錄對應資料庫) & loop
            //每個子目錄代表一個 Database
            foreach (var dirDb in Directory.GetDirectories(dirSql))
            {
                var dbName = Path.GetFileName(dirDb);
                _Log.Info($"{preLog}Folder is {dbName}");

                //讀取目錄下的 config.json
                var configFile = Path.Combine(dirDb, "config.json");
                if (!File.Exists(configFile))
                {
                    _Log.Error($"{preLog}config.json not found.");
                    continue;
                }

                var config = JsonSerializer.Deserialize<SqlConfigDto>(
                    await File.ReadAllTextAsync(configFile));
                if (config == null)
                {
                    _Log.Error($"{preLog}config parse failed.");
                    continue;
                }

                //依檔名排序
                var sqlFiles = Directory
                    .GetFiles(dirDb, "*.sql")
                    .OrderBy(x => x)
                    .ToList();

                //loop: 執行sql
                foreach (var sqlFile in sqlFiles)
                {
                    var fileName0 = Path.GetFileNameWithoutExtension(sqlFile);
                    var cols = fileName0.Split("-");
                    if (cols.Length != 2)
                    {
                        _Log.Error($"{preLog}sql FileName format is xxx-xxx.sql({fileName0})");
                        continue;
                    }

                    var fileName = cols[0];                    
                    var timeType = cols[1]; //時間格式:-1(1點執行), -P2(每2小時執行)

                    //判斷是否符合執行時間
                    if (!NeedRun(timeType)) continue;

                    //連線 DB if need
                    if (!ConnectDb(config.Db))
                    {
                        _Log.Error($"{preLog}connect DB failed.({fileName0})");
                        break;
                    }

                    //run sql & log
                    var sql = await File.ReadAllTextAsync(sqlFile);
                    var rows = await _db.ExecSqlA(sql);
                    _Log.Info($"{preLog}SQL file: {fileName0}");
                    _Log.Info($"{preLog}SQL OK rows: {rows}");
                }

                //Close Db if need
                if (_db != null)
                    await _db.DisposeAsync();
            }
            #endregion

            //log end
            _Log.Info(preLog + "End.");
        }

        /// <summary>
        /// 是否符合執行時間
        /// </summary>
        /// <param name="timeType">時間格式:0(現在執行), 1(1點執行), P2(每2小時執行)</param>
        /// <returns></returns>
        private bool NeedRun(string timeType)
        {
            if (string.IsNullOrWhiteSpace(timeType)) return false;

            timeType = timeType.Trim().ToUpper();
            if (timeType == "0")
            {
                return true;
            }
            else if (timeType.StartsWith("P"))
            {
                // P2、P6...
                if (!int.TryParse(timeType[1..], out int hours) || hours <= 0)
                    return false;

                return _nowHour % hours == 0;
            }

            // 每天固定幾點
            if (!int.TryParse(timeType, out int hour))
                return false;

            return (hour == _nowHour);
        }

        //connect db
        private bool ConnectDb(string connStr)
        {
            if (_db != null) return true;

            if (_Fun.Config.Encode)
                connStr = _Str.Decode(connStr);

            _db = new Db(connStr);
            return (_db != null);
        }

    }//class
}