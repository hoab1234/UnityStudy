using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StateMachine : MonoBehaviour
{
    public IState CurrentState { get; private set; }

    public MoveState moveState;
    public RotateState rotateState;
    public ControllerState controllerState;

    public event Action<IState> stateChanged;

    public void Initalize(IState state)
    {
        CurrentState = state;
        state.Enter();

        stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();


        // notify other objects that state has changed
        stateChanged?.Invoke(nextState);
    }

    // allow the StateMachine to update this state
    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
