using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State<StateMachine> state;
        private string currentState = "";

        public State<StateMachine> State { get => state; }

        public virtual void ChangeState(State<StateMachine> newState)
        {
            if (newState.CanEnter())
            {
                state.OnExit();
                state = newState;
                state.OnEnter();
                currentState = state.Name;
            }
            else
                Debug.LogError(string.Format("Invalid state transition {0} => {1}", state.Name, newState.Name), this);
        }

        protected virtual void Awake()
        {
            InitializeStates();
        }

        protected virtual void Update()
        {
            state.OnUpdate();
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            state.OnTriggerEnter(other);
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            state.OnTriggerExit(other);
        }
        protected abstract void InitializeStates();
    }

}
