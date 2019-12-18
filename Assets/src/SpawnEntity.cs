using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntity : BaseEntity 
{
	[SerializeField]
	private double _spawnTimeMS;
	[SerializeField]
	private UnitEntity _spawnPrefab;

	private double _lastSpawnTime = -1;

	private void Update()
	{
		TrySpawnUnit();
	}

	private void TrySpawnUnit()
	{
		if( entityManager == null )
		{
			return;
		}

		if( _lastSpawnTime == -1 || BootStrap.EpochNow() - _lastSpawnTime > _spawnTimeMS )
		{
			_lastSpawnTime = BootStrap.EpochNow();
			var unit = GameObject.Instantiate( _spawnPrefab, transform.position, Quaternion.identity, transform.parent );
			unit.name = "Unit";
		}
	}

	public override void TakeDamage( int damage )
	{
		//this unit is invincible
		return;
	}

	protected override void PlayDamageTakenAnim()
	{
	}
}
