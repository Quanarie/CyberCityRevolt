using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State CurrentState { get; private set; } = State.InGame;
    private State previousState = State.InGame;

    public void Pause()
    {
        Singleton.Instance.PlayerData.Input.DeactivateInput();
        previousState = CurrentState;
        CurrentState = State.Paused;
    }
    
    public void EnterDialogue()
    {
        Singleton.Instance.PlayerData.Input.DeactivateInput();
        previousState = CurrentState;
        CurrentState = State.InDialogue;
    }
    
    public void Resume()
    {
        if (previousState == State.InGame)
        {
            Singleton.Instance.PlayerData.Input.ActivateInput();
        }

        CurrentState = previousState;
        previousState = State.Paused;
    }
    
    public void LeaveDialogue()
    {
        Singleton.Instance.PlayerData.Input.ActivateInput();
        CurrentState = State.InGame;
        previousState = State.InDialogue;
    }
}

public enum State
{
    InGame,
    InDialogue,
    Paused
}