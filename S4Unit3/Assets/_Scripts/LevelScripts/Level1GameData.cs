using UnityEngine;

/// <summary>
/// �o�����ӥu�s��v�T�����d�������ܼƼƭ�
/// </summary>
public class Level1GameData : MonoBehaviour
{
    //�Ω󱱨�Boss�L���ɪ��a���|�Y��ˮ`,�H�βM�����W��L���Ӯ���������
    static public bool b_isCutScene = false;
    [SerializeField] bool isCutScene;

    void Start()
    {
        b_isCutScene = false;
    }


    void Update()
    {
        
    }
}
