using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void playGame() {
        Pickup.haventLost = true;
        Pickup.hits = 0;
        EnemyController.gameOver = false;
        SceneManager.LoadScene(1);
    }
	
    public void getHelp() {

    }

    public void QuitGame() {
        Application.Quit();
    }
}
