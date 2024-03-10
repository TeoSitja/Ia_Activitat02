using FSMs;
using UnityEngine;


[CreateAssetMenu(fileName = "Cat_Fsm", menuName = "Finite State Machines/Cat_Fsm", order = 1)]
public class Cat_Fsm : FiniteStateMachine
{
    PathFeeder_activitat02 _pathFeeder;
    Cat_BlackBoard _cat_BlackBoard;

    float _currentTime;
    GameObject _fish=null;

    public override void OnEnter()
    {
        _pathFeeder=gameObject.GetComponent<PathFeeder_activitat02>();
        _cat_BlackBoard= gameObject.GetComponent<Cat_BlackBoard>();

        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
         DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

         State travelThrowWaypoints = new State("travelThrowWaypoints",
            () => {_pathFeeder.target=_cat_BlackBoard._currentWaypoint; _pathFeeder.path=true;}, // write on enter logic inside {}
            () => {_currentTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { _currentTime=0;_pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State goToFish = new State("goToFish",
            () => {_pathFeeder.target=_fish;_pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {_pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State eatFish = new State("eatFish",
            () => {_pathFeeder.path=false;}, // write on enter logic inside {}
            () => { _currentTime+=Time.deltaTime;}, // write in state logic inside {}
            () => {_pathFeeder.path=true;_currentTime=0;}  // write on exit logic inisde {}  
        );
        
        Transition ChangeWaypoint = new Transition("ChangeWaypoint",
            () => { return _currentTime >= _pathFeeder.repathTime; }, // write the condition checkeing code in {}
            () => {_pathFeeder.target=_cat_BlackBoard.ChangeWaypoint();}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

          Transition FishDetected = new Transition("FishDetected",
            () => { return _fish=SensingUtils.FindRandomInstanceWithinRadius(gameObject,_cat_BlackBoard._fishTag,_cat_BlackBoard._fishDetectionRadius);}, // write the condition checkeing code in {}
            () => { _pathFeeder.seeker.StartPath(this.transform.position, _fish.transform.position, _pathFeeder.OnPathComplete);}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

          Transition FishEated = new Transition("FishEated",
            () => { return _currentTime>=_cat_BlackBoard._timeToEatFish;}, // write the condition checkeing code in {}
            () => {Destroy(_fish); _pathFeeder.seeker.StartPath(this.transform.position, _cat_BlackBoard.ChangeWaypoint().transform.position, _pathFeeder.OnPathComplete);}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

           Transition FishReached = new Transition("FishReached",
            () => { return _cat_BlackBoard._toFish>=SensingUtils.DistanceToTarget(gameObject,_fish);}, // write the condition checkeing code in {}
            () => {}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        AddStates(travelThrowWaypoints,goToFish,eatFish);

        AddTransition(travelThrowWaypoints,ChangeWaypoint,travelThrowWaypoints);

        AddTransition(travelThrowWaypoints,FishDetected,goToFish);
        AddTransition(goToFish,FishReached,eatFish);
        AddTransition(eatFish,FishEated,travelThrowWaypoints);





        initialState =travelThrowWaypoints;

    }
}
