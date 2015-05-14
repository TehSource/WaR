using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttachmentSockets {
	Root,
	LeftFoot,
	RightFoot,
	Pelvis,
	LeftHip,
	RightHip,
	MainHand,
	OffHand,
	Chest,
	Back,
	LeftShoulder,
	RightShoulder,
	Head,
	Mouth,
	LeftEye,
	RightEye,
	Overhead,
	MainWeapon,
	SecondaryWeapon
}

public class AtavismMobAppearance : MonoBehaviour {
	
	GameObject legs;
	GameObject chest;
	GameObject hands;
	GameObject feet;
	
	// Sockets for attaching weapons (and particles)
	public Transform mainHand;
	public Transform offHand;
	public Transform mainHandRest;
	public Transform offHandRest;
	public Transform head;
	public Transform leftShoulderSocket;
	public Transform rightShoulderSocket;
	
	// Sockets for particles
	public Transform rootSocket;
	public Transform leftFootSocket;
	public Transform rightFootSocket;
	public Transform pelvisSocket;
	public Transform leftHipSocket;
	public Transform rightHipSocket;
	public Transform chestSocket;
	public Transform backSocket;
	public Transform mouthSocket;
	public Transform leftEyeSocket;
	public Transform rightEyeSocket;
	public Transform overheadSocket;
	
	
	Dictionary<Transform, GameObject> attachedItems = new Dictionary<Transform, GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public Transform GetSocketTransform(AttachmentSockets slot) {
		switch (slot) {
		case AttachmentSockets.MainHand:
			if (mainHand != null)
				return mainHand;
			else
				return transform;
			break;
		case AttachmentSockets.OffHand:
			if (offHand != null)
				return offHand;
			else
				return transform;
			break;
		case AttachmentSockets.Head:
			if (head != null)
				return head;
			else
				return transform;
			break;
		case AttachmentSockets.LeftShoulder:
			if (leftShoulderSocket != null)
				return leftShoulderSocket;
			else
				return transform;
			break;
		case AttachmentSockets.RightShoulder:
			if (rightShoulderSocket != null)
				return rightShoulderSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Root:
			return transform;
			break;
		case AttachmentSockets.LeftFoot:
			if (leftFootSocket != null)
				return leftFootSocket;
			else
				return transform;
			break;
		case AttachmentSockets.RightFoot:
			if (rightFootSocket != null)
				return rightFootSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Pelvis:
			if (pelvisSocket != null)
				return pelvisSocket;
			else
				return transform;
			break;
		case AttachmentSockets.LeftHip:
			if (leftHipSocket != null)
				return leftHipSocket;
			else
				return transform;
			break;
		case AttachmentSockets.RightHip:
			if (rightHipSocket != null)
				return rightHipSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Chest:
			if (chest != null)
				return chestSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Back:
			if (backSocket != null)
				return backSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Mouth:
			if (mouthSocket != null)
				return mouthSocket;
			else
				return transform;
			break;
		case AttachmentSockets.LeftEye:
			if (leftEyeSocket != null)
				return leftEyeSocket;
			else
				return transform;
			break;
		case AttachmentSockets.RightEye:
			if (rightEyeSocket != null)
				return rightEyeSocket;
			else
				return transform;
			break;
		case AttachmentSockets.Overhead:
			if (overheadSocket != null)
				return overheadSocket;
			else
				return transform;
			break;
		case AttachmentSockets.MainWeapon:
			if (mainHand != null)
				return mainHand.GetChild(0).FindChild("socket");
			else
				return null;
			break;
		case AttachmentSockets.SecondaryWeapon:
			if (offHand != null)
				return offHand.GetChild(0).FindChild("socket");
			else
				return null;
			break;
		}
		return null;
	}
	
	void OnDestroy() {
		if (GetComponent<AtavismNode>()) {
			GetComponent<AtavismNode> ().RemoveObjectPropertyChangeHandler("weaponDisplayID", WeaponDisplayHandler);
			GetComponent<AtavismNode> ().RemoveObjectPropertyChangeHandler("weapon2DisplayID", Weapon2DisplayHandler);
			GetComponent<AtavismNode> ().RemoveObjectPropertyChangeHandler("legDisplayID", LegsDisplayHandler);
		}
	}
	
