using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TrainControllerScript : MonoBehaviour {


	public Tile groundTile;
    public Tilemap groundTileMap;
	public GameObject player;
	public GameObject log;

	private Vector3Int previouseTile;
	// Use this for initialization
	void Start () {
		SpriteRenderer sprite = log.GetComponent<SpriteRenderer>();
		sprite.sortingOrder = 1;
		Vector3Int currentTile = groundTileMap.WorldToCell(player.transform.position);
		for(int i = currentTile.x -3; i < currentTile.x + 3 ; i++){
			for(int j = currentTile.y -3; j < currentTile.y + 3 ; j++){
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
			for(int i = currentTile.x -3; i < currentTile.x + 3 ; i++){
				Vector3Int tile = new Vector3Int(i,currentTile.y + 3,0);
				groundTileMap.SetTile(tile, groundTile);

				System.Random random = new System.Random();
				
				int randInt = random.Next(4); // 20% chance of appearing
				if(randInt == 0){
					initializeRandomObject(log);
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
