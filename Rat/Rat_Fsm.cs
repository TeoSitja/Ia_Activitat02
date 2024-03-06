using FSMs;
using UnityEngine;
using Steerings;
using UnityEditor;
using Pathfinding;

[CreateAssetMenu(fileName = "Rat_Fsm", menuName = "Finite State Machines/Rat_Fsm", order = 1)]
public class Rat_Fsm : FiniteStateMachine
{
    PathFeeder _pathFeeder;
    Rat_Blackbord _blackbord;
    SteeringContext _steeringContext;
    float _currentTime;
    GameObject _actuallTunelEntry;
    SpriteRenderer _sprite;

    float _sailingSpeed;
    float _walkingSpeed;
    
    
    GraphNode node;

    public override void OnEnter()
    {
        _pathFeeder=GetComponent<PathFeeder>();
        _blackbord=GetComponent<Rat_Blackbord>();
        _sprite=GetComponent<SpriteRenderer>();
        _steeringContext=GetComponent<SteeringContext>();

        _sailingSpeed=_steeringContext.maxSpeed/10;
        _walkingSpeed=_steeringContext.maxSpeed;
       
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        base.DisableAllSteerings();
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
            () => {_currentTime+=Time.deltaTime;}, // write in state logic inside {}
            () => { _currentTime=0;}  // write on exit logic inisde {}  
        );