	void ObjectNodeReady () {
		GetComponent<AtavismNode> ().RegisterObjectPropertyChangeHandler ("legDisplayID", LegsDisplayHandler);
		GetComponent<AtavismNode> ().RegisterObjectPropertyChangeHandler ("weaponDisplayID", WeaponDisplayHandler);
		GetComponent<AtavismNode> ().RegisterObjectPropertyChangeHandler ("weapon2DisplayID", Weapon2DisplayHandler);
		
		if (GetComponent<AtavismNode>().PropertyExists("weaponDisplayID")) {
			Logger.LogDebugMessage("Got weapon display for: " + name);
			string displayID = (string)GetComponent<AtavismNode> ().GetProperty ("weaponDisplayID");
			SetWeaponDisplay(displayID);
		}
		if (GetComponent<AtavismNode>().PropertyExists("weapon2DisplayID")) {
			Logger.LogDebugMessage("Got weapon 2 display for: " + name);
			string displayID = (string)GetComponent<AtavismNode> ().GetProperty ("weapon2DisplayID");
			SetWeapon2Display(displayID);
		}
	}
	
	public void WeaponDisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got weapon display ID");
		string displayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		SetWeaponDisplay(displayID);
	}
	
	public void SetWeaponDisplay(string displayID) {
		// Remove existing item
		if (attachedItems.ContainsKey(mainHand)) {
			Destroy(attachedItems[mainHand]);
			attachedItems.Remove(mainHand);
		}
		if (displayID != null && displayID != "") {
			EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(displayID);
			GameObject weapon = (GameObject) Instantiate(display.model, mainHand.position, mainHand.rotation);
			weapon.transform.parent = mainHand;
			attachedItems.Add(mainHand, weapon);
		}
	}
	
	public void Weapon2DisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got weapon 2 display ID");
		string displayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		SetWeapon2Display(displayID);
	}
	
	public void SetWeapon2Display(string displayID) {
		// Remove existing item
		if (attachedItems.ContainsKey(offHand)) {
			Destroy(attachedItems[offHand]);
			attachedItems.Remove(offHand);
		}
		if (displayID != null && displayID != "") {
			EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(displayID);
			GameObject weapon2 = (GameObject) Instantiate(display.model, offHand.position, offHand.rotation);
			weapon2.transform.parent = offHand;
			attachedItems.Add(offHand, weapon2);
		}
	}
	
	public void ChestDisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got chest display ID");
		ObjectNode node = (ObjectNode)sender;
		string chestDisplayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(chestDisplayID);
		//Material mat = chest.GetComponent<SkinnedMeshRenderer>().material;
		//mat.SetTexture("_EquipmentTex", display.texture);
	}
	
	public void HandsDisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got hands display ID");
		ObjectNode node = (ObjectNode)sender;
		string handDisplayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(handDisplayID);
		//Material mat = hands.GetComponent<SkinnedMeshRenderer>().material;
		//mat.SetTexture("_EquipmentTex", display.texture);
	}
	
	public void LegsDisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got leg display ID");
		ObjectNode node = (ObjectNode)sender;
		string legsDisplayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(legsDisplayID);
		Material mat = legs.GetComponent<SkinnedMeshRenderer>().material;
		//legs.GetComponent<SkinnedMeshRenderer>().materials[1] = display.material;
		//mat.SetTexture("_EquipmentTex", display.texture);
	}
	
	public void FeetDisplayHandler(object sender, PropertyChangeEventArgs args) {
		Logger.LogDebugMessage("Got feet display ID");
		ObjectNode node = (ObjectNode)sender;
		string feetDisplayID = (string)GetComponent<AtavismNode> ().GetProperty (args.PropertyName);
		EquipmentDisplay display = ClientAPI.ScriptObject.GetComponent<Inventory>().LoadEquipmentDisplay(feetDisplayID);
		//Material mat = feet.GetComponent<SkinnedMeshRenderer>().material;
		//mat.SetTexture("_EquipmentTex", display.texture);
	}
}
