using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Static Instance
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned GameManager", typeof(GameManager)).GetComponent<GameManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic();
    }


    public void loadLevel(int levelID)
    {
        switch (levelID)
        {
            case 1:
                SceneManager.LoadScene("Level1");
                break;
            case 2:
                SceneManager.LoadScene("Level1 - Graph");
                break;

            //todo: other levels

            default:
                break;
        }
    }

   
}
