using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Pickupable {
   
    public override void Pickup()
    {
        print("Picked up power up!");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
