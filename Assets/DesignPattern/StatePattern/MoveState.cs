using UnityEngine;

public class MoveState : MonoBehaviour, IState
{
    public ContentLocater locater;
    bool focus;


    public MoveState(ContentLocater locater)
    {
        this.locater = locater;
    }

    public void Enter()
    {
        focus = true;
        Debug.Log($"MoveState Enter");
    }

    public void Update()
    {
        if (focus && locater.State is not ContentLocaterState.Moveable)
        {
            IState nextState = locater.State == ContentLocaterState.Rotateable ? locater.StateMachine.rotateState : locater.StateMachine.controllerState;
            locater.StateMachine.TransitionTo(nextState);
            Debug.Log($"Next State = {nextState}");
        }
    }

    public void Exit()
    {
        focus = false;
        Debug.Log($"MoveState Exit");
    }
}
