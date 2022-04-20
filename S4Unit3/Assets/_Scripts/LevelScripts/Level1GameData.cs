using UnityEngine;
using System.IO;

/// <summary>
/// 這個應該只存放影響本關卡的全域變數數值
/// </summary>
public class Level1GameData : MonoBehaviour
{
    //用於控制Boss過場時玩家不會吃到傷害,以及清除場上其他應該消失的物件
    static public bool b_isCutScene = false;
    //檢查Boss是否死掉了
    static public bool b_isBoss1Defeated = false;

    static public float f_timerS = 0f;
    static public float f_timerM = 0f;

    public bool Boss1Defeated()
    {
        return b_isBoss1Defeated;
    }

    void Start()
    {
        b_isCutScene = false;
        b_isBoss1Defeated = false;

        f_timerS = 0f;
        f_timerM = 0f;
    }

    void Update()
    {
        MainGameTimer();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PrintTime();
        }
    }

    void MainGameTimer()
    {
        f_timerS += Time.deltaTime;
        if (f_timerS >= 60)
        {
            f_timerM++;
            f_timerS = 0;
        }
        //Debug.Log(f_timerM + "m: " + (int)f_timerS + "s");
    }

    static public void PrintTime()
    {
        string path = "Assets/_System/Debug/timetest.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("[" + System.DateTime.UtcNow+ "] " + "Game finish in: " + f_timerM + "m:" + (int)f_timerS + "s");
        writer.Close();

        string sysPath = Application.dataPath + "/TimesLog.txt";
        StreamWriter writer2 = new StreamWriter(sysPath, true);
        writer2.WriteLine("[" + System.DateTime.UtcNow + "] " + "Game finish in: " + f_timerM + "m:" + (int)f_timerS + "s");
        writer2.Close();

        Debug.Log("Game finish in:" + f_timerM + "m: " + (int)f_timerS + "s");
    }

    static public void ClearTime()
    {
        f_timerS = 0f;
        f_timerM = 0f;
    }
}
