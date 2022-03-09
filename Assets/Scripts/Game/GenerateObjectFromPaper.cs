/* UI���W���擾�A���[���h���W�֕ϊ�������
 * �Q�l���Fhttps://alien-program.hatenablog.com/entry/2017/08/06/164258
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

		// �q�I�u�W�F�N�g�����X�g�Ɋi�[
		foreach (Transform cTransform in this.gameObject.transform)
		{
			// �V�[����ɑ��݂���I�u�W�F�N�g�Ȃ�Ώ���
			// if (obj.activeInHierarchy)
			// {
			ChildPaper.Add(cTransform.gameObject);
			// }
		}
		//for (int i = 0; i < ChildObject.Count; i++)
		//{
		//	Debug.Log($"List[{i}]:{ChildObject[i].name}");
		//}

		// ������I�u�W�F�N�g����
		foreach (GameObject PaperObject in ChildPaper)
		{
			// ��������I�u�W�F�N�g
			GameObject BookObject = null;

			// �ǉ�����R���|�[�l���g���X�g
			Components = new List<System.Type>();

			// Prefab�I�u�W�F�N�g�𗘗p���邩
			bool isPrefab = false;

			// Prefab�I�u�W�F�N�g�𗘗p���Ȃ��ꍇ�̃I�u�W�F�N�g�^�C�v
			PrimitiveType primitiveType = (PrimitiveType)(-1);

			// ���W�֘A
			RectTransform rect = PaperObject.GetComponent<RectTransform>();
			Vector3 pos = rect.position;

			// �p�x
			Quaternion rot = new Quaternion(0.123456789f, 0.123456789f, 0, 0);

			// �c���֘A
			Vector3 scl = new Vector3(PaperObject.GetComponent<RectTransform>().sizeDelta.x,
									�@PaperObject.GetComponent<RectTransform>().sizeDelta.y, 0.0f);
			scl = this.transform.TransformPoint(scl);
			scl = new Vector3(scl.x, scl.y - 1, scl.x);  // y�̒l��-1����i�Ӗ��s���j


			switch (PaperObject.tag)
			{
				case "���{":
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

				foreach (System.Type AddCompo in Components)
				{
					BookObject.AddComponent(AddCompo);
				}
			}

			// �R���|�[�l���g�̌ʐݒ�p
			switch (PaperObject.tag)
			{
				case "Player":
					// �I�u�W�F�N�g��]����
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
						//	new Vector3(vec2[3].x, vec2[3].y,  zScale / 2),		// �O����
						//	new Vector3(vec2[2].x, vec2[2].y,  zScale / 2),		// �O�E��
						//	new Vector3(vec2[1].x, vec2[1].y,  zScale / 2),		// �O�E��
						//	new Vector3(vec2[0].x, vec2[0].y,  zScale / 2),		// �O����
						//	new Vector3(vec2[0].x, vec2[0].y, -zScale / 2),		// �㍶��
						//	new Vector3(vec2[1].x, vec2[1].y, -zScale / 2),		// ��E��
						//	new Vector3(vec2[2].x, vec2[2].y, -zScale / 2),		// ��E��
						//	new Vector3(vec2[3].x, vec2[3].y, -zScale / 2),		// �㍶��
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


			// �R�s�[
			BookObject.name = PaperObject.name;
			BookObject.tag = PaperObject.tag;

			// "in the Book Object"�̎q�I�u�W�F�N�g�ɂȂ�
			BookObject.transform.parent = GameObject.FindWithTag("BookObjectParent").transform;
			//BookObject.transform.parent = GameObject.Find("in the Book Object").transform; // GameObject.Find�͏d���̂Ŕp�~

			ChildObject.Add(BookObject);
			BookObject.SetActive(false);
		}

		Active = false;
	}

	void Update()
	{
		// ���A�I�u�W�F�N�g�̐؂�ւ�
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
