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

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }
    }
    

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSo,
        IKitchenObjectParent kitchenObjectParent) {
        
        var kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
        
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
    
    
}