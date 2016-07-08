using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class game : MonoBehaviour {
	// -------------------------------------------------------------------------------------------------------------------------VARS
	public int gameMode = 1; // gamemode autre que 1 pour ne pas jouer avec ia

	// ui
	public heuristic H;
	public GameObject menu;
	public Ui uiScript;
	public int difficulty;
	public Text turnText;
	public Text timerText;
	public Text captBlackText;
	public Text captWhiteText;
	public Text winsText;
	public Text winnerText;
	public Text coupsText;
	public int endTurn;
	private bool playerJustPlayed = false;
	// ui
	private int endGame = 0;
	public GameObject black;
	public GameObject white;
	public GameObject trash;
	public GameObject[,] Tab2d = new GameObject[19, 19];
	private int turn = 1;
	public int Captured_by_black = 0;
	public int Captured_by_white = 0;
	public int fakeCaptured_by_black = 0;
	public int fakeCaptured_by_white = 0;
	public string winner;
	public int profondeur = 2;
	Vector3 l_localPoint;
	struct Point
	{
		public Point(int x, int y) 
		{
			this.x = x;
			this.y = y;
		}
		
		public int x, y;
	}
	// -------------------------------------------------------------------------------------------------------------------------VARS
	
	
	public void resetGame()
	{
		endGame = 0;
		turn = 1;
		playerJustPlayed = false;
		Captured_by_black = 0;
		Captured_by_white = 0;
		difficulty = uiScript.difficulty;
		coupsText.text = (0).ToString();
		for (int i = 0; i < 19; i++) {
			for (int y = 0; y < 19; y++) {
				Destroy (Tab2d [i, y]);
			}
		}
		Start ();
	}
	// Use this for initialization
	void Start () {
		winsText.enabled = false;
		winnerText.enabled = false;
		for (int i = 0; i < 19; i++) {
			for (int y = 0; y < 19; y++) {
				Tab2d [i, y] = (GameObject)Instantiate(trash, trash.transform.localPosition, trash.transform.localRotation);
			}
		}
	}
	
	
	
	
	Vector3 get_correct_point(Vector3 initial_pos)
	{
		bool oneval = false;
		bool twoval = false;
		
		for (float pos = 0f; pos <= 9f; pos += 0.495f)
		{
			if (initial_pos.x - pos > -0.20f && initial_pos.x - pos < 0.20f)
			{
				initial_pos.x = pos;
				oneval = true;
				break;
			}
		}
		
		for (float pos = 0f; pos <= 9f; pos += 0.495f)
		{
			if (-initial_pos.y - pos > -0.20f && -initial_pos.y - pos < 0.20f)
			{
				initial_pos.y = -pos;
				twoval = true;
				break;
			}
		}
		if (oneval == true && twoval == true)
			return initial_pos;
		return new Vector3(-1,-1,-1);
	}
	
	
	
	void OnMouseDown(){
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || endGame == 1)
			return;
		if (gameMode == 1 && turn == 0)
			return;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.gameObject.name == "board")
		{
			Vector3 localPoint = this.transform.InverseTransformPoint(hit.point);
			localPoint.x += 4.5f;
			localPoint.y -= 4.5f;
			localPoint = get_correct_point(localPoint);
			if (localPoint == new Vector3(-1, -1, -1))
				return;
			if (Tab2d [(int)Mathf.Round(localPoint.x * 2), (int)Mathf.Round(localPoint.y * -2)].name != "Cube(Clone)")
				return;
			if (CheckCaptures(localPoint, true) == false)
			{
				if (CheckdoubleFreeThree(localPoint))
					return;
			}
			if (turn == 0)
			{
				Destroy (Tab2d [(int)Mathf.Round(localPoint.x * 2), (int)Mathf.Round(localPoint.y * -2)]);
				Tab2d [(int)Mathf.Round(localPoint.x * 2), (int)Mathf.Round(localPoint.y * -2)] = (GameObject)Instantiate(black, localPoint, black.transform.localRotation);
				turn = 1;
			}
			else
			{
				Destroy (Tab2d [(int)Mathf.Round(localPoint.x * 2), (int)Mathf.Round(localPoint.y * -2)]);
				Tab2d [(int)Mathf.Round(localPoint.x * 2), (int)Mathf.Round(localPoint.y * -2)] = (GameObject)Instantiate(white, localPoint, white.transform.localRotation);
				turn = 0;
			}
			l_localPoint.x = localPoint.x;
			l_localPoint.y = localPoint.y;
			l_localPoint.z = localPoint.z;
			coupsText.text = (int.Parse (coupsText.text) + 1).ToString();
			playerJustPlayed = true;
			if (turn == 1 && H.AlignOrCaptWins(goToInt(), Captured_by_black, 0))
			{
				winner = "Black";
				endGame = 1;
			}
			
			if (turn == 0 && H.AlignOrCaptWins(goToInt(), Captured_by_white, 1))
			{
				winner = "White";
				endGame = 1;
			}
		}
		
	}
	
	bool  CheckdoubleFreeThree(Vector3 localPoint)
	{
		int nbFreeThree = 0;
		
		string player1 = "player_black(Clone)"; //black
		
		if (turn == 1) //white turn
			player1 = "player_white(Clone)"; //white
		Point pion = new Point ((int)Mathf.Round (localPoint.x * 2), (int)Mathf.Round (localPoint.y * -2));
		
		if (pion.y - 4 >= 0 && pion.y + 1 <= 18) { //ligne nord
			if (((Tab2d [pion.x, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y - 2].name == player1) && (Tab2d [pion.x, pion.y - 3].name == player1) && (Tab2d [pion.x, pion.y - 4].name == "Cube(Clone)")) || 
			    ((Tab2d [pion.x, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y - 1].name == player1) && (Tab2d [pion.x, pion.y - 2].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y - 3].name == player1) && (Tab2d [pion.x, pion.y - 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y - 1].name == player1) && (Tab2d [pion.x, pion.y - 2].name == player1) && (Tab2d [pion.x, pion.y - 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.y + 4 <= 18 && pion.y - 1 >= 0) { //ligne sud
			if (((Tab2d [pion.x, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y + 2].name == player1) && (Tab2d [pion.x, pion.y + 3].name == player1) && (Tab2d [pion.x, pion.y + 4].name == "Cube(Clone)")) || 
			    ((Tab2d [pion.x, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y + 1].name == player1) && (Tab2d [pion.x, pion.y + 2].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y + 3].name == player1) && (Tab2d [pion.x, pion.y + 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x, pion.y + 1].name == player1) && (Tab2d [pion.x, pion.y + 2].name == player1) && (Tab2d [pion.x, pion.y + 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x - 4 >= 0 && pion.x + 1 <= 18) { //ligne ouest
			if (((Tab2d [pion.x + 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x - 2, pion.y].name == player1) && (Tab2d [pion.x -3, pion.y].name == player1) && (Tab2d [pion.x - 4, pion.y].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y].name == player1) && (Tab2d [pion.x - 2, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x - 3, pion.y].name == player1) && (Tab2d [pion.x - 4, pion.y].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y].name == player1) && (Tab2d [pion.x - 2, pion.y].name == player1) && (Tab2d [pion.x - 3, pion.y].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x + 4 <= 18 && pion.x - 1 >= 0) { //ligne est
			if (((Tab2d [pion.x - 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x + 2, pion.y].name == player1) && (Tab2d [pion.x + 3, pion.y].name == player1) && (Tab2d [pion.x + 4, pion.y].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y].name == player1) && (Tab2d [pion.x + 2, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x + 3, pion.y].name == player1) && (Tab2d [pion.x + 4, pion.y].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y].name == player1) && (Tab2d [pion.x + 2, pion.y].name == player1) && (Tab2d [pion.x + 3, pion.y].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x + 4 <= 18 && pion.y - 4 >= 0 && pion.x - 1 >= 0 && pion.y + 1 <= 18) { //ligne nord est
			if (((Tab2d [pion.x - 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x + 2, pion.y - 2].name == player1) && (Tab2d [pion.x + 3, pion.y - 3].name == player1) && (Tab2d [pion.x + 4, pion.y - 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y - 1].name == player1) && (Tab2d [pion.x + 2, pion.y - 2].name == "Cube(Clone)") && (Tab2d [pion.x + 3, pion.y - 3].name == player1) && (Tab2d [pion.x + 4, pion.y - 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y - 1].name == player1) && (Tab2d [pion.x + 2, pion.y - 2].name == player1) && (Tab2d [pion.x + 3, pion.y - 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x - 4 >= 0 && pion.y + 4 <= 18 && pion.x + 1 <= 18 && pion.y - 1 >= 0) { //ligne sud ouest
			if (((Tab2d [pion.x + 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x - 2, pion.y + 2].name == player1) && (Tab2d [pion.x - 3, pion.y + 3].name == player1) && (Tab2d [pion.x - 4, pion.y + 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y + 1].name == player1) && (Tab2d [pion.x - 2, pion.y + 2].name == "Cube(Clone)") && (Tab2d [pion.x - 3, pion.y + 3].name == player1) && (Tab2d [pion.x - 4, pion.y + 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y + 1].name == player1) && (Tab2d [pion.x - 2, pion.y + 2].name == player1) && (Tab2d [pion.x - 3, pion.y + 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x - 4 >= 0 && pion.y - 4 >= 0 && pion.x + 1 <= 18 && pion.y + 1 <= 18) { //ligne nord ouest
			if (((Tab2d [pion.x + 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x - 2, pion.y - 2].name == player1) && (Tab2d [pion.x - 3, pion.y - 3].name == player1) && (Tab2d [pion.x - 4, pion.y - 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y - 1].name == player1) && (Tab2d [pion.x - 2, pion.y - 2].name == "Cube(Clone)") && (Tab2d [pion.x - 3, pion.y - 3].name == player1) && (Tab2d [pion.x - 4, pion.y - 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x + 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x - 1, pion.y - 1].name == player1) && (Tab2d [pion.x - 2, pion.y - 2].name == player1) && (Tab2d [pion.x - 3, pion.y - 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (pion.x + 4 <= 18 && pion.y + 4 <= 18 && pion.x - 1 >= 0 && pion.y - 1 >= 0) { //ligne sud est
			if (((Tab2d [pion.x - 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y + 1].name == "Cube(Clone)") && (Tab2d [pion.x + 2, pion.y + 2].name == player1) && (Tab2d [pion.x + 3, pion.y + 3].name == player1) && (Tab2d [pion.x + 4, pion.y + 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y + 1].name == player1) && (Tab2d [pion.x + 2, pion.y + 2].name == "Cube(Clone)") && (Tab2d [pion.x + 3, pion.y + 3].name == player1) && (Tab2d [pion.x + 4, pion.y + 4].name == "Cube(Clone)")) ||
			    ((Tab2d [pion.x - 1, pion.y - 1].name == "Cube(Clone)") && (Tab2d [pion.x + 1, pion.y + 1].name == player1) && (Tab2d [pion.x + 2, pion.y + 2].name == player1) && (Tab2d [pion.x + 3, pion.y + 3].name == "Cube(Clone)")))
				nbFreeThree++;
		}
		
		if (nbFreeThree != 2)
			return false;
		else
			return true;
	}
	
	bool CheckCaptures(Vector3 localPoint, bool docapture)
	{
		Point pion = new Point ((int)Mathf.Round (localPoint.x * 2), (int)Mathf.Round (localPoint.y * -2));
		//black turn by default
		string player1 = "player_black(Clone)"; //black
		string player2 = "player_white(Clone)"; //white
		
		if (turn == 1) { //white turn
			player1 = "player_white(Clone)"; //white
			player2 = "player_black(Clone)"; //black
		}
		
		bool capture = false;
		
		if (pion.y - 3 >= 0) { //ligne nord
			if ((Tab2d [pion.x, pion.y - 1].name == player2) && (Tab2d [pion.x , pion.y - 2].name == player2) && (Tab2d [pion.x, pion.y - 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x, pion.y - 1), new Point(pion.x, pion.y - 2)); //to remove player2 stones
				capture = true;
			}
		}
		
		if (pion.y + 3 <= 18) { //ligne sud
			if ((Tab2d [pion.x, pion.y + 1].name == player2) && (Tab2d [pion.x , pion.y + 2].name == player2) && (Tab2d [pion.x, pion.y + 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x, pion.y + 1), new Point(pion.x, pion.y + 2)); //to remove player2 stones
				capture = true;
			}
		}
		
		if (pion.x - 3 >= 0) { //ligne ouest
			if ((Tab2d [pion.x - 1, pion.y].name == player2) && (Tab2d [pion.x - 2, pion.y].name == player2) && (Tab2d [pion.x - 3, pion.y].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x - 1, pion.y), new Point(pion.x - 2, pion.y)); //to remove player2 stones
				capture = true;
			}
		}
		
		if (pion.x + 3 <= 18) { //ligne est
			if ((Tab2d [pion.x + 1, pion.y].name == player2) && (Tab2d [pion.x + 2, pion.y].name == player2) && (Tab2d [pion.x + 3, pion.y].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x + 1, pion.y), new Point(pion.x + 2, pion.y)); //to remove player2 stones
				capture = true;
			}
		}
		
		if (pion.x + 3 <= 18 && pion.y - 3 >= 0) { //ligne nord est
			if ((Tab2d [pion.x + 1, pion.y - 1].name == player2) && (Tab2d [pion.x + 2, pion.y - 2].name == player2) && (Tab2d [pion.x + 3, pion.y - 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x + 1, pion.y - 1), new Point(pion.x + 2, pion.y - 2)); //to remove player2 stones		
				capture = true;
			}
		}
		
		if (pion.x - 3 >= 0 && pion.y + 3 <= 18) { //ligne sud ouest
			if ((Tab2d [pion.x - 1, pion.y + 1].name == player2) && (Tab2d [pion.x - 2, pion.y + 2].name == player2) && (Tab2d [pion.x - 3, pion.y + 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x - 1, pion.y + 1), new Point(pion.x - 2, pion.y + 2)); //to remove player2 stones		
				capture = true;
			}
		}
		
		if (pion.x - 3 >= 0 && pion.y - 3 >= 0) { //ligne nord ouest
			if ((Tab2d [pion.x - 1, pion.y - 1].name == player2) && (Tab2d [pion.x - 2, pion.y - 2].name == player2) && (Tab2d [pion.x - 3, pion.y - 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x - 1, pion.y - 1), new Point(pion.x - 2, pion.y - 2)); //to remove player2 stones
				capture = true;
			}
		}
		
		if (pion.x + 3 <= 18 && pion.y + 3 <= 18) { //ligne sud est
			if ((Tab2d [pion.x + 1, pion.y + 1].name == player2) && (Tab2d [pion.x + 2, pion.y + 2].name == player2) && (Tab2d [pion.x + 3, pion.y + 3].name == player1))
			{
				if (docapture)
					DoCaptures(new Point(pion.x + 1, pion.y + 1), new Point(pion.x + 2, pion.y + 2)); //to remove player2 stones	
				capture = true;
			}
		}
		return capture;
	}
	
	void DoCaptures(Point p1, Point p2)//Stones which must be destroyed
	{
		if (turn == 0)
			Captured_by_black += 2;
		else
			Captured_by_white += 2;
		Destroy (Tab2d [p1.x, p1.y]);
		Destroy (Tab2d [p2.x, p2.y]);
		Tab2d [p1.x, p1.y] = (GameObject)Instantiate(trash, trash.transform.localPosition, trash.transform.localRotation);
		Tab2d [p2.x, p2.y] = (GameObject)Instantiate(trash, trash.transform.localPosition, trash.transform.localRotation);
		
	}
	
	bool iExist(Point p)
	{
		if (p.x >= 0 && p.x <= 18 && p.y >= 0 && p.y <= 18)
			return true;
		return false;
	}




	int[,] goToInt()
	{
		int[,] ret = new int[19,19];
		for (int x = 0; x < 19; x++)
		{
			for (int y = 0; y < 19; y++)
			{
				if (Tab2d[x,y].name == "Cube(Clone)")
					ret[x,y] = -1;
				else if (Tab2d[x,y].name == "player_white(Clone)")
					ret[x,y] = 1;
				else
					ret[x,y] = 0;
			}
		}
		return ret;
	}
	int[,] goToIntWithLocal(int [,] game, int xpos, int ypos, int color)
	{
		int p1 = 1;
		int p2 = 0;
		int[,] ret = new int[19,19];
		for (int x = 0; x < 19; x++)
		{
			for (int y = 0; y < 19; y++)
				ret [x, y] = game [x, y];
		}
		if (color == 0) {
			ret [xpos, ypos] = 0;
			p1 = 0;
			p2 = 1;
		}
		else
			ret [xpos, ypos] = 1;

		int capturesDone = 0;
		if (ypos - 3 >= 0 && ret[xpos, ypos - 1] == p2 && ret[xpos, ypos - 2] == p2 && ret[xpos, ypos - 3] == p1){
			ret[xpos, ypos - 1] = -1;
			ret[xpos, ypos - 2] = -1;
			capturesDone++;
		}
		if (ypos + 3 <= 18 && ret [xpos, ypos + 1] == p2 && ret [xpos, ypos + 2] == p2 && ret [xpos, ypos + 3] == p1) {
			ret[xpos, ypos + 1] = -1;
			ret[xpos, ypos + 2] = -1;
			capturesDone++;
		}
		if (xpos - 3 >= 0 && ret[xpos - 1, ypos] == p2 && ret[xpos - 2, ypos] == p2 && ret[xpos - 3, ypos] == p1){
			ret[xpos - 1, ypos] = -1;
			ret[xpos - 2, ypos] = -1;
			capturesDone++;
		}
		if (xpos + 3 <= 18 && ret [xpos + 1, ypos] == p2 && ret [xpos + 2, ypos] == p2 && ret [xpos + 3, ypos] == p1) {
			ret[xpos + 1, ypos] = -1;
			ret[xpos + 2, ypos] = -1;
			capturesDone++;
		}
		if (xpos - 3 >= 0 && ypos - 3 >= 0 && ret [xpos - 1, ypos - 1] == p2 && ret [xpos - 2, ypos - 2] == p2 && ret [xpos - 3, ypos - 3] == p1) {
			ret [xpos - 1, ypos - 1] = -1;
			ret [xpos - 2, ypos - 2] = -1;
			capturesDone++;
		}
		if (xpos + 3 <= 18 && ypos + 3 <= 18 && ret [xpos + 1, ypos + 1] == p2 && ret [xpos + 2, ypos + 2] == p2 && ret [xpos + 3, ypos + 3] == p1) {
			ret [xpos + 1, ypos + 1] = -1;
			ret [xpos + 2, ypos + 2] = -1;
			capturesDone++;
		}
		if (xpos - 3 >= 0 && ypos + 3 <= 18 && ret [xpos - 1, ypos + 1] == p2 && ret [xpos - 2, ypos + 2] == p2 && ret [xpos - 3, ypos + 3] == p1) {
			ret [xpos - 1, ypos + 1] = -1;
			ret [xpos - 2, ypos + 2] = -1;
			capturesDone++;
		}
		if (xpos + 3 <= 18 && ypos - 3 >= 0 && ret [xpos + 1, ypos - 1] == p2 && ret [xpos + 2, ypos - 2] == p2 && ret [xpos + 3, ypos - 3] == p1) {
			ret [xpos + 1, ypos - 1] = -1;
			ret [xpos + 2, ypos - 2] = -1;
			capturesDone++;
		}
		fakeCaptured_by_black = Captured_by_black;
		fakeCaptured_by_white = Captured_by_white;
		for (int i = 0; i < capturesDone; i++) {
			if (color == 1)
				fakeCaptured_by_white += 2;
			else
				fakeCaptured_by_black += 2;
		}
		return ret;
	}


	List<Vector3> FindAllAvailablePlay(int[,] game)
	{
		List<Vector3> Tab = new List<Vector3>();

		for (int y = 0; y < 19; y++) {
			for (int x = 0; x < 19; x++) {
				if (game[x, y] == -1  && IsAdjacent(x, y, game))
				{
					Tab.Add(new Vector3(x,y,0));
				}
			}
		}
		return Tab;
	}
	bool IsAdjacent(int posx, int posy, int[,] game)
	{
		for (int y = posy - 1; y <= posy + 1; y++) {
			for (int x = posx - 1; x <= posx + 1; x++)
			{
				if (iExist(new Point(x, y)) && !(x == posx && y == posy) && game[x, y] != -1)
					return true;
			}
		}
		return false;
	}



	// Update is called once per frame
	void Update () {
		//---------------------------------------------------UI
		difficulty = uiScript.difficulty;
		profondeur = uiScript.profondeur;
		if (gameMode == 0)
			timerText.text = "/";
		captBlackText.text = Captured_by_black.ToString();
		captWhiteText.text = Captured_by_white.ToString();
		if (endGame == 1)
		{
			winnerText.text = winner;
			winsText.enabled = true;
			winnerText.enabled = true;
		}
		if (turn == 1)
			turnText.text = "Blancs";
		else
			turnText.text = "Noirs";
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (uiScript.partie_en_cours == true)
			{
				uiScript.showing = !uiScript.showing;
				menu.SetActive(uiScript.showing);
			}
		}
		if (turn == 1)
			endTurn = 1;
		//---------------------------------------------------UI


		if (gameMode == 1 && turn == 0 && endGame == 0 && endTurn == 0)
		{
			int [,] currentGame = goToInt();
			List<Vector3> toplay = FindAllAvailablePlay(currentGame);
			Vector3 temp;
			for (int i = 0; i < toplay.Count; i++) {
				temp = toplay[i];
				temp.z =  H.GetCostFromMapStatus(goToIntWithLocal(currentGame, (int)toplay[i].x, (int)toplay[i].y, 0), 1, 0, fakeCaptured_by_white, fakeCaptured_by_black, 0);
				toplay[i] = temp;
			}
			toplay.Sort((left, right) => left.z.CompareTo(right.z));
			int isdone = 0;
			
			//-------------------------------------------------------- ALGO PART -- START
			float start_time = Time.realtimeSinceStartup;
			//-------------------------------------------------------- ALGO PART -- START
			int toDo = toplay.Count;// - toplay.Count/(2 + difficulty);
			for (int l = 0; l < toDo; l++){
				temp = toplay[l];
				temp.z = AlphaBeta (goToIntWithLocal(currentGame, (int)toplay[l].x, (int)toplay[l].y, 0), profondeur, int.MinValue, int.MaxValue);//H.GetCostFromMapStatus(goToIntWithLocal((int)Mathf.Round(toplay[l].x * 2), (int)Mathf.Round(toplay[l].y * -2), 0), 1, 0, Captured_by_white, Captured_by_black);//AlphaBeta (toplay[l], 4, -Mathf.Infinity, Mathf.Infinity);
				toplay[l] = temp;
			}
			toplay.Sort((left, right) => left.z.CompareTo(right.z));
			foreach (Vector3 lPoint in toplay)
			{
				if (CheckCaptures(new Vector3(lPoint.x / 2f, lPoint.y / -2f, 0f), true) == false)
				{
					if (CheckdoubleFreeThree(new Vector3(lPoint.x / 2f, lPoint.y / -2f, 0f)))
						continue;
				}
				Destroy (Tab2d [(int)lPoint.x, (int)lPoint.y]);
				Tab2d [(int)lPoint.x, (int)lPoint.y] = (GameObject)Instantiate(black, new Vector3(lPoint.x / 2f, lPoint.y / -2f, 0f), black.transform.localRotation);
				turn = 1;
				if (turn == 1 && H.AlignOrCaptWins(goToInt(), Captured_by_black, 0))
				{
					winner = "Black";
					endGame = 1;
				}

				if (turn == 0 && H.AlignOrCaptWins(goToInt(), Captured_by_white, 1))
				{
					winner = "White";
					endGame = 1;
				}
				isdone = 1;
				break;
			}
			//-------------------------------------------------------- ALGO PART -- END
			timerText.text = ((Time.realtimeSinceStartup) - start_time).ToString("f6");
			//-------------------------------------------------------- ALGO PART -- END
			if (isdone != 0)
				coupsText.text = (int.Parse (coupsText.text) + 1).ToString();
			if (isdone == 0)
			{
				winner = "None";
				endGame = 1;
			}
		}
		else if (gameMode == 0 && playerJustPlayed)
		{
			for (int x = 0; x < 19; x++) {
				for (int y = 0; y < 19; y++) {
					if (Tab2d [x, y].name == "Cube(Clone)")
						Tab2d [x, y].transform.localPosition = new Vector3 (trash.transform.localPosition.x, trash.transform.localPosition.x, 0f);
				}
			}
			int [,] currentGame = goToInt();
			List<Vector3> toplay = FindAllAvailablePlay(currentGame);
			Vector3 temp;
			for (int i = 0; i < toplay.Count; i++) {
				temp = toplay[i];
				if (turn == 1)
					temp.z =  H.GetCostFromMapStatus(goToIntWithLocal(currentGame, (int)toplay[i].x, (int)toplay[i].y, 1), 0, 1, fakeCaptured_by_white, fakeCaptured_by_black, 0);
				else
					temp.z =  H.GetCostFromMapStatus(goToIntWithLocal(currentGame, (int)toplay[i].x, (int)toplay[i].y, 0), 1, 0, fakeCaptured_by_white, fakeCaptured_by_black, 0);
				toplay[i] = temp;
			}
			toplay.Sort((left, right) => left.z.CompareTo(right.z));
			int isdone = 0;
			int toDo = toplay.Count - toplay.Count/(1 + difficulty);
			for (int l = 0; l < toDo; l++){
				temp = toplay[l];
				if (turn == 1)
					temp.z = AlphaBeta (goToIntWithLocal(currentGame, (int)toplay[l].x, (int)toplay[l].y, 1), 1, int.MinValue, int.MaxValue);//H.GetCostFromMapStatus(goToIntWithLocal((int)Mathf.Round(toplay[l].x * 2), (int)Mathf.Round(toplay[l].y * -2), 0), 1, 0, Captured_by_white, Captured_by_black);//AlphaBeta (toplay[l], 4, -Mathf.Infinity, Mathf.Infinity);
				else
					temp.z = AlphaBeta (goToIntWithLocal(currentGame, (int)toplay[l].x, (int)toplay[l].y, 0), 2, int.MinValue, int.MaxValue);//H.GetCostFromMapStatus(goToIntWithLocal((int)Mathf.Round(toplay[l].x * 2), (int)Mathf.Round(toplay[l].y * -2), 0), 1, 0, Captured_by_white, Captured_by_black);//AlphaBeta (toplay[l], 4, -Mathf.Infinity, Mathf.Infinity);
				toplay[l] = temp;
			}
			toplay.Sort((left, right) => left.z.CompareTo(right.z));
			foreach (Vector3 lPoint in toplay)
			{
				if (CheckCaptures(new Vector3(lPoint.x / 2f, lPoint.y / -2f, 0f), false) == false)
				{
					if (CheckdoubleFreeThree(new Vector3(lPoint.x / 2f, lPoint.y / -2f, 0f)))
						continue;
				}
				Tab2d [(int)lPoint.x, (int)lPoint.y].transform.localPosition = new Vector3 (lPoint.x / 2f, lPoint.y / -2f, 0f);
				isdone = 1;
				break;
			}
			if (isdone != 0)
				playerJustPlayed = false;
			if (isdone == 0)
			{
				winner = "None";
				endGame = 1;
			}
		}
		if (turn == 0)
			endTurn = 0;
	}





	//------------------------------------alpha beta



	int AlphaBeta (int[,] game, int profondeur, int alpha, int beta)
	{
		int color;
		int last_color;
		int result;
		if (profondeur % 2 == 0) {
			color = 1;
			last_color = 0;
		} else {
			last_color = 1;
			color = 0;
		}
		result = H.GetCostFromMapStatus (game, color, last_color, fakeCaptured_by_white, fakeCaptured_by_black, profondeur);
		if (profondeur == 0 || (result >= int.MaxValue - 10 || result <= int.MinValue + 10))
			return result;
		List<Vector3> Moves = FindAllAvailablePlay(game);
		int meilleur_coup = int.MinValue;
		int toDo = 10;
		if (Moves.Count < 10)
			toDo = Moves.Count;
		for (int i = 0; i < toDo; i++)
		{
			int score = -AlphaBeta(goToIntWithLocal(game, (int)Moves[i].x, (int)Moves[i].y, color), profondeur - 1, -alpha, -beta);
			if (score > meilleur_coup) {
				meilleur_coup = score;
				if (meilleur_coup > alpha) {
					alpha = meilleur_coup;
					if (alpha >= beta)
						return (meilleur_coup);
				}
			}
		}
		return meilleur_coup;
	}
}
