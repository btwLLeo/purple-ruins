using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTP : MonoBehaviour{
    public Vector3 tppos;
    public Vector3 pos;
    public GameObject tpPrefab;
    public bool dropped = false;
    public void dropT(){
        GameObject tpInit = Instantiate(tpPrefab, pos, Quaternion.identity);
        tpInit.GetComponent<tp>().pos = tppos;
        dropped = true;
    }
    public void tr(){
        if(dropped){
            GameObject tpInit = Instantiate(tpPrefab, pos, Quaternion.identity);
            tpInit.GetComponent<tp>().pos = tppos;
        }
    }
}
