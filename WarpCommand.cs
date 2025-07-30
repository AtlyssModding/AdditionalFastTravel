using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//Note:
//In singleplayer, Player gameObjects are in the Scene of the current loaded level.
//But in multiplayer, Player gameObjects are put in the 01_RootScene Scene.

// /warp AREA makes the player warp to the default spawn point of the given AREA.

namespace ATLYSS_Template
{
    public static class WarpCommand
    {
        
        public static void buildChatMessage(string inputMessage,bool useDividers=true)
        {
            string msg = ((useDividers ? "---\n" : "") + inputMessage + (useDividers ? "\n---" : ""));
            ChatBehaviour._current.New_ChatMessage(msg);
        }
        
        public static void ProcessCommand(string rawCommand)
        {
            String[] splitCmd = rawCommand.Split(' ');
            
            //Prevent usage of this command if player is in a dungeon to prevent cheesing
            if (SceneData.isInDungeon())
            {
                buildChatMessage("<color=orange>Cannot use this command while in a dungeon.</color>",false);
                return;
            }
            
            if (splitCmd.Length == 2)
            {
                if (splitCmd[1] == "*")
                {
                    string scenesString = String.Join("\n", SceneData.sceneNames.Keys.ToArray());
                    buildChatMessage("Available areas:\n" + scenesString + "\nUse <color=orange>/warp [Area name]</color> to warp to the given area.");
                    return;
                }
                else
                {
                    if (SceneData.sceneNames.ContainsKey(splitCmd[1]))
                    {
                        string scene = splitCmd[1];

                        //Check if the player is in the scene. If not, warp them to the default spawn point of the target area.
                        if (SceneData.currentScene == SceneData.sceneNames[scene])
                        {
                            buildChatMessage("<color=orange>You are already in the target area.</color>",false);
                            return;
                        }
                        else
                        {
                            //TP the player to the default spawn point for the current loaded scene.
                            //Spawns seem to be either called spawnPoint, startPoint or respawnPoint. Use one if the other can't be found.

                            string spawnPoint = "";
                            try
                            {
                                switch (SceneData.defaultSpawns[scene])
                                {
                                    case 0: { spawnPoint = "startPoint"; break; }
                                    case 1: { spawnPoint = "spawnPoint"; break; }
                                    case 2: { spawnPoint = "respawnPoint"; break; }
                                }
                            }
                            catch (KeyNotFoundException e)
                            {
                                Logging.Warn("Spawn name for area " + scene + " not found in data, using spawnPoint instead");
                                spawnPoint = "spawnPoint";
                            }

                            string scenePath = "Assets/Scenes/" + SceneData.sceneNames[scene] + ".unity";
                            SceneData.currentScene = SceneData.sceneNames[scene];

                            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._portalInteract,1.0f);
                            Player._mainPlayer.Cmd_SceneTransport(scenePath, spawnPoint,ZoneDifficulty.NORMAL);
                        }
                    }
                    else
                    {
                        buildChatMessage("<color=orange>ERROR</color>: Given area does not exist",false);
                    }

                    return;
                }
            }
            
            if (splitCmd.Length != 2)
            {
                buildChatMessage("Usage: <color=orange>/warp</color> [AREA] \n(Use <color=orange>/warp *</color> to view warpable areas)");
                return;
            }
        }
    }
}