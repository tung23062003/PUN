using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public class State : IState
    {
        public Action<IState> CallbackOnEnter;
        public Action<IState> CallbackOnExit;
        public Action<float> CallbackOnUpdate;
        public Action<float> CallbackOnLateUpdate;
        public Action CallbackOnFixedUpdate;
        public IStateMachine Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        public float Timer
        {
            get
            {
                return _timer;
            }
        }
        public List<ITransition> Transitions
        {
            get
            {
                return _transitions;
            }
        }
        public void AddTransition(ITransition t)
        {
            if (t != null && !_transitions.Contains(t))
            {
                _transitions.Add(t);
            }
        }
        public State(IStateMachine stateMachine)
        {
            _parent = stateMachine;
            _transitions = new List<ITransition>();
        }

        public virtual void OnEnter(IState prev)
        {
            _timer = 0f;
            if (CallbackOnEnter != null)
            {
                CallbackOnEnter(prev);
            }
        }
        public virtual void OnExit(IState next)
        {
            _timer = 0f;
            if (CallbackOnExit != null)
            {
                CallbackOnExit(next);
            }
        }
        public virtual void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if (CallbackOnUpdate != null)
            {
                CallbackOnUpdate(deltaTime);
            }
        }

        public virtual void OnLateUpdate(float deltaTime)
        {
            if (CallbackOnLateUpdate != null)
            {
                CallbackOnLateUpdate(deltaTime);
            }
        }
        public virtual void OnFixedUpdate()
        {
            if (CallbackOnFixedUpdate != null)
            {
                CallbackOnFixedUpdate();
            }
        }

        private float _timer;
        private IStateMachine _parent;
        private List<ITransition> _transitions;
    }
}