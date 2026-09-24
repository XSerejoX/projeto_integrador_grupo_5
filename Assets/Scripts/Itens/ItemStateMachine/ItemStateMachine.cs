using UnityEngine;

public class ItemStateMachine : MonoBehaviour
{
    public IItemState currentState;
    public void ChangeItemState(IItemState newItemState)
    {
        currentState?.Exit();
        currentState = newItemState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
    
    

    

}
