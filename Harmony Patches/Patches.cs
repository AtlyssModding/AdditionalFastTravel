using HarmonyLib;
using Mirror;

namespace ATLYSS_Template.Harmony_Patches
{
    [HarmonyPatch(typeof(RecallPortal), "Init_PickupItem")]
    public static class RecallPortalUpdateScenePatch
    {
        [HarmonyPrefix]
        public static bool RecallPortalUpdateScene(NetworkIdentity _netID)
        {
            SceneData.currentScene = Player._mainPlayer._recalledMapInstance.Replace("Assets/Scenes/", "").Replace(".unity", "");
            return true;
        }
    }

    [HarmonyPatch(typeof(ScriptableSceneTransferCondition), "Init_ConditionEffect")]
    public static class RecallSkillPatch
    {
        [HarmonyPrefix]
        public static bool RecallSkillUpdateScene(StatusEntity _targetEntity, ConditionData _conDat)
        {
            SceneData.currentScene = "map_hub_sanctum";
            return true;
        }
    }

    [HarmonyPatch(typeof(WorldPortalManager), "Init_WorldPortalTeleport")]
    public static class FastTravelPortalPatch
    {
        [HarmonyPrefix]
        public static bool FastTravelPortalUpdateScene(WorldPortalEntry ____selectedWorldPortalEntry)
        {
            string newSceneName = ____selectedWorldPortalEntry._scriptMapData._subScene.Replace("Assets/Scenes/", "").Replace(".unity", "");
            SceneData.currentScene = newSceneName;
            return true;
        }
    }


    [HarmonyPatch(typeof(PlayerInteract), "Cmd_InteractWithPortal")]
    public static class NormalPortalPatch
    {
        [HarmonyPrefix]
        public static bool NormalPortalUpdateScene(Portal _portal, ZoneDifficulty _setDifficulty)
        {
            string newSceneName = _portal._scenePortal._subScene.Replace("Assets/Scenes/", "").Replace(".unity", "");
            SceneData.currentScene = newSceneName;
            
            return true;
        }
    }

    [HarmonyPatch(typeof(AtlyssNetworkManager), "OnStopClient")]
    public static class ResetOnLeaveGamePatch
    {
        [HarmonyPostfix]
        public static void ResetCurrentScene()
        {
            Logging.Message("Resetting current scene back to Sanctum");
            SceneData.currentScene = "map_hub_sanctum";
        }
    }
    
    [HarmonyPatch(typeof(ChatBehaviour), "Cmd_SendChatMessage")]
    public static class ListenForWarpCommandPatch
    {
        [HarmonyPrefix]
        public static bool WarpCommandHandler(string _message, ChatBehaviour.ChatChannel _chatChannel)
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
    
}