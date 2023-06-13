using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
	[SerializeField] GameObject panel;
	[SerializeField]public GameObject titleMenu;
	[SerializeField]public GameObject pauseMenu;
	[SerializeField]public GameObject levelsMenu;

	private GameManager gameManager;

	private bool gamePaused = false;
	private int currentLevelIndex;

	private static UIMenu instance;
	

	private void Awake()
	{
		Time.timeScale = 1f;

	}

	private void Start()
	{
		Time.timeScale = 1f;

		gameManager = FindObjectOfType<GameManager>();
	}

	public void PlayGame()
	{
		int indx = PlayerPrefs.GetInt("currentLevel", 1);
		SceneManager.LoadScene(indx);
	}

	public void ShowLevelsMenu()
	{
		
		titleMenu.SetActive(false);
		levelsMenu.SetActive(true);
		
		
	}

	public void OpenLevel(int levelIndex)
	{
		SceneManager.LoadScene(levelIndex);
	}

	public void GoBack()
	{
		
		titleMenu.SetActive(true);
		levelsMenu.SetActive(false);
		
	}

	public void ShowTitleMenu()
	{
		
		SceneManager.LoadScene(0);
	}

	public void PauseGame()
	{
		gameManager.PauseGame();
		gamePaused = true;
		Time.timeScale = 0f;
		panel.SetActive(true);
		pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		gameManager.ResumeGame();
		gamePaused = false;
		Time.timeScale = 1f;
		panel.SetActive(false);
		pauseMenu.SetActive(false);
	}

	public void RestartLevel()
	{
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void ReturnToTitle()
	{
		
		SceneManager.LoadScene("TitleScreen");
		ShowTitleMenu();
	}

	public void QuitGame()
	{
		
		Application.Quit();
	}

	public void Update()
	{
		
	}

	public void openPauseMenu(){
		if (gamePaused)
				ResumeGame();
			else
				PauseGame();
	}
}
