using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ui : MonoBehaviour {

	// Use this for initialization
	public GameObject board;
	public GameObject menu;
	public game game_script; 
	public GameObject bouton_diff;
	public GameObject bouton_prof;
	public bool showing = true;
	public bool partie_en_cours = false;
	public int difficulty = 1;
	public int profondeur = 2;

	public void playIa()
	{
		game_script.gameMode = 1;
		game_script.resetGame();
		showing = false;
		partie_en_cours = true;

		menu.SetActive(showing);
	}

	public void playPvp()
	{
		game_script.gameMode = 0;
		game_script.resetGame();
		showing = false;
		partie_en_cours = true;

		menu.SetActive(showing);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void SetDifficulty()
	{
		if (difficulty == 1)
		{
			bouton_diff.GetComponentInChildren<Text>().text = "Difficulty : medium";
			difficulty = 2;
		}
		else if (difficulty == 2)
		{
			bouton_diff.GetComponentInChildren<Text>().text = "Difficulty : hard";
			difficulty = 3;
		}
		else
		{
			bouton_diff.GetComponentInChildren<Text>().text = "Difficulty : easy";
			difficulty = 1;
		}
	}

	public void SetProfondeur()
	{
		if (profondeur == 2)
		{
			bouton_prof.GetComponentInChildren<Text>().text = "Depth : 4";
			profondeur = 4;
		}
		else if (profondeur == 4)
		{
			bouton_prof.GetComponentInChildren<Text>().text = "Depth : 6";
			profondeur = 6;
		}
		else if (profondeur == 6)
		{
			bouton_prof.GetComponentInChildren<Text>().text = "Depth : 8";
			profondeur = 8;
		}
		else if (profondeur == 8)
		{
			bouton_prof.GetComponentInChildren<Text>().text = "Depth : 10";
			profondeur = 10;
		}
		else if (profondeur == 10)
		{
			bouton_prof.GetComponentInChildren<Text>().text = "Depth : 2";
			profondeur = 2;
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (partie_en_cours == true)
			{
				showing = !showing;
				menu.SetActive(showing);
			}
		}
	}
}
