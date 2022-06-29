using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScript : MonoBehaviour
{
    private PlayerController playerController;
    private float executionDelaySeconds = 4f;
    public GameObject levelsUI;

    
    // Start is called before the first frame update
    void Start()
    {
        //find the Playercontroller script
        playerController = GameObject.FindObjectOfType<PlayerController>();
        StartCoroutine(RandomMovePlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator RandomMovePlayer()
    {
        yield return new WaitForSeconds(1f);
        playerController.move = PlayerController.Move.moveRight;
        while (true)
        {
            yield return new WaitForSeconds(executionDelaySeconds);
            playerController.move = PlayerController.Move.moveRight;
            yield return new WaitForSeconds(executionDelaySeconds);
            playerController.move = PlayerController.Move.jumpLeft;
            yield return new WaitForSeconds(executionDelaySeconds*2f);
            playerController.move = PlayerController.Move.moveRight;
            yield return new WaitForSeconds(executionDelaySeconds*2f);
            playerController.move = PlayerController.Move.moveLeft;
            yield return new WaitForSeconds(executionDelaySeconds*1.5f);
            playerController.move = PlayerController.Move.jumpRight;
            yield return new WaitForSeconds(executionDelaySeconds*1.2f);
            playerController.move = PlayerController.Move.moveLeft;
        }
        
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Level1-1");
    }

    public void LoadCorrespondingScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