         State GoToCheese = new State("GoToCheese",
            () => { _pathFeeder.target=_blackbord._cheese; _pathFeeder.enabled=true; _pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {_pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State ReachTunel = new State("ReachTunel",
            () => { _pathFeeder.target=_actuallTunelEntry; _pathFeeder.enabled=true; _pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {_pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );
        
        State ReturnHome = new State("ReturnHome",
            () => { _pathFeeder.target=_blackbord._home; _pathFeeder.enabled=true;_pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { _pathFeeder.enabled=false;_pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State GoThrowTunel = new State("GoThrowTunel",
            () => {_actuallTunelEntry=_blackbord.ExitTunel(); _pathFeeder.target=_actuallTunelEntry;_pathFeeder.enabled=true;_pathFeeder.path=true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { _pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

        State GoThrowTunelBackHome = new State("GoThrowTunelBackHome",
            () => { _actuallTunelEntry=_blackbord._tunelEntries[0]; _pathFeeder.target=_actuallTunelEntry;_pathFeeder.enabled=true;_pathFeeder.path=true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { _pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

       State GoThrowRecalculatedTunel = new State("GoThrowRecalculatedTunel",
            () => {_actuallTunelEntry=_blackbord._tunelEntries[2]; _pathFeeder.target=_actuallTunelEntry;_pathFeeder.enabled=true;_pathFeeder.path=true;  }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { _pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

         State GoToCheeseFromTunel = new State("GoToCheeseFromTunel",
            () => {_pathFeeder.target=_blackbord._cheese; _pathFeeder.enabled=true; _pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { _pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );

         State ReachRecalculatedTunel = new State("ReachRecalculatedTunel",
            () => { _pathFeeder.target=_blackbord._tunelEntries[2]; _pathFeeder.enabled=true; _pathFeeder.path=true;}, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => {_pathFeeder.enabled=false; _pathFeeder.path=false;}  // write on exit logic inisde {}  
        );




        Transition SecondsWaited = new Transition("SecondsWaited",
            () => { return _blackbord._timeToGo <= _currentTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
 );

        Transition InObjective = new Transition("InObjective",
                   () => { return _blackbord._toReachObjective>=SensingUtils.DistanceToTarget(gameObject,_blackbord._cheese);}, // write the condition checkeing code in {}
                   () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition InHome = new Transition("InHome",
                   () => { return _blackbord._toReachHome>=SensingUtils.DistanceToTarget(gameObject,_blackbord._home);}, // write the condition checkeing code in {}
                   () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

         Transition CantReachByWalking = new Transition("CantReachByWalking",
                   () => { return _pathFeeder.currentPath.CompleteState==PathCompleteState.Error;}, // write the condition checkeing code in {}
                   () => {_actuallTunelEntry=_blackbord._tunelEntries[0]; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition CantReturnHomeByWalking = new Transition("CantReturnHomeByWalking",
                   () => { return _pathFeeder.currentPath.CompleteState==PathCompleteState.Error;}, // write the condition checkeing code in {}
                   () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

          Transition CantReachWithYhisTunel = new Transition("CantReachWithYhisTunel",
                   () => {  return _pathFeeder.currentPath.CompleteState==PathCompleteState.Error;}, // write the condition checkeing code in {}
                   () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
           Transition CanReachWithYhisTunel = new Transition("CanReachWithYhisTunel",
                   () => { return _pathFeeder.currentPath.CompleteState!=PathCompleteState.Error;}, // write the condition checkeing code in {}
                   () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
         

        Transition InTunelEntry = new Transition("InTunelEntry",
                   () => { return _blackbord._toReachTunel>=SensingUtils.DistanceToTarget(gameObject,_actuallTunelEntry)&&_actuallTunelEntry==_blackbord._tunelEntries[0];}, // write the condition checkeing code in {}
                   () => {_pathFeeder.seeker.graphMask.value=GraphMask.FromGraphName("Tunel"); }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

         Transition InForeignTunelEntry = new Transition("InTunelEntry",
                   () => { return _blackbord._toReachTunel>=SensingUtils.DistanceToTarget(gameObject,_actuallTunelEntry);}, // write the condition checkeing code in {}
                   () => {_pathFeeder.seeker.graphMask.value=GraphMask.FromGraphName("Tunel"); }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

         Transition InRecalculatedTunelEntry = new Transition(" InRecalculatedTunelEntry",
                   () => { return _blackbord._toReachTunel>=SensingUtils.DistanceToTarget(gameObject,_actuallTunelEntry);}, // write the condition checkeing code in {}
                   () => {_pathFeeder.seeker.graphMask.value=GraphMask.FromGraphName("Tunel"); }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
     

        Transition ExitTunel = new Transition("ExitTunel",
                   () => { return _blackbord._toReachTunel>=SensingUtils.DistanceToTarget(gameObject,_actuallTunelEntry);}, // write the condition checkeing code in {}
                   () => { _pathFeeder.seeker.graphMask.value=GraphMask.FromGraphName("Grid Graph");}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        
        Transition InWater = new Transition("InWater",
                   () => {node=AstarPath.active.GetNearest(transform.position).node; 
                          return node.Penalty>0; }, // write the condition checkeing code in {}
                   () => {_sprite.sprite=_blackbord._sailorSprite; _steeringContext.maxSpeed=_sailingSpeed;}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition OutOfWater = new Transition("OutOfWater",
                   () => {node=AstarPath.active.GetNearest(transform.position).node; 
                          return node.Penalty==0; }, // write the condition checkeing code in {}
                   () => { _sprite.sprite=_blackbord._ratSprite;_steeringContext.maxSpeed=_walkingSpeed;}  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );
     
     



        AddStates(WaitForSeconds,GoToCheese,ReturnHome,ReachTunel,GoThrowTunel,GoThrowTunelBackHome,GoToCheeseFromTunel,GoThrowRecalculatedTunel,ReachRecalculatedTunel);
       
        AddTransition(WaitForSeconds,SecondsWaited,GoToCheese);
        AddTransition(GoToCheese,InObjective,ReturnHome);
        AddTransition(ReturnHome,InHome,WaitForSeconds);

        // AddTransition(GoToCheese,InWater,GoToCheese);
        // AddTransition(GoToCheese,OutOfWater,GoToCheese);

        // AddTransition(ReturnHome,InWater,ReturnHome);
        // AddTransition(ReturnHome,OutOfWater,ReturnHome);



        AddTransition(GoToCheese,CantReachByWalking,ReachTunel);
         AddTransition(ReachTunel,InTunelEntry,GoThrowTunel);
        AddTransition(GoThrowTunel,ExitTunel,GoToCheeseFromTunel);
        
        
        AddTransition(GoToCheeseFromTunel,CantReachWithYhisTunel,ReachRecalculatedTunel);
        
        AddTransition(ReachRecalculatedTunel,InForeignTunelEntry,GoThrowRecalculatedTunel);
        AddTransition(GoThrowRecalculatedTunel,ExitTunel,GoToCheese);

        AddTransition(ReturnHome,CantReturnHomeByWalking,GoToCheeseFromTunel);
        AddTransition(ReachTunel,InForeignTunelEntry,GoThrowTunelBackHome);
        AddTransition(GoThrowTunelBackHome,ExitTunel,ReturnHome);


        initialState=WaitForSeconds;
    }

        
}
