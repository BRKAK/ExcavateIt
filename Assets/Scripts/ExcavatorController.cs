using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcavatorController : MonoBehaviour
{
	[SerializeField] GameObject palette;
	[SerializeField] GameObject cabine;
	[SerializeField] GameObject boom;
	[SerializeField] GameObject stick;
	[SerializeField] GameObject bucket; 

	[SerializeField] float paletteMovementSpeed;
	[SerializeField] float cabineMovementSpeed;
	[SerializeField] float boomMovementSpeed;
	[SerializeField] float stickMovementSpeed;
	[SerializeField] float bucketMovementSpeed;

	[SerializeField] Transform leftPalettePivot;
	[SerializeField] Transform righttPalettePivot;

	[SerializeField] Rigidbody rb;

	void Update()
	{
		leftJoystickMovement();
		rightJoystickMovement();
	}

	private void FixedUpdate() {
		pedalMovement();
	}

	void leftJoystickMovement(){

		if (Mathf.Abs(Input.GetAxis("LeftVertical")) > Mathf.Epsilon){
			// stick goes up W, and down S (works with v3.1)
			//Debug.Log("LEFT stick vertical");
			stick.transform.Rotate(Input.GetAxis("LeftVertical") * stickMovementSpeed, 0.0f, 0.0f);
		}

		if (Mathf.Abs(Input.GetAxis("LeftHorizontal")) > Mathf.Epsilon){
			// Swing rotates A left, D right (works with v3.1)
			//Debug.Log("LEFT stick horitonzal");
			cabine.transform.Rotate(0.0f, Input.GetAxis("LeftHorizontal") * cabineMovementSpeed, 0.0f);
		} 
	}

	void rightJoystickMovement(){
		if (Mathf.Abs(Input.GetAxis("RightVertical")) > Mathf.Epsilon){
			// boom goes down UpArrow, down DownArrow (works with 3.1)
			//Debug.Log("RIGHT stick vertical");
			boom.transform.Rotate(Input.GetAxis("RightVertical") * boomMovementSpeed * -1, 0.0f, 0.0f);
		}

		if (Mathf.Abs(Input.GetAxis("RightHorizontal")) > Mathf.Epsilon){
			// bucket closes leftArrow, opens RightArrow (works with 3.1)
			//Debug.Log("RIGHT stick horizontal");
			bucket.transform.Rotate(Input.GetAxis("RightHorizontal") * bucketMovementSpeed * -1, 0.0f, 0.0f);
		} 
	}

	 void pedalMovement(){
		Vector3 movementVec = new Vector3(0,0,0);

		if (Mathf.Abs(Input.GetAxis("RightPedal")) > Mathf.Epsilon && Mathf.Abs(Input.GetAxis("LeftPedal")) > Mathf.Epsilon){
			movementVec += new Vector3(Input.GetAxis("RightPedal") + Input.GetAxis("LeftPedal"),0.0f,0.0f);
		} else {
			//movementVec += new Vector3(Input.GetAxis("RightPedal") + Input.GetAxis("LeftPedal"),0.0f, Mathf.Abs(Input.GetAxis("RightPedal")) - Mathf.Abs(Input.GetAxis("LeftPedal")) );
		}
		
		rb.AddForce(movementVec * paletteMovementSpeed * Time.fixedDeltaTime);
	}

	

	
}
