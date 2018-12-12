using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TrainControllerScript : MonoBehaviour {

	private readonly int GRID_X = 7; 
	private readonly int GRID_Y = 4; 

	public Tile groundTile;
    public Tilemap groundTileMap;
	public GameObject player;
	public GameObject log;
	public GameObject stone;

	private Vector3Int previouseTile;
	// Use this for initialization
	void Start () {
		SpriteRenderer sprite = log.GetComponent<SpriteRenderer>();
		sprite.sortingOrder = 1;
		Vector3Int currentTile = groundTileMap.WorldToCell(player.transform.position);
		for(int i = currentTile.x -GRID_X; i < currentTile.x + GRID_X ; i++){
			for(int j = currentTile.y -GRID_Y; j < currentTile.y + GRID_Y ; j++){
				Vector3Int tile = new Vector3Int(i,j,0);
				groundTileMap.SetTile(tile, groundTile);
			}	
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
        Vector3Int currentTile = groundTileMap.WorldToCell(player.transform.position);
		if(currentTile.y != previouseTile.y)
        {
			for(int i = currentTile.x -GRID_X; i < currentTile.x + GRID_X ; i++){
				Vector3Int tile = new Vector3Int(i,currentTile.y + GRID_Y, 0);
				groundTileMap.SetTile(tile, groundTile);

				System.Random random = new System.Random();
				int randInt = random.Next(2);
				if(randInt == 0){
					GameObject gameObject = log;
				}
				else{
					GameObject gameObject = stone;
				}
				randInt = random.Next(4); // 20% chance of appearing
				if(randInt == 0){
					initializeRandomObject(gameObject);
				}
			}
			previouseTile = currentTile;
		}
	}

	private void initializeRandomObject(GameObject gameObject){
		System.Random random = new System.Random();
		int randInt = random.Next(10,20);
					
		int switchInt = random.Next(2);
		if(switchInt == 1){ //make appear on both sides of train
			randInt = -randInt;
		}

		gameObject.transform.position = new Vector3Int(
						(int)player.transform.position.x + randInt,
						(int)player.transform.position.y + 10, 0 
						);
		randInt = random.Next(180);
		var rotationVector = gameObject.transform.rotation.eulerAngles;
   		rotationVector.z = randInt;
   		gameObject.transform.rotation = Quaternion.Euler(rotationVector);

		Instantiate(gameObject);
	}	
}
