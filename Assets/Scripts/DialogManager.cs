using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{

    public GameObject dialogHolder;
    public static bool isVisible = false;
    
    public Text messageText;
    public static string message = "What do you want?";
    public Text exitButtonText;
    public static string exitButton = "exit";
    public Text continueButtonText;
    public static string continueButton = "reset";

    void Update()
    {
        messageText.text = message;
        exitButtonText.text = exitButton;
        continueButtonText.text = continueButton;
        dialogHolder.SetActive(isVisible);

        onESC();
    }

    void OnContinue()
    {
        isVisible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnOkay()
    {
        isVisible = false;
        Score.score = Score.initScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnExit()
    {
        isVisible = false;
        Lives.lives = 3;
        Score.initScore = 0;
        Score.score = 0;
        Score.totalPosibleScore = 0;
        SceneManager.LoadSceneAsync(0);
    }

    void OnReset()
    {
        Lives.lives = 3;
        Score.initScore = 0;
        Score.score = 0;
        Score.totalPosibleScore = 0;
        isVisible = false;
        SceneManager.LoadSceneAsync(1);
    }


    public void ExitClick() {
        isVisible = false;
        OnExit();
    }

    public void ContinueClick() {
        isVisible = false;
        if(continueButton == "continue") 
        {
            OnContinue();
        } else if(continueButton == "reset") 
        {
            OnReset();
        } else if(continueButton == "okay") 
        {
            OnOkay();
        }
    }


    public static void onESC() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(!isVisible) {
                message = "What do you want?";
                exitButton = "exit";
                continueButton = "reset";
            }
            isVisible = !isVisible;
        }
    }

    public static void LostLife() {
        message = "You have lost your life";
        exitButton = "exit";
        continueButton = "okay";
        Lives.lives -= 1;
    }
    public static void Dead() {
        message = "You are dead!";
        exitButton = "exit";
        continueButton = "reset";
        Lives.lives -= 1;
    }

    public static void NextLevel() {
        message = "Go to next level";
        exitButton = "exit";
        continueButton = "continue";
    }
    public static void Win() {
        message = "You have succeeded!";
        exitButton = "exit";
        continueButton = "reset";
    }

    public static void HitByGhostTriggerDialog() {
        if(Lives.lives == 1) {
            Dead();
        } else {
            LostLife();
        }
        isVisible = true;
    }

    public static void PassLevelTriggerDialog() {
        if(Score.score == Score.totalPosibleScore) { 
            if(SceneManager.GetActiveScene().buildIndex == 3) {
                Win();
            } else {
                Score.initScore = Score.score;
                NextLevel();
            }
            isVisible = true;
        }
    }

}
