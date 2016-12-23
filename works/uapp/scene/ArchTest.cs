using UnityEngine;
using System.Collections;

public class ArchTest : MonoBehaviour {

	private uapp.ArchitectureBuilder mArchitectureBuilder;

	// Use this for initialization
	void Start () {
		
		var arch = new uapp.Architecture();
		var room = new uapp.Room();
		arch.AddRoom(room);
		room.WallTexture = new uapp.RealWorldTexture(Resources.Load<Texture2D>("texture/wall_default"), 1.0f, 1.0f);
		room.FloorTexture = new uapp.RealWorldTexture(Resources.Load<Texture2D>("texture/floor_default"), 2.0f, 1.0f);
		room.GroundHeight = 0.0f;
		room.PushPoint(new Vector3(0.0f, 0.0f, 0.0f));
		room.PushPoint(new Vector3(5.0f, 0.0f, 0.0f));
		room.PushPoint(new Vector3(5.0f, 0.0f, -5.0f));
		room.PushPoint(new Vector3(2.5f, 0.0f, -10.0f));
		room.PushPoint(new Vector3(0.0f, 0.0f, -5.0f));

		var wall = room.WallAt(0);
		var wallItem = new uapp.WallItem();
		wallItem.Data = new Rect(1.0f, 1.0f, 1.0f, 1.0f);
		wall.AddWallItem(wallItem);
		wallItem = new uapp.WallItem();
		wallItem.Data = new Rect(3.01f, 0.01f, 1.0f, 2.0f);
		wall.AddWallItem(wallItem);

		mArchitectureBuilder = new uapp.ArchitectureBuilder(arch);
		mArchitectureBuilder.Build();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
