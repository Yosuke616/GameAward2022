using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collsion_test : MonoBehaviour
{
    // �J�����ɉf���Ă��邠���蔻��̂��ƂƂȂ�I�u�W�F�N�g
    private GameObject originalObject = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "paper")
        {
            return;
        }

        if (collider.gameObject.tag != "none")
        {
            //--- �J�����ɉf���Ă���I�u�W�F�N�g�������蔻��t�B�[���h�ɔ��f�����邽�ߏ��𔲂����
            // �^�O
            this.tag = collider.gameObject.tag;
            // �X��
            transform.Rotate(collider.gameObject.transform.localEulerAngles);
            // �J�����ɉf���Ă���I�u�W�F�N�g
            originalObject = collider.gameObject;

            //�G�l�~�[�̏ꍇ�̓G�l�~�[�̋@�\��ǉ�����
            if (collider.gameObject.tag == "enemy")
            {
                //if(collider.gameObject.GetComponent<Enemy>() == null)
                //    collider.gameObject.AddComponent<Enemy>();
            }
            else if (collider.gameObject.tag == "CardSoldier")
            {
                if (collider.gameObject.GetComponent<EnemyBehavior>() == null)
                    collider.gameObject.AddComponent<EnemyBehavior>();
            }
        }
    }

    // �J�����ɉf���Ă��邠���蔻��̂��ƂƂȂ�I�u�W�F�N�g
    public GameObject getOriginalObject()
    {
        return originalObject;
    }

}
