using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	private float respawnTimer;

	private const float RESPAWN_DURATION = 5;

	[SerializeField]
	private Collider boxCollider;

	public void OnPlucked()
	{
		boxCollider.enabled = false;
		respawnTimer = RESPAWN_DURATION;
		transform.localScale = Vector3.zero;
	}

	private void Update()
	{

		if (respawnTimer <= 0)
		{
			if (transform.localScale.x < 10)
			{
				transform.localScale += 0.01f * Vector3.one;

				if(transform.localScale.x >= 10){
				boxCollider.enabled = true;
				transform.localScale = Vector3.one * 10;
				}
			}
		}
		else
		{
			respawnTimer -= UnityEngine.Time.deltaTime;
		}



	}

}