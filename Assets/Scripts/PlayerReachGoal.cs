using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerReachGoal : MonoBehaviour
{
    public string nextSceneName;
    public GameObject levelSuccessUI;
    public GameObject codePanel;
    public GameObject duplicatedCodePanel;//duplication destination
    public AudioSource successAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0f; //freeze the game
            successAudio.Play();
            levelSuccessUI.SetActive(true);
            LevelSuccess();
            //SceneManager.LoadScene(nextSceneName);
        }
    }
    public void LevelSuccess()
    {
        Instantiate(codePanel.transform, duplicatedCodePanel.transform);

    }
    
}
