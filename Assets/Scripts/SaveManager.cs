using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SaveData{
    public int enKill;
    public bool droppedTp;
    public bool initialized;
    public int last;
    public int coins;
    public Vector3 playerPosition;
    public List<EnemyData> enemyDataList;
    public List<bool> coin = new List<bool>();
}

[System.Serializable]
public class EnemyData{
    public string enemyName;
    public Vector3 enemyPosition;
    public string parEne;
    public bool isActive;
    public bool hasDropped;

    public EnemyData(string parent, Vector3 position, bool active, bool dropped){
        parEne = parent;
        enemyPosition = position;
        isActive = active;
        hasDropped = dropped;
    }
}

public class SaveManager : MonoBehaviour{
    public Player player;
    public CoinManager coinManager;
    public EnemyAI dropTP;
    private const string SAVE_KEY = "GameSave";

    public void SaveGame(){
        SaveData saveData = new SaveData();

        saveData.playerPosition = player.transform.position;
        saveData.enemyDataList = new List<EnemyData>();
        saveData.coins = coinManager.coinCount;
        saveData.initialized = player.initialized;
        saveData.last = player.last;
        saveData.droppedTp = dropTP.GetComponent<DropTP>().dropped;
        saveData.enKill = FindAnyObjectByType<GameManager>().enKill;

        string enemyTag = "Enemies S";

        for (int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(enemyTag + i);
            if (enemiesGameObject != null){
                foreach(Transform child in enemiesGameObject.transform){
                    bool isActive = child.gameObject.activeSelf;
                    bool hasDropped = child.GetComponent<EnemyAI>().drop;

                    saveData.enemyDataList.Add(new EnemyData(enemyTag + i, child.transform.position, isActive, hasDropped));
                }
            }
        }
        string t = "Coin";
        GameObject co = GameObject.FindGameObjectWithTag("coin");
        for(int i = 0; i < co.transform.childCount - 1; i++){
            Transform child = co.transform.Find(t + i);
            bool b = child.gameObject.activeSelf;
            saveData.coin.Add(b);
        }
        string saveDataJson = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString(SAVE_KEY, saveDataJson);
        PlayerPrefs.Save();
    }
    public void LoadGame(){
        string saveDataJson = PlayerPrefs.GetString(SAVE_KEY);

        if (!string.IsNullOrEmpty(saveDataJson)){
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

            player.transform.position = saveData.playerPosition;
            player.initialized = saveData.initialized;
            player.last = saveData.last;
            dropTP.GetComponent<DropTP>().dropped = saveData.droppedTp;
            dropTP.GetComponent<DropTP>().tr();
            FindAnyObjectByType<GameManager>().enKill = saveData.enKill;
            coinManager.coinCount = saveData.coins;

            player.whereAmI(true);
            if (saveData.enemyDataList != null){
                foreach(var enemyData in saveData.enemyDataList){
                    GameObject enemyObject = GameObject.Find(enemyData.parEne);
                    foreach(Transform child in enemyObject.transform){
                        child.transform.position = enemyData.enemyPosition;
                        child.gameObject.SetActive(enemyData.isActive);
                        child.GetComponent<EnemyAI>().drop = enemyData.hasDropped;
                    }
                }
            }
            string t = "Coin";
            GameObject co = GameObject.FindGameObjectWithTag("coin");
            int d = 0;
            foreach(bool i in saveData.coin){
                co.transform.Find(t + d).gameObject.SetActive(i);
                d++;
            }
        }
    }
}
