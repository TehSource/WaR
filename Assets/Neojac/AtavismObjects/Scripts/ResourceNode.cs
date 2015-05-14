using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HarvestType {
	Axe,
	Pickaxe,
	None
}

public class ResourceNode : MonoBehaviour {

	public int id = -1;
	public List<ResourceDrop> resources;
	public int resourceCount = 1;
	int currentResourceCount;
	public HarvestType harvestTool;
	bool toolMustBeEquipped = true;
	public int skillType = 0;
	public int reqSkillLevel = 0;
	public int skillLevelMax = 0;
	public float cooldown = 2;
	float cooldownEnds;
	public float refreshDuration = 60;
	public GameObject harvestCoordEffect;
	public GameObject activateCoordEffect;
	public GameObject deactivateCoordEffect;

	Color initialColor;
	bool active = true;
	bool selected = false;
	Renderer[] renderers;
	Color[] initialColors;

	// Use this for initialization
	void Start () {
		cooldownEnds = Time.time;
		currentResourceCount = resourceCount;
		gameObject.AddComponent<AtavismNode>();
		GetComponent<AtavismNode>().AddLocalProperty("harvestType", harvestTool);
		GetComponent<AtavismNode>().AddLocalProperty("targetable", false);
		GetComponent<AtavismNode>().AddLocalProperty("active", active);

		if (renderer != null) {
			initialColor = renderer.material.color;
		} else {
			renderers = GetComponentsInChildren<Renderer>();
			initialColors = new Color[renderers.Length];
			for (int i = 0; i < renderers.Length; i++) {
				initialColors[i] = renderers[i].material.color;
			}
		}
		
		ClientAPI.ScriptObject.GetComponent<Crafting>().RegisterResourceNode(this);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void HarvestResource() {
		if (Time.time < cooldownEnds) {
			// Send error message
			string[] args = new string[1];
			args[0] = "You cannot perform that action yet.";
			EventSystem.DispatchEvent("ERROR_MESSAGE", args);
		} else {
			Dictionary<string, object> props = new Dictionary<string, object> ();
			props.Add ("resourceID", id);
			NetworkAPI.SendExtensionMessage (ClientAPI.GetPlayerOid(), false, "crafting.HARVEST_RESOURCE", props);
			// Check skill level
			/*if (reqSkillLevel > 0 && skillType > 0) {
				int currentSkillLevel = ClientAPI.ScriptObject.GetComponent<Skills>().GetPlayerSkillLevel(skillType);
				if (currentSkillLevel < reqSkillLevel) {
					string skillName = ClientAPI.ScriptObject.GetComponent<Skills>().GetSkillByID(skillType).name;
					string[] args = new string[1];
					args[0] = "Requires level " + reqSkillLevel + " " + skillName;
					EventSystem.DispatchEvent("ERROR_MESSAGE", args);
					return;
				}
			}
			// Check weapon
			if (harvestTool != HarvestType.None) {
				if (!ClientAPI.GetPlayerObject().PropertyExists("equipType")) {
					string[] args = new string[1];
					string message = "";
					if (harvestTool == HarvestType.Axe) {
						message = "You need an axe equipped to chop wood from trees.";
					} else {
						message = "You need a pickaxe equipped to mine minerals";
					}
					args[0] = message;
					EventSystem.DispatchEvent("ERROR_MESSAGE", args);
					return;
				}
				string equipType = (string)ClientAPI.GetPlayerObject().GetProperty("equipType");
				if (harvestTool == HarvestType.Axe && equipType != "Axe") {
					string[] args = new string[1];
					args[0] = "You need an axe equipped to chop wood from trees.";
					EventSystem.DispatchEvent("ERROR_MESSAGE", args);
					return;
				} else if (harvestTool == HarvestType.Pickaxe && equipType != "Pickaxe") {
					string[] args = new string[1];
					args[0] = "You need a pickaxe equipped to mine minerals";
					EventSystem.DispatchEvent("ERROR_MESSAGE", args);
					return;
				}
			}*/
			cooldownEnds = Time.time + cooldown;
			/*for (int i = 0; i < dropChances.Count; i++) {
				if (roll < dropChances[i]) {
					
					props = new Dictionary<string, object> ();
					props.Add ("coordEffect", coordEffect.name);
					props.Add ("hasTarget", false);
					NetworkAPI.SendExtensionMessage (ClientAPI.GetPlayerOid(), false, "ao.PLAY_COORD_EFFECT", props);
					currentResourceCount--;
				}
			}*/
			/*int itemID = itemGenerated.GetComponent<AtavismInventoryItem>().TemplateId;
			NetworkAPI.SendTargetedCommand (ClientAPI.GetPlayerOid(), "/generateItem " + itemID);
			cooldownEnds = Time.time + cooldown;
			resourceCount--;
			if (resourceCount == 0) {
				GetComponent<AtavismNode>().RemoveLocalProperty("harvestType");

			}*/
		}
	}

	public void Highlight() {
		if (renderer != null) {
			renderer.material.color = Color.red;
		} else {
			for (int i = 0; i < renderers.Length; i++) {
				renderers[i].material.color = Color.red;
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
	}
	
	public bool ToolMustBeEquipped {
		get {
			return toolMustBeEquipped;
		}
		set {
			toolMustBeEquipped = value;
		}
	}
	
	public bool Active {
		get {
			return active;
		} 
		set {
			active = value;
			GetComponent<AtavismNode>().AddLocalProperty("active", active);
			if (active) {
				if (GetComponent<MeshRenderer>() != null) {
					GetComponent<MeshRenderer>().enabled = true;
				}
			} else {
				if (GetComponent<MeshRenderer>() != null) {
					GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}
	}
}
