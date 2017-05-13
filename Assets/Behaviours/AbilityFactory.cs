using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class AbilityFactory : MonoBehaviour
{
    public GameObject[] particle_object_prefabs;
    public GameObject[] projectile_object_prefabs;
    public AudioClip[] ability_sounds;

    private Dictionary<string, GameObject> particle_dictionary;
    private Dictionary<string, GameObject> projectile_dictionary;
    private Dictionary<string, AudioClip> audio_dictionary;

    private Dictionary<string, AbilityProperties> ability_properties_dictionary;


	void Start()
    {
        CreatePrefabDictionaries();
        EnumerateProperties();
	}


    // Transfers data from the arrays to dictionaries because the Inspector doesn't show dictionaries.
    void CreatePrefabDictionaries()
    {
        particle_dictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < particle_object_prefabs.Length; ++i)
        {
            string obj_name = particle_object_prefabs[i].name;
            obj_name = obj_name.Split(' ')[0];

            particle_dictionary.Add(obj_name, particle_object_prefabs[i]);
        }

        projectile_dictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < projectile_object_prefabs.Length; ++i)
        {
            string obj_name = projectile_object_prefabs[i].name;
            obj_name = obj_name.Split(' ')[0];

            projectile_dictionary.Add(obj_name, projectile_object_prefabs[i]);
        }

        audio_dictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < ability_sounds.Length; ++i)
        {
            string obj_name = ability_sounds[i].name;
            obj_name = obj_name.Split(' ')[0];

            audio_dictionary.Add(obj_name, ability_sounds[i]);
        }
    }
	

    // Read all ability properties from the JSON file and populate the abilities dictionary.
    void EnumerateProperties()
    {
        string file_name = Application.streamingAssetsPath + "/abilities.json";

        JsonData abilities_data = JsonMapper.ToObject(File.ReadAllText(file_name));
        ability_properties_dictionary = new Dictionary<string, AbilityProperties>();

        for (int index = 0; index < abilities_data.Count; ++index)
        {
            var elem = abilities_data[index];
            var type = (string)elem["class"];

            AbilityProperties properties = new AbilityProperties();

            if (elem.Keys.Contains("activate_effect"))
                properties.activate_effect = particle_dictionary[(string)elem["activate_effect"]];

            if (elem.Keys.Contains("audio_clip"))
                properties.audio_clip = audio_dictionary[(string)elem["audio_clip"]];

            if (elem.Keys.Contains("cooldown"))
                properties.cooldown = float.Parse(elem["cooldown"].ToString());

            if (elem.Keys.Contains("damage"))
                properties.damage = float.Parse(elem["damage"].ToString());

            if (elem.Keys.Contains("effect_radius"))
                properties.effect_radius = float.Parse(elem["effect_radius"].ToString());

            if (elem.Keys.Contains("knockback_force"))
                properties.knockback_force = float.Parse(elem["knockback_force"].ToString()) * 1000;

            if (elem.Keys.Contains("projectile"))
                properties.projectile = projectile_dictionary[(string)elem["projectile"]];

            if (elem.Keys.Contains("projectile_speed"))
                properties.projectile_speed = float.Parse(elem["projectile_speed"].ToString());

            if (elem.Keys.Contains("camera_shake_strength"))
                properties.camera_shake_strength = float.Parse(elem["camera_shake_strength"].ToString());

            if (elem.Keys.Contains("camera_shake_duration"))
                properties.camera_shake_duration = float.Parse(elem["camera_shake_duration"].ToString());

            ability_properties_dictionary.Add(type, properties);
        }
    }


    public void AssignLoadout(PlayerController player)
    {
        // Remove current loadout.
        foreach (Ability ability in player.GetComponents<Ability>())
        {
            Destroy(ability);
        }

        // JTODO -- need a way to fetch specific abilities..
                
        player.basic_ability = player.gameObject.AddComponent<AbilityLaser>();
        player.basic_ability.properties = ability_properties_dictionary["AbilityLaser"];

        player.special_ability = player.gameObject.AddComponent<AbilityShockwave>();
        player.special_ability.properties = ability_properties_dictionary["AbilityShockwave"];
    }

}
