using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ToScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void LoadScene()
    {
        Debug.Log("Loading Scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}