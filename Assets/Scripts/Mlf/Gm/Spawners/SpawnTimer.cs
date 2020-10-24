using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mlf.Gm.Spawners
{
  public class SpawnTimer : MonoBehaviour {


   
    public List<BaseSpawner> every15SecSpawners = new List<BaseSpawner>();
    public List<BaseSpawner> every1MinSpawners = new List<BaseSpawner>();
    public List<BaseSpawner> every5MinSpawners = new List<BaseSpawner>();
    public int delay1MinCount = 0;
    public int delay5MinCount = 0;


    public bool isCoroutine15SecExecuting = false;
    public static  SpawnTimer instance;

    private void Awake() {
      if(SpawnTimer.instance == null) 
        SpawnTimer.instance = this;
      else
        Debug.LogWarning("Spanwer singleton has a double instance");

      Debug.Log("-----------------Spawn Timer-------------");
    }


    private void Update() {
      StartCoroutine(ExecuteAfter15SecTime());
    }

    IEnumerator ExecuteAfter15SecTime()
    {
        if(isCoroutine15SecExecuting) yield break;

        isCoroutine15SecExecuting = true;

        yield return new WaitForSeconds(1);

        delay1MinCount++;
        delay5MinCount++;

        if(delay1MinCount >= 4) {
          execute1Min();
          delay1MinCount = 0;

        }

        if(delay5MinCount >= 20) {
          execute5Min();
          delay5MinCount = 0;

        }
        //Debug.Log("Running 15 Sec " + every15SecSpawners.Count);
        for(int i = 0; i < every15SecSpawners.Count; i ++){
          //Debug.Log("IIII " + i);
          every15SecSpawners[i].spawn();
        }

        isCoroutine15SecExecuting = false;
    
        // Code to execute after the delay
    }

    private void execute1Min() {
      for(int i = 0; i < every1MinSpawners.Count; i ++){
        every1MinSpawners[i].spawn();
      }
    }

    private void execute5Min() {
      for(int i = 0; i < every5MinSpawners.Count; i ++){
        every5MinSpawners[i].spawn();
      }
    }
    
  } 
}



