using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace XTranslation.Utils
{
    public class TencentTranslation : ITranslation
    {
       
        string SECRET_ID;
        
        string SECRET_KEY;

        string translatedText;
        
        public void SetCred(string Id, string Key)
        {
            SECRET_ID = Id;
            SECRET_KEY = Key;
        }

        public void Translation(string src, string From, string To)
        {
            try
            {
                Credential cred = new Credential { SecretId = SECRET_ID, SecretKey = SECRET_KEY };
                // 实例化一个client选项，可选的，没有特殊需求可以跳过
                ClientProfile clientProfile = new ClientProfile();
                // 实例化一个http选项，可选的，没有特殊需求可以跳过
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;
                // 实例化要请求产品的client对象,clientProfile是可选的
                TmtClient client = new TmtClient(cred, "ap-beijing", clientProfile);
                // 实例化一个请求对象,每个接口都会对应一个request对象
                TextTranslateRequest req = new TextTranslateRequest();
                req.Source = From;
                req.Target = To;
                req.ProjectId = 0;
                req.SourceText = src;
                // 返回的resp是一个TextTranslateBatchResponse的实例，与请求对象对应
                TextTranslateResponse resp = client.TextTranslateSync(req);
                // 输出json格式的字符串回包
                translatedText=AbstractModel.ToJsonString(resp);
            }
            catch (Exception e)
            {
                translatedText= e.ToString();
            }
          
        }

        public string GetResult()
        {
            try
            {
                JObject jArray = JsonConvert.DeserializeObject<JObject>(translatedText);
                string text =  jArray["TargetText"].ToString();
                return text;
            }
            catch (Exception e)
            {
                return "error -1";
            }
        }
        
        
      
    }
}