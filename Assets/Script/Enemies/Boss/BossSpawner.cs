using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyTemplate;
    [SerializeField] private GameObject spawnObject;

    [SerializeField] private float SpawnTime;
    [SerializeField] private float enemyCount;
    [SerializeField] private GameObject minionTemplate;
    [SerializeField] private GameObject minionTemplate1;

    public static BossSpawner instance;
    private GameObject spawn;
     private float timeToSpawn;
    private float totalTime;
    
    private Transform enemyTransform;

    private Transform bossTransform;

    // Start is called before the first frame update

    void Awake(){
        instance=this;
    }
    void Start()
    {
        SetTimetoSpawn();
        bossTransform=Instantiate(this.enemyTemplate).transform;
        bossTransform.parent=transform;
        Vector3 randomPosition=GenerateNavMeshPosition();
        bossTransform.GetComponent<NavMeshAgent>().Warp(randomPosition);
        spawn = Instantiate(spawnObject, randomPosition, Quaternion.identity);
        FindObjectOfType<SoundManager>().Play("Boss Spawn");
        Destroy(spawn, 0.55f);

    }

    void Update()
    {
        timeToSpawn -= Time.deltaTime;
        totalTime += Time.deltaTime;
        

        if(timeToSpawn <=0 && totalTime<120){
            for(var enemy=0; enemy<enemyCount;enemy++){
                var preset=Random.Range(0,2);
                if(preset==0){
                    enemyTransform= Instantiate(this.minionTemplate).transform;
                }
                else{
                    enemyTransform= Instantiate(this.minionTemplate1).transform;    
                }
                enemyTransform.parent= transform;
                Vector3 randomPosition=GenerateNavMeshPosition();
                enemyTransform.GetComponent<NavMeshAgent>().Warp(randomPosition);
                spawn = Instantiate(spawnObject, randomPosition, Quaternion.identity);
                FindObjectOfType<SoundManager>().Play("Enemy Spawning Sound");
                Destroy(spawn, 0.55f);


            }
            SetTimetoSpawn();
        }
        
    }


    private Vector3 GeneratePosition(){
        //NavMeshHit hit;
        float x = Random.Range(-30.0f,-10.0f);
        float y= Random.Range(-1.0f,1.0f);
        float z= Random.Range(-10.0f,10.0f);
        Vector3 position = new Vector3(x,y,z);
        return position;
    }

    public Vector3 GenerateNavMeshPosition(){
        NavMeshHit hit;
        Vector3 position=GeneratePosition();
        while(NavMesh.SamplePosition(position,out hit, Mathf.Infinity, NavMesh.AllAreas)== false){
            position=GeneratePosition();
        }
        Vector3 randomPosition= hit.position;
        return randomPosition;
    }

    private void SetTimetoSpawn(){
        timeToSpawn = SpawnTime;
    }


}
