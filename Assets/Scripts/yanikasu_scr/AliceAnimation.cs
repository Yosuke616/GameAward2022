using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceAnimation : MonoBehaviour
{
	private ModelAnimation animation;

	// Start is called before the first frame update
	void Start()
	{
		animation = GetComponent<ModelAnimation>();
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.A))
		{
			animation.SetAnim("stand-by");
		}

		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	animation.SetAnim("walk");
		//}
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	animation.SetAnim("clear");
		//}
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	animation.SetAnim("miss");
		//}
	}
}
