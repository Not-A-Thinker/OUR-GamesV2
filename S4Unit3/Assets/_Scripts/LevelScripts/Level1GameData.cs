using UnityEngine;

/// <summary>
/// �o�����ӥu�s��v�T�����d�������ܼƼƭ�
/// </summary>
public class Level1GameData : MonoBehaviour
{
    //�Ω󱱨�Boss�L���ɪ��a���|�Y��ˮ`,�H�βM�����W��L���Ӯ���������
    static public bool b_isCutScene = false;
    //�ˬdBoss�O�_�����F
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
    }

    void MainGameTimer()
    {
        f_timerS += Time.deltaTime;
        if (f_timerS >= 60)
        {
            f_timerM++;
            f_timerS = 0;
        }
        Debug.Log(f_timerM + "m: " + (int)f_timerS + "s");
    }

    static void PrintTime()
    {

    }
}
