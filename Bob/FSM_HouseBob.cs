using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_HouseBob", menuName = "Finite State Machines/FSM_HouseBob", order = 1)]
public class FSM_HouseBob : FiniteStateMachine
{
    PathFeeder_activitat02 _pathFeeder;
    HouseBob_blackBord _blackBoard;

    private float _currentTime;

    public override void OnEnter()
    {
         _pathFeeder=gameObject.GetComponent<PathFeeder_activitat02>();
        _blackBoard= gameObject.GetComponent<HouseBob_blackBord>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
       State travelThrowWaypoints = new State("travelThrowWaypoints",
            () => {_pathFeeder.target=_blackBoard.ChangeWaypoint();_pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

        State waitInWaypoint = new State("waitInWaypoint",
            () => {_currentTime=0;_pathFeeder.path=false;}, // write on enter logic inside {}
            () => {_currentTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

        Transition TimeOutInWaypoint = new Transition("TimeOutInWaypoint",
            () => { return _currentTime >=_blackBoard._timeInWaypoint; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition InWaypoint = new Transition("InWaypoint",
            () => { return _blackBoard._ToWaypoint>=SensingUtils.DistanceToTarget(gameObject,_blackBoard._currentWaypoint);}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        AddStates(travelThrowWaypoints,waitInWaypoint);

        AddTransition(travelThrowWaypoints,InWaypoint,waitInWaypoint);
        AddTransition(waitInWaypoint,TimeOutInWaypoint,travelThrowWaypoints);

        initialState=travelThrowWaypoints;

        
    }
}
