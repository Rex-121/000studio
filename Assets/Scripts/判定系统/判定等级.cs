using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "判定", menuName = "Game/判定等级")]
public class 判定等级 : ScriptableObject
{
    [LabelWidth(80)]
    public string 名称;
    
    [LabelText("判定区间(ms)")][MinMaxSlider(0, 1000, true)][ShowInInspector] 
    private Vector2Int 区间 = new Vector2Int(0, 0);

    
    public 判定等级 判定(float 延迟时间ms)
    {
        var abs = Mathf.Abs(延迟时间ms);
        return abs >= 区间.x && abs <= 区间.y ? this : null; 
    }
}
