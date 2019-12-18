using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityManager
{
    public static System.Action<BaseEntity> RegisterEntity = ( BaseEntity ) => {};
    public static System.Action<BaseEntity> DeregisterEntity = ( BaseEntity ) => {};

    public static readonly string ATTACK_TEAM_TAG = "attacker";
    public static readonly string DEFEND_TEAM_TAG = "defender";

    private List<BaseEntity> _entities;
    private int _defenderRefCount = 0;

	public TextMeshProUGUI towerHealth;
	public TextMeshProUGUI unitCount;
    private int _unitRefCount;

    public EntityManager()
    {
        _entities = new List<BaseEntity>();
        _unitRefCount = 0;
    }

    public void Setup( TextMeshProUGUI towerHealth, TextMeshProUGUI unitCount )
    {
        RegisterEntity += OnRegisterEntity;
        DeregisterEntity += OnDeregisterEntity;

        this.towerHealth = towerHealth;
        this.unitCount = unitCount;
    }

    public void Teardown()
    {
        RegisterEntity -= OnRegisterEntity;
        DeregisterEntity -= OnDeregisterEntity;

        int count = _entities.Count;
        for( int i = 0; i < count; ++i )
        {
            if( _entities[ i ] != null )
            {
                GameObject.Destroy( _entities[ i ].gameObject );
            }
        }
    }

    private void OnRegisterEntity( BaseEntity entity )
    {
        _entities.Add( entity );
        entity.entityManager = this;

        if( entity.HasTag( DEFEND_TEAM_TAG ) )
        {
            _defenderRefCount++;
        }
        if( entity is UnitEntity )
        {
            _unitRefCount++;
            unitCount.text = "Unit Count: " + _unitRefCount;
        }
    }

    private void OnDeregisterEntity( BaseEntity entity )
    {
        _entities.Remove( entity );
        entity.entityManager = null;

        if( entity.HasTag( DEFEND_TEAM_TAG ) )
        {
            _defenderRefCount--;
            TryCompleteGame();
        }
        if( entity is UnitEntity )
        {
            _unitRefCount--;
            unitCount.text = "Unit Count: " + _unitRefCount;
        }
    }

    private void TryCompleteGame()
    {
        if( _defenderRefCount <= 0 )
        {
            //game is complete--cleanup
            towerHealth.text = string.Format( "Defenders lasted for {0} seconds!", System.Math.Floor( Time.fixedTime ) );
            Teardown();
        }
    }

    // returns transform of thing damaged
    public Transform DamageClosest( BaseEntity actor, string targetTag, int damage )
    {
        BaseEntity closest = FindClosest( actor, targetTag );

        if( closest != null && actor.IsInRange( closest.transform ) )
        {
            Debug.DrawLine( actor.transform.position, closest.transform.position, Color.red, 500 );
            closest.TakeDamage( damage );

            return closest.transform;
        }

        return null;
    }

    public void DamageUnitAt( Transform position, int damage )
    {
        position.gameObject.GetComponent<BaseEntity>().TakeDamage( damage );
    }

    //copy pasta is bad, but I'm trying to do this quickly.
    private BaseEntity FindClosest( BaseEntity actor, string targetTag )
    {
        int count = _entities.Count;
        BaseEntity closest = null;
        float closestDist = -1;
        for( int i = 0; i < count; ++i )
        {
            BaseEntity target = _entities[ i ];
            if( !target.HasTag( targetTag ) )
            {
                continue;
            }

            float dist = BootStrap.DistSqrd( actor.transform.position, target.transform.position );

            if( closestDist == -1 || dist < closestDist )
            {
                closest = target;
                closestDist = dist;
            }
        }

        return closest;
    }

    public Transform FindClosestTarget( BaseEntity actor, string targetTag )
    {
        BaseEntity closest = FindClosest( actor, targetTag );
        return closest != null ? closest.transform : null;
    }

    public void DamageAll( string targetTag, int damage )
    {
        int count = _entities.Count;
        for( int i = 0; i < count; ++i )
        {
            if( _entities[ i ].HasTag( targetTag ) )
            {
                _entities[ i ].TakeDamage( damage );
            }
        }
    }
}
