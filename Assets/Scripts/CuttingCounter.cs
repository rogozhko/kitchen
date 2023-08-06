using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private CuttingRecipeSo[] cuttingRecipeSOArray;
    
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is nothing here
            if (player.HasKitchenObject()) {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) {
                    // Player carrying something than can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
            else {
                // Player carrying anything
            }
        }
        else {
            // There is a KitchenObject here
            if (player.HasKitchenObject()) {
                // Player is carrying something
            }
            else {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo())) {
            // There is a KitchenObject here AND it can be cut

            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
            GetKitchenObject().DestroySelf();
            
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return true;
            }
        }

        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (var cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) return cuttingRecipeSO.output;
        }

        return null;
    }
}
