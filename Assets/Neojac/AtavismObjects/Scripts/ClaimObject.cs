using UnityEngine;
using System.Collections;

public class ClaimObject : MonoBehaviour {

	int id;
	bool ready = false;
	Color initialColor;
	bool active = false;
	bool selected = false;
	Renderer[] renderers;
	Color[] initialColors;
	
	// Use this for initialization
	void Start () {
		if (renderer != null) {
			initialColor = renderer.material.color;
		} else {
			renderers = GetComponentsInChildren<Renderer>();
			initialColors = new Color[renderers.Length];
			for (int i = 0; i < renderers.Length; i++) {
				initialColors[i] = renderers[i].material.color;
			}
		}
		ready = true;
	}
	
	void OnMouseOver()
	{
		if (active)
			Highlight();
	}
	
	void OnMouseExit()
	{
		if (!selected)
			ResetHighlight();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Highlight() {
		if (!ready)
			Start ();
		if (renderer != null) {
			renderer.material.color = Color.cyan;
		} else {
			for (int i = 0; i < renderers.Length; i++) {
				renderers[i].material.color = Color.cyan;
			}
		}
	}
	
	public void ResetHighlight() {
		if (renderer != null) {
			renderer.material.color = initialColor;
		} else {
			for (int i = 0; i < renderers.Length; i++) {
				renderers[i].material.color = initialColors[i];
			}
		}
	}
	
	public int ID {
		set {
			id = value;
		}
		get {
			return id;
		}
	}
	
	public bool Active
	{
		set {
			active = value;
			if (!active && !selected) {
				if (renderer != null) 
					ResetHighlight();
			}
		}
		get {
			return active;
		}
	}
	
	public bool Selected
	{
		set {
			selected = value;
			if (selected) {
				Highlight();
			} else {
				ResetHighlight();
			}
		}
		get {
			return selected;
		}
	}
}
