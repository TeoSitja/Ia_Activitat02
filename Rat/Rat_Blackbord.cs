using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Blackbord : MonoBehaviour
{
    public GameObject _cheese;
    public GameObject _home;
    public GameObject _tunelEntryA;
    public GameObject _tunelEntryB;

    public string _tunelTag="TUNEL";
    public float _timeToGo;
    public float _toReachHome;
    public float _toReachTunel;
    public float _toReachObjective;

    public GameObject ExitTunel(GameObject tunelEntry){
        if (tunelEntry==_tunelEntryA)
        {
            return _tunelEntryB;
        }
        if (tunelEntry==_tunelEntryB)
        {
            return _tunelEntryA;
        }
        return _home;
    }
}
