using UnityEngine;

public class Coin : MonoBehaviour
{
     private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            FindAnyObjectByType<CoinManager>().addCoin();
            string parentName = transform.parent.name;
            bool isDifferent = true;
                for (int i = 1; i <= 5; i++){
                    string expectedName = "Enemies S" + i;
                    if (parentName == expectedName){
                        isDifferent = false;
                        break;
                    }
                }
            if(isDifferent){
                gameObject.SetActive(false);
            }else{
                Destroy(gameObject);
            }

        }
    }
}