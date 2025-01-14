using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjParent
{
    public static Player Instance { get; private set; } // Singleton instance for the Player

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter; // The currently selected counter
    }

    [SerializeField] private float playerSpeed = 7f; // Player's movement speed
    [SerializeField] private float rotationSmoothness = 5f; // Smoothness of rotation
    [SerializeField] private GameInput gameInput; // Reference to the input handler
    [SerializeField] private LayerMask layerMask; // LayerMask for raycasting
    [SerializeField] private float id = 1.5f; // Interaction distance
    [SerializeField] private Transform kitchenObjHoldPoint; // Point where kitchen objects are held

    private bool isWalking = false; // Tracks if the player is walking
    private Vector3 lastInteractDir; // Last direction of interaction
    private BaseCounter selectedCounter; // Currently selected counter
    private KitchenObject kitchenObject; // Reference to the held kitchen object

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAction;
    }

    private void Update()
    {
        HandleMovement();    // Handles player movement
        HandleInteractions(); // Handles interaction logic
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAltAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlt(this);
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVec = gameInput.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVec.x, 0f, inputVec.y);

        if (moveDir != Vector3.zero) lastInteractDir = moveDir; // Update interaction direction

        float interactDist = id;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDist, layerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter) SetSelectedCounter(baseCounter); // Update selected counter
            }
            else SetSelectedCounter(null);
        }
        else SetSelectedCounter(null);
    }

    private void HandleMovement()
    {
        Vector2 inputVec = gameInput.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVec.x, 0f, inputVec.y);
        float playerRadius = 0.7f, playerHeight = 2f, moveDist = playerSpeed * Time.deltaTime;

        // Check if the player can move in the intended direction
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDist);

        if (!canMove)
        {
            // Attempt movement only along the X axis
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDist);

            // If X-axis movement is not possible, try the Z-axis
            if (!canMove)
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDist);

                if (canMove) moveDir = moveDirZ; // Update move direction
            }
            else moveDir = moveDirX; // Update move direction
        }

        if (canMove) transform.position += moveDir * moveDist; // Update position if movement is valid

        isWalking = moveDir != Vector3.zero; // Update walking state

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up); // Target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness); // Smooth rotation
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
    }

    public bool IsWalking() => isWalking; // Returns if the player is walking

    public Transform GetKitchenObjectFollowTransform() => kitchenObjHoldPoint; // Returns the transform for holding kitchen objects

    public void SetKitchenObj(KitchenObject kitchenObject) => this.kitchenObject = kitchenObject; // Sets the held kitchen object

    public KitchenObject GetKitchenObject() => kitchenObject; // Gets the held kitchen object

    public void ClearKitchenObj() => kitchenObject = null; // Clears the held kitchen object

    public bool HasKitchenObj() => kitchenObject != null; // Checks if a kitchen object is being held
}
