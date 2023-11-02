using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{

    [SerializeField] private GameObject enemyTemplate;
    [SerializeField] private GameObject enemyTemplate1;
    [SerializeField] private float SpawnTime;
    [SerializeField] private int enemySpawnNum;
    [SerializeField] private int maxEnemy;
    [SerializeField] private int maxProjectileEnemy;
    [SerializeField] private GameObject spawnObject;
    

   
    private int totalEnemy=0;
    private int enemy2Num=0;
    private  int preset;
    private float timeToSpawn;
    private float totalTime;
    private GameObject spawn;
    private float halfMarker;

    // Start is called before the first frame update
    void Start()
    {
        SetTimetoSpawn();
        preset=Random.Range(0,3);
        int levels=FindObjectOfType<ManageScene>().levelCount;
        if(levels==1){
           preset=0;

        }
        else if(levels==2){
            preset=2;
        }
    }

    // Update is called once per frame
    // Adapted from Workshop 4 of COMP30019
    void Update()
    {
        timeToSpawn -= Time.deltaTime;
        totalTime += Time.deltaTime;
        halfMarker+= Time.deltaTime;
        if(halfMarker>=30){
            if(SpawnTime-1!=0){
                SpawnTime-=0.5f;
            }
            enemySpawnNum+=1;
            halfMarker=0;
        }
        if(timeToSpawn <=0 && totalTime<90){
            for(var enemy=0; enemy<this.enemySpawnNum;enemy++){
                //Randomly Select the enemy to Spawn 
                if(preset==0){
                    preset1();
                }
                else if(preset==1){
                    preset2();
                }
                else if(preset==2){
                    preset3();
                }

            }
            SetTimetoSpawn();
        }
        
    }
    private void preset1(){
        if(totalEnemy<=maxEnemy){
            var enemyType=Random.Range(0,1);
            Transform enemyTransform= Instantiate(this.enemyTemplate).transform;
            if(enemyType==0){
                enemyTransform= Instantiate(this.enemyTemplate).transform;
            }
            enemyTransform.parent= transform;
            //enemyTransform.localPosition= GenerateNavMeshPosition();
            Vector3 randomPosition=GenerateNavMeshPosition();
            enemyTransform.GetComponent<NavMeshAgent>().Warp(randomPosition);
            spawn = Instantiate(spawnObject, randomPosition, Quaternion.identity);
            totalEnemy+=1;
            FindObjectOfType<SoundManager>().Play("Enemy Spawning Sound");
            Destroy(spawn, 0.55f);
        }
    }

    private void preset2(){
      if(totalEnemy<=maxEnemy){
            var enemyType=Random.Range(0,1);
            Transform enemyTransform= Instantiate(this.enemyTemplate).transform;
            if(enemyType==0){
                enemyTransform= Instantiate(this.enemyTemplate1).transform;
            }
            enemyTransform.parent= transform;
            //enemyTransform.localPosition= GenerateNavMeshPosition();
            Vector3 randomPosition=GenerateNavMeshPosition();
            enemyTransform.GetComponent<NavMeshAgent>().Warp(randomPosition);
            spawn = Instantiate(spawnObject, randomPosition, Quaternion.identity);
            totalEnemy+=1;
            FindObjectOfType<SoundManager>().Play("Enemy Spawning Sound");
            Destroy(spawn, 0.55f);
        }  
    }

    private void preset3(){
        if(totalEnemy<=maxEnemy){
            var enemyType=Random.Range(0,2);
            Transform enemyTransform= Instantiate(this.enemyTemplate).transform;
            if(enemyType==0){
                enemyTransform= Instantiate(this.enemyTemplate).transform;
            }
            else if(enemyType==1 && enemy2Num<=maxProjectileEnemy){
                enemyTransform= Instantiate(this.enemyTemplate1).transform;
                enemy2Num+=1;
            }
            enemyTransform.parent= transform;
            //enemyTransform.localPosition= GenerateNavMeshPosition();
            Vector3 randomPosition=GenerateNavMeshPosition();
            enemyTransform.GetComponent<NavMeshAgent>().Warp(randomPosition);
            spawn = Instantiate(spawnObject, randomPosition, Quaternion.identity);
            totalEnemy+=1;
            FindObjectOfType<SoundManager>().Play("Enemy Spawning Sound");
            Destroy(spawn, 0.55f);
        }    
    }

    private void SetTimetoSpawn(){
        timeToSpawn = SpawnTime;
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

    public void enemyDead(){
        totalEnemy-=1;
    }

    public void enemy2Dead(){
        enemy2Num-=1;
    }
}
