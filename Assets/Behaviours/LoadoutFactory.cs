using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;


public class LoadoutFactory : MonoBehaviour
{
    public GameObject[] projectile_object_prefabs;
    public AudioClip[] ability_sounds;
    public Sprite[] hats;

    private Dictionary<string, GameObject> projectile_dictionary;
    private Dictionary<string, AudioClip> audio_dictionary;
    private Dictionary<string, Sprite> hat_dictionary;

    private Dictionary<string, AbilityProperties> ability_properties_dictionary;

    private Dictionary<string, Loadout> starter_loadouts = new Dictionary<string, Loadout>();
    private Dictionary<string, Loadout> general_loadouts = new Dictionary<string, Loadout>();
    private Dictionary<string, Loadout> misc_loadouts = new Dictionary<string, Loadout>();
    private Dictionary<string, Loadout> titan_loadouts = new Dictionary<string, Loadout>();


	void Start()
    {
        CreatePrefabDictionaries();
        EnumerateProperties();
        EnumerateLoadouts();
	}


    // Transfers data from the arrays to dictionaries because the Inspector doesn't show dictionaries.
    void CreatePrefabDictionaries()
    {
        projectile_dictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < projectile_object_prefabs.Length; ++i)
        {
            string obj_name = projectile_object_prefabs[i].name.Substring(0);

            projectile_dictionary.Add(obj_name, projectile_object_prefabs[i]);
        }

        audio_dictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < ability_sounds.Length; ++i)
        {
            string obj_name = ability_sounds[i].name.Substring(0);

            audio_dictionary.Add(obj_name, ability_sounds[i]);
        }

        hat_dictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < hats.Length; ++i)
        {
            Sprite spr = hats[i];

            hat_dictionary.Add(spr.name.Substring(0), spr);
        }
    }
	

    // Read all ability properties from the JSON file and populate the abilities dictionary.
    void EnumerateProperties()
    {
        string file_name = Application.streamingAssetsPath + "/abilities.json";

        JsonData abilities_data = JsonMapper.ToObject(File.ReadAllText(file_name));
        ability_properties_dictionary = new Dictionary<string, AbilityProperties>();

        var keys = JHelper.GetObjectKeys(abilities_data);

        for (int index = 0; index < abilities_data.Count; ++index)
        {
            var elem = abilities_data[index];
            string projectile_name = keys[index];

            AbilityProperties properties = new AbilityProperties();

            if (elem.Keys.Contains("projectile"))
                properties.projectile = projectile_dictionary[(string)elem["projectile"]];

            if (elem.Keys.Contains("audio_clip"))
                properties.audio_clip = audio_dictionary[(string)elem["audio_clip"]];

            if (elem.Keys.Contains("cooldown"))
                properties.cooldown = float.Parse(elem["cooldown"].ToString());

            if (elem.Keys.Contains("lifetime"))
                properties.lifetime = float.Parse(elem["lifetime"].ToString());

            if (elem.Keys.Contains("damage"))
                properties.damage = int.Parse(elem["damage"].ToString());

            if (elem.Keys.Contains("effect_radius"))
                properties.effect_radius = float.Parse(elem["effect_radius"].ToString());

            if (elem.Keys.Contains("knockback_force"))
                properties.knockback_force = float.Parse(elem["knockback_force"].ToString()) * 1000;

            if (elem.Keys.Contains("stun_duration"))
                properties.stun_duration = float.Parse(elem["stun_duration"].ToString());

            if (elem.Keys.Contains("projectile_speed"))
                properties.projectile_speed = float.Parse(elem["projectile_speed"].ToString());

            if (elem.Keys.Contains("camera_shake_strength"))
                properties.camera_shake_strength = float.Parse(elem["camera_shake_strength"].ToString());

            if (elem.Keys.Contains("camera_shake_duration"))
                properties.camera_shake_duration = float.Parse(elem["camera_shake_duration"].ToString());

            ability_properties_dictionary.Add(projectile_name, properties);
        }
    }


    void EnumerateLoadouts()
    {
        string file_name = Application.streamingAssetsPath + "/loadouts.json";

        JsonData loadouts_data = JsonMapper.ToObject(File.ReadAllText(file_name));

        var current_array = loadouts_data["Starter"];
        for (int i = 0; i < current_array.Count; ++i)
        {
            PushLoadout(current_array[i], starter_loadouts);
        }

        current_array = loadouts_data["General"];
        for (int i = 0; i < current_array.Count; ++i)
        {
            PushLoadout(current_array[i], general_loadouts);
        }

        current_array = loadouts_data["Misc"];
        for (int i = 0; i < current_array.Count; ++i)
        {
            PushLoadout(current_array[i], misc_loadouts);
        }

        current_array = loadouts_data["Titan"];
        for (int i = 0; i < current_array.Count; ++i)
        {
            PushLoadout(current_array[i], titan_loadouts);
        }
    }


    void PushLoadout(JsonData elem, Dictionary<string, Loadout> dictionary)
    {
            Loadout loadout = new Loadout();

            loadout.name = (string)elem["name"];
            string hat_name = loadout.name + "Hat";

            if (hat_dictionary.ContainsKey(hat_name))
                loadout.hat = hat_dictionary[hat_name];

            loadout.basic_ability_name = (string)elem["basic"];
            loadout.special_ability_name = (string)elem["special"];

            dictionary.Add(loadout.name, loadout);
    }


    Loadout FindLoadout(string loadout_name)
    {
        if (starter_loadouts.ContainsKey(loadout_name))
        {
            return starter_loadouts[loadout_name];
        }
        else if (general_loadouts.ContainsKey(loadout_name))
        {
            return general_loadouts[loadout_name];
        }
        else if (misc_loadouts.ContainsKey(loadout_name))
        {
            return misc_loadouts[loadout_name];
        }
        else if (titan_loadouts.ContainsKey(loadout_name))
        {
            return titan_loadouts[loadout_name];
        }
        else
        {
            return null;
        }
    }


    public void AssignLoadout(PlayerController player, string loadout_name)
    {
        // Remove current loadout.
        foreach (Ability ability in player.GetComponents<Ability>())
        {
            Destroy(ability);
        }

        Loadout loadout = FindLoadout(loadout_name);
        player.hat.sprite = loadout.hat;
        player.loadout_name = loadout_name;

        // Hacky way to enable broken particle for Broken loadout.
        player.broken_particle.SetActive(loadout_name == "Broken");

        // Add basic ability.
        player.basic_ability = player.gameObject.AddComponent<Ability>();
        player.basic_ability.properties = ability_properties_dictionary[loadout.basic_ability_name];

        // Add special ability.
        player.special_ability = player.gameObject.AddComponent<Ability>();
        player.special_ability.properties = ability_properties_dictionary[loadout.special_ability_name];
    }


    public void AssignRandomLoadout(PlayerController player)
    {
        List<string> loadout_names = new List<string>(general_loadouts.Keys);
        string new_loadout_name;;

        do
        {
            new_loadout_name = loadout_names[Random.Range(0, loadout_names.Count)];
        } while (new_loadout_name == player.loadout_name);

        AssignLoadout(player, new_loadout_name);
    }


    public AudioClip GetAudioClip(string clip_name)
    {
        if (audio_dictionary.ContainsKey(clip_name))
            return audio_dictionary[clip_name];

        return null;
    }

}
