using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClaimObjectState : MonoBehaviour
{
	
	public List<GameObject> coordEffects;
	string currentState = "";
	int claimID = -1;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnClick ()
	{
		int nextPos = 0;
		foreach (GameObject coordEffect in coordEffects) {
			nextPos++;
			if (coordEffect.name == currentState) {
				if (nextPos == coordEffects.Count) {
					nextPos = 0;
				}
				currentState = coordEffects[nextPos].name;
				Dictionary<string, object> props = new Dictionary<string, object>();
				props.Add("action", "state");
				props.Add("claimID", claimID);
				props.Add("objectID", GetComponent<ClaimObject>().ID);
				props.Add("state", currentState);
				NetworkAPI.SendExtensionMessage(ClientAPI.GetPlayerOid(), false, "voxel.EDIT_CLAIM_OBJECT", props);
			}
		}
	}
	
	public void StateUpdated (string state) {
		if (state == null || state == "null" || state == currentState)
			return;
		currentState = state;
		Dictionary<string, object> props = new Dictionary<string, object>();
		props["gameObject"] = gameObject;
		CoordinatedEffectSystem.ExecuteCoordinatedEffect(currentState, props);
	}
	
	public void SetID(int ID) 
	{
		claimID = ID;
	}
}
