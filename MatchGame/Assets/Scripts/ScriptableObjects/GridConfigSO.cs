using UnityEngine;

[CreateAssetMenu(menuName = "Card Match/Grid Config")]
public class GridConfigSO : ScriptableObject
{
    public int rows;
    public int cols;
    public float spacingX;
    public float spacingY;
    public float paddingX; // left + right margin
    public float paddingY; // top + bottom margin
}