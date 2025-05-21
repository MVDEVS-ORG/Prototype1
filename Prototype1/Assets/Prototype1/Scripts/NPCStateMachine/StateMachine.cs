using UnityEngine;
using prototype1.scripts.stateMachine;
using System;
using System.Collections.Generic;

namespace prototype1.scripts.stateMachine
{
    public class StateMachine : IStateMachine
    {
        private NPCState _currentState = NPCState.Idle;
        private NPCState _previousState = NPCState.Idle;
        NPCState IStateMachine.CurrentState => _currentState;

        NPCState IStateMachine.PreviousState => _previousState;

        public event Action<NPCState, NPCState> OnStateChange;

        private Dictionary<NPCState, List<NPCState>> _possibleTransitions = new();

        void IStateMachine.changeState(NPCState newState)
        {
            if ((this as IStateMachine).stateChangePossible(newState))
            {
                if (OnStateChange != null)
                {
                    OnStateChange.Invoke(_currentState, newState);
                }
                _currentState = newState;
            }
            else
            {
                Debug.LogWarning($"{_currentState} {newState}");
                Debug.LogError("Transistion Not possible");
            }
        }

        bool IStateMachine.stateChangePossible(NPCState newState)
        {
            return _possibleTransitions[_currentState].Contains(newState);
        }

        public void Initialize()
        {
            _possibleTransitions[NPCState.Idle] = new List<NPCState>() { NPCState.Attack, NPCState.Move, NPCState.Idle };
            _possibleTransitions[NPCState.Move] = new List<NPCState>() { NPCState.Attack, NPCState.Idle };
            _possibleTransitions[NPCState.Attack] = new List<NPCState>() { NPCState.Move, NPCState.Idle };
        }
    }
}
