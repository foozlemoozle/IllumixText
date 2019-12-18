using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : BaseEntity 
{
	[SerializeField]
	private float _movePerFrame;

	Transform _destination;
	
	// Update is called once per frame
	void Update() 
	{
		if( entityManager == null )
		{
			return;
		}

		AdvanceToTarget();
	}

	private void AdvanceToTarget()
	{
		if( _destination == null )
		{
			_destination = entityManager.FindClosestTarget( this, EntityManager.DEFEND_TEAM_TAG );
			if( _destination == null )
			{
				//there are no targets available if we hit this state
				GameObject.Destroy( this.gameObject );
			}
		}
		else if( IsInRange( _destination ) )
		{
			entityManager.DamageUnitAt( _destination, _power );
			GameObject.Destroy( this.gameObject );
		}
		else
		{
			int xDir = _destination.position.x - transform.position.x < 0 ? -1 : 1;
			int yDir = _destination.position.y - transform.position.y < 0 ? -1 : 1;
			int zDir = _destination.position.z - transform.position.z < 0 ? -1 : 1;
			Vector3 newPos = transform.position;
			newPos.x += xDir * _movePerFrame;
			newPos.y += yDir * _movePerFrame;
			newPos.z += zDir * _movePerFrame;

			transform.position = newPos;
		}
	}

	protected override void PlayDamageTakenAnim()
	{
	}
}
