using FSMs;
using UnityEngine;
using Steerings;
using System.ComponentModel;

[CreateAssetMenu(fileName = "Fsm_CatSon", menuName = "Finite State Machines/Fsm_CatSon", order = 1)]
public class Fsm_CatSon : FiniteStateMachine
{
   PathFeeder_activitat02 _pathFeeder;
    CatSon_Blackboard _blackBoard;

    

    public override void OnEnter()
    {
         _pathFeeder=gameObject.GetComponent<PathFeeder_activitat02>();
        _blackBoard= gameObject.GetComponent<CatSon_Blackboard>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State followParent = new State("followParent",
            () => {_pathFeeder.target=_blackBoard._parent;_pathFeeder.path=true;}, // write on enter logic inside {}
            () => {_blackBoard._currentTiredeness-=Time.deltaTime; }, // write in state logic inside {}
            () => { _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State restingTime = new State("restingTime",
            () => {_pathFeeder.path=false;}, // write on enter logic inside {}
            () => {_blackBoard._currentTiredeness+=Time.deltaTime; }, // write in state logic inside {}
            () => {_pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

          Transition Tired = new Transition("Tired",
            () => { return _blackBoard._currentTiredeness<=_blackBoard._tirednessToRest;}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

          Transition Rested = new Transition("Rested",
            () => { return _blackBoard._currentTiredeness>=_blackBoard._tirednessToBeRested;}, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        AddStates(followParent,restingTime);

        AddTransition(followParent,Tired,restingTime);
        AddTransition(restingTime,Rested,followParent);

        initialState=followParent;


    }
}
