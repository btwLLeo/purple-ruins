using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour{

    public Vector3 lastPos;
    public Vector3 posNow;
    public GameObject swordPrefab;
    private GameObject sword;
    public Vector3 direction;
    public SpriteRenderer spriteRenderer;
    public float vMax = .25f;
    private bool canL = true;
    private bool canR = true;
    public bool canU = true;
    private bool canD = true;
    private bool canA = true;
    private double gradi = 0.0d;
    public int last = -1;
    public bool inside = true;
    public bool initialized = false;

    private void Start(){
        whereAmI(true);
        if(!initialized){
            last = CosoInsideSec();
            initialized = true;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        sword = null;
        lastPos = transform.position;
        posNow = new Vector3(0, 1, 0);
    }

    private void Update(){
        if(inside){
            inside = isInsideMenu();
        }

        if(CosoInsideSec() != last){
            whereAmI(true);
            last = CosoInsideSec();
        }
        direction = Vector3.zero;
        if (Input.GetMouseButtonDown(0) && canA){
            SpawnSword();
            canA = false;
            Invoke(nameof(attackAgain), 2);
        }
        if(Input.GetKey(KeyCode.W) && canU){
            direction += Vector3.up;
        }
        if(Input.GetKey(KeyCode.S) && canD){
            direction += Vector3.down;
        }
        if(Input.GetKey(KeyCode.A) && canL){
            direction += Vector3.left;
            spriteRenderer.flipX = true;
        }
        if(Input.GetKey(KeyCode.D) && canR){
            direction += Vector3.right;
            spriteRenderer.flipX = false;
        }
        if(lastPos != transform.position){
            posNow = (transform.position - lastPos).normalized;
        }
        lastPos = transform.position;

        transform.position += direction.normalized * vMax * Time.deltaTime;
        if(sword != null){
            sword.transform.position = transform.position + posNow;
            gradi = Math.Atan2(posNow.y, posNow.x) * (180.0 / Math.PI) + 90.0f;
            sword.transform.eulerAngles = Vector3.forward * (float)gradi;
        }
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            FindAnyObjectByType<GameManager>().Hit(.5f);
            knock(other.gameObject.transform.position);
        }
        if(other.gameObject.tag == "Water"){
            FindFirstObjectByType<GameManager>().Hit(9999999999f);
        }
    }

    void SpawnSword(){
        gradi = Math.Atan2(posNow.y, posNow.x) * (180.0 / Math.PI) + 90.0f;
        
        if(sword == null){
            sword = Instantiate(swordPrefab, transform.position + posNow, Quaternion.identity);
            sword.transform.eulerAngles = Vector3.forward * (float)gradi;
        }
        Destroy(sword, 1);
    }

    public void gotTP(){
        canU = false;
        direction.y = 0;
        Invoke(nameof(resetMov), 1f);
    }
    public void resetMov(){
        canD = true;
        canR = true;
        canU = true;
        canL = true;
        canA = true;
    }
    public void blockMov(){
        canD = false;
        canL = false;
        canR = false;
        canU = false;
        canA = false;
    }
    public void knock(Vector3 en){
        Vector3 knockBack = transform.position - en;
        knockBack.z = transform.position.z;
        transform.position += knockBack * 0.5f;
        if(sword != null){
            sword.transform.position += knockBack * 0.5f;
        }
    }
    public int CosoInsideSec(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

        if (hit.collider.CompareTag("sec1")){
            return 1;
        }
        if (hit.collider.CompareTag("sec2")){
            return 2;
        }
        if (hit.collider.CompareTag("sec3")){
            return 3;
        }
        if (hit.collider.CompareTag("sec4")){
            return 4;
        }
        if (hit.collider.CompareTag("sec5")){
            return 5;
        }
        return 0;
    }
    public void whereAmI(bool ene){
        GameObject[] hides = new GameObject[5];

        for (int i = 0; i < hides.Length; i++){
            hides[i] = GameObject.Find("Hide" + (i + 1));
        }

        int n = CosoInsideSec();
        Debug.Log(n);
        if(ene){
            DisableAllExceptNumber(n);
        }   
        for (int i = 0; i < hides.Length; i++){
            SetActiveSafely(hides[i], i + 1 != n);
        }
    }
    void SetActiveSafely(GameObject obj, bool stato){
        if (obj != null){
            foreach (Transform child in obj.transform){
                child.gameObject.SetActive(stato);
            }
        }
    }
    void DisableAllExceptNumber(int n){
        string s = "Enemies S";

        for(int i = 1; i <= 5; i++){
            GameObject enemiesGameObject = GameObject.Find(s + i);
            if(enemiesGameObject != null){
                foreach (Transform child in enemiesGameObject.transform){
                    child.gameObject.SetActive(i == n);
                }
            }
        }
    }
    private bool isInsideMenu(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        Debug.Log(hit.collider.tag);
        if(hit.collider.CompareTag("Start")){
            blockMov();
            last = -1;
            return true;
        }else{
            resetMov();
            posNow = new Vector3(0, 1, 0);
            return false;
        }
    }
    private void attackAgain(){
        canA = true;
    }
}