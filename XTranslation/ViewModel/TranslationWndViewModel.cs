namespace XTranslation.ViewModel
{
    public class TranslationWndViewModel : ViewModelBase
    {
        public string srcText { get; set; } = "";

        public string dstText { get; set; } = "";

        public int fontsize { get; set; } = 20;

        public bool TopWindow { get; set; } = true;
    }
}