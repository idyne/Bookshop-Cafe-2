using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    [System.Serializable]
    public abstract class State<T> where T : StateMachine
    {
        protected string name;
        protected T stateMachine;

        public string Name { get => name; }

        public State(T stateMachine)
        {
            name = GetType().ToString();
            this.stateMachine = stateMachine;
        }

        public abstract bool CanEnter();
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void OnTriggerEnter(Collider other);
        public abstract void OnTriggerExit(Collider other);

    }

}
