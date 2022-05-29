using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Animator rabbitAnimator;
    public GameObject openingCamera;

    private bool onceFunc;
	private ModelAnimation rabbitAnimation;

	// Start is called before the first frame update
	void Start()
    {
        onceFunc = true;

		// �q�I�u�W�F�N�g����R���|�[�l���g���擾
		rabbitAnimation = transform.Find("Usagi_Anim_Run").gameObject.GetComponent<ModelAnimation>();
	}

    // Update is called once per frame
    void Update()
    {
        // ���݂̃A�j���[�V������Ԃ��擾
        var state = rabbitAnimator.GetCurrentAnimatorStateInfo(0); // ������Animator��Layers�̔ԍ�

        if(onceFunc && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1")))
        {
            rabbitAnimator.SetFloat("Speed", 100);
            onceFunc = false;

            //rabbitAnimation.SetAnim("Take 001");

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
        }

        // �A�j���[�V������Ԃ�"Move" �� "Idle"�ɑJ�ڂ����ꍇ
        if(onceFunc && state.IsName("RabbitIdle"))
        {
            onceFunc = false;

			//rabbitAnimation.SetAnim("Take 001");

            openingCamera.GetComponent<ZoomOut>().ZoomStart();
            // �I�[�v�j���O�J�����̃Y�[���A�E�g�@�\��ON�ɂ���
           // Debug.LogWarning("�ҋ@����");
		}
    }
}
