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

    void Start()
    {
        b_isCutScene = false;
        b_isBoss1Defeated = false;
    }


    void Update()
    {
        
    }
}
