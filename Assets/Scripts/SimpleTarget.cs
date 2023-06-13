using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTarget : MonoBehaviour
{
	[SerializeField] Material firstStateMaterial;
	[SerializeField] Material secondStateMaterial;
	[SerializeField] AudioClip targetDoneSound;

	AudioSource ar;
	MeshRenderer mr;
	GameController gameController;

	void Start()
	{
		gameController = FindObjectOfType<GameController>();
		ar = GetComponent<AudioSource>();
		mr = GetComponent<MeshRenderer>();
		mr.material = firstStateMaterial;
	}


	private void OnTriggerEnter(Collider other) {
		Debug.Log("touch");
		if (other.gameObject.tag == "bucket"){
			mr.material = secondStateMaterial;
			ar.PlayOneShot(targetDoneSound);
			gameController.increaseScore();
			mr.enabled = false;
			// Todo External Audio manager to avoid redundant scoring
		}
	}
}
