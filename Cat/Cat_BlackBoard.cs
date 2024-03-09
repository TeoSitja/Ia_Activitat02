using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_BlackBoard : MonoBehaviour
{

   public GameObject[] _waypointList;
   public GameObject _currentWaypoint;
   private int counter;

   public float _toReachObjective;
   public float _toFish=3;
   public string _fishTag="FISH";
   public float _fishDetectionRadius=10;
   public float _timeToEatFish=5;


   public GameObject ChangeWaypoint(){
      counter++;
		if (counter>=_waypointList.Length)
		{
			counter=0;
		}
		return _currentWaypoint=_waypointList[counter];
   }
}
   