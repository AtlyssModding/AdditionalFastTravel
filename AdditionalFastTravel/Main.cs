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
        Logging.Info("-- LOADING " + ModInfo.NAME + "--");
            
        Harmony harmony = new Harmony(ModInfo.GUID);
        harmony.PatchAll();
    }

    private void LateUpdate()
    {
        if (!string.IsNullOrWhiteSpace(StoredGoto) && Player._mainPlayer && !Player._mainPlayer._bufferingStatus && DateTime.Now >= StoredGotoNotBefore)
        {
            FastTravelActions.GoToLocation(StoredGoto);
            StoredGoto = "";
        }
    }

    public static DateTime StoredGotoNotBefore { get; internal set; } = DateTime.Now;
    public static string StoredGoto { get; internal set; } = "";
}