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

public class PokemonGenerator : EditorWindow
{
    bool showBaseSettingsArea = true;
    bool showVisualsArea = true;
    bool baseStatsArea = true;
    bool showAttacksArea = true;

    UnityEngine.Object frontSprite;
    UnityEngine.Object backSprite;

    const float MINSIZE_X = 425f;
    const float MINSIZE_Y = 200f;

    /* Toutes les variables pour afficher list */

    ScriptableLearnableAttacks learnableAttackSo;



    [MenuItem("Window/Pokemon generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PokemonGenerator)).Show();  
       
    }

    void OnGUI()
    {
        // Début de l'area englobant tout, permettant d'avoir une taille min et max par rapport à la window.
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        #region BASE SETTINGS
        /* BASE SETTINGS AREA */
        showBaseSettingsArea = EditorGUILayout.BeginFoldoutHeaderGroup(showBaseSettingsArea, "Base Settings");

        if (showBaseSettingsArea) // Si l'area est dépliée :
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Name :");
            GUILayout.TextField("");
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
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ATK :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("DEF :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPD :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPE.ATK :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SPE.DEF :");
            GUILayout.TextField("");
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        #endregion

        /* ATTACKS AREA */
        showAttacksArea = EditorGUILayout.BeginFoldoutHeaderGroup(showAttacksArea, "Attacks");

        if (showAttacksArea) // Si l'area est dépliée :
        {
            //SerializedObject so = new SerializedObject((Object)learnableAttacks);

            //EditorGUILayout.PropertyField(so.FindProperty("learnableAttacks"), new GUIContent("My list here"), true);
        }

        // Fin de l'area englobant tout
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        EditorGUILayout.EndFoldoutHeaderGroup();

        /*if (!maximized)
        {
            maxSize = minSize * 4;
            minSize = new Vector2(MINSIZE_X + 25, MINSIZE_Y + 25);
        }*/

        if (!learnableAttackSo)
            learnableAttackSo = CreateInstance<ScriptableLearnableAttacks>();

        SerializedObject so = new SerializedObject(learnableAttackSo);

        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(so.FindProperty("attacks"));
        EditorGUILayout.PropertyField(so.FindProperty("attackLevels"));
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Add Attack"))
        {

        }

        GUILayout.EndHorizontal();

    }
}
