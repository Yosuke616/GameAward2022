/*
 2022/3/26 ShimizuYosuke�@�d���̓�������邽�߂̃X�N���v�g
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiary_Script : MonoBehaviour
{
    //�萔��`=================================================
    //�d���̏�Ԃ�����
    public enum FIARY_MOVE {
        FIARY_PLAYER_TRACKING = 0,
        FIARY_BREAK_PAPER,

        FIARY_MAX
    }

    //�񋓑��������ϐ�
    FIARY_MOVE g_FiaryMove;
    
    //=========================================================

    // Start is called before the first frame update
    void Start()
    {
        //�񋓑�������������(�ŏ��̓v���C���[�̎����Ǐ]����)
        g_FiaryMove = FIARY_MOVE.FIARY_PLAYER_TRACKING;

        //���̃I�u�W�F�N�g�̐e�̏����擾����
        GameObject ParentObj = this.transform.parent.gameObject;

        Vector3 PlayerPos = ParentObj.transform.position;

        //�v���C���[�̏������Ɉړ�������
        this.transform.position = new Vector3(PlayerPos.x+1.0f, PlayerPos.y + 1.0f, PlayerPos.z) ;
    }

    // Update is called once per frame
    void Update()
    {
        //��Ԃɂ���čX�V�̓��e��ς���
        switch (g_FiaryMove) {
            case FIARY_MOVE.FIARY_PLAYER_TRACKING:
                break;
            case FIARY_MOVE.FIARY_BREAK_PAPER:
                break;
            default:break;
        }


    }
}
