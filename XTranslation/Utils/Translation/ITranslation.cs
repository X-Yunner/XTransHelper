namespace XTranslation.Utils
{
    public interface ITranslation
    {
	    /// <summary>
	    ///     设置翻译Api的Id与密钥
	    /// </summary>
	    /// <param name="Id"></param>
	    /// <param name="Key"></param>
	    void SetCred(string Id, string Key);

	    /// <summary>
	    ///     设置翻译数据
	    /// </summary>
	    /// <param name="src">原文</param>
	    /// <param name="From">原文语种</param>
	    /// <param name="To">译文语种</param>
	    void Translation(string src, string From, string To);

	    /// <summary>
	    ///     返回译文
	    /// </summary>
	    /// <returns></returns>
	    string GetResult();
    }
}