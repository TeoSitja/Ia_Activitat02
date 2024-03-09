using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBob_blackBord : MonoBehaviour
{
    public GameObject[] _waypointList;
   public GameObject _currentWaypoint;
   private int counter;

   public float _timeInWaypoint=5;
   public float _ToWaypoint=5;
    public GameObject ChangeWaypoint(){
      counter++;
		if (counter>=_waypointList.Length)
		{
			counter=0;
		}
		return _currentWaypoint=_waypointList[counter];
   }
}
