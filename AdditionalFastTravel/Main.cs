using BepInEx;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace ATLYSS_AdditionalFastTravel;

[BepInPlugin(ModInfo.GUID, ModInfo.NAME, ModInfo.VERSION)]
public class Main : BaseUnityPlugin
{
    //This method is called when your mod is first loaded. Use this to handle any startup & initialisation logic.
    private void Awake()
    {
        Logging.Warn("-- LOADING " + ModInfo.NAME + "--");
            
        Harmony harmony = new Harmony(ModInfo.GUID);
        harmony.PatchAll();
    }
        
    public void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
}