using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AIBehaviour : TickBehaviour
{
    protected StateMachine StateMachine;
    public IState CurrentState => StateMachine.CurrentState;
    public abstract void Init();
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void AddTransition<T>(T from, T to) where T : IState
    {
        StateMachine.AddTransition(new Transition(from, to));
    }
    public void AddTransitionsTo<T>(T target, params T[] froms) where T : IState
    {
        foreach (var from in froms)
        {
            StateMachine.AddTransition(new Transition(from, target));
        }
    }

    /// <summary>
    /// Set current state
    /// </summary>
    /// <param name="state"></param>
    public void ToState<T>() where T : IState
    {
        StateMachine.ToState<T>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        StateMachine.OnUpdate(Time.deltaTime);
    }
  
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        StateMachine.OnFixedUpdate();
    }
    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        StateMachine.OnLateUpdate(Time.deltaTime);
    }
}