using UnityEngine;
using System.IO;

/// <summary>
/// �o�����ӥu�s��v�T�����d�������ܼƼƭ�
/// </summary>
public class Level1GameData : MonoBehaviour
{
    //�Ω󱱨�Boss�L���ɪ��a���|�Y��ˮ`,�H�βM�����W��L���Ӯ���������
    static public bool b_isCutScene = false;
    //�Ω󱱨�Boss���`�ʵe�L���ɪ��a���|�Y��ˮ`,�H�βM�����W��L���Ӯ���������
    static public bool b_isBossDeathCutScene = false;
    //�ˬdBoss�O�_�����F
    static public bool b_isBoss1Defeated = false;

    //�p�⪱�a���q���ɪ�
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
        b_isBossDeathCutScene = false;

        f_timerS = 0f;
        f_timerM = 0f;
    }

    void Update()
    {
        MainGameTimer();
    }

    static public void ResetData()
    {
        b_isCutScene = false;
        b_isBossDeathCutScene = false;
        b_isBoss1Defeated = false;
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

        string buildPath = Application.persistentDataPath + "/TimesLog.txt";
        StreamWriter writer2 = new StreamWriter(buildPath, true);
        writer2.WriteLine("[" + System.DateTime.UtcNow + "] " + "Game finish in: " + f_timerM + "m:" + (int)f_timerS + "s");
        writer2.Close();

        string SAPath = Application.streamingAssetsPath + "/TimesLog.txt";
        StreamWriter writer3 = new StreamWriter(SAPath, true);
        writer3.WriteLine("[" + System.DateTime.UtcNow + "] " + "Game finish in: " + f_timerM + "m:" + (int)f_timerS + "s");
        writer3.Close();

        Debug.Log("Game finish in:" + f_timerM + "m: " + (int)f_timerS + "s");
    }

    static public void ClearTime()
    {
        f_timerS = 0f;
        f_timerM = 0f;
    }
}
