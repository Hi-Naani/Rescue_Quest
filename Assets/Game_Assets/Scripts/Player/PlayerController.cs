using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>  
{   
    private PlayerInputControl playerInputControl;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCD = 0.25f;
    [SerializeField] private bool isDashing = false;

    [SerializeField] private TrailRenderer trailRenderer;

    private KnockBack knockBack;
    private SpriteRenderer spriteRenderer;
    private Animator playerAnimator;
    private Vector2 movement;
    private int animatorHash_1;
    private int animatorHash_2;
    private float currentSpeed;
    private bool facingLeft = false;

    public bool FacingLeft
    {
        get { return facingLeft; }
        private set { facingLeft = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        playerInputControl = new PlayerInputControl();
        playerAnimator = GetComponent<Animator>();
 
    }

    private void Start()
    {
        animatorHash_1 = Animator.StringToHash("MoveX");
        animatorHash_2 = Animator.StringToHash("MoveY");
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInputControl.Combat.Dash.performed += _ => Dash();
        currentSpeed = moveSpeed;

        knockBack = GetComponent<KnockBack>();

        //ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable()
    {
        playerInputControl.Enable();
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
    }

    private void Update()
    {
        PlayerInput();
        AdjustingPlayerDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    { 
        movement = playerInputControl.Movement.Move.ReadValue<Vector2>();
        
        SetPlayerAnimation();
    }

    private void Move()
    {
        if(knockBack.GettingKnocked || PlayerHealth.Instance.IsDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            trailRenderer.emitting = true;
            StartCoroutine(StoppingDashRoutine());
        }
        
    }

    private IEnumerator StoppingDashRoutine()
    {
        
        yield return new WaitForSeconds(dashTime);
        moveSpeed = currentSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void SetPlayerAnimation()
    {    
        playerAnimator.SetFloat(animatorHash_1, movement.x);
        playerAnimator.SetFloat(animatorHash_2, movement.y);
    }

    private void AdjustingPlayerDirection()
    {
        Vector3 _mousePosition = Input.mousePosition;
        Vector3 _playerSceenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        if (_mousePosition.x < _playerSceenPosition.x)
        {
            spriteRenderer.flipX = true;
            FacingLeft = true;
        }
        else if (_mousePosition.x > _playerSceenPosition.x)
        {
            spriteRenderer.flipX = false;
            FacingLeft = false;
        }
        else if(Mathf.Approximately(_mousePosition.x,  _playerSceenPosition.x))
        {
            spriteRenderer.flipX = true;
        }

    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint()
    {
        return slashAnimSpawnPoint;
    }

}
