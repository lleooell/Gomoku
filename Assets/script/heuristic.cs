using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class heuristic : MonoBehaviour {

	private bool Capturable(int[,] game, int x, int y, int col) 
	{
		int p1 = 1;
		int p2 = 0;
		if (col == 0) {
			p1 = 0;
			p2 = 1;
		}
		// ------------------------------------------------y
		if (y - 2 >= 0 && y + 1 <= 18){
			if ((game[x, y - 1] == p1 && game[x, y - 2] == p2 && game[x, y + 1] == -1) ||
				(game[x, y - 1] == p1 && game[x, y + 1] == p2 && game[x, y - 2] == -1))
				return true;
		}
		if (y + 2 <= 18 && y - 1 >= 0){
			if ((game[x, y + 1] == p1 && game[x, y + 2] == p2 && game[x, y - 1] == -1) ||
				(game[x, y + 1] == p1 && game[x, y - 1] == p2 && game[x, y + 2] == -1))
				return true;
		}
		// ------------------------------------------------x
		if (x - 2 >= 0 && x + 1 <= 18){
			if ((game[x - 1, y] == p1 && game[x - 2, y] == p2 && game[x + 1, y] == -1) ||
				(game[x - 1, y] == p1 && game[x + 1, y] == p2 && game[x - 2, y] == -1))
				return true;
		}
		if (x + 2 <= 18 && x - 1 >= 0){
			if ((game[x + 1, y] == p1 && game[x + 2, y] == p2 && game[x - 1, y] == -1) ||
				(game[x + 1, y] == p1 && game[x - 1, y] == p2 && game[x + 2, y] == -1))
				return true;
		}
		// ------------------------------------------------xy
		if (x - 2 >= 0 && y - 2 >= 0 && x + 1 <= 18 && y + 1 <= 18){
			if ((game[x - 1, y - 1] == p1 && game[x - 2, y - 2] == p2 && game[x + 1, y + 1] == -1) ||
				(game[x - 1, y - 1] == p1 && game[x + 1, y + 1] == p2 && game[x - 2, y - 2] == -1))
				return true;
		}
		if (x + 2 <= 18 && y + 2 <= 18 && x - 1 >= 0 && y - 1 >= 0){
			if ((game[x + 1, y + 1] == p1 && game[x + 2, y + 2] == p2 && game[x - 1, y - 1] == -1) ||
				(game[x + 1, y + 1] == p1 && game[x - 1, y - 1] == p2 && game[x + 2, y + 2] == -1))
				return true;
		}
		// ------------------------------------------------yx
		if (x - 2 >= 0 && y + 2 <= 18 && x + 1 <= 18 && y - 1 >= 0){
			if ((game[x - 1, y + 1] == p1 && game[x - 2, y + 2] == p2 && game[x + 1, y - 1] == -1) ||
				(game[x - 1, y + 1] == p1 && game[x + 1, y - 1] == p2 && game[x - 2, y + 2] == -1))
				return true;
		}
		if (x + 2 <= 18 && y - 2 >= 0 && x - 1 >= 0 && y + 1 <= 18){
			if ((game[x + 1, y - 1] == p1 && game[x + 2, y - 2] == p2 && game[x - 1, y + 1] == -1) ||
				(game[x + 1, y - 1] == p1 && game[x - 1, y + 1] == p2 && game[x + 2, y - 2] == -1))
				return true;
		}
		return false;
	}

	public bool AlignOrCaptWins(int[,] game, int capt, int col)
	{
		if (capt == 10)
			return true;
		int count;
		// ------------------------------------------------y
		for (int x = 0; x < 19; x++) {
			count = 0;
			for (int y = 0; y < 19; y++) {
				if (game [x, y] == col && !Capturable(game, x, y, col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		// ------------------------------------------------x
		for (int y = 0; y < 19; y++) {
			count = 0;
			for (int x = 0; x < 19; x++) {
				if (game [x, y] == col && !Capturable(game, x, y, col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		// ------------------------------------------------xy
		for (int x = 18; x >= 0; x--) {
			count = 0;
			for (int y = 0; y < 19-x; y++){
				if (game [x+y, y] == col && !Capturable(game, x+y, y, col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		for (int y = 18; y > 0; y--) {
			count = 0;
			for (int x = 0; x < 19 - y; x++){
				if (game [x, y + x] == col && !Capturable(game, x, y + x, col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		// ------------------------------------------------xy
		for (int x = 18; x >= 0; x--) {
			count = 0;
			for (int y = 0; y <= x; y++){
				if (game [x-y, y] == col && !Capturable(game, x-y, y, col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		for (int y = 1; y < 19; y++) {
			count = 0;
			for (int x = 18; x >= y; x--){
				if (game [x, y + (18 - x)] == col && !Capturable(game, x, y  + (18 - x), col))
					count++;
				else
					count = 0;
				if (count == 5)
					return true;
			}
		}
		return false;
	}

	private int[] CurrentAligns(int[,] game, int col)
	{
		List<int> toBeArray = new List<int>();
		int count;

		// ------------------------------------------------y
		for (int x = 0; x < 19; x++) {
			for (int y = 0; y < 19; y++) {
				count = 0;
				if ((game [x, y] == col) ||  (game [x, y] == -1)) {
					if (game [x, y] == col)
						count++;
					if ((y + 1 <= 18 && game [x, y + 1] == col) || (y + 1 <= 18 && game [x, y + 1] == -1)) {
						if (game [x, y + 1] == col)
							count++;
						if ((y + 2 <= 18 && game [x, y + 2] == col) || (y + 2 <= 18 && game [x, y + 2] == -1)) {
							if (game [x, y + 2] == col)
								count++;
							if ((y + 3 <= 18 && game [x, y + 3] == col) || (y + 3 <= 18 && game [x, y + 3] == -1)) {
								if (game [x, y + 3] == col)
									count++;
								if ((y + 4 <= 18 && game [x, y + 4] == col) || (y + 4 <= 18 && game [x, y + 4] == -1)) {
									if (game [x, y + 4] == col)
										count++;
									if (count >= 3) {
										if (y + 5 <= 18 && game [x, y + 5] != col && game [x, y + 5] != -1)
											toBeArray.Add (count - 2);
										else if (y - 1 >= 0 && game [x, y - 1] != col && game [x, y - 1] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}
		// ------------------------------------------------x
		for (int y = 0; y < 19; y++) {
			for (int x = 0; x < 19; x++) {
				count = 0;
				if ((game [x, y] == col) || (game [x, y] == -1)) {
					if (game [x, y] == col)
						count++;
					if ((x + 1 <= 18 && game [x + 1, y] == col) || (x + 1 <= 18 && game [x + 1, y] == -1)) {
						if (game [x + 1, y] == col)
							count++;
						if ((x + 2 <= 18 && game [x + 2, y] == col) || (x + 2 <= 18 && game [x + 2, y] == -1)) {
							if (game [x + 2, y] == col)
								count++;
							if ((x + 3 <= 18 && game [x + 3, y] == col) || (x + 3 <= 18 && game [x + 3, y] == -1)) {
								if (game [x + 3, y] == col)
									count++;
								if ((x + 4 <= 18 && game [x + 4, y] == col) || (x + 4 <= 18 && game [x + 4, y] == -1)) {
									if (game [x + 4, y] == col)
										count++;
									if (count >= 3) {
										if (x + 5 <= 18 && game [x + 5, y] != col && game [x + 5, y] != -1)
											toBeArray.Add (count - 2);
										else if (x - 1 >= 0 && game [x - 1, y] != col && game [x - 1, y] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}
		// ------------------------------------------------gauche-droite et haut-bas diago
		for (int x = 0; x < 19; x++) {
			for (int y = 0; y < 19-x; y++){
				count = 0;
				if ((game [x + y, y] == col) || (game [x + y, y] == -1)) {
					if (game [x + y, y] == col)
						count++;
					if ((x + y + 1 <= 18 && y + 1 <= 18 && game [x + y + 1, y + 1] == col) || (x + y + 1 <= 18 && y + 1 <= 18  && game [x + y + 1, y + 1] == -1)) {
						if (game [x + y + 1, y + 1] == col)
							count++;
						if ((x + y + 2 <= 18 && y + 2 <= 18 && game [x + y + 2, y + 2] == col) || (x + y + 2 <= 18 && y + 2 <= 18  && game [x + y + 2, y + 2] == -1)) {
							if (game [x + y + 2, y + 2] == col)
								count++;
							if ((x + y + 3 <= 18 && y + 3 <= 18 && game [x + y + 3, y + 3] == col) || (x + y + 3 <= 18 && y + 3 <= 18  && game [x + y + 3, y + 3] == -1)) {
								if (game [x + y + 3, y + 3] == col)
									count++;
								if ((x + y + 4 <= 18 && y + 4 <= 18 && game [x + y + 4, y + 4] == col) || (x + y + 4 <= 18 && y + 4 <= 18  && game [x + y + 4, y + 4] == -1)) {
									if (game [x + y + 4, y + 4] == col)
										count++;
									if (count >= 3) {
										if (x + y + 5 <= 18 && y + 5 <= 18 && game [x + y + 5, y + 5] != col && game [x + y + 5, y + 5] != -1)
											toBeArray.Add (count - 2);
										else if (x + y - 1 >= 0 && y - 1 >= 0 && game [x + y - 1, y - 1] != col && game [x + y - 1, y - 1] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}
		for (int y = 1; y < 19; y++) {
			for (int x = 0; x < 19-y; x++){
				count = 0;
				if ((game [x, y + x] == col) || (game [x, y + x] == -1)) {
					if (game [x, y + x] == col)
						count++;
					if ((x + 1 <= 18 && y + x + 1 <= 18 && game [x + 1, y + x + 1] == col) || (x + 1 <= 18 && y + x + 1 <= 18  && game [x + 1, y + x + 1] == -1)) {
						if (game [x + 1, y + x + 1] == col)
							count++;
						if ((x + 2 <= 18 && y + x + 2 <= 18 && game [x + 2, y + x + 2] == col) || (x + 2 <= 18 && y + x + 2 <= 18  && game [x + 2, y + x + 2] == -1)) {
							if (game [x + 2, y + x + 2] == col)
								count++;
							if ((x + 3 <= 18 && y + x + 3 <= 18 && game [x + 3, y + x + 3] == col) || (x + 3 <= 18 && y + x + 3 <= 18  && game [x + 3, y + x + 3] == -1)) {
								if (game [x + 3, y + x + 3] == col)
									count++;
								if ((x + 4 <= 18 && y + x + 4 <= 18 && game [x + 4, y + x + 4] == col) || (x + 4 <= 18 && y + x + 4 <= 18  && game [x + 4, y + x + 4] == -1)) {
									if (game [x + 4, y + x + 4] == col)
										count++;
									if (count >= 3) {
										if (x + 5 <= 18 && y + x + 5 <= 18 && game [x + 5, y + x + 5] != col && game [x + 5, y + x + 5] != -1)
											toBeArray.Add (count - 2);
										else if (x - 1 >= 0 && y + x - 1 >= 0 && game [x - 1, y + x - 1] != col && game [x - 1, y + x - 1] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}


		// ------------------------------------------------droite-gauche et haut-bas diago
		for (int x = 0; x < 19; x++) {
			for (int y = 0; y < x + 1; y++){
				count = 0;
				if ((game [x - y, y] == col) || (game [x - y, y] == -1)) {
					if (game [x - y, y] == col)
						count++;
					if ((x - y - 1 >= 0 && y + 1 <= 18 && game [x - y - 1, y + 1] == col) || (x - y - 1 >= 0 && y + 1 <= 18  && game [x - y - 1, y + 1] == -1)) {
						if (game [x - y - 1, y + 1] == col)
							count++;
						if ((x - y - 2 >= 0 && y + 2 <= 18 && game [x - y - 2, y + 2] == col) || (x - y - 2 >= 0 && y + 2 <= 18  && game [x - y - 2, y + 2] == -1)) {
							if (game [x - y - 2, y + 2] == col)
								count++;
							if ((x - y - 3 >= 0 && y + 3 <= 18 && game [x - y - 3, y + 3] == col) || (x - y - 3 >= 0 && y + 3 <= 18  && game [x - y - 3, y + 3] == -1)) {
								if (game [x - y - 3, y + 3] == col)
									count++;
								if ((x - y - 4 >= 0 && y + 4 <= 18 && game [x - y - 4, y + 4] == col) || (x - y - 4 >= 0 && y + 4 <= 18  && game [x - y - 4, y + 4] == -1)) {
									if (game [x - y - 4, y + 4] == col)
										count++;
									if (count >= 3) {
										if (x - y - 5 >= 0 && y + 5 <= 18 && game [x - y - 5, y + 5] != col && game [x - y - 5, y + 5] != -1)
											toBeArray.Add (count - 2);
										else if (x - y + 1 <= 18 && y - 1 >= 0 && game [x - y + 1, y - 1] != col && game [x - y + 1, y - 1] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}
		for (int y = 1; y < 19; y++) {
			for (int x = 18; x >= y; x--){
				game [x, y + (18 - x)] = game [x, y + (18 - x)];
				count = 0;
				if ((game [x, y + (18 - x)] == col) || (game [x, y + (18 - x)] == -1)) {
					if (game [x, y + (18 - x)] == col)
						count++;
					if ((x - 1 >= 0 && y + (18 - x) + 1 <= 18 && game [x - 1, y + (18 - x) + 1] == col) || (x - 1 >= 0 && y + (18 - x) + 1 <= 18  && game [x - 1, y + (18 - x) + 1] == -1)) {
						if (game [x - 1, y + (18 - x) + 1] == col)
							count++;
						if ((x - 2 >= 0 && y + (18 - x) + 2 <= 18 && game [x - 2, y + (18 - x) + 2] == col) || (x - 2 >= 0 && y + (18 - x) + 2 <= 18  && game [x - 2, y + (18 - x) + 2] == -1)) {
							if (game [x - 2, y + (18 - x) + 2] == col)
								count++;
							if ((x - 3 >= 0 && y + (18 - x) + 3 <= 18 && game [x - 3, y + (18 - x) + 3] == col) || (x - 3 >= 0 && y + (18 - x) + 3 <= 18  && game [x - 3, y + (18 - x) + 3] == -1)) {
								if (game [x - 3, y + (18 - x) + 3] == col)
									count++;
								if ((x - 4 >= 0 && y + (18 - x) + 4 <= 18 && game [x - 4, y + (18 - x) + 4] == col) || (x - 4 >= 0 && y + (18 - x) + 4 <= 18  && game [x - 4, y + (18 - x) + 4] == -1)) {
									if (game [x - 4, y + (18 - x) + 4] == col)
										count++;
									if (count >= 3) {
										if (x - 5 >= 0 && y + (18 - x) + 5 <= 18 && game [x - 5, y + (18 - x) + 5] != col && game [x - 5, y + (18 - x) + 5] != -1)
											toBeArray.Add (count - 2);
										else if (x + 1 <= 18 && y + (18 - x) - 1 >= 0 && game [x + 1, y + (18 - x) - 1] != col && game [x + 1, y + (18 - x) - 1] != -1)
											toBeArray.Add (count - 2);
										else
											toBeArray.Add (count);
									}
								}
							}
						}
					}
				}
			}
		}
		int[] result = new int[toBeArray.Count];
		for (int i = 0; i < toBeArray.Count; i++)
			result [i] = toBeArray [i];
		return result;
	}

	private int CanCapture(int[,] game, int col)
	{
		int result = 0;
		for (int x = 0; x < 19; x++) {
			for (int y = 0; y < 19; y++) {
				if (game [x, y] == col && Capturable (game, x, y, col))
					result++;
			}
		}
		return result;
	}






	/*----------------------------------------------------------------------------------------------------------------
	 * 
	 * 							FONCTION PRINCIPALE
	 * 
	 ---------------------------------------------------------------------------------------------------------------*/
	public int GetCostFromMapStatus(int[,] game, int nextTurnToPlay, int myColor, int captByWhite, int captByBlack, int profondeur)
	{
		int finalCost = 0;

		//-------------------------------------------------------Regarde si le jeu est fini
		bool whiteWins = AlignOrCaptWins (game, captByWhite, 1);
		bool blackWins = AlignOrCaptWins (game, captByBlack, 0);
		if (whiteWins || blackWins)
		{
			if ((whiteWins && !blackWins && myColor == 1) || (!whiteWins && blackWins && myColor == 0))
				return int.MinValue + (4 - profondeur);
			else
				return int.MaxValue - (4 - profondeur);
		}
			
		if (profondeur > 0)
			return 0;
		//-------------------------------------------------------Compte le nombre d'alignement avec possibilité de win
		int[] whiteAligns = CurrentAligns(game, 1);
		int[] blackAligns = CurrentAligns(game, 0);
		if ((myColor == 0 && System.Array.IndexOf (whiteAligns, 4) != -1 && nextTurnToPlay == 1) || (myColor == 1 && System.Array.IndexOf (blackAligns, 4) != -1 && nextTurnToPlay == 0))
			return int.MaxValue;
		else if ((myColor == 1 && System.Array.IndexOf (whiteAligns, 4) != -1 && nextTurnToPlay == 1) || (myColor == 0 && System.Array.IndexOf (blackAligns, 4) != -1 && nextTurnToPlay == 0))
			return int.MinValue;
		else if (myColor == 1) {
			foreach (int align in blackAligns)
				finalCost += align * 25;
			foreach (int align in whiteAligns){
				if (align == 4)
					finalCost -= 40;
				else if (align < 4)
					finalCost -= 19;
			}
		} 
		else {
			foreach (int align in blackAligns){
				if (align == 4)
					finalCost -= 40;
				else if (align < 4 )
					finalCost -= 19;
			}
			foreach (int align in whiteAligns)
				finalCost += align * 25;
		}


		//-------------------------------------------------------Compte le nombre de création de captures possibles
		int capturedBlack = CanCapture (game, 0);
		int capturedWhite = CanCapture (game, 1);
		if ((myColor == 0 && captByWhite == 8 && nextTurnToPlay == 1 && capturedBlack > 0) || (myColor == 1 && captByBlack == 8 && nextTurnToPlay == 0 && capturedWhite > 0))
			return int.MaxValue;
		else if ((myColor == 0 && captByBlack == 8 && nextTurnToPlay == 0 && capturedWhite > 0) || (myColor == 1 && captByWhite == 8 && nextTurnToPlay == 1 && capturedBlack > 0))
			return int.MinValue;
		else if (myColor == 1) {
			finalCost += (capturedWhite/2 * 10) * captByBlack;
			finalCost -= (capturedBlack/2 * 10) * captByWhite;
		} 
		else {
			finalCost += (capturedBlack/2 * 10) * captByWhite;
			finalCost -= (capturedWhite/2 * 10) * captByBlack;
		}

		//end
		return finalCost;
	}
}
