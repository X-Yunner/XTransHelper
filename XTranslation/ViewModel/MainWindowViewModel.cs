namespace XTranslation.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region 标题栏行为

        //标题栏双击、右击、拖动等动作判定高度
        public int captionHeight { get; set; } = 26;

        //是否置顶窗口
        public bool TopWindow { get; set; } = false;

        #endregion
    }
}