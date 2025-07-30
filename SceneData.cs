using System.Collections.Generic;
using UnityEngine;

namespace ATLYSS_Template
{
    public static class SceneData
    {
        public static string currentScene = "map_hub_sanctum";

        public static List<string> dungeonScenes = new List<string>()
        {
            "map_dungeon00_sanctumCatacombs",
            "map_dungeon01_crescentGrove"
        };

        public static bool isInDungeon()
        {
            return dungeonScenes.Contains(currentScene);
        }
        
        public static Dictionary<string, int> defaultSpawns = new Dictionary<string, int>()
        {
            //0 = "startPoint" - 1 = "respawnPoint"
            { "Sanctum", 0 },
            { "EffoldTerrace", 0 },
            { "TuulValley", 0 },
            { "SanctumCourtyard", 0 },
            { "CrescentRoad", 0}
        };
        
        
        public static Dictionary<string, string> sceneNames = new Dictionary<string, string>()
        {
            //Hub areas
            { "Sanctum", "map_hub_sanctum" },
            { "SanctumCourtyard", "map_zone00_sanctumCourtyard" },
            { "OuterSanctum", "map_zone00_outerSanctum" },
            
            { "EffoldTerrace", "map_zone00_effoldTerrace" },
            { "TuulValley", "map_zone00_tuulValley" },
            { "CrescentRoad", "map_zone00_crescentKeep" },
            
            { "GateOfTheMoon", "map_zone00_gateOfTheMoon" },
            { "WallOfTheStars", "map_hub_wallOfTheStars" },
            { "TrialOfTheStars", "map_zone00_starwall_A1" },
            
            { "SanctumArena", "map_pvp_sanctumArena"},
            { "CatacombsArena", "map_pvp_catacombsArena"}
            
        };


        public static Dictionary<string, Dictionary<string,Vector3>> data = new Dictionary<string, Dictionary<string,Vector3>>()
        {
            {
                "map_hub_sanctum", new Dictionary<string,Vector3>()
                {
                    {"spawn", new Vector3(8.4f,7.3f,-16.1f)},
                    {"shop", new Vector3(200f,11f,-110f)},
                    {"enchanting", new Vector3(310f,11f,-281f)},
                    {"barracks", new Vector3(-160f,29f,-600f)},
                    {"lake", new Vector3(500f,43f,90f)},
                }
            },
            {
                "map_zone00_outerSanctum", new Dictionary<string,Vector3>()
                {
                    {"sanctumPortal", new Vector3(0f,-45f,-295f)},
                    {"effoldTerracePortal", new Vector3(-342f,6f,926f)},
                    {"crescentRoadPortal", new Vector3(570f,20f,1295f)},
                    {"tuulValleyPortal", new Vector3(512f,76f,660f)},
                    {"catacombsEntrance", new Vector3(300f,80f,2245f)},
                    {"catacombsPVPPortal", new Vector3(300f,80f,2310f)},
                    {"catacombsDungeonPortal", new Vector3(75f,62f,2245f)},
                    {"catacombsMerchants", new Vector3(-10f,0f,1680f)},
                    {"catacombsBossMerchants", new Vector3(135f,81f,2580f)},
                    {"fishingPond", new Vector3(460f,45f,1765f)},
                }
            },
            
            {
                "map_zone00_effoldTerrace", new Dictionary<string,Vector3>()
                {
                    {"outerSanctumPortal", new Vector3(-195f,20f,-275f)},
                    {"centerArea", new Vector3(20f,18f,45f)},
                }
            },
            {
                "map_zone00_tuulValley", new Dictionary<string,Vector3>()
                {
                    {"outerSanctumPortal", new Vector3(-460f,30f,385f)},
                    {"tuulEnclaveEntrance", new Vector3(-750f,30f,40f)},
                    {"fastTravelPortal", new Vector3(25f,12f,-715f)},
                    {"rageboarCastleEntrance", new Vector3(-37f,52f,-1390f)},
                    {"rageboarCastleEnd", new Vector3(-50f,143f,-2690f)},
                }
            },
            {
                "map_zone00_sanctumCourtyard", new Dictionary<string,Vector3>()
                {
                    {"sanctumPortal", new Vector3(-15f,18f,-230f)},
                    {"courtyardBack", new Vector3(0f,2f,385f)},
                }
            },
            {
                "map_zone00_crescentKeep", new Dictionary<string,Vector3>()
                {
                    {"outerSanctumPortal", new Vector3(650f,47f,990f)},
                    {"keepEntrance", new Vector3(-150f,55f,235f)},
                    {"groveDungeonLobby", new Vector3(-1265f,225f,575f)},
                    {"groveDungeonBossMerchants", new Vector3(-1265f,225f,655f)},
                    {"gateOftheMoonPortal", new Vector3(305f,13f,1040f)},
                }
            },
            {
                "map_hub_wallOfTheStars", new Dictionary<string,Vector3>()
                {
                    {"fastTravelPortal", new Vector3(137f,12f,-268f)},
                    {"trialOfTheStarsPortal", new Vector3(54f,78f,-355f)},
                    {"gateOfTheMoonPortal", new Vector3(-313f,37f,-410f)},
                    {"merchant", new Vector3(-100f,12f,-345f)},
                }
            },
            {
                "map_map00_gateOfTheMoon", new Dictionary<string,Vector3>()
                {
                    {"wallOfTheStarsPortal", new Vector3(156f,102f,911f)},
                    {"redwoudEntrance", new Vector3(805f,5f,820f)},
                    {"crescentRoadPortal", new Vector3(612f,3f,93f)},
                }
            },
            {
                "map_zone00_starwall_A1", new Dictionary<string,Vector3>()
                {
                    {"wallOfTheStarsPortal", new Vector3(0-12f,4f,3f)},
                    {"checkpointHub", new Vector3(30f,0f,188f)},
                    {"checkpoint1", new Vector3(60f,448f,185f)},
                    {"checkpoint2", new Vector3(-10f,878f,184f)},
                    {"checkpoint3", new Vector3(50f,1130f,181f)},
                    {"summit", new Vector3(-9f,1507f,289f)},
                }
            }
        };
    }
}