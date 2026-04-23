using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour{

    public Vector3 lastPos;
    public Vector3 posNow;
    public Player player;
    public Vector3 posIn;
    public float vita = 1.0f;
    private float vitaI = 1.0f;
    public Seeker seeker;
    public AIDestinationSetter destinationSetter;
    public Rigidbody2D rb;
    public GameObject coin;
    public AIPath aIPath;
    public bool drop = false;
    public Sprite altLook;
    public Sprite nLook;
    
    private void Start(){
        lastPos = transform.position;
        posIn = transform.position;
        Debug.Log(posIn);
    }
    private void Update(){
        if(lastPos != posNow){
            posNow = (transform.position - lastPos).normalized;
        }
        if(vita <= 0){
            Death();
        }
    }
    public void Hit(float danno, Vector3 en){
        vita -= danno;
        Vector3 knockBack = transform.position - en;
        knockBack.z = 0;
        transform.position += knockBack * 0.5f;
    }
    public void OnEnable(){
        if(Random.Range(0f, 1f) < 0.05f){
            GetComponent<SpriteRenderer>().sprite = altLook;
        }else{
            GetComponent<SpriteRenderer>().sprite = nLook;
        }
        destinationSetter.target = player.transform;
        seeker.StartPath(transform.position, player.transform.position);
    }
    public void OnDisable(){
        seeker.CancelCurrentPathRequest();
        rb.velocity = Vector3.zero;
    }
    public void Death(){
        if(!drop){
            Vector3 d = new Vector3(transform.position.x, transform.position.y, -9.0f);
            GameObject spawnedPrefab = Instantiate(coin, d, Quaternion.identity);
            spawnedPrefab.transform.SetParent(transform.parent);
            spawnedPrefab.transform.position = d;
            spawnedPrefab.SetActive(true);
            drop = !drop;
            FindAnyObjectByType<GameManager>().enKill++;
        }
        DropTP tpd = GetComponent<DropTP>();
        if(tpd != null){
            tpd.dropT();
        }
        transform.position = posIn;
        gameObject.SetActive(false);
    }
    public void Stop(){
        seeker.StartPath(transform.position, transform.position);
        destinationSetter.target = null;
        rb.velocity = Vector3.zero;
        aIPath = null;
    }
    public void StartAgain(){
        destinationSetter.target = player.transform;
        seeker.StartPath(transform.position, player.transform.position);
    }
    public int EnInsideSec(){
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
    public void resetVita(){
        vita = vitaI;
    }
    public void resetPos(){
        transform.position = posIn;
    }
}