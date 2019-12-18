using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class BaseEntity : MonoBehaviour 
{

	[SerializeField]
	protected string[] _tags;
	[SerializeField]
	protected int _health;//how resilient an entity is
	[SerializeField]
	protected int _power;//how much damage an entity deals
	[SerializeField]
	protected float _attackRange;//how far a unit can attack from in unity world units

	public EntityManager entityManager;

	public bool IsDead { get { return _health <= 0; } }

	protected virtual void Start()
	{
		EntityManager.RegisterEntity( this );
	}

	protected void OnDestroy()
	{
		EntityManager.DeregisterEntity( this );
		PreDestroy();
	}

	protected virtual void PreDestroy()
	{
	}

	public bool HasTag( string tag )
	{
		if( _tags == null )
		{
			return false;
		}

		int count = _tags.Length;
		for( int i = 0; i < count; ++i )
		{
			if( _tags[ i ] == tag )
			{
				return true;
			}
		}

		return false;
	}

	public bool IsInRange( Transform other )
	{
		if( other == null )
		{
			return false;
		}

		float dist = BootStrap.DistSqrd( transform.position, other.position );
		return ( _attackRange * _attackRange ) >= dist;
	}

	public virtual void TakeDamage( int damage )
	{
		_health -= damage;
		if( IsDead )
		{
			GameObject.Destroy( this.gameObject );
		}
		else
		{
			PlayDamageTakenAnim();
		}
	}

	protected abstract void PlayDamageTakenAnim();
}
