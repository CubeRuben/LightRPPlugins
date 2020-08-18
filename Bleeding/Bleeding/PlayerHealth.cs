using CustomPlayerEffects;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine.Assertions.Must;

namespace Bleeding
{
    public class PlayerHealth
    {
        public enum BleedingTypes  
        { 
            None,
            Low,
            Medium,
            High,
            Arterial
        }

        public class Bleeding
        {
            public class BleedingTypeData
            {
                public float DamagePerInterval;
                public float Interval;
                public float Time;
                public string RankName;
                public string BroadcastMessage;
            }

            PlayerHealth PlayerHealth;
            public static BleedingTypeData[] BleedingTypesData = new BleedingTypeData[] {
                new BleedingTypeData() {DamagePerInterval = 1, Interval = 1, Time = 10, RankName = "Слегка истекает кровью", BroadcastMessage = "<color=#dc314c> Вы слегка истекаете кровью. Используйте любой медикамент, либо подождите, пока кровь сама остановится </color>"},
                new BleedingTypeData() {DamagePerInterval = 1, Interval = 0.5f, Time = 15, RankName = "Истекает кровью", BroadcastMessage = "<color=#8b0000> Вы истекаете кровью. Найдите Аптечку, Шприц, SCP-500 </color>"},
                new BleedingTypeData() {DamagePerInterval = 2, Interval = 0.5f, Time = float.PositiveInfinity, RankName = "Хлещет кровью", BroadcastMessage = "<color=#9b2d30> У вас сильное кровотечение, найдите Аптечку или SCP-500, иначе вам конец </color>"},
                new BleedingTypeData() {DamagePerInterval = 2, Interval = 0.2f, Time = float.PositiveInfinity, RankName = "Умирает от кровотечения", BroadcastMessage = "<color=#c10020> У вас артериальное кровотечение. Найдите SCP-500 и быстро, иначе вам конец </color>"}
            };

            public BleedingTypes BleedingType;

            bool StopCoroutine = false;

            public Bleeding(BleedingTypes bleedingType, PlayerHealth playerHealth) 
            {
                BleedingType = bleedingType;
                PlayerHealth = playerHealth;
                int i = (int)BleedingType - 1;

                if (i == -1) 
                {
                    End();
                    return;
                }

                if (PlayerHealth.CurrentHighestBleedingType < BleedingType) 
                {
                    PlayerHealth.CurrentHighestBleedingType = BleedingType;
                    PlayerHealth.Player.RankName = BleedingTypesData[i].RankName;
                    PlayerHealth.Player.RankColor = "red";
                    PlayerHealth.Player.ClearBroadcasts();
                    PlayerHealth.Player.Broadcast(10, BleedingTypesData[i].BroadcastMessage);
                }
                Timing.RunCoroutine(HurtCoroutine(BleedingTypesData[i].Interval, BleedingTypesData[i].DamagePerInterval, BleedingTypesData[i].Time));
            }

            IEnumerator<float> HurtCoroutine(float interval, float damage, float time) 
            {
                yield return Timing.WaitForSeconds(interval);
                for (float timer = 0; (timer < time) && !StopCoroutine; timer += interval) 
                {
                    PlayerHealth.Player.Hurt(damage, DamageTypes.Bleeding);
                    yield return Timing.WaitForSeconds(interval);
                }

                if (!StopCoroutine)
                {
                    End();
                }
            }

            public void End()
            {
                StopCoroutine = true;

                PlayerHealth.CurrentHighestBleedingType = PlayerHealth.GetHighestBleedingType(BleedingType);

                if (PlayerHealth.CurrentHighestBleedingType < BleedingType)
                {
                    PlayerHealth.Player.ClearBroadcasts();
                    PlayerHealth.Player.SetRank(PlayerHealth.Player.Group.BadgeText, PlayerHealth.Player.Group);
                }
                
                PlayerHealth.Bleedings.Remove(this);
            }
        }
        
        public Player Player;

        public List<Bleeding> Bleedings;

        System.Random Random;

        BleedingTypes CurrentHighestBleedingType;

        CharacterClassManager CharacterClassManager;

        bool BloodSpawnStarted;

