using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public float hearts = 2f;
    private float vitaT = 2f;
    public Player player;
    private bool pause = false;
    private Vector3 pos;
    public CoinManager coinManager;
    public int enKill = 0;
    public TextMeshProUGUI HealtCount;
    private void Start(){
        showHealth();
        pos = new Vector3(-0.41f, -1.45f, -1.16f);
        InvokeRepeating(nameof(PassiveHeal), 4f, 4f);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space) && pause && !player.inside) {
            Unpause();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !pause && !player.inside) {
            Pause();
        }
        if(hearts <= 0 && !pause){
            GameOver();
            pause = true;
        }
        if(coinManager.coinCount >= 10 && enKill >= 6){
            win();
            pause = true;
        }
    }

    private void GameOver(){
        string s = "Enemies S";

        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(false);
                    child.gameObject.GetComponent<EnemyAI>().vita = -1f;
                }
            }
        }
        player.blockMov();
        GameObject gen = GameObject.FindWithTag("GameOver");
        Vector3 pos = player.transform.position;
        pos.z -= 10f;
        gen.transform.position = pos;
        foreach (Transform child in gen.transform){
            child.gameObject.SetActive(true);
        }
        CancelInvoke(nameof(PassiveHeal));  
    }

    public void Hit(float dmg){
        dmg = Mathf.Max(dmg, 0);
        hearts -= dmg;
        hearts = Mathf.Max(hearts, 0);
        showHealth();
    }
    public void StartNew(){
        player.transform.position = pos;
        coinManager.coinCount = 0;
        string s = "Enemies S";
        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(false);
                    EnemyAI g = child.gameObject.GetComponent<EnemyAI>();
                    g.drop = false;
                    g.resetVita();
                }
            }
        }
    }
    public void Restart(){
        InvokeRepeating(nameof(PassiveHeal), 4f, 4f);
        string s = "Enemies S";

        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(false);
                    EnemyAI g = child.gameObject.GetComponent<EnemyAI>();
                    g.drop = false;
                    g.resetVita();
                    g.gameObject.transform.position = g.posIn;
                }
            }
        }
        player.transform.position =  pos;
        FindAnyObjectByType<CoinManager>().resetCoin();
        hearts = vitaT;
        player.resetMov();

        GameObject gen = GameObject.FindWithTag("GameOver");
        foreach (Transform child in gen.transform){
            child.gameObject.SetActive(false);
        }
        player.whereAmI(true);
        pause = false;
        string t = "Coin";
        GameObject co = GameObject.FindGameObjectWithTag("coin");
        for(int i = 0; i < co.transform.childCount - 1; i++){
            co.transform.Find(t + i).gameObject.SetActive(true);
        }
        showHealth();
    }

    public void ToTheMenu(){
        Restart();
        player.transform.position = new Vector3(-0.41f, -24.66f, -1.16f);
        player.inside = !player.inside;
        Unpause();
    }
    public void Unpause(){
        GameObject pa = GameObject.FindWithTag("pause");
        GameObject gen = GameObject.FindWithTag("win");
        Vector3 pas = player.transform.position;
        pas.z = 3000.0f;
        foreach (Transform child in pa.transform){
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in gen.transform){
            child.gameObject.SetActive(false);
        }
        string s = "Enemies S";
        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(i == player.CosoInsideSec());
                }
            }
        }

        player.resetMov();
        pause = false;
    }
    public void Pause(){
        player.blockMov();
        GameObject gen = GameObject.FindWithTag("pause");
        Vector3 pos = player.transform.position;
        pos.z = -10.0f;
        foreach (Transform child in gen.transform){
            child.transform.position = pos;
            child.gameObject.SetActive(true);
        }

        string s = "Enemies S";

        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(false);
                }
            }
        }
        pause = true;
    }
    public void Quit(){
        Application.Quit();
    }
    private void win(){
        GameObject gen = GameObject.FindWithTag("win");
        Vector3 pos = player.transform.position;
        pos.z = -10.0f;
        gen.transform.position = pos;
        foreach (Transform child in gen.transform){
            child.transform.position = pos;
            child.gameObject.SetActive(true);
        }

        string s = "Enemies S";

        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(false);
                }
            }
        }
            
        player.blockMov();
        pause = true;
    }
    private void PassiveHeal(){
        if(hearts < vitaT){
            hearts += .5f;
            showHealth();
        }
    }
    private void showHealth(){
        HealtCount.text = "vita: " + hearts.ToString();
    }
}
