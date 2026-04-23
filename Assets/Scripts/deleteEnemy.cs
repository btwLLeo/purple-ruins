using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteEnemy : MonoBehaviour{
    public float danno = .5f;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            EnemyAI t = other.GetComponent<EnemyAI>();
            Debug.Log(t.vita);
    
            if(t.vita > 0.0f){
                t.Hit(danno, transform.position);
            }
        }
    }
}