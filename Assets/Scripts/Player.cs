using System;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent {
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    #region Variables

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    #endregion

    private void Awake() {
        if (Instance != null) Debug.LogError("There is more than one Player instance");
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteraction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractionAlternateAction;
    }

    private void GameInput_OnInteractionAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteraction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) lastInteractDir = moveDir;

        var interactDistance = 2f;
        if (Physics.Raycast(
                transform.position,
                lastInteractDir, out var raycastHit,
                interactDistance,
                countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // Has ClearCounter
                if (baseCounter != selectedCounter) SetSelectedCounter(baseCounter);
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement() {
        var inputVector = gameInput.GetMovementVectorNormalized();

        var moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        var moveDistance = moveSpeed * Time.deltaTime;
        var playerRadius = .7f;
        var playerHeight = 2f;
        var canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // Cannot move towards moveDir

            // Attempt only X movement
            var moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirX, moveDistance);
            if (canMove) {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else {
                // Cannot move only on the X

                // Attempt only Z movement
                var moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDirZ, moveDistance);
                if (canMove)
                    // Can move only on the Z
                    moveDir = moveDirZ;
                // Cannot move any direction
            }
        }

        if (canMove) transform.position += moveDir * moveDistance;

        isWalking = moveDir != Vector3.zero;

        var rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(
            transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}