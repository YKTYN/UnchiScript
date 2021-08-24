using System;

public static class Util
{
    /// <summary>
    /// 関数名を取得する
    /// Invoke系の関数の文字列書くのが間違えそうで嫌なので作った
    /// </summary>
    /// <param name="action">関数</param>
    /// <returns>関数名（string）</returns>
    public static string GetMethodName(Action action)
    {
        return action.Method.Name;
    }
}
