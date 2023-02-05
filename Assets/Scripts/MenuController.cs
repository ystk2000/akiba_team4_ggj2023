using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{


    void Start()
    {
    }

    void OnStartGame()
    {
        SceneManager.LoadScene("Main");
    }
    

}
