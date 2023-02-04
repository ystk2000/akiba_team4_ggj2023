using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController inputController;

    public float speedX = 5f;
    public float speedJump = 16f;

    private bool jumping = false;
	
	private float velocityY = 0f;
	Collider[] collisionArray = new Collider[10];

	[SerializeField] SpriteRenderer spriteRenderer;
	//int layerMaskPlatform = 1 << 7;


	private void Update()
    {
		float dt = Time.deltaTime;

		// 移動入力
		float moveX = inputController.HorizontalMovement;

		if (inputController.JumpPressed() && jumping == false)
		{
			// ジャンプ実行
			jumping = true;
			velocityY = speedJump;
		}

		Vector2 movePos = transform.position;

		// X軸の移動
		movePos.x += (moveX * speedX * dt);


		// Y軸の移動 (ジャンプ)
		float currentY = movePos.y;
		velocityY += (-20f * dt);
		movePos.y += (velocityY * dt);
		bool falling = currentY > movePos.y;


		// Platformとの当たり判定
		int hitCount = Physics.OverlapBoxNonAlloc(movePos + Vector2.up * 0.2f, new Vector2(0.4f, 0.4f), collisionArray, Quaternion.identity, LayerMask.GetMask("Platform"));
		bool hitPlatform = Physics.Raycast(movePos + Vector2.up, Vector2.down, out RaycastHit raycastHit, 2f, LayerMask.GetMask("Platform"));

		// 着地判定
		if (hitCount > 0  && falling && hitPlatform)
		{
			// 着地した場合
			movePos.y = raycastHit.point.y;
			velocityY = 0f;
			jumping = false;
		}

		// 反映させる
		transform.position = movePos;

		// 体の向きを変更
		if (moveX != 0f)
		{
			spriteRenderer.flipX = (moveX < 0f);
		}
	}
}
