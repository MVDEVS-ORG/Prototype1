using System;

namespace prototype1.scripts.stateMachine
{
    public interface IStateMachine
    {
        void Initialize();
        void changeState(NPCState newState);
        bool stateChangePossible(NPCState newState);
        NPCState CurrentState { get; }
        NPCState PreviousState { get; }
        event Action<NPCState, NPCState> OnStateChange;
    }

    public enum NPCState
    {
        Idle,
        Attack,
        Move,
        Ability //not in use currently
    }

}
