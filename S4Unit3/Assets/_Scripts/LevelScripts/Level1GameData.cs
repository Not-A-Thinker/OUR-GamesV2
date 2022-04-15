using UnityEngine;

/// <summary>
/// 這個應該只存放影響本關卡的全域變數數值
/// </summary>
public class Level1GameData : MonoBehaviour
{
    //用於控制Boss過場時玩家不會吃到傷害,以及清除場上其他應該消失的物件
    public static bool b_isCutScene = false;


    void Start()
    {
        b_isCutScene = true;
    }


    void Update()
    {
        
    }
}
