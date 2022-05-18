using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto_Directing : MonoBehaviour
{
	public float MoveNum;
	public float MovePow;
	private Vector3 InitPos;
	private bool bUp;

	void Start()
	{
		InitPos = transform.position;
		bUp = true;
	}
    void Update()
    {
		if (bUp)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + MovePow, transform.position.z);

			if (transform.position.y - InitPos.y > MoveNum)
				bUp = false;
		}

		else
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - MovePow, transform.position.z);

			if (transform.position.y - InitPos.y <= 0)
				bUp = true;
		}
	}
}
