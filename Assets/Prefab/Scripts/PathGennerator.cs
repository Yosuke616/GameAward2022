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
	private Vector3 vMousePos;
	private Vector3 vOldMousePos;

	// SerializeFieldAttribute ... UnityEditer���Œl�̕ύX���\�ɂ���
	// TooltipAttribute ... UnityEditer���Ń}�E�X�I�[�o�[�����ۂ̐�����
	[SerializeField] float PointSize = 1.0f;
	[SerializeField] float LineWidth = 1.0f;
	[SerializeField, Tooltip("�|�C���g���m�̍ŏ�����(float)")] float Distance = 1.0f;

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
			// �}�E�X���W�����AZ���W�̒l��ǉ��i
			vMousePos = Input.mousePosition;
			vMousePos.z = 10.0f;

			// �Ԋu�����l�ȏ�Ȃ�ΐ�������
			if (Vector3.Distance(vMousePos, vOldMousePos) >= Distance)
			{
				vOldMousePos = vMousePos;

				// �}�E�X�X�N���[�����W�����[���h���W�ɒ����iZ���W���Ȃ��Ƃ��������Ȃ�̂Œ��Ӂj
				vMousePos = mainCamera.ScreenToWorldPoint(vMousePos);
				//����Ƀ��[�J�����W�ɒ����B
				//vMousePos = transform.InverseTransformPoint(vMousePos);

				// point��������
				{
					GameObject cloneObject = Instantiate(objPoint, vMousePos, Quaternion.identity);		// ���W�����ɃI�u�W�F�N�g�𐶐�
					cloneObject.transform.parent = this.transform;										// ���̃X�N���v�g���K�p����Ă���I�u�W�F�N�g�̎q�ɂȂ�
					cloneObject.name = "Point(" + ClickNum + " ," + (ObjectNum - 1) + ")";				// �I�u�W�F�N�g����ύX
					cloneObject.AddComponent<Image>();													// Image�R���|�[�l���g��ǉ�
					cloneObject.transform.localScale = new Vector3(PointSize, PointSize, PointSize);	// �X�P�[����ύX
				}

				// line�ǉ�����
				{
					//--- ���_����ǉ�
					ObjectNum++;
					this.line.positionCount = ObjectNum;

					// �ŏ��̒��_�̏���
					if (ObjectNum == 2)
					{
						this.line.SetPosition(0, vMousePos);
						this.line.SetPosition(ObjectNum - 1, vMousePos);
					}
					else
					{
						//this.line.SetPosition(ObjectNum - 1, cloneObject.transform.position);               // ���[���h���W�ʒu�ɐ���
						this.line.SetPosition(ObjectNum - 1, vMousePos);                                         // �X�N���[�����W�ʒu�ɐ���
					}
				}
			}

		}

		//---�m�菈��
		if (Input.GetKeyDown(KeyCode.Return))
		{
			IList<Vector2> posList = new List<Vector2>();
			Vector3[] pos = new Vector3[line.positionCount];
			line.GetPositions(pos);

			for (int j = line.positionCount - 1; j >= 0; j--)
			{
				Debug.Log($"pos[{j}]({pos[j].x}, {pos[j].y}, {pos[j].z})");		// {}�ŕϐ����L�q�\
				posList.Add(new Vector2(pos[j].x, pos[j].y));
			}
			for (int k = line.positionCount - 1; k >= 0; k--)
			{
				Debug.Log($"List[{k}]({posList[k].x}, {posList[k].y})");
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
			vOldMousePos = new Vector3(0.0f, 0.0f, 0.0f);

		}
	}
}
