using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public interface IState
    {
        IStateMachine Parent { get; set; }
        float Timer { get; }
        void OnEnter(IState prev);
        void OnExit(IState next);
        void OnUpdate(float deltaTime);
        void OnLateUpdate(float deltaTime);
        void OnFixedUpdate();
    }
}