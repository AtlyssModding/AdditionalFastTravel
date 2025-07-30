using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// /goto TELEPORT makes the player teleport to a given TELEPORT point in the area.

namespace ATLYSS_Template
{
    public static class GotoCommand
    {
        public static void buildChatMessage(string inputMessage,bool useDividers=true)
        {
            string msg = (useDividers ? "---\n" : "") + inputMessage + (useDividers ? "---\n" : "");
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
                string targetLocation = splitCmd[1];
                try
                {
                    Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._warp,1.0f);
                    Vector3 targetVector = SceneData.data[SceneData.currentScene][targetLocation];
                    Player._mainPlayer.gameObject.SetActive(false);
                    Player._mainPlayer.transform.position = targetVector;
                    Player._mainPlayer.gameObject.SetActive(true);

                    buildChatMessage("Moved to " + targetLocation + ".",false);
                }
                catch (KeyNotFoundException e)
                {
                    buildChatMessage("<color=orange>ERROR</color>: This goto point does not exist for this area.", false);
                }
            }
            else
            {
                try
                {
                    string scenesString = String.Join("\n", SceneData.data[SceneData.currentScene].Keys.ToArray());
                    buildChatMessage("Available goto points for this area:\n" + scenesString + "\nUse <color=orange>/goto [POINT]</color> to goto the given point.");
                }
                catch (KeyNotFoundException e)
                {
                    buildChatMessage("ERROR: No goto points were found for the current area (" + SceneData.currentScene + ")",false);
                    buildChatMessage("(Tell Clearwater to add some goto points for this area!)",false);
                }
            }
        }
    }
}