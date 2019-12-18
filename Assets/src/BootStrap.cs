using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using DateTime = System.DateTime;
using TimeSpan = System.TimeSpan;

public class BootStrap : MonoBehaviour 
{
	public static double EpochNow()
	{
		return ( DateTime.UtcNow - new DateTime( 1970, 1, 1 ) ).TotalMilliseconds;
	}

	public static float DistSqrd( Vector3 a, Vector3 b )
    {
        return ( a.x - b.x ) * ( a.x - b.x ) + ( a.y - b.y ) * ( a.y - b.y ) + ( a.z - b.z ) * ( a.z - b.z );
    }

	[SerializeField]
	protected TextMeshProUGUI _towerHealth;
	[SerializeField]
	protected TextMeshProUGUI _unitCount;
	[SerializeField]
	protected TextMeshProUGUI _timer;

	private EntityManager _entityManager;

	// Use this for initialization
	void Awake() 
	{
		_entityManager = new EntityManager();
		_entityManager.Setup( _towerHealth, _unitCount );
	}

	void Update()
	{
		if( _timer != null )
		{
			_timer.text = string.Format( "Time: {0} seconds", Mathf.Floor( Time.fixedTime ) );
		}
	}
	
	void OnDestroy()
	{
		_entityManager.Teardown();
	}
}
