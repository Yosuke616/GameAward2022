using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjectFromPaper : MonoBehaviour
{
	private List<GameObject> ChildObject;

	void Start()
	{
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
		for (int i = 0; i < ChildObject.Count; i++)
		{
			Debug.Log($"List[{i}]:{ChildObject[i].name}");
		}

		foreach (GameObject PaperObject in ChildObject)
		{
			GameObject BookObject;

			bool isPrefab = false;
			PrimitiveType primitiveType = PrimitiveType.Cube;
			Vector3 pos = new Vector3(0.123456789f, 0.987654321f, 0);
			Quaternion rot = new Quaternion(0.123456789f, 0.987654321f, 0, 0);

			switch(PaperObject.tag)
			{
				case "Player":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					pos = PaperObject.transform.position;
					rot = Quaternion.identity;
					break;
				case "Ground":
					isPrefab = false;
					primitiveType = PrimitiveType.Plane;
					pos = PaperObject.transform.position;
					Debug.Log($":{PaperObject.GetComponent<RectTransform>().sizeDelta.y / 2}");
					Debug.Log($":{pos.y}");
					pos.y += PaperObject.GetComponent<RectTransform>().sizeDelta.y / 2;
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
				BookObject = GameObject.CreatePrimitive(primitiveType);
				BookObject.transform.position = pos;
				BookObject.transform.rotation = rot;
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
