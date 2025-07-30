using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using PluginInfo = BepInEx.PluginInfo;

namespace ATLYSS_Template
{
    [BepInPlugin(pluginId, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginId = "clearwater.atlyss.additionalfasttravel";
        public const string pluginName = "AdditionalFastTravel";
        public const string pluginVersion = "1.0.0";
        
        //This method is called when your mod is first loaded. Use this to handle any startup & initialisation logic.
        private void Awake()
        {
            Logging.Warn("-- LOADING " + pluginName + "--");
            
            Harmony harmony = new Harmony(pluginId);
            harmony.PatchAll();
        }
        
        public void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }
    }

}