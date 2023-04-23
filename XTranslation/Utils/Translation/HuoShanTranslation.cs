using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HandyControl.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XTranslation.Utils
{
    public class HuoShanRequest
    {
        [JsonProperty("SourceLanguage")] 
        public string SourceLanguage { get; set; }

        [JsonProperty("TargetLanguage")] 
        public string TargetLanguage { get; set; }

        [JsonProperty("TextList")] 
        public string[] TextList { get; set; }
    }
    public class HuoShanTranslation : ITranslation
    {
        private string Access_Key_ID;

        private string Secret_Access_Key;

        private string translatedText;

        private string Host = "open.volcengineapi.com";

        private string URL = "http://open.volcengineapi.com/?Action=TranslateText&Version=2020-06-01";

        string NowDate;

        string NowTime;

        string dateTimeSignStr;

        private HttpWebRequest request;

        public void SetCred(string Id, string Key)
        {
            Access_Key_ID = Id;
            Secret_Access_Key = Key;
        }

        public void Translation(string src, string From, string To)
        {
            HuoShanRequest huoShanRequest = new HuoShanRequest();
            huoShanRequest.SourceLanguage = From;
            huoShanRequest.TargetLanguage = To;
            huoShanRequest.TextList = new string[] { src };
            string requestBody = JsonConvert.SerializeObject(huoShanRequest);

            DateTime dateTimeSign = DateTime.UtcNow;
            NowDate = dateTimeSign.ToString("yyyyMMdd");
            NowTime = dateTimeSign.ToString("hhmmss");
            dateTimeSignStr = NowDate + "T" + NowTime + "Z";

            translatedText = SendRequest(requestBody);
        }

        public string GetResult()
        {
            try
            {
                JObject jArray = JsonConvert.DeserializeObject<JObject>(translatedText);
                string text = "";
                int size = jArray["TranslationList"].Count();
                for (int i = 0; i < size - 1; i++)
                {
                    text += jArray["TranslationList"][i]["Translation"];
                    text += "\n";
                }
                text += jArray["TranslationList"][size - 1]["Translation"];
                return text;
            }
            catch (Exception e)
            {
                return "error -1";
            }
        }


        private string SendRequest(string reqBody)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(reqBody);

            string bodyhash = ComputeHash256(reqBody, new SHA256CryptoServiceProvider());

            request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.ContentLength = byteArray.Length;
            request.Host = Host;
            request.Headers.Add("X-Date", dateTimeSignStr);
            request.Headers.Add("X-Content-Sha256", bodyhash);
            request.UserAgent = "volc-sdk-java/v1.0.16";
            request.Headers.Add("Authorization", GetAuthHeader(request, bodyhash));
            request.KeepAlive = false;

            try
            {
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(byteArray, 0, byteArray.Length);
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                if ((int)webResponse.StatusCode == 200)
                {
                    StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                return "error -1";
            }
            return "";
        }

        private string GetAuthHeader(HttpWebRequest request, string bodyhash)
        {
            List<string> signedHeaders = new List<string>
            {
                "content-type",
                "host",
                "x-content-sha256",
                "x-date"
            };
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var signedHeader in signedHeaders)
            {
                if (signedHeader == "host")
                {
                    stringBuilder.Append(signedHeader).Append(":").Append(Host).Append("\n");
                }
                else
                {
                    string temp = request.Headers.Get(signedHeader).Trim();
                    stringBuilder.Append(signedHeader).Append(":").Append(temp).Append("\n");
                }
            }

            string signHeaderStr = String.Join(";", signedHeaders);
            string canonicalRequest = String.Join("\n", new string[]
            {
                request.Method,
                "/",
                "Action=TranslateText&Version=2020-06-01",
                stringBuilder.ToString(),
                signHeaderStr,
                bodyhash
            });
            string hashCanonReq = ComputeHash256(canonicalRequest, new SHA256CryptoServiceProvider());

            string strTosign = "HMAC-SHA256\n" + dateTimeSignStr + "\n"
                               + string.Join("/", new string[]
                               {
                                   NowDate,
                                   "cn-north-1",
                                   "translate",
                                   "request"
                               }) + "\n" + hashCanonReq;

            byte[] kDate = hmacsha256(NowDate, Encoding.UTF8.GetBytes(Secret_Access_Key));
            byte[] kRegion = hmacsha256("cn-north-1", kDate);
            byte[] kService = hmacsha256("translate", kRegion);
            byte[] signingKey = hmacsha256("request", kService);
            byte[] signature = hmacsha256(strTosign, signingKey);
            stringBuilder.Clear();
            for (int i = 0; i < signature.Length; i++)
            {
                stringBuilder.Append(signature[i].ToString("X2"));
            }
            string authHeader = "HMAC-SHA256 Credential=" + Access_Key_ID + "/" + NowDate + "/" +
                                "cn-north-1/translate/request, SignedHeaders=" + signHeaderStr + ", Signature=" +
                                stringBuilder.ToString().ToLower();
            return authHeader;
        }

        private string ComputeHash256(string input, HashAlgorithm algorithm)
        {
            if (input.Length == 0) return "";
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                stringBuilder.Append(hashedBytes[i].ToString("X2"));
            }
            return stringBuilder.ToString().ToLower();
        }

        private static byte[] hmacsha256(string text, byte[] secret)
        {
            byte[] hash;
            using (HMACSHA256 hmacsha256 = new HMACSHA256(secret))
            {
                hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
            return hash;
        }
    }
}