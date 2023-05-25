using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using BepInEx;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.XR;
using Utilla;

namespace lol {
    [BepInPlugin("org.Emattew1", "GorillaTagDebugger", "1.0.0")]
    public class Main : BaseUnityPlugin {
        private void OnEnable() {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        private void OnDisable() {
            HarmonyPatches.RemoveHarmonyPatches();
        }

        private float deltaTime = 0.0f;
        public static bool disconnectedfromplayfab = false;
        private void Update() {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
        private void OnGUI() {
            GUILayout.BeginVertical();
            GUI.color = Color.cyan;

            if(GUILayout.Button("Disconnect From Room")) {
                PhotonNetworkController.Instance.AttemptDisconnect();
            }
            if(GUILayout.Button("Disconnect From Photon")) {
                PhotonNetworkController.Instance.FullDisconnect();
            }
            if(GUILayout.Button("Disconnect From Playfab")) {
                disconnectedfromplayfab = !disconnectedfromplayfab;
            }

            GUI.skin.label.fontSize = 18;
            GUILayout.Label("Movement Stats:");
            GUI.skin.label.fontSize = 15;

            GUILayout.Label($"Speed: {GorillaLocomotion.Player.Instance.maxJumpSpeed}");
            GUILayout.Label($"Slide Control: {GorillaLocomotion.Player.Instance.slideControl}");
            GUILayout.Label($"Volocity Limit: {GorillaLocomotion.Player.Instance.velocityLimit}");
            GUILayout.Label($"Is Right Hand Touching: {GorillaLocomotion.Player.Instance.wasRightHandTouching}");
            GUILayout.Label($"Is Left Hand Touching: {GorillaLocomotion.Player.Instance.wasLeftHandTouching}");
            GUILayout.Label($"Hand Tap Volume: {GorillaTagger.Instance.handTapVolume}");

            GUILayout.Space(10);

            GUI.skin.label.fontSize = 18;
            GUILayout.Label("Unity Stats:");
            GUI.skin.label.fontSize = 15;

            GUILayout.Label($"FPS: {Mathf.RoundToInt(1.0f / deltaTime)}");
            GUILayout.Label($"Gravity: {Physics.gravity}");

            GUILayout.Space(10);

            GUI.skin.label.fontSize = 18;
            GUILayout.Label("Networking Stats:");
            GUI.skin.label.fontSize = 15;

            GUILayout.Label($"Ping: {PhotonNetwork.GetPing()}");
            GUILayout.Label($"Photon Name: {PhotonNetwork.NickName}");
            if(PhotonNetwork.InRoom) {
                GUILayout.Label($"In Room: {PhotonNetwork.InRoom}");
                GUILayout.Label($"Room Name: {PhotonNetwork.CurrentRoom.Name}");
                GUILayout.Label($"Max Players: {PhotonNetwork.CurrentRoom.MaxPlayers}");
                GUILayout.Label($"Is Master Client: {PhotonNetwork.IsMasterClient}");
                GUILayout.Label($"Players In Room: {PhotonNetwork.CurrentRoom.PlayerCount}");
                GUILayout.Label($"View Count: {PhotonNetwork.ViewCount}");
                GUILayout.Label($"Is Room Open: {PhotonNetwork.CurrentRoom.IsOpen}");
                GUILayout.Label($"Is Room Visable: {PhotonNetwork.CurrentRoom.IsVisible}");
            }
            GUILayout.Label($"Networking Client State: {PhotonNetwork.NetworkClientState}");
            GUILayout.Label($"Offline Mode: {PhotonNetwork.OfflineMode}");
            GUILayout.Label($"Allowed In Competitive: {GorillaComputer.instance.allowedInCompetitive}");
        }
    }
    [HarmonyPatch(typeof(PlayFabAuthenticator), "AuthenticateWithPlayFab")]
    public class InitialLoginPatch {
        [HarmonyPrefix]
        private static bool Prefix() {
            return Main.disconnectedfromplayfab;
        }
    }
}