        public PlayerHealth(Player player) 
        {
            Player = player;
            Random = new System.Random();
            Bleedings = new List<Bleeding>();
            CurrentHighestBleedingType = BleedingTypes.None;
            CharacterClassManager = Player.GameObject.GetComponent<CharacterClassManager>();
            BloodSpawnStarted = false;
        }

        public bool AddBleeding(BleedingTypes bleedingType, float chance = 100) 
        {
            if (Random.Next(0, 100) < chance) 
            {
                Bleedings.Add(new Bleeding(bleedingType, this));
                UpdateBloodSpawn();
                return true;
            }
            return false;
        }

        public void OnHurting(DamageTypes.DamageType damageType) 
        {
            bool flag = false;
            //Расчитываем артериальное
            switch (damageType.name) 
            {
                case "Com15":
                case "USP":
                case "P90":
                case "MP7":
                    flag = AddBleeding(BleedingTypes.Arterial, 1);
                    break;
                case "E11 Standard Rifle":
                    flag = AddBleeding(BleedingTypes.Arterial, 2);
                    break;
                case "Logicier":
                    flag = AddBleeding(BleedingTypes.Arterial, 3);
                    break;
                case "SCP-939":
                    flag = AddBleeding(BleedingTypes.Arterial, 30);
                    break;
            }

            if (flag)
                return;

            //Расчитываем сильное
            switch (damageType.name)
            {
                case "USP":
                    flag = AddBleeding(BleedingTypes.High, 5);
                    break;
                case "P90":
                case "MP7":
                    flag = AddBleeding(BleedingTypes.High, 10);
                    break;
                case "E11 Standard Rifle":
                    flag = AddBleeding(BleedingTypes.High, 20);
                    break;
                case "Logicier":
                    flag = AddBleeding(BleedingTypes.High, 30);
                    break;
                case "SCP-939":
                    flag = AddBleeding(BleedingTypes.High, 80);
                    break;
            }

            if (flag)
                return;

            //Расчитываем обычное
            switch (damageType.name)
            {
                case "FALLDOWN":
                    flag = AddBleeding(BleedingTypes.Medium, 15);
                    break;
                case "P90":
                case "MP7":
                    flag = AddBleeding(BleedingTypes.Medium, 40);
                    break;
                case "E11 Standard Rifle":
                case "Logicier":
                case "USP":
                    flag = AddBleeding(BleedingTypes.Medium, 50);
                    break;
            }

            if (flag)
                return;

            //Расчитываем слабое
            switch (damageType.name) 
            {
                case "SCP-049-2":
                    AddBleeding(BleedingTypes.Low, 10);
                    break;
                case "Com15":
                    AddBleeding(BleedingTypes.Low, 40);
                    break;
            }
        }

        void UpdateBloodSpawn() 
        {
            if ((CurrentHighestBleedingType != BleedingTypes.None) && !BloodSpawnStarted) 
            {
                BloodSpawnStarted = true;
                Timing.RunCoroutine(BloodSpawnCoroutine());
            }
        }

        IEnumerator<float> BloodSpawnCoroutine() 
        {
            while (CurrentHighestBleedingType != BleedingTypes.None) 
            {
                CharacterClassManager.RpcPlaceBlood(Player.Position, 0, (int)CurrentHighestBleedingType * 0.3f);
                yield return Timing.WaitForSeconds(1f);
            }
            BloodSpawnStarted = false;
        }

        public void Clear() 
        {
            for (int i = 0; i < Bleedings.Count;) 
            {
                Bleedings[i].End();
            }
        }

        public BleedingTypes GetHighestBleedingType(BleedingTypes ignore) 
        {
            BleedingTypes bleedingType = BleedingTypes.None;

            bool flag = false;

            for (int i = 0; i < Bleedings.Count; i++)
            {
                if (bleedingType < Bleedings[i].BleedingType) 
                {
                    if ((ignore == Bleedings[i].BleedingType) && !flag)
                    {
                        flag = true;
                    }
                    else
                    {
                        bleedingType = Bleedings[i].BleedingType;
                    }
                }
            }

            return bleedingType;
        }
    }
}
