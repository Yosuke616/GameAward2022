/* UI���W���擾�A���[���h���W�֕ϊ�
 * �Q�l���Fhttps://alien-program.hatenablog.com/entry/2017/08/06/164258
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjectFromPaper : MonoBehaviour
{
	private Camera mainCamera;
	private List<GameObject> ChildObject;

	void Start()
	{
		mainCamera = Camera.main;
		ChildObject = new List<GameObject>();
		Debug.Log($"<ChildObject>");

		// �q�I�u�W�F�N�g���擾
		foreach (Transform cTransform in this.gameObject.transform)
		{
			// �V�[����ɑ��݂���I�u�W�F�N�g�Ȃ�Ώ���.
			// if (obj.activeInHierarchy)
			// {
			ChildObject.Add(cTransform.gameObject);
			// }
		}
		//for (int i = 0; i < ChildObject.Count; i++)
		//{
		//	Debug.Log($"List[{i}]:{ChildObject[i].name}");
		//}

		foreach (GameObject PaperObject in ChildObject)
		{
			GameObject BookObject;

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
			Vector3 sclx = Vector3.zero;
			Vector3 scly = Vector3.zero;
			sclx.x = SizeDelta.x;
			scly.y = SizeDelta.y;
			Debug.Log($"x({sclx.x},{sclx.y},{sclx.z}),y({scly.x},{scly.y},{scly.z})");
			sclx = this.transform.TransformPoint(sclx);
			scly = this.transform.TransformPoint(scly);
			Debug.Log($"x({sclx.x},{sclx.y},{sclx.z}),y({scly.x},{scly.y},{scly.z})");
			scl = new Vector3(sclx.x, scly.y, 0.0f);

			switch (PaperObject.tag)
			{
				case "Player":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					rot = Quaternion.identity;
					break;
				case "Ground":
					isPrefab = false;
					primitiveType = PrimitiveType.Plane;
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
			//PaperObject.transform.localScale = this.transform.transform.localScale / 100;
			//BookObject.transform.localScale = new Vector3(this.GetComponent<RectTransform>().localScale.x / 100, this.GetComponent<RectTransform>().localScale.y / 100, 0.01f);
			BookObject.transform.parent = GameObject.Find("in the Book Object").transform;
		}
	}

    // Update is called once per frame
    void Update()
    {

    }
}
