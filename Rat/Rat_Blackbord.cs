using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Blackbord : MonoBehaviour
{
    public GameObject _cheese;
    public GameObject _home;

    public Sprite _sailorSprite;
    public Sprite _ratSprite;

    public GameObject[] _tunelEntries;

    

    public string _tunelTag="TUNEL";
    public float _timeToGo;
    public float _toReachHome;
    public float _toReachTunel;
    public float _toReachObjective;

  

    public GameObject ExitTunel(){
        
       float distanceToObjective=SensingUtils.DistanceToTarget(gameObject,_cheese);
       GameObject exitTunel=null;

       foreach (var item in _tunelEntries)
       {
                if (distanceToObjective>=SensingUtils.DistanceToTarget(item,_cheese))
          {
            
                distanceToObjective=SensingUtils.DistanceToTarget(item,_cheese);
                exitTunel=item;
          }
          
       }
       
       return exitTunel;
    }

    public GameObject ReCalculateTunel(GameObject lastTunel){
        
       float distanceToObjective=SensingUtils.DistanceToTarget(gameObject,_cheese);
       GameObject exitTunel=null;

       foreach (var item in _tunelEntries)
       {
        if (item!=lastTunel)
        {
           if (distanceToObjective>=SensingUtils.DistanceToTarget(item,_cheese))
          {
            
                distanceToObjective=SensingUtils.DistanceToTarget(item,_cheese);
                exitTunel=item;
          }
                
        }
          
       }
       
       return exitTunel;
    }
}
