
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour{
    public int coinCount;
    public Text coinText;

    public void addCoin(){
        coinCount++;
    }
    private void Update() {
        coinText.text = coinCount.ToString();
    }
    public void resetCoin(){
        coinCount = 0;
    }
}
