using UnityEngine;
using UnityEngine.UI;


public enum ContentLocaterState
{
    Moveable,
    Rotateable,
    Controllerable
}

public class ContentLocater : MonoBehaviour
{
    public StateMachine StateMachine;

    public Toggle rotateToggle;
    public Toggle controllerToggle;
    public Toggle MoveToggle;

    public ContentLocaterState State;

    public void Awake()
    {
        rotateToggle.onValueChanged.AddListener(isOn =>
        {
            if(isOn)
            {
                State = ContentLocaterState.Rotateable;
            }
        });

        controllerToggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                State = ContentLocaterState.Controllerable;
            }
        });

        MoveToggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                State = ContentLocaterState.Moveable;
            }
        });

        StateMachine.Initalize(StateMachine.moveState);
    }
}
