/* UI座標を取得、ワールド座標へ変換
 * 参考元：https://alien-program.hatenablog.com/entry/2017/08/06/164258
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

		// 子オブジェクトを取得
		foreach (Transform cTransform in this.gameObject.transform)
		{
			// シーン上に存在するオブジェクトならば処理.
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
			// 座標
			Vector3 pos = rect.position;
			// 角度
			Quaternion rot = new Quaternion(0.123456789f, 0.123456789f, 0, 0);
			// 縦横
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
				// prefabを使う場合は後ほど追加予定
				BookObject = Instantiate(GameObject.CreatePrimitive(primitiveType), this.transform.position, Quaternion.identity);
			}
			else
			{
				//UI座標からスクリーン座標に変換
				Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, pos);
				//スクリーン座標→ワールド座標に変換
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
