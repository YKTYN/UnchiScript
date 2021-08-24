
/// <summary>
/// 定数置き場
/// </summary>
public class Fixed
{
    public enum LEVEL
    {
        easy = 0,
        normal,
        hard
    }

    public const float left = -10.0f;
    public const float right = 10.0f;
    public const float top = 6.0f;
    public const float bottom = -4.0f;

    public const float slow = 0.25f;

    public const string genName = "generator";

    public const string unTag = "unTag";
    public const string enemyTag = "Enemy";
    public const string playerTag = "Player";
    public const string bulletTag = "Bullet";

    public const string levelKey = "level";
    public const string resultKey = "result";
}
