using System.Collections.Generic;

namespace XTranslation.ViewModel
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel()
        {
            Translation_Platform.Add("百度翻译");
            Translation_Platform.Add("火山翻译");
            Translation_Platform.Add("腾讯翻译");
            //Translation_Platform.Add("有道翻译");
            //Translation_Platform.Add("彩云翻译");

            #region 翻译语言

            Translation_From.Add("自动检测");
            Translation_From.Add("中文");
            Translation_From.Add("繁体中文");
            Translation_From.Add("英语");
            Translation_From.Add("日语");
            Translation_From.Add("韩语");
            Translation_From.Add("法语");
            Translation_From.Add("西班牙语");
            Translation_From.Add("泰语");
            Translation_From.Add("阿拉伯语");
            Translation_From.Add("俄语");
            Translation_From.Add("葡萄牙语");
            Translation_From.Add("德语");
            Translation_From.Add("意大利语");
            Translation_From.Add("希腊语");
            Translation_From.Add("荷兰语");
            Translation_From.Add("波兰语");
            Translation_From.Add("保加利亚语");
            Translation_From.Add("爱沙尼亚语");
            Translation_From.Add("丹麦语");
            Translation_From.Add("芬兰语");
            Translation_From.Add("捷克语");
            Translation_From.Add("罗马尼亚语");
            Translation_From.Add("斯洛文尼亚语");
            Translation_From.Add("瑞典语");
            Translation_From.Add("匈牙利语");
            Translation_From.Add("越南语");
            Translation_From.Add("印尼语");
            Translation_From.Add("泰米尔语");
            Translation_From.Add("乌尔都语");
            Translation_From.Add("马来语");
            Translation_From.Add("印地语");
            Translation_From.Add("孟加拉语");
            Translation_From.Add("土耳其语");

            Translation_To.Add("中文");
            Translation_To.Add("繁体中文");
            Translation_To.Add("英语");
            Translation_To.Add("日语");
            Translation_To.Add("韩语");
            Translation_To.Add("法语");
            Translation_To.Add("西班牙语");
            Translation_To.Add("泰语");
            Translation_To.Add("阿拉伯语");
            Translation_To.Add("俄语");
            Translation_To.Add("葡萄牙语");
            Translation_To.Add("德语");
            Translation_To.Add("意大利语");
            Translation_To.Add("希腊语");
            Translation_To.Add("荷兰语");
            Translation_To.Add("波兰语");
            Translation_To.Add("保加利亚语");
            Translation_To.Add("爱沙尼亚语");
            Translation_To.Add("丹麦语");
            Translation_To.Add("芬兰语");
            Translation_To.Add("捷克语");
            Translation_To.Add("罗马尼亚语");
            Translation_To.Add("斯洛文尼亚语");
            Translation_To.Add("瑞典语");
            Translation_To.Add("匈牙利语");
            Translation_To.Add("越南语");
            Translation_To.Add("印尼语");
            Translation_To.Add("泰米尔语");
            Translation_To.Add("乌尔都语");
            Translation_To.Add("马来语");
            Translation_To.Add("印地语");
            Translation_To.Add("孟加拉语");
            Translation_To.Add("土耳其语");

            #endregion

            for (var i = 1; i <= 120; i++) fontsizes.Add(i);
        }

        public List<string> Translation_Platform { get; set; } = new List<string>();

        public List<string> Translation_From { get; set; } = new List<string>();

        public List<string> Translation_To { get; set; } = new List<string>();

        public List<int> fontsizes { get; set; } = new List<int>();
    }
}