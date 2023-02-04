using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using State = StateMachine<Actor>.State;

public class Actor : MonoBehaviour
{
	private StateMachine<Actor> stateMachine;

	[SerializeField] private int hp;
	[SerializeField] private int attackDamage;
	[SerializeField] private int attackSpeed;
	[SerializeField] private int idleTime;

	private enum Action : int
	{
		// 攻撃
		Attack,
		// 移動
		Move,
		// 回避
		Avoid,
		// ひるみ
		Stan,
	}
	private enum Emotion : int
	{
		// 普通
		Normal,
		// 怒り
		Angry,
		// 好き
		Love,
	}

	private void Start()
	{
		stateMachine = new StateMachine<Actor>(this);

		// 
		//stateMachine.AddTransition<StateRotation, StateMoveForward>((int)Action.Move);
		stateMachine.AddAnyTransition<StateAttack>((int)Action.Attack);
		stateMachine.AddAnyTransition<StateMoveToPlayer>((int)Action.Move);
	

		// 
		// // 敵を見つけたら回転を終了して前進する
		// stateMachine.AddTransition<StateRotation, StateMoveForward>((int)Action.Attack);
		// // 敵を倒したら前進を終了。回転に戻る
		// stateMachine.AddTransition<StateMoveForward, StateRotation>((int)Action.Move);
		// // 一定の時間、前進しても敵が見つからなかった場合も回転に戻る
		// stateMachine.AddTransition<StateMoveForward, StateRotation>((int)Action.Avoid);

		// // 敵が全て死んだら終了
		// stateMachine.AddAnyTransition<StateEnd>((int)Action.Stan);

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

	// 攻撃
	private class StateAttack : State
	{
		// 経過時間
		private float elapsedTime;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:攻撃だよ");
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
				StateMachine.Dispatch((int)Action.Attack);
				elapsedTime = 0f;
				direction = direction * (-1);
			}	
			//Owner.transform.Rotate(0, 0, Speed * direction * Time.deltaTime);
			Owner.transform.position += Owner.transform.right * Speed * direction;
		}
	}
	// プレイヤーを追いかける
	private class StateMoveToPlayer : State
	{
		// 回転の速度
		private const float Speed = 10f;
		// 回転の方向
		private float direction = -1;
		// 経過時間
		private float elapsedTime;

		private GameObject target;

		protected override void OnEnter(State prevState)
		{
			Debug.Log("敵:プレイヤー追いかけるよ");
			target = GameObject.Find("Player");
		}

		protected override void OnUpdate()
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= 3)
			{
				StateMachine.Dispatch((int)Action.Attack);
				elapsedTime = 0f;
			}	
        	Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, target.transform.position, Speed * Time.deltaTime);
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
