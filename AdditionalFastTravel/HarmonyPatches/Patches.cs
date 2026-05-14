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

[HarmonyPatch(typeof(ChatBehaviour), nameof(ChatBehaviour.Cmd_SendChatMessage))]
static class ListenForWarpCommandPatch
{
    private static bool Prefix(string _message)
    {
        if (_message.Contains("/warp"))
        {
            WarpCommand.ProcessCommand(_message);
            return false;
        }
        else if (_message.Contains("/goto"))
        {
            GotoCommand.ProcessCommand(_message);
            return false;
        }
            
        return true;
    }
}