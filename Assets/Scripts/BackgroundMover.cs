using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
	int counter = 0;
	int step = 500;
	float speed = 30f;

	void Update()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material mat = mr.material;
		Vector2 offset = mat.mainTextureOffset;

 		if( counter >= 0 && counter < step * 1)
        {
			offset.y += Time.deltaTime / speed;
			offset.x += Time.deltaTime / speed;
		}
		else if (counter >= step * 1 && counter < step * 2)
		{
			offset.y += Time.deltaTime / speed;
			offset.x -= Time.deltaTime / speed;
		}
		else if ( counter >= step * 2 && counter < step * 3)
		{
			offset.y -= Time.deltaTime / speed;
			offset.x -= Time.deltaTime / speed;
		}
		else if (counter >= step * 3 && counter < step * 4)
		{
			offset.y -= Time.deltaTime / speed;
			offset.x += Time.deltaTime / speed;
		}
		
		if (counter == step * 4)
		{
			counter = 0;
		} else
        {
			counter++;
		}
		mat.mainTextureOffset = offset;
	}

}
