using XTranslation.Model;

namespace XTranslation.Utils
{
    public class TranslationOptions
    {
        public static string ChineseToStr(string str,TranslationPlatformEnum platformEnum)
        {
            switch (str)
            {
                case "自动检测":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "";
                    return "auto";
                }
                case "中文":
                {
                    return "zh";
                }
                case "繁体中文":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "zh-Hant";
                    return "cht";
                }
                case "英语":
                {
                    return "en";
                }
                case "日语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ja";
                    return "jp";
                }
                case "韩语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ko";
                    return "kor";
                }
                case "法语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "fr";
                    return "fra";
                }
                case "西班牙语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "es";
                    return "spa";
                }
                case "泰语":
                {
                    return "th";
                }
                case "阿拉伯语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ar";
                    return "ara";
                }
                case "俄语":
                {
                    return "ru";
                }
                case "葡萄牙语":
                {
                    return "pt";
                }
                case "德语":
                {
                    return "de";
                }
                case "意大利语":
                {
                    return "it";
                }
                case "希腊语":
                {
                    return "el";
                }
                case "荷兰语":
                {
                    return "nl";
                }
                case "波兰语":
                {
                    return "pl";
                }
                case "保加利亚语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "bg";
                    return "bul";
                }
                case "爱沙尼亚语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "et";
                    return "est";
                }
                case "丹麦语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "da";
                    return "dan";
                }
                case "芬兰语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "fi";
                    return "fin";
                }
                case "捷克语":
                {
                    return "cs";
                }
                case "罗马尼亚语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ro";
                    return "rom";
                }
                case "斯洛文尼亚语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "sl";
                    return "slo";
                }
                case "瑞典语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "sv";
                    return "swe";
                }
                case "匈牙利语":
                {
                    return "hu";
                }
                case "越南语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "vi";
                    return "vie";
                }
                case "印尼语":
                {
                    return "id";
                }
                case "泰米尔语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ta";
                    return "tam";
                }
                case "乌尔都语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ur";
                    return "urd";
                }
                case "马来语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "ms";
                    return "may";
                }
                case "印地语":
                {
                    return "hi";
                }
                case "孟加拉语":
                {
                    if (platformEnum == TranslationPlatformEnum.HuoShan)
                        return "bn";
                    return "ben";
                }
                case "土耳其语":
                {
                    return "tr";
                }
            }

            return "";
        }
    }
}