using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [SerializeField]private int playerHP;
    [SerializeField]private int enemyHP;
    [SerializeField] private int MaxPlayerHP;
    [SerializeField] private int MaxEnemyHP;
    private int loveScore;
 
    /*[SerializeField]private int reduceValue_playerHp;
    [SerializeField] private int reduceValue_enemyHp;
    [SerializeField] private int increaseValue_loveScore;*/


    [SerializeField] private GameObject overTextObject;
    [SerializeField] private GameObject clearTextObject;
    private TextMeshProUGUI overText;
    private TextMeshProUGUI clearText;
    private bool gameClear;
    private bool gameOver;
    [SerializeField] private Slider slider_playerHP;
    [SerializeField] private Slider slider_enemyHP;
    [SerializeField] private float currentTime;
    [SerializeField] private float loveTime;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private int heartCount;
    private GameObject heartobj;
    [SerializeField] private GameObject heartpare;


    //public int score; //New!

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitGame()
    {
        playerHP = MaxPlayerHP;
        enemyHP = MaxEnemyHP;
        loveScore = 0;
        gameClear = false;
        gameOver = false;
        overText = overTextObject.GetComponent<TextMeshProUGUI>();
        clearText = clearTextObject.GetComponent<TextMeshProUGUI>();
        overText.text = "";
        clearText.text = "";
        slider_playerHP.maxValue = MaxPlayerHP;
        slider_enemyHP.maxValue = MaxEnemyHP;
        slider_playerHP.value = MaxPlayerHP;
        slider_enemyHP.value = MaxEnemyHP;
        currentTime = 0f;
        loveTime = 10f;
        heartCount = 0;
    }

    public void ReducePlayerHP(int reduceValue_playerHp)
    {
        playerHP -= reduceValue_playerHp;
        slider_playerHP.value -= reduceValue_playerHp;
    }

    public void ReduceEnemyHP(int reduceValue_enemyHp)
    {
        if (heartCount >= 1)
        {
            heartCount--;
            Destroy(heartobj);
        }
        currentTime = 0f;
        enemyHP -= reduceValue_enemyHp;
        slider_enemyHP.value -= reduceValue_enemyHp;
    }

    public void IncreaseLoveScore(int increaseValue_loveScore)
    {
        heartCount+= 1;
        heartobj = Instantiate(heartPrefab, new Vector3(-12.31f + heartCount, 8.06f, 3.0f), Quaternion.identity);
        heartobj.transform.parent = heartpare.transform;
        loveScore += increaseValue_loveScore;
    }




    private void GameOver()
    {
        SceneManager.LoadScene("OverScene");
    }

    private void GameClear()
    {
        SceneManager.LoadScene("ClearScene");
    }

    private void Start()
    {
        InitGame();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        //ゲームオーバー処理
        if(playerHP <= 0 && (!gameClear))
        {
            gameOver = true;
            GameOver();
        }

        //ゲームクリア処理
        if(enemyHP <= 0 &&(!gameOver))
        {
            gameClear = true;
            GameClear();
        }

        if(heartCount >= 4.0f)
        {
            SceneManager.LoadScene("LoveScene");
        }

        if(currentTime > loveTime)
        {
            currentTime = 0f;
            IncreaseLoveScore(1);
        }
    }
}
