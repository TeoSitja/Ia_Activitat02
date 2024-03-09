using FSMs;
using UnityEngine;


[CreateAssetMenu(fileName = "Cat_Fsm", menuName = "Finite State Machines/Cat_Fsm", order = 1)]
public class Cat_Fsm : FiniteStateMachine
{
     PathFeeder _pathFeeder;
    Cat_BlackBoard _cat_BlackBoard;

    float _currentTime;

    public override void OnEnter()
    {
        _pathFeeder=gameObject.GetComponent<PathFeeder>();
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

         State WaitForSeconds = new State("WaitForSeconds",
            () => { }, // write on enter logic inside {}
            () => {_currentTime+=Time.deltaTime; }, // write in state logic inside {}
            () => { _currentTime=0;}  // write on exit logic inisde {}  
        );

        Transition SecondsWaited = new Transition("SecondsWaited",
            () => { return _cat_BlackBoard._timeToWait <= _currentTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
 );





        initialState=WaitForSeconds;

    }
}
