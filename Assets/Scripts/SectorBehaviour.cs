using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorBehaviour : MonoBehaviour{

    public  Player player;
    private void Update(){
        EnemyAI en = transform.GetChild(0).GetComponent<EnemyAI>();

        if (player.CosoInsideSec() != en.EnInsideSec()){
            en.resetVita();
        }
            
    }
}
