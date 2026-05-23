using HarmonyLib;
using Mirror;

namespace ATLYSS_AdditionalFastTravel.HarmonyPatches;

// [HarmonyPatch(typeof(RecallPortal), nameof(RecallPortal.Init_PickupItem))]
// static class RecallPortalUpdateScenePatch
// {
//     private static void Prefix()
//     {
//         SceneData.currentScene = Player._mainPlayer._recalledMapInstance;
//     }
// }
//
// [HarmonyPatch(typeof(SkillBehavior_Recall), nameof(SkillBehavior_Recall.Init_SkillBehavior))]
// static class RecallSkillPatch
// {
//     private static void Prefix(SkillBehavior_Recall __instance)
//     {
//         SceneData.currentScene = __instance._sceneToTransfer;
//     }
// }
//
// [HarmonyPatch(typeof(WorldPortalManager), nameof(WorldPortalManager.Init_WorldPortalTeleport))]
// static class FastTravelPortalPatch
// {
//     private static void Prefix(WorldPortalEntry ____selectedWorldPortalEntry)
//     {
//         SceneData.currentScene = ____selectedWorldPortalEntry._scriptMapData._subScene;
//     }
// }
//
// [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.Cmd_InteractWithPortal))]
// static class NormalPortalPatch
// {
//     private static void Prefix(Portal _portal)
//     {
//         SceneData.currentScene = _portal._scenePortal._subScene;
//     }
// }
//
// [HarmonyPatch(typeof(AtlyssNetworkManager), nameof(AtlyssNetworkManager.OnStopClient))]
// static class ResetOnLeaveGamePatch
// {
//     private static void Postfix()
//     {
//         Logging.Message("Resetting current scene back to Sanctum");
//         SceneData.currentScene = "Assets/Scenes/00_zone_forest/_zone00_sanctum.unity";
//     }
// }

[HarmonyPatch(typeof(NetworkManager), nameof(NetworkManager.ClientChangeScene))]
static class ListenForSceneChanges
{
    private static void Prefix(string newSceneName, SceneOperation sceneOperation)
    {
        if (sceneOperation == SceneOperation.LoadAdditive)
        {
            SceneData.LastNetworkSceneLoaded = newSceneName;
        }
    }
}

[HarmonyPatch(typeof(ChatBehaviour), nameof(ChatBehaviour.Cmd_SendChatMessage))]
static class ListenForWarpCommandPatch
{
    private static void Usage(string msg) => Utils.ChatMsg($"<color=orange>Usage:</color> {msg}");
    
    private static bool Prefix(string _message)
    {
        var splitCmds = _message.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (splitCmds.Length == 0)
            return true;

        if (splitCmds[0] == "/warp")
        {
            if (splitCmds.Length == 1)
            {
                FastTravelActions.ListWarps();
            }

            if (splitCmds.Length == 2)
            {
                FastTravelActions.WarpToScene(splitCmds[1], "");
            }
            else if (splitCmds.Length == 3)
            {
                FastTravelActions.WarpAndGoto(splitCmds[1], splitCmds[2]);
            }
            else
            {
                Usage("\n  <color=orange>/warp [area]</color> - warp to map\n  <color=orange>/warp [area] [goto]</color> - warp and goto at the same time");
            }
            
            return false;
        }
        else if (splitCmds[0] == "/dungeon")
        {
            if (splitCmds.Length == 1)
            {
                FastTravelActions.ListWarps();
            }
            
            if (splitCmds.Length == 3)
            {
                FastTravelActions.WarpToScene(splitCmds[1], splitCmds[2]);
            }
            else
            {
                Usage("\n  <color=orange>/dungeon [area] [difficulty]</color> - warp to map with EASY / NORMAL / HARD difficulty");
            }
            
            return false;
        }
        else if (splitCmds[0] == "/goto")
        {
            if (splitCmds.Length == 1)
            {
                FastTravelActions.ListGotos();
            }
            
            if (splitCmds.Length == 2)
            {
                FastTravelActions.GoToLocation(splitCmds[1]);
            }
            else
            {
                Usage("\n  <color=orange>/goto [location]</color> - go to location in current map");
            }
            
            return false;
        }
            
        return true;
    }
}