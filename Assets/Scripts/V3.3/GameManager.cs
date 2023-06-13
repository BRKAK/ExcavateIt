using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private bool gamePaused = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Girdi");
			if (gamePaused)
				ResumeGame();
			else
				PauseGame();
		}
	}

	public void PauseGame()
	{
		gamePaused = true;
		Time.timeScale = 0f;
	}

	public void ResumeGame()
	{
		gamePaused = false;
		Time.timeScale = 1f;
	}
}
