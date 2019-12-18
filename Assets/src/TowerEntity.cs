using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEntity : BaseEntity 
{
	[SerializeField]
	private int _attackRateMS;
	[SerializeField]
	private LineRenderer _laser;

	public int AttackRateMS { get { return _attackRateMS; } }

	private double _lastAttacked = -1;

	protected override void Start()
	{
		base.Start();
		entityManager.towerHealth.text = "Tower Health: " + _health;
	}

	private void Update()
	{
		TryAttack();
	}

	private void TryAttack()
	{
		if( entityManager == null )
		{
			return;
		}

		double now = BootStrap.EpochNow();
		if( _lastAttacked == -1 || now - _lastAttacked >= _attackRateMS )
		{
			_lastAttacked = now;
			Attack();
		}
	}

	private void Attack()
	{
		if( entityManager == null )
		{
			return;
		}
		
		Transform target = entityManager.DamageClosest( this, EntityManager.ATTACK_TEAM_TAG, _power );

		if( target != null )
		{
			StartCoroutine( PlayAttackAnimation( target.position ) );
		}
	}

	private IEnumerator PlayAttackAnimation( Vector3 endPos )
	{
		_laser.gameObject.SetActive( true );
		_laser.SetPosition( 0, this.transform.position );
		_laser.SetPosition( 1, endPos );
		yield return new WaitForSeconds( 1 );
		_laser.gameObject.SetActive( false );
	}

	public override void TakeDamage( int damage )
	{
		base.TakeDamage( damage );
		entityManager.towerHealth.text = "Tower Health: " + _health;
	}

	protected override void PlayDamageTakenAnim()
	{
		;
	}
}
