/* UI���W���擾�A���[���h���W�֕ϊ�
 * �Q�l���Fhttps://alien-program.hatenablog.com/entry/2017/08/06/164258
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjectFromPaper : MonoBehaviour
{
	private Camera mainCamera;
	private List<GameObject> ChildPaper;
	private List<GameObject> ChildObject;
	private bool Active;

	void Start()
	{
		Debug.Log($"<ChildObject>");

		mainCamera = Camera.main;
		ChildPaper = new List<GameObject>();
		ChildObject = new List<GameObject>();

		// �q�I�u�W�F�N�g���擾
		foreach (Transform cTransform in this.gameObject.transform)
		{
			// �V�[����ɑ��݂���I�u�W�F�N�g�Ȃ�Ώ���.
			// if (obj.activeInHierarchy)
			// {
			ChildPaper.Add(cTransform.gameObject);
			// }
		}
		//for (int i = 0; i < ChildObject.Count; i++)
		//{
		//	Debug.Log($"List[{i}]:{ChildObject[i].name}");
		//}

		foreach (GameObject PaperObject in ChildPaper)
		{
			GameObject BookObject = new GameObject();

			bool isPrefab = false;
			PrimitiveType primitiveType = (PrimitiveType)(-1);
			RectTransform rect = PaperObject.GetComponent<RectTransform>();
			// ���W
			Vector3 pos = rect.position;
			// �p�x
			Quaternion rot = new Quaternion(0.123456789f, 0.123456789f, 0, 0);
			// �c��
			Vector2 SizeDelta = PaperObject.GetComponent<RectTransform>().sizeDelta;
			Vector3 scl = new Vector3(SizeDelta.x, SizeDelta.y, 0.0f);
			scl = this.transform.TransformPoint(scl);
			scl = new Vector3(scl.x, scl.y - 1, scl.x);  // y�̒l��-1����i�Ӗ��s���j

			switch (PaperObject.tag)
			{
				case "Player":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					rot = Quaternion.identity;
					break;
				case "Ground":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					//pos.y += rect.sizeDelta.y / 2;
					rot = Quaternion.identity;
					break;
				default:
					break;
			}

			if (isPrefab)
			{
				// prefab���g���ꍇ�͌�قǒǉ��\��
				BookObject = Instantiate(GameObject.CreatePrimitive(primitiveType), this.transform.position, Quaternion.identity);
			}
			else
			{
				//UI���W����X�N���[�����W�ɕϊ�
				Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, pos);
				//�X�N���[�����W�����[���h���W�ɕϊ�
				RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, mainCamera, out pos);

				BookObject = GameObject.CreatePrimitive(primitiveType);
				BookObject.transform.position = pos;
				BookObject.transform.rotation = rot;
				BookObject.transform.localScale = scl;
			}

			BookObject.name = PaperObject.name;
			BookObject.tag = PaperObject.tag;
			BookObject.transform.parent = GameObject.Find("in the Book Object").transform;

			ChildObject.Add(BookObject);
			BookObject.SetActive(false);
		}

		Active = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log($"ChangeActive:{Active}��{!Active}");
			Active = !Active;
			foreach (GameObject PaperObject in ChildPaper)
			{
				PaperObject.SetActive(!Active);
			}
			foreach (GameObject Object in ChildObject)
			{
				Object.SetActive(Active);
			}
		}
	}
}
