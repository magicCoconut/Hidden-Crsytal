﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour 
{
	public TeamColor BallColor;
	public Dimple CurrentDimple;


	SelectionState selectionState;

	public static int i= 0;
	public static int k = 0;

	public static List<Ball> Balls = new List<Ball>();
	public static bool Selected;
	public static Ball SelectedBall;
	
	// Use this for initialization
	void Start ()
	{
		setDefaultColor();

		Balls.Add(this);
	}

	void OnMouseOver()
	{
		//If we click on a ball, it is selected and we lock into that
		if(Input.GetMouseButtonDown(0) && !Selected)
		{
			selectionState = SelectionState.SELECTED;

			Selected = true;
			SelectedBall = this;

			HighlightNeighboringDimples();


		}

		//Otherwise we're just highlighting the ball if there are no other balls selected
		else if(!Selected)
		{
			selectionState = SelectionState.HIGHLIGHTED;

			//Make sure every other ball is not in a highlighted state
			foreach(Ball b in Balls)
			{
				if(b != this)
					b.selectionState = SelectionState.NONE;
			}
		}
	
	}

	void OnMouseExit()
	{
		if(selectionState != SelectionState.SELECTED)
			selectionState = SelectionState.NONE;
	}

	void Update()
	{
		switch(selectionState)
		{
		case SelectionState.HIGHLIGHTED:
			gameObject.renderer.material.SetColor("_OutlineColor", Color.cyan);
			break;
		case SelectionState.SELECTED:
			gameObject.renderer.material.SetColor("_OutlineColor", Color.green);
			break;
		default:
			gameObject.renderer.material.SetColor("_OutlineColor", Color.black);
			break;
		}

		//Input to break out of selection state
		if(Input.GetMouseButtonDown(1))
		{
			if(Selected)
			{
				dodefult();
			}
		}

	}
	public static void dodefult(){
		SelectedBall.setDefaultColor();
		SelectedBall.selectionState = SelectionState.NONE;
		
		Selected = false;
		
		//Fix dimple colours
		foreach(Neighbor n in SelectedBall.CurrentDimple.neighbors)
		{
			n.dimple.SetDefaultColor();
		}
		
		SelectedBall = null;
		
		if(i == 1){
			currentPlayer.title = "blue color player's turn";
			i=0;
		}else if(i == 0){
			currentPlayer.title = "red color player's turn";
			i = 1;
		}
		}

	public void HighlightNeighboringDimples()
	{
		List<Neighbor> neighbors = CurrentDimple.neighbors;

		foreach(Neighbor n in neighbors)
		{
			n.dimple.renderer.material.SetColor("_OutlineColor", Color.yellow);
		}
	}

	void setDefaultColor()
	{
		switch(BallColor)
		{
		case TeamColor.BLUE:
			gameObject.renderer.material.color = Color.blue;
			break;
		case TeamColor.RED:
			gameObject.renderer.material.color = Color.red;
			break;
		default:
			break;
		}
	}

	/* method that will move a ball in a certain direction, or hop in a certain direction */

	public bool moveBall(Direction d){
		Dimple moveToDimple = CurrentDimple.getNeighborAtDirection(d);
		//Debug.Log (d);
		//Debug.Log (moveToDimple);
		if(moveToDimple==null){
			return false;
		}
		else{
			if(moveToDimple.isOccupied()){
				Debug.Log ("occupied");

				// if moveToDimple alreayd has a ball, check if we can jump
				moveToDimple = moveToDimple.getNeighborAtDirection(d);
				// valid jump?
				if(moveToDimple == null || moveToDimple.isOccupied()){
					return false;
				}

			}
		}
		CurrentDimple.toggleOccupied();
		// This is where we update position?

		CurrentDimple = moveToDimple;
		CurrentDimple.toggleOccupied();

		Vector3 newPos = this.transform.position;
		newPos.x = CurrentDimple.transform.position.x;
		newPos.z = CurrentDimple.transform.position.z;
		this.transform.position = newPos;

		return true;
	
	}
}
