using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public class StateMachine : IStateMachine
    {
        public AIBehaviour AIBehaviour { get; }
        public IState CurrentState
        {
            get
            {
                return _currentState;
            }
        }
        public IState DefaultState
        {
            get
            {
                return _defaultState;
            }
            set
            {
                _defaultState = value;
            }
        }
        private List<IState> _states { get; set; }
        public IState PrevState { get; set; }

        private IState _currentState;
        private IState _defaultState;
        private List<ITransition> _transitions;
        public StateMachine(AIBehaviour aIBehaviour)
        {
            _transitions = new List<ITransition>();
            _states = new List<IState>();
            AIBehaviour = aIBehaviour;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }

            else
                _currentState.OnUpdate(deltaTime);
        }
        public void OnLateUpdate(float deltaTime)
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }
            else
                _currentState.OnLateUpdate(deltaTime);
        }
        public void OnFixedUpdate()
        {
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }
            else
            {
                _currentState.OnFixedUpdate();
            }
        }
        public void ToState<T>() where T : IState
        {
            foreach (var state in _states)
            {
                if (state is T)
                {
                    ToState(state);
                    return;
                }
            }
        }
        protected void ToState(IState state)
        {
            if (CurrentState == null)
            {
                PrevState = CurrentState;
                _currentState = state;
                _currentState.OnEnter(state);
                return;
            }
            foreach (var item in _transitions)
            {
                if (item.From == _currentState && item.To == state)
                {
                    state.OnEnter(_currentState);
                    CurrentState.OnExit(state);
                    _currentState = state;
                    PrevState = item.From;
                    break;
                }
            }
        }

        public void AddTransition(ITransition transition)
        {
            if (!_transitions.Contains(transition))
            {
                if (DefaultState == null)
                {
                    DefaultState = transition.From;
                }
                _transitions.Add(transition);
                if (!_states.Contains(transition.From)) { _states.Add(transition.From); };
                if (!_states.Contains(transition.To)) { _states.Add(transition.To); };
            }
        }

        public void ToPrevState()
        {
            ToState(PrevState);
        }
    }
}