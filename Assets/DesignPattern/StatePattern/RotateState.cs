using UnityEngine;
using UnityEngine.UI;

public class RotateState : MonoBehaviour, IState
{
    public ContentLocater locater;
    bool focus;

    public void Enter()
    {
        focus = true;
        Debug.Log("Rotate State Enter");
    }

    public void Update()
    {
        if (focus && locater.State is not ContentLocaterState.Rotateable)
        {
            IState nextState = locater.State == ContentLocaterState.Moveable ? locater.StateMachine.moveState : locater.StateMachine.controllerState;
            locater.StateMachine.TransitionTo(nextState);
            Debug.Log($"Next State = {nextState}");
        }
    }

    public void Exit()
    {
        focus = false;
        Debug.Log($"Rotate State Exit");
    }
}
