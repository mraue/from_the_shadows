using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
	public Light directionalLight;
	public GameObject player;
	public GameObject gameOverOverlay;
	public AudioService audioService;

	public Text timer;

	public int amountMonsters;

	public float lightMin;
	public float lightMax;
	public float lightDuration;

	public float spawnMinDuration;
	public float spawnMaxDuration;

	public GameObject columnPrefab;
	public GameObject monsterPrefab;

	public int gridWidth;
	public int gridHeight;

	float _nextSpawnDelay;
	float _currentLightRotation;
	float _currentMonsterDelay;

	List<Position> _usedPositions = new List<Position>();
	List<Column> _activeColumns = new List<Column>();

	Queue<Monster> _monsters = new Queue<Monster>();

	bool _isActive;
	bool _isGameOver;

	float _duration;

	void Start()
	{
		for (int i = 0; i < amountMonsters; i++)
		{
			var monster = (Instantiate(monsterPrefab) as GameObject).GetComponent<Monster>();
			monster.player = player;
			monster.onGameOver = OnGameOver;
			monster.isActive = false;
			monster.gameObject.SetActive(false);
			_monsters.Enqueue(monster);
		}

		_isActive = true;
		_currentMonsterDelay = 0.5f;
	}

	void Reset()
	{
		foreach (var column in _activeColumns)
		{
			Destroy(column.gameObject);
		}

		_activeColumns.Clear();
		_usedPositions.Clear();

		_nextSpawnDelay = 0f;
	}

	public void OnRestart()
	{
		Reset();

		_duration = 0f;
		_isGameOver = false;
		_isActive = true;

		player.transform.position = new Vector3(1f, 1f, 0f);
		player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		player.SetActive(true);
		player.GetComponent<Player>().canMove = true;

		gameOverOverlay.SetActive(false);
	}

	void OnGameOver()
	{
		if (_isGameOver)
		{
			return;
		}
			
		_isGameOver = true;

		foreach (var monster in _monsters)		
		{			
			monster.ShutDown(1f);
		}

		foreach (var column in _activeColumns)
		{
			column.isActive = false;
		}

		player.GetComponent<Player>().Destroyed();
		_isActive = false;

		gameOverOverlay.SetActive(true);


		audioService.Play(0, false);
	}

	void Update()
	{
		if (!_isActive)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				OnRestart();
			}
			return;
		}

		UpdateTimer();

//		UpatedLightPosition();
		SpawnColumns();
		SpawnMonsters();
	}

	void UpdateTimer()
	{
		_duration += Time.deltaTime;
		timer.text = string.Format("{0:000.000}", _duration);
	}

	void SpawnMonsters()
	{
		_currentMonsterDelay -= Time.deltaTime;
	
		if (_currentMonsterDelay > 0f)
		{
			return;
		}

		if (_activeColumns.Count > 0)
		{
			var spawningColumn = _activeColumns[Random.Range(0, _activeColumns.Count)];
			var height = spawningColumn.transform.localScale.z / 2f;
			var colPos = spawningColumn.transform.position;

			if (height > 0.5f)
			{
				var xDist = Mathf.Cos(directionalLight.transform.eulerAngles.x / 180f * Mathf.PI) * height;
				var yDist = Mathf.Cos(directionalLight.transform.eulerAngles.y / 180f * Mathf.PI) * height;

				var monster = _monsters.Dequeue();
				monster.transform.position = new Vector3(colPos.x + xDist, colPos.y - yDist * 0.5f, 1f);
				monster.gameObject.SetActive(true);
				monster.speed =  Random.Range(1f, Mathf.Max(1f, _duration / 8f));
				monster.isActive = true;
				_monsters.Enqueue(monster);
//				Debug.LogFormat("SPAWNING {0} {1} {2} {3}", colPos.x, colPos.y, xDist, yDist);
			}
		}

		_currentMonsterDelay = Random.Range(0.5f, 2f);
	}

	void UpatedLightPosition()
	{
		_currentLightRotation += Time.deltaTime;
		var currentLightRotationAngle = (lightMax - lightMin) * Mathf.Min(1f, _currentLightRotation / lightDuration) + lightMin;
		directionalLight.transform.eulerAngles = new Vector3(currentLightRotationAngle, currentLightRotationAngle, 0f);
	}

	void SpawnColumns()
	{
		_nextSpawnDelay -= Time.deltaTime;

		if (_nextSpawnDelay > 0f)
		{
			return;
		}

		var emptyPosition = GetEmptyPositions();

		if (emptyPosition.Count > 0)
		{
			var newPos = emptyPosition[Random.Range(0, emptyPosition.Count)];
			var column = (Instantiate(columnPrefab) as GameObject).GetComponent<Column>();
			column.transform.position = new Vector3(newPos.x, newPos.y, 0f);
			column.height = Random.Range(3f, 15f);
			column.growthDuration = Random.Range(5f, 20f);
			column.isActive = true;
			_usedPositions.Add(newPos);
			_activeColumns.Add(column);
		}
		else
		{
			Debug.Log("NO EMPTY POSITION FOUND");
		}

		_nextSpawnDelay = Random.Range(spawnMinDuration, spawnMaxDuration);
	}

	List<Position> GetEmptyPositions()
	{
		var emptyPositions = new List<Position>();

		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				var position = _usedPositions.Count > 0 ? _usedPositions.Find((p) => p.x == x && p.y == y) : null;

				if (position == null)
				{
					emptyPositions.Add(new Position { x = x, y = y });
				}
			}
		}

		return emptyPositions;
	}

	public class Position
	{
		public int x;
		public int y;
	}
}
