using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// RequireComponent ... �w�肵���R���|�[�l���g�������Œǉ��i�ǉ����Y��ȂǂɗL�p�j
[RequireComponent(typeof(LineRenderer))]
public class PathGennerator : MonoBehaviour
{
	private LineRenderer line;
	private Camera mainCamera;
	private GameObject objPoint;
	private int ObjectNum;
	private int ClickNum;
	private Vector3 vPos;
	private Vector3 vOldPos;

	// SerializeFieldAttribute ... UnityEditer���Œl�̕ύX���\�ɂ���
	// TooltipAttribute ... UnityEditer���Ń}�E�X�I�[�o�[�����ۂ̐�����
	[SerializeField] float PointSize = 0.05f;
	[SerializeField] float LineWidth = 0.05f;
	[SerializeField, Tooltip("�|�C���g���m�̍ŏ�����(float)")] float Distance = 10.0f;

	void Start()
	{
		this.line = GetComponent<LineRenderer>();
		this.line.startWidth = LineWidth;
		this.line.endWidth = LineWidth;
		this.line.positionCount = 0;
		//this.line.material = (Material)Resources.Load("Materials/Point", typeof(Material));
		this.line.material = new Material(Shader.Find("Sprites/Default"));
		this.line.startColor = Color.green;
		this.line.endColor = Color.green;

		mainCamera = Camera.main;
		objPoint = (GameObject)Resources.Load("Objects/Point");     // Resources�t�H���_����ǂݍ���
		ObjectNum = 1;
		ClickNum = -1;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ClickNum++;
		}
		//if (Input.GetMouseButtonUp(0))
		//{
		//	ObjectNum = 1;
		//}

		if (Input.GetMouseButton(0))
		{
			// �}�E�X���W�����AZ�l�Œ�
			vPos = Input.mousePosition;
			vPos.z = 10.0f;

			// �Ԋu�����l�ȏ�Ȃ�ΐ�������
			if (Vector3.Distance(vPos, vOldPos) >= Distance)
			{
				GameObject cloneObject = Instantiate(objPoint, vPos, Quaternion.identity);          // ���W�����ɃI�u�W�F�N�g�𐶐�
				cloneObject.transform.parent = this.transform;                                      // ���̃X�N���v�g���K�p����Ă���I�u�W�F�N�g�̎q�ɂȂ�
				cloneObject.name = "Point(" + ClickNum + " ," + (ObjectNum - 1) + ")";                    // �I�u�W�F�N�g����ύX
				cloneObject.AddComponent<Image>();                                                  // Image�R���|�[�l���g��ǉ�
				cloneObject.transform.localScale = new Vector3(PointSize, PointSize, PointSize);    // �X�P�[����ύX

				// �}�E�X�X�N���[�����W�����[���h���W�ɒ���
				vPos = mainCamera.ScreenToWorldPoint(vPos);
				//// ����Ƀ��[�J�����W�ɒ����B
				//vPos = transform.InverseTransformPoint(vPos);

				//--- ���_����ǉ�
				ObjectNum++;
				this.line.positionCount = ObjectNum;

				// �ŏ��̒��_�̏���
				if (ObjectNum == 2)
				{
					this.line.SetPosition(0, vPos);
					this.line.SetPosition(ObjectNum - 1, vPos);
				}
				else
				{
					//this.line.SetPosition(ObjectNum - 1, cloneObject.transform.position);				// ���[���h���W�ʒu�ɐ���
					this.line.SetPosition(ObjectNum - 1, vPos);                                         // �X�N���[�����W�ʒu�ɐ���
				}

				vOldPos = cloneObject.transform.position;
			}
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			//---�m�菈��
			IList<Vector2> posList = new List<Vector2>();
			Vector3[] pos = new Vector3[line.positionCount];
			line.GetPositions(pos);

			for (int i = line.positionCount - 1; i >= 0; i--)
			{
				Debug.Log("pos[" + i + "](" + pos[i].x + ", " + pos[i].y + ", " + pos[i].z);
				//Debug.Log("pos[" + i + "](" + pos);
			}
			for (int j = line.positionCount - 1; j >= 0; j--)
			{
				posList.Add(new Vector2(pos[j].x, pos[j].y));
			}

			var cutObject = GameObject.Find("Image").GetComponent<SpriteJigsaw.SpriteJigsaw>();
			cutObject.Cut(posList);

			// �q�I�u�W�F�N�g��S����
			foreach (Transform child in gameObject.transform)
			{
				Destroy(child.gameObject);
			}

			this.line.positionCount = 0;
			ObjectNum = 1;
			ClickNum = -1;
			vOldPos = new Vector3(0.0f, 0.0f, 0.0f);

		}
	}
}
