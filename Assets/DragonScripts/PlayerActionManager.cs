using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController inputController;

    [SerializeField] private Animator animator;
		[SerializeField] private ParticleSystem pluckParticles;
		const float speedX = 5f;
    public const float speedJump = 16f;
    public const float pullDuration = 0.8f;

    private bool jumping = false;
	
	private float velocityY = 0f;
	private const float VELOCITY_MIN_Y = -20f;
	public float pullTimer = 0f;
	Collider[] collisionArray = new Collider[10];
	GameObject plantToPull = null;
	Vector3 origin;
	bool holdingItem = false;

	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] GameObject heldItem;
	[SerializeField] GameObject projectilePrefab;

	private bool OnGround => velocityY == 0;

    private void Start()
    {
		origin = transform.position;
		heldItem.SetActive(false);
	}

    private void FixedUpdate()
    {
		if (transform.position.y < -10)
        {
			transform.position = origin;
			velocityY = 0;
        }

		float dt = Time.deltaTime;

        //��؂𔲂�
        if (pullTimer > 0)
        {
            pullTimer -= dt;
            if (pullTimer <= 0)
            {
                plantToPull.SetActive(false);
                heldItem.SetActive(true);
                //transform.localScale = Vector3.one;
								holdingItem = true;
								animator.SetBool("IsCarrying", true);
								inputController.ClearInputs();
								pluckParticles.Play();

			}
            else
            {
                //transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.95f, 1.05f, 0.95f));
                return;
            }

        }

        Vector2 currentPosition = transform.position;

		if (inputController.PullPressed() && !holdingItem && OnGround)
        {
            int plantCount = Physics.OverlapBoxNonAlloc(currentPosition + Vector2.up * 0.2f, new Vector2(0.4f, 0.4f), collisionArray, Quaternion.identity, LayerMask.GetMask("Plant"));
            if (plantCount > 0)
			{
				plantToPull = collisionArray[0].gameObject;
				animator.SetTrigger("Pull");
                pullTimer = pullDuration;
                return;
            }
        }

        // �ړ�����
        float moveX = inputController.HorizontalMovement;
		animator.SetBool("IsRunning", moveX != 0);
		if (inputController.JumpPressed() && jumping == false)
		{
			// �W�����v���s
			jumping = true;
			velocityY = speedJump;
		}

		Vector2 movePos = transform.position;

		// X���̈ړ�
		movePos.x += (moveX * speedX * dt);


		// Y���̈ړ� (�W�����v)
		float currentY = movePos.y;
		velocityY = Mathf.Max(VELOCITY_MIN_Y, velocityY + (-20f * dt));
		movePos.y += (velocityY * dt);
		bool falling = currentY > movePos.y;

		// Platform�Ƃ̓����蔻��
		int hitCount = Physics.OverlapBoxNonAlloc(movePos + Vector2.up * 0.2f, new Vector2(0.4f, 0.4f), collisionArray, Quaternion.identity, LayerMask.GetMask("Platform"));
		bool hitPlatform = Physics.Raycast(movePos + Vector2.up, Vector2.down, out RaycastHit raycastHit, 2f, LayerMask.GetMask("Platform"));

		// ���n����
		if (hitCount > 0  && falling && hitPlatform)
		{
			// ���n�����ꍇ
			movePos.y = raycastHit.point.y;
			velocityY = 0f;
			jumping = false;
		}

		transform.position = movePos;

		if (moveX != 0f)
		{
			spriteRenderer.flipX = (moveX > 0f);
		}

		if (inputController.ThrowPressed() && holdingItem)
		{
			holdingItem = false;
			animator.SetBool("IsCarrying", false);
			animator.SetTrigger("Throw");
			GameObject projectile = Instantiate(projectilePrefab);
			projectile.transform.position = heldItem.transform.position;
			projectile.GetComponent<Projectile>().Init();
			heldItem.SetActive(false);
		}
	}
}
