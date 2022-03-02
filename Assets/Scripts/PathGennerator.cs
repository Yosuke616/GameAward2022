using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// RequireComponent ... 指定したコンポーネントを自動で追加（追加し忘れなどに有用）
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

	// SerializeFieldAttribute ... UnityEditer側で値の変更を可能にする
	// TooltipAttribute ... UnityEditer側でマウスオーバーした際の説明文
	[SerializeField] float PointSize = 1.0f;
	[SerializeField] float LineWidth = 1.0f;
	[SerializeField, Tooltip("ポイント同士の最小距離(float)")] float Distance = 1.0f;

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
		objPoint = (GameObject)Resources.Load("Objects/Point");     // Resourcesフォルダから読み込み
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
			// マウス座標を代入、Z座標の値を追加（
			vMousePos = Input.mousePosition;
			vMousePos.z = 10.0f;

			// 間隔が一定値以上ならば生成処理
			if (Vector3.Distance(vMousePos, vOldMousePos) >= Distance)
			{
				vOldMousePos = vMousePos;

				// マウススクリーン座標をワールド座標に直す（Z座標がないとおかしくなるので注意）
				vMousePos = mainCamera.ScreenToWorldPoint(vMousePos);
				//さらにローカル座標に直す。
				//vMousePos = transform.InverseTransformPoint(vMousePos);

				// point生成処理
				{
					GameObject cloneObject = Instantiate(objPoint, vMousePos, Quaternion.identity);		// 座標を元にオブジェクトを生成
					cloneObject.transform.parent = this.transform;										// このスクリプトが適用されているオブジェクトの子になる
					cloneObject.name = "Point(" + ClickNum + " ," + (ObjectNum - 1) + ")";				// オブジェクト名を変更
					cloneObject.AddComponent<Image>();													// Imageコンポーネントを追加
					cloneObject.transform.localScale = new Vector3(PointSize, PointSize, PointSize);	// スケールを変更
				}

				// line追加処理
				{
					//--- 頂点数を追加
					ObjectNum++;
					this.line.positionCount = ObjectNum;

					// 最初の頂点の処理
					if (ObjectNum == 2)
					{
						this.line.SetPosition(0, vMousePos);
						this.line.SetPosition(ObjectNum - 1, vMousePos);
					}
					else
					{
						//this.line.SetPosition(ObjectNum - 1, cloneObject.transform.position);               // ワールド座標位置に生成
						this.line.SetPosition(ObjectNum - 1, vMousePos);                                         // スクリーン座標位置に生成
					}
				}
			}

		}

		//---確定処理
		if (Input.GetKeyDown(KeyCode.Return))
		{
			IList<Vector2> posList = new List<Vector2>();
			Vector3[] pos = new Vector3[line.positionCount];
			line.GetPositions(pos);

			for (int j = line.positionCount - 1; j >= 0; j--)
			{
				Debug.Log($"pos[{j}]({pos[j].x}, {pos[j].y}, {pos[j].z})");		// {}で変数を記述可能
				posList.Add(new Vector2(pos[j].x, pos[j].y));
			}
			for (int k = line.positionCount - 1; k >= 0; k--)
			{
				Debug.Log($"List[{k}]({posList[k].x}, {posList[k].y})");
			}

			var cutObject = GameObject.Find("Image").GetComponent<SpriteJigsaw.SpriteJigsaw>();
			cutObject.Cut(posList);

			// 子オブジェクトを全消去
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
