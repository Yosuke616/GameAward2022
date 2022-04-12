/* UI座標を取得、ワールド座標へ変換し生成
 * 参考元：https://alien-program.hatenablog.com/entry/2017/08/06/164258
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjectFromPaper : MonoBehaviour
{
	[SerializeField] float zScale = 100.0f;

	private Camera mainCamera;
	private List<GameObject> ChildPaper;
	private List<GameObject> ChildObject;
	private List<System.Type> Components;
	private bool Active;

	void Start()
	{
		Debug.Log($"<GenerateObjectFromPaper> - Start");

		mainCamera = Camera.main;
		ChildPaper = new List<GameObject>();
		ChildObject = new List<GameObject>();

		// 子オブジェクトをリストに格納
		foreach (Transform cTransform in this.gameObject.transform)
		{
			// シーン上に存在するオブジェクトならば処理
			// if (obj.activeInHierarchy)
			// {
			ChildPaper.Add(cTransform.gameObject);
			// }
		}
		//for (int i = 0; i < ChildObject.Count; i++)
		//{
		//	Debug.Log($"List[{i}]:{ChildObject[i].name}");
		//}

		// 紙からオブジェクト生成
		foreach (GameObject PaperObject in ChildPaper)
		{
			// 生成するオブジェクト
			GameObject BookObject = null;

			// 追加するコンポーネントリスト
			Components = new List<System.Type>();

			// Prefabオブジェクトを利用するか
			bool isPrefab = false;

			// Prefabオブジェクトを利用しない場合のオブジェクトタイプ
			PrimitiveType primitiveType = (PrimitiveType)(-1);

			// 座標関連
			RectTransform rect = PaperObject.GetComponent<RectTransform>();
			Vector3 pos = rect.position;

			// 角度
			Quaternion rot = new Quaternion(0.123456789f, 0.123456789f, 0, 0);

			// 縦横関連
			Vector3 scl = new Vector3(PaperObject.GetComponent<RectTransform>().sizeDelta.x,
									　PaperObject.GetComponent<RectTransform>().sizeDelta.y, 0.0f);
			scl = this.transform.TransformPoint(scl);
			scl = new Vector3(scl.x, scl.y - 1, scl.x);  // yの値を-1する（意味不明）


			switch (PaperObject.tag)
			{
				case "見本":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					rot = Quaternion.identity;
					Components.Add(typeof(Rigidbody));

					break;
				case "Player":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
					rot = Quaternion.identity;

					Components.Add(typeof(PlayerMove));
					Components.Add(typeof(Rigidbody));
					break;
				case "Ground":
					isPrefab = false;
					primitiveType = PrimitiveType.Cube;
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

				foreach (System.Type AddCompo in Components)
				{
					BookObject.AddComponent(AddCompo);
				}
			}

			// コンポーネントの個別設定用
			switch (PaperObject.tag)
			{
				case "Player":
					// オブジェクト回転無効
					BookObject.GetComponent<Rigidbody>().freezeRotation = true;
					break;
				case "Ground":
					var vec2 = PaperObject.GetComponent<PolygonCollider2D>().points;
					foreach (var vec in vec2)
					{
						Debug.Log($"vertex:{vec.x},{vec.y}");
					}

					// https://zenn.dev/daichi_gamedev/articles/76c98eceb2294b
					{
						//Vector3[] vertices = {
						//	new Vector3(vec2[3].x, vec2[3].y,  zScale / 2),		// 前左下
						//	new Vector3(vec2[2].x, vec2[2].y,  zScale / 2),		// 前右下
						//	new Vector3(vec2[1].x, vec2[1].y,  zScale / 2),		// 前右上
						//	new Vector3(vec2[0].x, vec2[0].y,  zScale / 2),		// 前左上
						//	new Vector3(vec2[0].x, vec2[0].y, -zScale / 2),		// 後左上
						//	new Vector3(vec2[1].x, vec2[1].y, -zScale / 2),		// 後右上
						//	new Vector3(vec2[2].x, vec2[2].y, -zScale / 2),		// 後右下
						//	new Vector3(vec2[3].x, vec2[3].y, -zScale / 2),		// 後左下
						//	};
						Vector3[] vertices = {
					new Vector3 (-0.5f, -0.5f, -0.5f),
					new Vector3 (0.5f, -0.5f, -0.5f),
					new Vector3 (0.5f, 0.5f, -0.5f),
					new Vector3 (-0.5f, 0.5f, -0.5f),
					new Vector3 (-0.5f, 0.5f, 0.5f),
					new Vector3 (0.5f, 0.5f, 0.5f),
					new Vector3 (0.5f, -0.5f, 0.5f),
					new Vector3 (-0.5f, -0.5f, 0.5f),
					};
						int[] triangles = {
						0, 2, 1, //face front
						0, 3, 2,
						2, 3, 4, //face top
						2, 4, 5,
						1, 2, 5, //face right
						1, 5, 6,
						0, 7, 4, //face left
						0, 4, 3,
						5, 4, 7, //face back
						5, 7, 6,
						0, 6, 7, //face bottom
						0, 1, 6
						};

						//Mesh mesh = BookObject.GetComponent<MeshFilter>().mesh;
						//mesh.Clear();
						//mesh.vertices = vertices;
						//mesh.triangles = triangles;
						//mesh.Optimize();
						//mesh.RecalculateNormals();
					}

					break;
				default:
					break;
			}


			// コピー
			BookObject.name = PaperObject.name;
			BookObject.tag = PaperObject.tag;

			// "in the Book Object"の子オブジェクトになる
			BookObject.transform.parent = GameObject.FindWithTag("BookObjectParent").transform;
			//BookObject.transform.parent = GameObject.Find("in the Book Object").transform; // GameObject.Findは重いので廃止

			ChildObject.Add(BookObject);
			BookObject.SetActive(false);
		}

		Active = false;
	}

	void Update()
	{
		// 紙、オブジェクトの切り替え
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log($"ChangeActive:{Active}→{!Active}");
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
