using BepInEx;
using System;
using UnityEngine;
using Discord;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Landfall.Network;
using Landfall;

namespace SeventysTotallyAccurateRPC
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

       public Discord.Discord discord;
        PhotonServerHandler serverHandler;
        string map = "NULL";
        string partyType = "NULL";
        string smallImageText = "NULL";
        string largeImageText = "NULL";
        int currentkills;
        string currentWeapon = "NULL";

        // Images

        string[] tabgLogos = new string[] {"logo","logo2", "logo3" };
        string[] battlepics = new string[] {"car", "bluebattle", "redman", "redbattle", "awp","logo1","redman2" };
        string[] gunrangePics = new string[] { "dummycannon", "target", "strawman", "logo1" };
        string deagleImage = "gunimage";
        string smallimage;

        string largeimage;
        void OnEnable()
        {
            Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        ActivityTimestamps stamp = new ActivityTimestamps();
      
        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "WilhelmTest")
            {
                SetGunRangeStatus();
            }
            else if(scene.name == "MainMenu")
            {
                SetMainMenuStatus();
            }
            else if (scene.name == "MainWorld_Base")
            {
                SetBRStatus();
            }
        }
        void SetMainMenuStatus()
        {
            
            map = "Main menu";
            largeImageText = "Chillin' in the main menu";
            smallImageText = "";
            smallimage = "";
            largeimage = tabgLogos[UnityEngine.Random.Range(0, tabgLogos.Length)];
            allowedToDisplayheldWeapon = false;
            allowedToDisplayPartyType = true;
            allowedToLookForKills = false;
            allowedToTrackPlayersAlive = false;
            allowedToTrackHealth = false;
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = partyType,
                Details = map,
                Assets =
                {
                    LargeImage = largeimage,
                    LargeText = largeImageText,
                    SmallImage = smallimage,
                    SmallText = smallImageText

                },
              
        };
           
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("--------->Yay Set Discord Status");
                }
                else
                {
                    Debug.LogError("--------> Ohoh Something went wrong when trying to apply RPC");
                }
            });
        }
        void SetGunRangeStatus()
        {
            map = "Shooting Range";
            largeImageText = "Testing the latest upda.... hm.";
            largeimage = gunrangePics[UnityEngine.Random.Range(0, gunrangePics.Length)];
            smallimage = deagleImage;
            allowedToDisplayheldWeapon = true;
            allowedToDisplayPartyType = false;
            partyType = "Training for our next meeting!";
            allowedToLookForKills = false;
            allowedToTrackPlayersAlive = false;
            allowedToTrackHealth = true;
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = partyType,
                Details = map,
                Assets =
                {
                    LargeImage = largeimage,
                    LargeText = largeImageText,
                    SmallImage = smallimage,
                    SmallText = smallImageText,
                },
                
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("--------->Yay Set Discord Status");
                }
                else
                {
                    Debug.LogError("--------> Ohoh Something went wrong when trying to apply RPC");
                }
            });
        }
        void SetBRStatus()
        {
            map = "The Island | Battle Royale";
            largeImageText = "Running up them kills!";
            largeimage = battlepics[UnityEngine.Random.Range(0, battlepics.Length)];
            smallimage = deagleImage;
            allowedToDisplayheldWeapon = true;
            allowedToDisplayPartyType = true;
            allowedToLookForKills = true;
            allowedToTrackPlayersAlive = true;
            allowedToTrackHealth = true;
           
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = partyType,
                Details = map,
                Assets =
                {
                    LargeImage = largeimage,
                    LargeText = largeImageText,
                    SmallImage = smallimage,
                    SmallText = smallImageText


                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Debug.Log("--------->Yay Set Discord Status");
                }
                else
                {
                    Debug.LogError("--------> Ohoh Something went wrong when trying to apply RPC");
                }
            });
        }

        bool allowedToLookForKills;
        bool allowedToDisplayheldWeapon;
        bool allowedToDisplayPartyType;
        bool allowedToTrackPlayersAlive;
        bool allowedToTrackHealth;
        IEnumerator Start()
        {
            
            yield return new WaitForSeconds(2);
           LaterStart();
        }
        bool gotDiscord;
        void LaterStart()
        {

            discord = new Discord.Discord(1003712519216566293, (System.UInt64)Discord.CreateFlags.Default);
            gotDiscord = true;
            stamp.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            var activityManager =  discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = partyType,
                Details = map,
                Assets =
                {
                    LargeImage = tabgLogos[UnityEngine.Random.Range(0,tabgLogos.Length)],
                    LargeText = largeImageText,
                    SmallImage = "",
                    SmallText = smallImageText
            
                },

                Timestamps = stamp,
            };
            // activity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            activityManager.UpdateActivity(activity, (res) =>
            {
                if(res == Discord.Result.Ok)
                {
                    Debug.Log("--------->Yay Set Discord Status");
                }
                else
                {
                    Debug.LogError("--------> Ohoh Something went wrong when trying to apply RPC");
                }
            } );

            
            StartCoroutine(SlowNumerator());
          
        }
        
            PhotonServerConnector a;

        KillFeedback killfeed;
        IEnumerator SlowNumerator()
        {
            while (true)
            {
                Debug.Log("Yay");
                yield return new WaitForSeconds(1);
                if (FindObjectOfType<PhotonServerConnector>() != null)
                {

                    a = FindObjectOfType<PhotonServerConnector>();
                }
                else
                {

                }

                if (allowedToDisplayPartyType)
                {
                    MatchMode partymode = a.CurrentMatchMode;
                    partyType = GetPartyType(partymode);
                }

                if (allowedToTrackPlayersAlive && ClientGameHandler.CurrentGameState == GameState.Started)
                {
                    largeImageText = serverHandler.GetPlayersAlive().ToString() + " Players Alive!";
                }
                else if(allowedToTrackPlayersAlive && ClientGameHandler.CurrentGameState == GameState.WaitingForPlayers)
                {
                    largeImageText = serverHandler.GetPlayersAlive().ToString() + " Players Alive! Game has not yet started, Still waiting for players";
                }


                yield return new WaitForEndOfFrame();
                if (Player.localPlayer != null && allowedToDisplayheldWeapon )
                {
                    if (Player.localPlayer.m_weaponHandler.rightWeapon != null)
                    {
                        currentWeapon = Player.localPlayer.m_weaponHandler.rightWeapon.name;
                        smallImageText = "Holding " + currentWeapon;
                        smallImageText = smallImageText.Replace("(Clone)", "!");
                    }
                    if (Player.localPlayer.m_weaponHandler.rightWeapon == null)
                    {
                        smallImageText = "Holding nothing";
                    }
                    
                }
                if (allowedToTrackHealth)
                {
                    smallImageText += " | Health : " + Player.localPlayer.bodyDamagable.CurrentHealth;
                }

                if(allowedToLookForKills)
                {
                  currentkills =  ResultsStats.Kills.Count;
                    partyType += " | Kills : " + currentkills.ToString();
                }
                var activityManager = discord.GetActivityManager();
                var activity = new Discord.Activity
                {
                    State = partyType,
                    Details = map,
                    Assets =
                    {
                        LargeImage = largeimage,
                        LargeText = largeImageText,
                        SmallImage = smallimage,
                        SmallText = smallImageText

                    },
                    Timestamps = stamp,
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res == Discord.Result.Ok)
                    {
                        Debug.Log("--------->Yay Set Discord Status");
                    }
                    else
                    {
                        Debug.LogError("--------> Ohoh Something went wrong when trying to apply RPC");
                    }
                });
            }

        }
        string GetPartyType(MatchMode partymode)
        {
            string returnText = "";
            switch (partymode)
            {
                case MatchMode.SOLO:
                    {

                        returnText = "Solo";
                        return returnText;
                        break;
                    }
                case MatchMode.DUO:
                    {
                        returnText = "Duo";
                        return returnText;
                        break;

                    }
                case MatchMode.SQUAD:
                    {
                        returnText = "Squad";
                        return returnText;
                        break;

                    }
            }
            return returnText;
        }
        float nextSlowNumerator;
        float numeratorCooldown = 1f;
        void Update()
        {
            if(serverHandler == null)
            {
                if(FindObjectOfType<PhotonServerHandler>() != null)
                {
                    serverHandler = FindObjectOfType<PhotonServerHandler>();
                }
            }
            if (gotDiscord)
            {
                
                discord.RunCallbacks();
            }
        }
    }
}
