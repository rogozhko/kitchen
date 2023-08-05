using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;


    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSo() {
        return kitchenObjectSo;
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        // Clear object from old IKitchenObjectParent
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        
        // Set New IKitchenObjectParent
        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject");
        }
        
        kitchenObjectParent.SetKitchenObject(this);
        
        // Update visual
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }
}