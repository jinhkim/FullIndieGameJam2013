using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {
	public float minDistance = 200;
	public bool isPursuing;
	public int smooth = 10, doubleSpeed = 20, normalSpeed = 3;
	public GameObject closest;
	public float distance;
	public List<PlayerPathfinding.Node> nodes;
	public int _curNode;
	public Vector3[] patrolPoints;
	public int curPatrolPoint;
	bool offPatrol, returning;
	Vector3 dest;
	// Use this for initialization
	void Start () {
		_curNode = -1;
		dest = transform.position;
		patrolPoints = null;
		offPatrol = false;
		returning = false;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		calculateClosest (players);

		isPursuing = distance < minDistance;


		if(isPursuing)
		{
			offPatrol = true;
			nodes = PlayerPathfinding.AStar.findPath(transform.position, closest.transform.position, PlayerPathfinding._gameMap);

			if(nodes != null)
			{
				_curNode = 0;
				if(_curNode != nodes.Count - 1)
				{
					dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode + 1].position);
				}
				else
				{
					dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode].position);
				}
			
				dest.y = 1;
					
				if (Vector3.Distance (dest, transform.position) < 0.1f) 
				{
					++_curNode;
					if(_curNode == nodes.Count)
					{
						_curNode = -1;
					}
				}
			}

			if(distance < 40)
			{
				smooth = doubleSpeed;
			}
			else
			{
				smooth = normalSpeed;
			}
		}
		else if(offPatrol && patrolPoints != null)
		{
			offPatrol = false;
			curPatrolPoint = 0;
			nodes = PlayerPathfinding.AStar.findPath(transform.position, patrolPoints[0], PlayerPathfinding._gameMap);
			if(nodes != null)
			{
				_curNode = 0;
				if(_curNode != nodes.Count - 1)
				{
					dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode + 1].position);
				}
				else
				{
					dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode].position);
				}
				dest.y = 1;
				returning = true;
				if (Vector3.Distance (dest, transform.position) < 0.1f)
				{
					++_curNode;
					if(_curNode == nodes.Count)
					{
						_curNode = -1;
						returning = false;
					}
				}
			}
		}
		else if(returning && nodes != null)
		{
			if(_curNode == -1)
			{
				returning = false;
			}
			else if(_curNode != nodes.Count - 1)
			{
				dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode + 1].position);
			}
			else
			{
				dest = PlayerPathfinding._gameMap.fromMapCoords (nodes [_curNode].position);
			}
			dest.y = 1;
			if (Vector3.Distance (dest, transform.position) < 0.1f) 
			{
				++_curNode;
				if(_curNode == nodes.Count)
				{
					_curNode = -1;
					returning = false;
				}
			}
				

		}
		else if(patrolPoints != null && Vector3.Distance(dest, transform.position) < 0.1f)
		{
			++curPatrolPoint;
			if(curPatrolPoint == patrolPoints.Length)
			{
				curPatrolPoint = 0;
			}
			dest = patrolPoints[curPatrolPoint];
		}

		transform.position = Vector3.Lerp (transform.position, dest, Time.deltaTime * smooth);
	}

	public void assignPatrolPoints(Vector3[] pPoints)
	{
		patrolPoints = pPoints;
		curPatrolPoint = 0;
	}

	Vector3 wander(PlayerPathfinding.Map map)
	{
		int direction = Random.Range (0, 3);
		Vector3 v = new Vector3 ();
		Vector2 tile;

		switch(direction)
		{
		case 0:
			v.x = PlayerPathfinding.TILE_SIZE;
			break;

		case 1:
			v.x = -PlayerPathfinding.TILE_SIZE;
			break;

		case 2:
			v.y = PlayerPathfinding.TILE_SIZE;
			break;

		case 3:
			v.y = -PlayerPathfinding.TILE_SIZE;
			break;
		}

		tile = map.toMapCoords (v + transform.position);
		if (tile.x < map.width && tile.y < map.height && !map.grid [(int)tile.y] [(int)tile.x].isWall) 
		{
			return v;
		}
		return new Vector3 ();
	}

	void calculateClosest(GameObject[] objects)
	{
		int indexOfClosest = 0;
		float curDist;
		distance = 99999;
		
		for (int i = 0; i < objects.Length; i++) {
			curDist = Vector3.Distance(transform.position, objects[i].transform.position);
			if(curDist < distance)
			{
				distance = curDist;
				indexOfClosest = i;
			}
		}
		closest = objects [indexOfClosest];
	}

	GameObject getPositionOfClosest(GameObject[] objects)
	{
		int indexOfClosest = 0;
		float dist = 99999, curDist;

		for (int i = 0; i < objects.Length; i++) {
			curDist = Vector3.Distance(transform.position, objects[i].transform.position);
			if(curDist < dist)
			{
				dist = curDist;
				indexOfClosest = i;
			}
		}
		return objects [indexOfClosest];
	}

	GameObject getPositionOfClosest(GameObject[] objects, float withinDist)
	{
		int indexOfClosest = 0;
		float dist = 99999, curDist;
		
		for (int i = 0; i < objects.Length; i++) {
			curDist = Vector3.Distance(transform.position, objects[i].transform.position);
			if(curDist < dist)
			{
				dist = curDist;
				indexOfClosest = i;
			}
		}
		if (dist > withinDist) {
			return null;
		}
		return objects [indexOfClosest];
	}

	Vector3 flee(Vector3 awayFrom)
	{
		return Vector3.Normalize(new Vector3(transform.position.x, 0, transform.position.z) - awayFrom);
	}

	Vector3 chase(Vector3 towards)
	{
		return Vector3.Normalize(towards - transform.position);
	}
}
