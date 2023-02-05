using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using State = StateMachine<BigBoss>.State;

public class BigBoss : MonoBehaviour
{
	private StateMachine<BigBoss> stateMachine;

	public static int hp = 20;
	public static int attackDamage = 20;
	public static int attackSpeed = 20;
	public static float angrySpeedRate = 1.5f;
	public static float lovelySpeedRate = 0.5f;
	public static int idleTime = 10;
	public static int emotionNum = 0; // 0:普通, 1:怒り, 2:好き
	
	private enum Action : int
	{
		// 弾丸攻撃
		AttackBullet,
		// 殴る右攻撃
		AttackHandRight,
		// 殴る左攻撃
		AttackHandLeft,
		// 殴る上から攻撃
		AttackHandUp,
		// 移動
		Move,
		// 回避
		Avoid,
		// ひるみ
		Stan,
	}

	private void Start()
	{
		stateMachine = new StateMachine<BigBoss>(this);

		//stateMachine.AddTransition<StateRotation, StateMoveForward>((int)Action.Move);
		stateMachine.AddAnyTransition<StateAttackBullet>((int)Action.AttackBullet);
		stateMachine.AddAnyTransition<StateAttackHandRight>((int)Action.AttackHandRight);
		stateMachine.AddAnyTransition<StateAttackHandLeft>((int)Action.AttackHandLeft);
		stateMachine.AddAnyTransition<StateAttackHandUp>((int)Action.AttackHandUp);
		stateMachine.AddAnyTransition<StateMoveToPlayer>((int)Action.Move);

		stateMachine.Start<StateMoveToPlayer>();
	}

