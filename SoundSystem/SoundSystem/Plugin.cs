using Exiled.API.Features;
using Exiled.API.Enums;


using System;
using System.Reflection;


using UnityEngine;
using Mirror;
using System.Linq;
using RemoteAdmin;

using Assets._Scripts.Dissonance;
using Dissonance;
using NAudio.Wave;

namespace SoundSystem
{
    public class SoundSystemPlugin : Plugin<Config>
    {
        #region Info
        public override string Name { get; } = "Sound System";
        public override string Prefix { get; } = "SndSs";
        public override string Author { get; } = "CubeRuben";
        public override PluginPriority Priority { get; } = PluginPriority.Highest;
        public override System.Version Version { get; } = new System.Version(1, 0, 0);
        public override System.Version RequiredExiledVersion { get; } = new System.Version(2, 1, 0);
        #endregion

        EventHandlers EventHandlers;

        public SoundSystemPlugin()
        {

        }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnSendingConsoleCommand;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnSendingConsoleCommand;

            EventHandlers = null;
        }

        public static void PlayAudio(Vector3 position) 
        {
            GameObject obj = GameObject.Instantiate(NetworkManager.singleton.spawnPrefabs.FirstOrDefault(p => p.gameObject.name == "Player"));
            CharacterClassManager ccm = obj.GetComponent<CharacterClassManager>();
            ccm.CurClass = RoleType.Tutorial;
            obj.GetComponent<NicknameSync>().Network_myNickSync = "Audio Player";
            obj.GetComponent<QueryProcessor>().PlayerId = 9999;
            obj.GetComponent<QueryProcessor>().NetworkPlayerId = 9999;
            obj.transform.position = position;
            NetworkServer.Spawn(obj);
            DissonanceUserSetup dissonanceComms = obj.GetComponent<DissonanceUserSetup>();
            dissonanceComms.TryGetVoiceTrigger(TriggerType.Proximity, false, out Dissonance.BaseCommsTrigger trigger);
            Type type = typeof(Dissonance.BaseCommsTrigger);
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;


            FieldInfo finfo = type.GetField("Comms", bindingFlags);


            DissonanceComms someThingField = (DissonanceComms)finfo.GetValue(trigger);

            AudioPlay audio = new AudioPlay();
            someThingField.SubcribeToRecordedAudio(audio);
            
        }
    }

    class AudioPlay : Dissonance.Audio.Capture.IMicrophoneSubscriber
    {
        public void ReceiveMicrophoneData(ArraySegment<float> buffer, WaveFormat format)
        {
            
        }

        public void Reset()
        {
            
        }
    }
}
