using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;


    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSo() {
        return kitchenObjectSo;
    }
    
    public void SetClearCounter(ClearCounter clearCounter) {
        // Clear object from old ClearCounter
        if (this.clearCounter != null) {
            this.clearCounter.ClearKitchenObject();
        }
        
        // Set New Counter
        this.clearCounter = clearCounter;

        if (clearCounter.HasKitchenObject()) {
            Debug.LogError("Counter already has a KitchenObject");
        }
        
        clearCounter.SetKitchenObject(this);
        
        // Update visual
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter() {
        return clearCounter;
    }
}