	private void Update()
	{
		// if (Enemy.Count == 0)
		// {
		// 	stateMachine.Dispatch((int)Action.Move);
		// 	return;
		// }

		stateMachine.Update();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.TryGetComponent(out Enemy enemy))
		{
			Destroy(enemy.gameObject);
			stateMachine.Dispatch((int)Action.Move);
		}
	}
	// プレイヤーを追いかける
	private class StateMoveToPlayer : State
	{
		// 速度
		private const float moveSpeed = 20f;
		// 経過時間
		private float elapsedTime;
		// 次の行動への時間感覚
		private int nextIntervalTime;

		private GameObject target;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:プレイヤー追いかけるよ");
			target = GameObject.Find("Player");
			nextIntervalTime = Random.Range(1, 4);
		}

		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= nextIntervalTime)
			{
				elapsedTime = 0f;
				if(emotionNum == 0){
					DecisionNormal();
				}
			}	
        	Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, new Vector3(target.transform.position.x, target.transform.position.y, Owner.transform.position.z), attackSpeed * Time.deltaTime);
		}
		private void DecisionNormal()
		{
			int num = Random.Range(0, 4);
			if(num >= 5){
				StateMachine.Dispatch((int)Action.AttackBullet);
			}
			else if(num >= 2){
				StateMachine.Dispatch((int)Action.AttackHandLeft);
			}
			else if(num >= 0){
				StateMachine.Dispatch((int)Action.AttackHandRight);
			}
			else{
				StateMachine.Dispatch((int)Action.AttackHandUp);
			}
		}
	}
	// 弾幕攻撃
	private class StateAttackBullet : State
	{
		// 経過時間
		private float elapsedTime;

		private GameObject bullet;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:弾幕攻撃だよ");

			//弾幕の攻撃をする
        	bullet = (GameObject)Resources.Load ("EnemyBall");
			Vector3 placePosition = Owner.transform.position;
			Vector3 offsetGun = new Vector3 (0,0,2);
			Quaternion q1 = Owner.transform.rotation;
			Quaternion q2 = Quaternion.AngleAxis(90, new Vector3(1,0,0));
			Quaternion q = q1 * q2;
			placePosition = q1 * offsetGun + placePosition;
			Instantiate (bullet, placePosition, q);
			
			elapsedTime = 0;
		}
		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= 2)
			{
				elapsedTime = 0;
				StateMachine.Dispatch((int)Action.Move);
			}
		}
	}
	/// <summary>
	/// 右殴り攻撃
	/// </summary>
	private class StateAttackHandRight : State
	{
		// 経過時間
		private float elapsedTime;

		private GameObject hand;
		private GameObject target;
		private GameObject attackArea;
		private Vector3 placePosition;
		private bool attackSet;
		private bool attackFinish;
		private bool attackAreaSet;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:右殴り攻撃だよ");
			attackAreaSet = false;
			attackSet = false;
			attackFinish = false;
			elapsedTime = 0;

			target = GameObject.Find("AttackPoint--1");
			placePosition = target.transform.position;
			hand = (GameObject)Resources.Load ("RightHand");
			attackArea = (GameObject)Resources.Load ("AttackArea-HandSide");
			Owner.transform.Rotate(0, -45, 0);
		}
		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(attackFinish && elapsedTime >= 0.5){

				Owner.transform.Rotate(0, -65, 0);
				StateMachine.Dispatch((int)Action.Move);
				elapsedTime = 0;
			}
			else if(attackSet && elapsedTime >= 1){
				attackFinish = true;
				elapsedTime = 0;

				Owner.transform.Rotate(0, 110, 0);
				Instantiate(hand, placePosition, Quaternion.identity);
			}
			else if(attackAreaSet && elapsedTime >= 1){
				attackSet = true;
				elapsedTime = 0;
			}
			else{
				if(elapsedTime >= 1)
				{
					attackAreaSet = true;
					Instantiate(attackArea, placePosition, Quaternion.identity);
					elapsedTime = 0;
				}
			}	
		}
	}

	/// <summary>
	/// 左殴り攻撃
	/// </summary>
	private class StateAttackHandLeft : State
	{
		// 経過時間
		private float elapsedTime;

		private GameObject hand;
		private GameObject target;
		private GameObject attackArea;
		private Vector3 placePosition;
		private bool attackSet;
		private bool attackFinish;
		private bool attackAreaSet;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:左殴り攻撃だよ");
			attackAreaSet = false;
			attackSet = false;
			attackFinish = false;
			elapsedTime = 0;

			target = GameObject.Find("AttackPoint--2");
			placePosition = target.transform.position;
			hand = (GameObject)Resources.Load ("LeftHand");
			attackArea = (GameObject)Resources.Load ("AttackArea-HandSide");
			Owner.transform.Rotate(0, 45, 0);
		}
		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(attackFinish && elapsedTime >= 0.5){

				Owner.transform.Rotate(0, 65, 0);
				StateMachine.Dispatch((int)Action.Move);
				elapsedTime = 0;
			}
			else if(attackSet && elapsedTime >= 1){
				attackFinish = true;
				elapsedTime = 0;

				Owner.transform.Rotate(0, -110, 0);
				Instantiate(hand, placePosition, Quaternion.identity);
			}
			else if(attackAreaSet && elapsedTime >= 1){
				attackSet = true;
				elapsedTime = 0;
			}
			else{
				if(elapsedTime >= 1)
				{
					attackAreaSet = true;
					Instantiate(attackArea, placePosition, Quaternion.identity);
					elapsedTime = 0;
				}
			}	
		}
	}
	// 上殴り攻撃
	private class StateAttackHandUp : State
	{
		// 経過時間
		private float elapsedTime;

		private GameObject hand;
		private GameObject target;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:上殴り攻撃だよ");
			target = GameObject.Find("AttackPoint--3");
        	hand = (GameObject)Resources.Load ("UpHand");
			Vector3 placePosition = target.transform.position;
			Instantiate(hand, placePosition, Quaternion.identity);
			elapsedTime = 0;
		}
		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= 2)
			{
				elapsedTime = 0;
				StateMachine.Dispatch((int)Action.Move);
			}	
		}
	}

	// 
	private class StateChangeNormalToLove : State
	{
		// 経過時間
		private float elapsedTime;

		private GameObject hand;
		private GameObject target;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:Loveに変化したよ");
			target = GameObject.Find("Sprite-HeartEye");
			target.SetActive (true);
		}
		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= 2)
			{
				elapsedTime = 0;
				StateMachine.Dispatch((int)Action.Move);
			}	
		}
	}


	// 左右にステップ
	private class StateSideStep : State
	{
		// 回転の速度
		private const float Speed = 0.01f;
		// 回転の方向
		private float direction = -1;
		// 経過時間
		private float elapsedTime;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:移動だよ");
			// ランダムで右回転するか、左回転するかを決める
			direction = Random.Range(0, 2) == 0 ? 1 : -1;
		}

		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= 2)
			{
				StateMachine.Dispatch((int)Action.AttackBullet);
				elapsedTime = 0f;
				direction = direction * (-1);
			}	
			Owner.transform.position += Owner.transform.right * Speed * direction;
		}
	}

	// 前進する
	private class StateMoveForward : State
	{
		private const float Speed = 2f;
		private const float Timeout = 1f;
		private float timeoutAt;

		protected override void OnEnter(State prevState)
		{
			timeoutAt = Time.time + Timeout;
		}

		protected override void OnUpdate()
		{
			Owner.transform.position += Owner.transform.up * Speed * Time.deltaTime;

			if (timeoutAt <= Time.time)
			{
				StateMachine.Dispatch((int)Action.Move);
			}
		}
	}

	// 終了
	private class StateEnd : State
	{
		protected override void OnEnter(State prevState)
		{
			Owner.enabled = false;
		}
	}
}
