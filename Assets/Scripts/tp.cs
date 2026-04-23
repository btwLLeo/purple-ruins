using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class tp : MonoBehaviour{

    public Vector3 pos;
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Player t = other.GetComponent<Player>();
            if(t.canU){
                t.transform.position = pos;
            }
            Transform parent = transform.parent;
            if(parent == null || (parent != null && parent.name != "TPs")){
                GameObject en = GameObject.Find("Enemies S4");
                en.GetComponent<DropTP>().dropped = true;
                Destroy(gameObject);
            }
            t.gotTP();
        }
    }
}
