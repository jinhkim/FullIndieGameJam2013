using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPathfinding : MonoBehaviour {

	BaseMinion minion;
	public static float TILE_SIZE = 10;
	public Vector3 dest;
	public static Map _gameMap = null;
	public List<Node> _curPath;
	public int _curNode;
	public float smooth = 10f;
	// Use this for initialization
	void Start () 
	{
		minion = gameObject.GetComponent<BaseMinion>();
		if(minion == null)
			Debug.Log("ERROR: BaseMinion script not found in this game object!");
		else {
			minion.initMinion();
			Debug.Log("minion initialized of name: " + minion.minionName);
		}
		
		dest = transform.position;

		if (_gameMap == null)
		{
			_gameMap = Map.createMap ();
		}
		_curNode = -1;
		_curPath = new List<Node> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (0)) {
			
//			RaycastHit hit;
//			
//			if(Physics.Raycast(ray, out hit)){
//				if(hit.collider == minion.collider){
//					BaseMinion baseMinion = hit.collider.gameObject.GetComponent<BaseMinion>();
//					if(baseMinion.minionName.Equals(minion.minionName)){
//						Debug.Log(baseMinion.minionName + " Minion Selected!");
//						minion.isActive = true;
//					}
//				}
//			}
			
			if(minion.isActive){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				Vector3 direction = ray.direction;
				
				Vector3 to = ray.GetPoint((-ray.origin.y / direction.y) - transform.position.y);
	
				to.x += TILE_SIZE / 2; 
				to.z += TILE_SIZE / 2;
	
				_curPath = AStar.findPath(transform.position, to, _gameMap);
				if(_curPath != null)
				{
					_curNode = 0;
				}
			}
		}

		if (Input.GetMouseButtonDown (1)) 
		{
			//special ability if there are any minions active
			//only one minion is active at one time
			if(minion.isActive)
				minion.specialAbility();
		}
		
		if (_curNode != -1 && _curPath != null) 
		{
			if(_curNode != _curPath.Count - 1)
			{
				dest = _gameMap.fromMapCoords (_curPath [_curNode + 1].position);
			}
			else
			{
				dest = _gameMap.fromMapCoords (_curPath [_curNode].position);
			}
			
			if (Vector3.Distance (dest, transform.position) < 0.1f) 
			{
				smooth = 200;
			}
			dest.y = transform.position.y;
			if (Vector3.Distance (dest, transform.position) < 0.01f) 
			{
				smooth = minion.smooth;
				++_curNode;
				if(_curNode == _curPath.Count)
				{
					_curNode = -1;
				}
			}
			
			
		}
		transform.position = dest;
		transform.position = Vector3.Lerp (transform.position, dest, Time.deltaTime * smooth);
//		minion.pos.x = transform.position.x;
//		minion.pos.y = transform.position.z;
	}
	
	
	public class Node
	{
		public List<Node> neighbors;
		public Vector2 position;
		public bool isWall;
		
		public Node(int xPos, int yPos)
		{
			position = new Vector2(xPos, yPos);
			neighbors = new List<Node>();
			isWall = false;
		}
		
		public Node(Vector3 pos)
		{
			position = new Vector2(pos.x, pos.z);
			neighbors = new List<Node>();
			isWall = false;
		}
		
		public Node(int xPos, int yPos, bool isAWall)
		{
			position = new Vector2(xPos, yPos);
			neighbors = new List<Node>();
			isWall = isAWall;
		}
		
		public Node(Vector3 pos, bool isAWall)
		{
			position = new Vector2(pos.x, pos.z);
			neighbors = new List<Node>();
			isWall = isAWall;
		}
		
		public void linkNode(Node neighbor)
		{
			neighbors.Add (neighbor);
		}
	}
	
	public static class AStar
	{
		public static List<Node> findPath(Vector3 from, Vector3 to, Map map)
		{
			//The path
			List<Node> result = null;
			
			//All the nodes in the map
			//List<Node> nodes = map.nodes;
			
			//List of closed nodes
			List<Node> closedNodes = new List<Node>();
			
			//List of open nodes
			List<Node> openNodes = new List<Node>();
			
			//Stores the distance from the start to each node
			Dictionary<Node, int> distanceFromStart = new Dictionary<Node, int>();
			
			//Stores the estimated distace
			Dictionary<Node, int> estimatedDistanceToEnd = new Dictionary<Node, int>();
			
			//Stores which node came from which node on the path.
			Dictionary<Node, Node> nodeOrigin = new Dictionary<Node, Node>();
			
			Vector2 mapCoordsFrom = map.toMapCoords(from);
			mapCoordsFrom.x = Mathf.RoundToInt (mapCoordsFrom.x);
			mapCoordsFrom.y = Mathf.RoundToInt (mapCoordsFrom.y);
			Vector2 mapCoordsTo = map.toMapCoords (to);
			
			if (mapCoordsTo.x >= map.width) 
			{
				mapCoordsTo.x = map.width - 1;
			} 
			else if (mapCoordsTo.x < 0) 
			{
				mapCoordsTo.x = 0;
			}
			
			if (mapCoordsTo.y >= map.height)
			{
				mapCoordsTo.y = map.height - 1;
			}
			else if (mapCoordsTo.y < 0) 
			{
				mapCoordsTo.y = 0;
			}
			
			//The start node
			Node curNode = map.grid [(int)mapCoordsFrom.y] [(int)mapCoordsFrom.x];
			
			//The goal node
			Node goal = map.grid [(int)mapCoordsTo.y] [(int)mapCoordsTo.x];
			
			//Add the start node to distanceFromStart with a distance 0
			distanceFromStart.Add(curNode, 0);
			
			//Calculate the estimated cost to get to the end from the start node
			estimatedDistanceToEnd.Add(curNode, estimateCost(curNode, goal));
			
			//Add the start node to the list of open nodes (technically its closed but it flows easier this way)
			openNodes.Add(curNode);
			
			//Used for checking distance to the current neighbhor being checked.
			int curDistance;
			
			//While there are still open nodes
			while (openNodes.Count > 0)
			{
				
				//Get the node in the set of open nodes that has the lowest estimated distance to the end.
				curNode = findClosestNode(openNodes, estimatedDistanceToEnd);
				
				//If the selected node is the goal, return the path.
				if (curNode == goal)
				{
					return constructPath(nodeOrigin, goal);
				}
				
				//Otherwise, remove the current node from the set of open nodes
				openNodes.Remove(curNode);
				
				//And add the current node to the set of closed nodes
				closedNodes.Add(curNode);
				
				
				//For each neighbor node
				for (int i = 0; i < curNode.neighbors.Count; i++)
				{
					//Calculate the distance from start to the neighbor through the current node
					curDistance = distanceFromStart[curNode] + 1;
					
					//If the neighbhor was already in the set of closed nodes, or if the current distance to the neighbhor is smaller than going through the current node, continue
					if (closedNodes.Contains(curNode.neighbors[i]) || (distanceFromStart.ContainsKey(curNode.neighbors[i]) && (curDistance >= distanceFromStart[curNode.neighbors[i]])))
					{
						continue;
					}
					
					//Else if the neighbor isnt in the set of open nodes or the distance from the start to the neighbor stored currently is larger than the distance to the neighbor through the current node
					if (!openNodes.Contains(curNode.neighbors[i]) || (curDistance < distanceFromStart[curNode.neighbors[i]]))
					{
						//Set the current node as where the neighbor node came from
						if (!nodeOrigin.ContainsKey(curNode.neighbors[i]))
						{
							nodeOrigin.Add(curNode.neighbors[i], curNode);
						}
						else
						{
							nodeOrigin[curNode.neighbors[i]] = curNode;
						}
						
						//Set the distance from the start to the neighbor node as the current distance
						if (!distanceFromStart.ContainsKey(curNode.neighbors[i]))
						{
							distanceFromStart.Add(curNode.neighbors[i], curDistance);
						}
						else
						{
							distanceFromStart[curNode.neighbors[i]] = curDistance;
						}
						
						//Calculate and set the estimated distance from the neighbor to the end
						if (!estimatedDistanceToEnd.ContainsKey(curNode.neighbors[i]))
						{
							estimatedDistanceToEnd.Add(curNode.neighbors[i], distanceFromStart[curNode.neighbors[i]] + estimateCost(curNode.neighbors[i], goal));
						}
						else
						{
							estimatedDistanceToEnd[curNode.neighbors[i]] = distanceFromStart[curNode.neighbors[i]] + estimateCost(curNode.neighbors[i], goal);
						}
						
						//Add the neighbor the the set of open ndoes if it isnt already in there
						if (!openNodes.Contains(curNode.neighbors[i]))
						{
							openNodes.Add(curNode.neighbors[i]);
						}
					}
				}
			}
			
			//Returns null
			return result;
		}
		
		private static int estimateCost(Node from, Node to)
		{
			Vector2 difference = from.position - to.position;
			return (int) (Mathf.Abs(difference.x) + Mathf.Abs(difference.y));
		}
		
		public static Node findClosestNode(List<Node> openNodes, Vector2 position)
		{
			float minDist = 999999;
			Node closest = null;
			float distance;
			
			for (int i = 0; i < openNodes.Count; i++)
			{
				distance = Vector2.Distance(position, openNodes[i].position);
				if (distance < minDist)
				{
					minDist = distance;
					closest = openNodes[i];
				}
			}
			
			return closest;
		}
		
		private static Node findClosestNode(List<Node> openNodes, Dictionary<Node, int> distances)
		{
			int minDist = 999999;
			Node closest = null;
			
			for (int i = 0; i < openNodes.Count; i++)
			{
				if (distances[openNodes[i]] < minDist)
				{
					minDist = distances[openNodes[i]];
					closest = openNodes[i];
				}
			}
			
			return closest;
		}
		
		private static List<Node> constructPath(Dictionary<Node, Node> nodeOrigins, Node from)
		{
			List<Node> result;
			if (nodeOrigins.ContainsKey(from))
			{
				result = constructPath(nodeOrigins, nodeOrigins[from]);
				result.Add(from);
				return result;
			}
			else
			{
				result = new List<Node>();
				result.Add(from);
				return result;
			}
		}
	}
	
	public class Map
	{
		public List<Node> nodes;
		public List<Node> walls;
		public List<List<Node>> grid;
		public Vector2 min, max;
		public int width, height;
		
		private Map()
		{
			nodes = new List<Node>();
			walls = new List<Node>();
			grid = new List<List<Node>>();
			grid.Add(new List<Node>());
		}
		public void moveWall(Vector3 prevPosition, Vector3 newPosition)
		{
			Vector2 prevPos = toMapCoords(prevPosition);
			Vector2 newPos = toMapCoords(newPosition);
			
			grid[(int)prevPos.y][(int)prevPos.x].isWall = false;
			grid[(int)newPos.y][(int)newPos.x].isWall = true;
			
			linkNeighbors( grid[(int)prevPos.y][(int)prevPos.x]);
			
			int x = (int)prevPos.x;
			int y = (int)prevPos.y;
			
			if(((x - 1) >= 0) && !grid[y][x - 1].isWall)
			{
				linkNeighbors( grid[y][x-1]);
			}
			
			if(((x + 1) < width) && !grid[y][x + 1].isWall)
			{
				linkNeighbors( grid[y][x+1]);
			}
			
			if(((y - 1) >= 0) && !grid[y - 1][x].isWall)
			{
				linkNeighbors( grid[y - 1][x]);
			}
			
			if(((y + 1) < height) && !grid[y + 1][x].isWall)
			{
				linkNeighbors( grid[y+ 1][x]);
			}
			
			x = (int)newPos.x;
			y = (int)newPos.y;
			
			if(((x - 1) >= 0) && !grid[y][x - 1].isWall)
			{
				linkNeighbors( grid[y][x-1]);
			}
			
			if(((x + 1) < width) && !grid[y][x + 1].isWall)
			{
				linkNeighbors( grid[y][x+1]);
			}
			
			if(((y - 1) >= 0) && !grid[y - 1][x].isWall)
			{
				linkNeighbors( grid[y - 1][x]);
			}
			
			if(((y + 1) < height) && !grid[y + 1][x].isWall)
			{
				linkNeighbors( grid[y+ 1][x]);
			}
		}
		
		public void linkNeighbors(Node toLink)
		{
			toLink.neighbors = new List<Node>();
			int x = (int) toLink.position.x;
			int y = (int) toLink.position.y;
			if(((x - 1) >= 0) && !grid[y][x - 1].isWall)
			{
				grid[y][x].linkNode(grid[y][x - 1]);
			}
			
			if(((x + 1) < width) && !grid[y][x + 1].isWall)
			{
				grid[y][x].linkNode(grid[y][x + 1]);
			}
			
			if(((y - 1) >= 0) && !grid[y - 1][x].isWall)
			{
				grid[y][x].linkNode(grid[y - 1][x]);
			}
			
			if(((y + 1) < height) && !grid[y + 1][x].isWall)
			{
				grid[y][x].linkNode(grid[y + 1][x]);
			}
		}
		
		public bool isClear (Vector3 Position)
		{
			Vector2 Pos = toMapCoords(Position);
			
			return !grid[(int)Pos.y][(int)Pos.x].isWall;
		}
		public static Map createMap()
		{
			Map map = new Map ();
			GameObject[] gameWalls =  GameObject.FindGameObjectsWithTag ("Wall");
			map.min = new Vector2 (9999, 9999);
			map.max = new Vector2 (-9999, -9999);
			
			
			for (int i = 0; i < gameWalls.Length; i++) {
				
				if(gameWalls[i].transform.position.x < map.min.x)
				{
					map.min.x = gameWalls[i].transform.position.x;
				}
				
				if(gameWalls[i].transform.position.x > map.max.x)
				{
					map.max.x = gameWalls[i].transform.position.x;
				}
				
				if(gameWalls[i].transform.position.z < map.min.y)
				{
					map.min.y = gameWalls[i].transform.position.z;
				}
				
				if(gameWalls[i].transform.position.z > map.max.y)
				{
					map.max.y = gameWalls[i].transform.position.z;
				}
				
				map.walls.Add(new Node(gameWalls[i].transform.position, true));
			}


			map.height = (int)((map.max.y - map.min.y) / TILE_SIZE) + 1;
			map.width = (int)((map.max.x - map.min.x) / TILE_SIZE) + 1;

			for (int y = 0; y < map.height; y++) 
			{
				for(int x = 0; x < map.width; x++)
				{
					map.grid[y].Add(new Node(x, y));
				}
				map.grid.Add(new List<Node>());
			}

			Vector2 curWall;

			for (int i = 0; i < map.walls.Count; i++) 
			{

				curWall = (map.walls[i].position - map.min) / TILE_SIZE;
				curWall.x = Mathf.Min(map.width - 1, (int)curWall.x);
				curWall.y = Mathf.Min(map.height - 1, (int)curWall.y);
				map.grid[(int)curWall.y][(int)curWall.x].isWall = true;
			}
			
			map.linkNodes ();
			
			
			
			return map;
		}
		
		public Vector2 toMapCoords(Vector3 position)
		{
			Vector2 result = new Vector2 (position.x, position.z);
			
			result -= min;
			
			result /= TILE_SIZE;
			
			if (result.x > width) {
				result.x = width ;
			}
			
			if (result.y > height) {
				result.y = height ;
			}
			
			if (result.x < 0) {
				result.x = 0;
			}
			
			if (result.y < 0) {
				result.y = 0;
			}
			
			return result;
		}
		
		public Vector3 fromMapCoords(Vector2 position)
		{
			Vector3 result = new Vector3 (position.x, 0, position.y);
			
			result *= TILE_SIZE;
			result.x += min.x;
			result.z += min.y;
			
			return result;
		}
		
		private void linkNodes()
		{
			//width = (int)((max.x - min.x) / TILE_SIZE) + 1;;
			//height = (int)((max.y - min.y) / TILE_SIZE) + 1;;
			
			for (int y = 0; y < grid.Count; y++) {
				for(int x = 0; x < grid[y].Count; x++){
					if(!grid[y][x].isWall){
						
						if(((x - 1) >= 0) && !grid[y][x - 1].isWall){
							grid[y][x].linkNode(grid[y][x - 1]);
						}
						
						if(((x + 1) < width) && !grid[y][x + 1].isWall){
							grid[y][x].linkNode(grid[y][x + 1]);
						}
						
						if(((y - 1) >= 0) && !grid[y - 1][x].isWall){
							grid[y][x].linkNode(grid[y - 1][x]);
						}
						
						if(((y + 1) < height) && !grid[y + 1][x].isWall){
							grid[y][x].linkNode(grid[y + 1][x]);
						}
						
						nodes.Add(grid[y][x]);
					}
				}
			}
		}
	}
}
