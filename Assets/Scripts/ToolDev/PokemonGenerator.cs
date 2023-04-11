using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class PokemonGenerator : EditorWindow
{
    bool showBaseSettingsArea = true;
    bool showVisualsArea = true;
    bool baseStatsArea = true;
    bool showLearnableAttacksArea = true;
    bool showDefaultAttacks = true;
    bool showEvolves = true;

    Vector2 scrollPosition;

    UnityEngine.Object frontSprite;
    UnityEngine.Object backSprite;

    const float MINSIZE_X = 425f;
    const float MINSIZE_Y = 200f;

    /* Variables pour afficher list */

    ScriptableLearnableAttacks LearnableAttack;
    ScriptableEvolves Evolves;

    /* Variables pour créer le nouveau pokémon */
    private int newPokemonId;
    private string newPokemonName;
    private int newPokemonHealthPoints;
    private int newPokemonAttack;
    private int newPokemonDefense;
    private int newPokemonSpeed;
    private int newPokemonSpecialAttack;
    private int newPokemonSpecialDefense;
    private Sprite newPokemonFrontSprite;
    private Sprite newPokemonBackSprite;
    private Attack[] newPokemonDefaultAttacks = new Attack[4];
    private List<PokemonData> newPokemonEvolves = new List<PokemonData>();
    private List<ScriptableLearnableAttacks.LearnableAttackStruct> newPokemonLearnableAttacks;

    [MenuItem("Window/Pokemon generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PokemonGenerator)).Show();        
    }

    void OnGUI()
    {
        // Début de l'area englobant tout, permettant d'avoir une taille min et max par rapport à la window.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width));
        #region BASE SETTINGS
        /* BASE SETTINGS AREA */
        showBaseSettingsArea = EditorGUILayout.BeginFoldoutHeaderGroup(showBaseSettingsArea, "Base Settings");

        if (showBaseSettingsArea) // Si l'area est dépliée :
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID :");
            newPokemonId = EditorGUILayout.IntField(newPokemonId);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name :");
            newPokemonName = GUILayout.TextField(newPokemonName);
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region VISUALS
        /* BASE SETTINGS AREA */
        showVisualsArea = EditorGUILayout.BeginFoldoutHeaderGroup(showVisualsArea, "Visuals");

        if (showVisualsArea)
        {
            EditorGUILayout.BeginHorizontal();
                #region FRONTSPRITE
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("Front Sprite :");
                        frontSprite = EditorGUILayout.ObjectField(frontSprite, typeof(Sprite), false);
                        newPokemonFrontSprite = (Sprite)frontSprite;

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Label("", GUILayout.Height(50), GUILayout.Width(50));
                    if (frontSprite != null)
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), AssetPreview.GetAssetPreview(frontSprite));

                EditorGUILayout.EndVertical();
                #endregion
                GUILayout.Space(10);
                #region BACKSPRITE
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("Back Sprite :");
                        backSprite = EditorGUILayout.ObjectField(backSprite, typeof(Sprite), false);
                        newPokemonBackSprite = (Sprite)backSprite;

            EditorGUILayout.EndHorizontal();

                    GUILayout.Label("", GUILayout.Height(50), GUILayout.Width(50));
                    if (backSprite != null)
                        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), AssetPreview.GetAssetPreview(backSprite));

                EditorGUILayout.EndVertical();
                #endregion
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region BASE STATS
        baseStatsArea = EditorGUILayout.BeginFoldoutHeaderGroup(baseStatsArea, "Base Stats");

        if (baseStatsArea) // Si l'area est dépliée :
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("HP :");
            newPokemonHealthPoints = EditorGUILayout.IntField(newPokemonHealthPoints);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ATK :");
            newPokemonAttack = EditorGUILayout.IntField(newPokemonAttack);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("DEF :");
            newPokemonDefense = EditorGUILayout.IntField(newPokemonDefense);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPD :");
            newPokemonSpeed = EditorGUILayout.IntField(newPokemonSpeed);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPE.ATK :");
            newPokemonSpecialAttack = EditorGUILayout.IntField(newPokemonSpecialAttack);
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPE.DEF :");
            newPokemonSpecialDefense = EditorGUILayout.IntField(newPokemonSpecialDefense);
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        #endregion

        #region LEARNABLE ATTACKS
        /* ATTACKS AREA */
        showLearnableAttacksArea = EditorGUILayout.BeginFoldoutHeaderGroup(showLearnableAttacksArea, "Learnable DefaultAttacks");

        if (showLearnableAttacksArea) // Si l'area est dépliée :
        {
            ShowLearnableAttacks();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region Default Attacks
        /* DEFAULT ATTACKS AREA */
        showDefaultAttacks = EditorGUILayout.BeginFoldoutHeaderGroup(showDefaultAttacks, "Default DefaultAttacks");

        if (showDefaultAttacks)
        {
            ShowDefaultAttacks();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        #region Evolves
        /* DEFAULT ATTACKS AREA */
        showEvolves = EditorGUILayout.BeginFoldoutHeaderGroup(showEvolves, "Evolves");

        if (showEvolves)
        {
            ShowEvolves();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        GUILayout.Space(50);

        if (GUILayout.Button("Generate Pokemon"))
        {
            GeneratePokemon();
        }

        GUILayout.EndScrollView();
    }

    #region Show Functions
    void ShowLearnableAttacks()
    {
        if (LearnableAttack == null)
        {
            LearnableAttack = CreateInstance<ScriptableLearnableAttacks>();
        }

        for (int i = 0; i < LearnableAttack.LearnableAttacksStructList.Count; i++)
        {
            ScriptableLearnableAttacks.LearnableAttackStruct newLearnableAttack = LearnableAttack.LearnableAttacksStructList[i];
            EditorGUILayout.BeginHorizontal();
            newLearnableAttack.attack = (Attack)EditorGUILayout.ObjectField(newLearnableAttack.attack, typeof(Attack), false);
            newLearnableAttack.level = EditorGUILayout.IntSlider("Level", newLearnableAttack.level, 1, 100);
            LearnableAttack.LearnableAttacksStructList[i] = newLearnableAttack;

            if (GUILayout.Button("Remove LearnableAttack"))
            {
                LearnableAttack.RemoveLearnableAttack(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Add Attack"))
        {
            LearnableAttack.AddLearnableAttack();
        }

        newPokemonLearnableAttacks = LearnableAttack.LearnableAttacksStructList;
    }

    void ShowDefaultAttacks()
    {
        newPokemonDefaultAttacks[0] = (Attack)EditorGUILayout.ObjectField(newPokemonDefaultAttacks[0], typeof(Attack), false);
        newPokemonDefaultAttacks[1] = (Attack)EditorGUILayout.ObjectField(newPokemonDefaultAttacks[1], typeof(Attack), false);
        newPokemonDefaultAttacks[2] = (Attack)EditorGUILayout.ObjectField(newPokemonDefaultAttacks[2], typeof(Attack), false);
        newPokemonDefaultAttacks[3] = (Attack)EditorGUILayout.ObjectField(newPokemonDefaultAttacks[3], typeof(Attack), false);
    }

    void ShowEvolves()
    {
        if (Evolves == null)
        {
            Evolves = CreateInstance<ScriptableEvolves>();
        }

        for (int i = 0; i < Evolves.EvolvesStructList.Count; i++)
        {
            ScriptableEvolves.EvolveStruct newEvolve = Evolves.EvolvesStructList[i];
            EditorGUILayout.BeginHorizontal();
            newEvolve.pokemon = (PokemonData)EditorGUILayout.ObjectField(newEvolve.pokemon, typeof(PokemonData), false);
            newEvolve.level = EditorGUILayout.IntSlider("Level", newEvolve.level, 2, 100);
            Evolves.EvolvesStructList[i] = newEvolve;

            if (GUILayout.Button("Remove Evolve"))
            {
                Evolves.RemoveEvolve(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Add Evolve"))
        {
            Evolves.AddEvolve();
        }

        newPokemonLearnableAttacks = LearnableAttack.LearnableAttacksStructList;
    }
    #endregion

    #region Reset functions

    void ResetValues()
    {
        ResetBasicStats();
        ResetSprites();
        ResetDefaultAttacks();
        ResetLearnableAttacks();
        ResetEvolves();

        if (LearnableAttack)
            LearnableAttack = null;
    }

    void ResetBasicStats()
    {
        newPokemonId = 0;
        newPokemonName = "";
        newPokemonHealthPoints = 0;
        newPokemonAttack = 0;
        newPokemonDefense = 0;
        newPokemonSpeed = 0;
        newPokemonSpecialAttack = 0;
        newPokemonSpecialDefense = 0;
    }

    void ResetSprites()
    {
        frontSprite = null;
        backSprite = null;
        newPokemonFrontSprite = null;
        newPokemonBackSprite = null;
    }

    void ResetLearnableAttacks()
    {
        newPokemonLearnableAttacks = null;
    }

    void ResetDefaultAttacks()
    {
        for (int i = 0; i < newPokemonDefaultAttacks.Length; i++)
        {
            if (newPokemonDefaultAttacks[0])
            {
                newPokemonDefaultAttacks[0] = null;
            }
        }
    }

    void ResetEvolves()
    {
        if (newPokemonEvolves != null)
            newPokemonEvolves = null;
    }

    #endregion

    #region Generation Functions
    void GeneratePokemon()
    {
        if (!GenerateLogic())
        {
            return;
        }

        PokemonData newPokemon = CreateInstance<PokemonData>();

        newPokemon.Id = newPokemonId;
        newPokemon.Name = newPokemonName;
        newPokemon.Level = 1;
        newPokemon.HealthPoints = newPokemonHealthPoints;
        newPokemon.Attack = newPokemonAttack;
        newPokemon.Defense = newPokemonDefense;
        newPokemon.Speed = newPokemonSpeed;
        newPokemon.SpecialAttack = newPokemonSpecialAttack;
        newPokemon.SpecialDefense = newPokemonSpecialDefense;
        newPokemon.FrontSprite = newPokemonFrontSprite;
        newPokemon.BackSprite = newPokemonBackSprite;
        newPokemon.DefaultAttacks[0] = newPokemonDefaultAttacks[0];
        newPokemon.DefaultAttacks[1] = newPokemonDefaultAttacks[1];
        newPokemon.DefaultAttacks[2] = newPokemonDefaultAttacks[2];
        newPokemon.DefaultAttacks[3] = newPokemonDefaultAttacks[3];
        //newPokemon.Evolves = newPokemonEvolves;
        newPokemon.LearnableAttacks = newPokemonLearnableAttacks;

        AssetDatabase.CreateAsset(newPokemon, "Assets/Resources/ScriptableObjects/Pokemons/" + newPokemonName + ".asset");
        AssetDatabase.SaveAssets();
        // Faire le reset ici pour une meilleur UX
        ResetValues();
    }

    bool GenerateLogic()
    {
        if (newPokemonName == "")
        {
            return false;
        }

        if (newPokemonHealthPoints == 0)
        {
            return false;
        }

        if (newPokemonAttack == 0)
        {
            return false;
        }

        if (newPokemonSpeed == 0)
        {
            return false;
        }

        if (newPokemonSpecialAttack == 0)
        {
            return false;
        }

        if (newPokemonFrontSprite == null)
        {
            return false;
        }

        if (newPokemonBackSprite == null)
        {
            return false;
        }

        if (newPokemonLearnableAttacks.Count == 0)
        {
            return false;
        }
        else
        {
            if (IsOneOfLearnableAttacksEmpty())
            {
                return false;
            }
        }

        if (IsDefaultAttacksEmpty())
        {
            return false;
        }


        return true;
    }

    bool IsOneOfLearnableAttacksEmpty()
    {
        bool result = false;

        for (int i = 0; i < newPokemonLearnableAttacks.Count; i++)
        {
            if (newPokemonLearnableAttacks[i].attack == null)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    bool IsDefaultAttacksEmpty()
    {
        bool result = true;

        for (int i = 0; i < newPokemonDefaultAttacks.Length; i++)
        {
            if (newPokemonDefaultAttacks[i] != null)
            {
                result = false;
                break;
            }
        }

        return result;
    }
    #endregion
}
