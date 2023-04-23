using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using XTranslation.Utils;

public class BaiDuTranslation : ITranslation
{
    private string Id;
    private string Key;
    private HttpWebRequest request;

    public void SetCred(string Id, string Key)
    {
        this.Id = Id;
        this.Key = Key;
    }

    public void Translation(string src, string From, string To)
    {
        var url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";

        var rd = new Random();
        var salt = rd.Next(100000).ToString();
        var sign = EncryptString(Id + src + salt + Key);

        url += "q=" + HttpUtility.UrlEncode(src);
        url += "&from=" + From;
        url += "&to=" + To;
        url += "&appid=" + Id;
        url += "&salt=" + salt;
        url += "&sign=" + sign;

        request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";
        request.UserAgent = null;
        request.Timeout = 6000;
    }


    public string GetResult()
    {
        var response = (HttpWebResponse)request.GetResponse();
        var myResponseStream = response.GetResponseStream();
        var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        var retString = myStreamReader.ReadToEnd();
        myStreamReader.Close();
        myResponseStream.Close();

        var i = retString.IndexOf("dst");
        var j = retString.IndexOf("}");

        retString = retString.Substring(i + 6, j - i - 7);
        var str = Regex.Unescape(retString);

        return str;
    }

    public static string EncryptString(string str)
    {
        var md5 = MD5.Create();
        // 将字符串转换成字节数组
        var byteOld = Encoding.UTF8.GetBytes(str);
        // 调用加密方法
        var byteNew = md5.ComputeHash(byteOld);
        // 将加密结果转换为字符串
        var sb = new StringBuilder();
        foreach (var b in byteNew)
            // 将字节转换成16进制表示的字符串，
            sb.Append(b.ToString("x2"));
        // 返回加密的字符串
        return sb.ToString();
    }
}