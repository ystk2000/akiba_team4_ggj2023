using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

	public　bool FullyGrown => transform.localScale.x == 10;
	public bool Growing => !FullyGrown && respawnTimer <= 0;
	public bool IsWatered => watered;
	private float respawnTimer;
	private bool watered = false;
	private const float RESPAWN_DURATION = 5;

	[SerializeField]
	private Collider boxCollider;

	public void OnPlucked()
	{
		boxCollider.enabled = false;
		respawnTimer = RESPAWN_DURATION;
		transform.localScale = Vector3.zero;
		watered = false;
	}
	
	public void OnWatered()
	{
		watered = true;
	}

	private void Update()
	{
		if (respawnTimer <= 0)
		{
			if (transform.localScale.x < 10)
			{
				float growRate = watered == false ? 0.01f : 0.05f;
				transform.localScale += growRate * Vector3.one;

				if(transform.localScale.x >= 10){
				transform.localScale = Vector3.one * 10;
				}
			}
		}
		else
		{
			respawnTimer -= UnityEngine.Time.deltaTime;
			if(respawnTimer <= 0){				
				boxCollider.enabled = true;
			}
		}
	}
}