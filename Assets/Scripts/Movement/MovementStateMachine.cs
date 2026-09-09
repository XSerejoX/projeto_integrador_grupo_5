using UnityEngine;

public class MoveStateMachine : MonoBehaviour
{
   public IState currentState;
    

   public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    } 

    public void Update()
    {
        currentState?.Update();
    }

}
