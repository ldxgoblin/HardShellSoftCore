using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bloodbath{
	
	public class DemoManager : MonoBehaviour {
		public TextMesh text_fx_name;
		public TextMesh text_color;
		public GameObject[] prefabs_r;
		public GameObject[] prefabs_g;
		public GameObject[] prefabs_o;
		public int index_fx = 0;
		public int index_color = 0;


		private Ray ray;
		private RaycastHit ray_cast_hit;


		void Start () {
			ChangeTextPrefab ();
			ChangeTextColor ();
			Destroy(GameObject.Find("Instructions"), 12.5f);
		}


		void Update () 
		{
			if ( Input.GetMouseButtonDown(0) )
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast (ray.origin, ray.direction, out ray_cast_hit, 1000f)) 
				{
					ShowFX ();
				}
			}
			//Change-FX Keyboard	
			if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
				index_fx--;
				if(index_fx <= -1)
					index_fx = prefabs_r.Length - 1;
				ChangeTextPrefab ();
			}
			if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
				index_fx++;
				if(index_fx >= prefabs_r.Length)
					index_fx = 0;
				ChangeTextPrefab ();
			}
			if ( Input.GetKeyDown("a") || Input.GetKeyDown("up")){
				index_color++;
				if(index_color >= 3)
					index_color = 0;
				ChangeTextColor ();
			}
			if ( Input.GetKeyDown("s") || Input.GetKeyDown("down")){
				index_color--;
				if(index_color <= -1)
					index_color = 2;
				ChangeTextColor ();
			}
			if (Input.GetKeyDown ("space")) {
				ShowFX ();
			}				
		}


		void ChangeTextPrefab()
		{
			switch(index_color){
			case 0: text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs_r[ index_fx ].name;
				break;
			case 1: text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs_g[ index_fx ].name;
				break;
			case 2: text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs_o[ index_fx ].name;
				break;
			} 
		}


		void ChangeTextColor()
		{
			switch(index_color){
			case 0: text_color.text = "RED";
				break;
			case 1: text_color.text = "GREEN";
				break;
			case 2: text_color.text = "OIL";
				break;
			}
		}

		void ShowFX()
		{
			switch(index_color){
			case 0: Instantiate (prefabs_r[index_fx], ray_cast_hit.point, transform.rotation);
				break;
			case 1: Instantiate (prefabs_g[index_fx], ray_cast_hit.point, transform.rotation);
				break;
			case 2: Instantiate (prefabs_o[index_fx], ray_cast_hit.point, transform.rotation);
				break;
			}
		}

	}

}


