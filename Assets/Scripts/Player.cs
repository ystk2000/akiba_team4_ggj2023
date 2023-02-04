using UnityEngine;


public class Player : MonoBehaviour
{
	float speedMove = 5f; // 移動スピード
	float speedJump = 16f; // ジャンプ力

	// ジャンプ用
	bool flgJumping = false;
	float accY = 0f;


	SpriteRenderer cmpSpRender;


	// Start is called before the first frame update
	void Start()
    {
		cmpSpRender = transform.Find("Body").GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
    {
		var dt = Time.deltaTime;

		// 移動入力
		var moveX = 0f;
		moveX = Input.GetAxis("Horizontal");

		if ( flgJumping == false && Input.GetButtonDown("Jump") )
		{
			// ジャンプ実行
			flgJumping = true;
			accY = speedJump;
		}

		var posTmp = transform.position;

		// X軸の移動
		posTmp.x += ( moveX * speedMove * dt );

		// Y軸の移動 (ジャンプ)
		var bupY = posTmp.y;
		accY += ( -20f * dt );
		posTmp.y += ( accY * dt );

		// Platformとの当たり判定
		var hits = Physics.OverlapBox( posTmp + Vector3.up * 0.2f, new Vector3( 0.4f, 0.4f, 0.4f ), Quaternion.identity, LayerMask.GetMask("Platform") );
		var flgHitPlatform = Physics.Raycast(posTmp + Vector3.up, Vector3.down, out RaycastHit raycastHit, 2f, LayerMask.GetMask("Platform"));

		// 着地判定
		if ( ( bupY > posTmp.y ) && ( hits.Length > 0 ) && flgHitPlatform )
		{
			// 着地した場合
			posTmp.y = raycastHit.point.y;
			accY = 0f;
			flgJumping = false;
		}

		// 反映させる
		transform.position = posTmp;

		// 体の向きを変更
		if( moveX != 0f )
		{
			cmpSpRender.flipX = ( moveX < 0f );
		}
	}
}
