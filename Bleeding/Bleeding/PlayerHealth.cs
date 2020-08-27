using CustomPlayerEffects;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine.Assertions.Must;
using System.Runtime.InteropServices.ComTypes;
using YamlDotNet.Core;
using System.Diagnostics;
using YamlDotNet.Serialization;

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
            Arterial,
            Custom
        }

        public enum StopBlood
        {
            None,
            Stoping,
            Failed
        }

        enum StopBloodHeal
        {
            Succeed,
            Failed,
            FaildeWithDamage
        }

        public class Bleeding
        {
            public class BleedingTypeData
            {
                public BleedingTypeData(float damagePerInterval, float interval, float time, string rankName, string broadcastMessageSCP049, string broadcastMessageHuman)
                {
                    DamagePerInterval = damagePerInterval;
                    Interval = interval;
                    Time = time;
                    RankName = rankName;
                    BroadcastMessageSCP049 = broadcastMessageSCP049;
                    BroadcastMessageHuman = broadcastMessageHuman;
                    DPS = damagePerInterval / interval;
                }

                public float DamagePerInterval;
                public float Interval;
                public float Time;
                public float DPS;
                public string RankName;
                public string BroadcastMessageSCP049;
                public string BroadcastMessageHuman;
            }

            
            public static BleedingTypeData[] BleedingTypesData = new BleedingTypeData[] {
                new BleedingTypeData(1, 1,    10,                      "*Слегка истекает кровью*",  "<color=#dc314c>Вы слегка истекаете кровью</color>",      "<color=#dc314c>Вы слегка истекаете кровью. Используйте любой медикамент, либо подождите, пока кровь сама остановится</color>"),
                new BleedingTypeData(1, 0.5f, 15,                      "*Истекает кровью*",         "<color=#8b0000>Вы истекаете кровью</color>",             "<color=#8b0000>Вы истекаете кровью. Найдите Аптечку, Шприц, SCP-500</color>"),
                new BleedingTypeData(2, 0.5f, float.PositiveInfinity,  "*Хлещет кровью*",           "<color=#9b2d30>У вас сильное кровотечение</color>",      "<color=#9b2d30>У вас сильное кровотечение, найдите Аптечку или SCP-500, иначе вам конец</color>"),
                new BleedingTypeData(2, 0.2f, float.PositiveInfinity,  "*Умирает от кровотечения*", "<color=#c10020>У вас артериальное кровотечение</color>", "<color=#c10020>У вас артериальное кровотечение. Найдите SCP-500 и быстро, иначе вам конец </color>")
            };

            PlayerHealth PlayerHealth;

            public BleedingTypes BleedingType;

            CoroutineHandle CoroutineHandle;

            public bool IsCustom;

            public float BloodSize;

            public float BloodSpawnRate;

            public Bleeding(BleedingTypes bleedingType, PlayerHealth playerHealth, float damage = 0, float time = 0, float interval = 0, float bloodSize = 0, float bloodSpawnRate = 0)
            {
                BleedingType = bleedingType;
                PlayerHealth = playerHealth;
                IsCustom = false;
                int i = (int)BleedingType - 1;

                if (i == -1)
                {
                    End();
                    return;
                }

                if (BleedingType == BleedingTypes.Custom)
                {
                    IsCustom = true;
                    BloodSize = bloodSize;
                    BloodSpawnRate = bloodSpawnRate;

                    if (PlayerHealth.CurrentHighestBloodSize < BloodSize) 
                    {
                        PlayerHealth.CurrentHighestBloodSize = BloodSize;
                    }

                    float dps = damage / interval;

                    if (dps == 0)
                    {
                        End();
                        return;
                    }

                    for (int a = BleedingTypesData.Length - 1; a >= 0; a--)
                    {
                        if (BleedingTypesData[a].DPS < dps)
                        {
                            i = a;
                            BleedingType = (BleedingTypes)a + 1;
                            break;
                        }
                    }

                    if (BleedingType == BleedingTypes.Custom)
                    {
                        i = 0;
                        BleedingType = BleedingTypes.Low;
                    }

                    CoroutineHandle = Timing.RunCoroutine(HurtCoroutine(interval, damage, time));
                }


                if (PlayerHealth.CurrentHighestBleedingType < BleedingType)
                {
                    PlayerHealth.CurrentHighestBleedingType = BleedingType;
                    PlayerHealth.Player.RankName = BleedingTypesData[i].RankName;
                    PlayerHealth.Player.RankColor = "red";
                    PlayerHealth.Player.ClearBroadcasts();
                    PlayerHealth.Player.Broadcast(300, PlayerHealth.GetClassBleedingBroadcast());
                }

                if (!IsCustom)
                {
                    CoroutineHandle = Timing.RunCoroutine(HurtCoroutine(BleedingTypesData[i].Interval, BleedingTypesData[i].DamagePerInterval, BleedingTypesData[i].Time));
                }
            }

            IEnumerator<float> HurtCoroutine(float interval, float damage, float time)
            {
                yield return Timing.WaitForSeconds(interval);
                for (float timer = 0; timer < time; timer += interval)
                {
                    PlayerHealth.Player.Hurt(damage, DamageTypes.Bleeding);
                    yield return Timing.WaitForSeconds(interval);
                }
                End();
            }

            public void End()
            {
                PlayerHealth.CurrentHighestBleedingType = PlayerHealth.GetHighestBleedingType(BleedingType);
                PlayerHealth.CurrentHighestBloodSize = PlayerHealth.GetHighestBloodSize(this);
                PlayerHealth.CurrentHighestBloodSpawnRate = PlayerHealth.GetHighestBloodSpawnRate(this);

                if (PlayerHealth.CurrentHighestBleedingType < BleedingType)
                {
                    PlayerHealth.Player.ClearBroadcasts();
                    if (PlayerHealth.Player.Group != null)
                    {
                        PlayerHealth.Player.RankName = PlayerHealth.Player.Group.BadgeText;
                        PlayerHealth.Player.RankColor = PlayerHealth.Player.Group.BadgeColor;
                    }
                    else 
                    {
                        PlayerHealth.Player.RankName = "";
                        PlayerHealth.Player.RankColor = "";
                    }
                }

                Timing.KillCoroutines(CoroutineHandle);
                PlayerHealth.Bleedings.Remove(this);
            }
        }

        public Player Player;

        public List<Bleeding> Bleedings;

        System.Random Random;

        public BleedingTypes CurrentHighestBleedingType;

        float CurrentHighestBloodSize;

        float CurrentHighestBloodSpawnRate;

        public CharacterClassManager CharacterClassManager;

        CoroutineHandle? BloodSpawnCoroutineHandle;

        CoroutineHandle? BleedingBroadcastCoroutineHandle;

        public CoroutineHandle? CommandStopBloodCoroutineHandle;

        public StopBlood StopBloodStatus;

        
        public PlayerHealth(Player player)
        {
            StopBloodStatus = StopBlood.None;
            Player = player;
            Random = new System.Random();
            Bleedings = new List<Bleeding>();
            CurrentHighestBleedingType = BleedingTypes.None;
            CharacterClassManager = Player.GameObject.GetComponent<CharacterClassManager>();
            CurrentHighestBloodSize = 0;
        }

        public bool AddBleeding(BleedingTypes bleedingType, float chance = 100, float damage = 0, float time = 0, float interval = 0, float bloodSize = 0, float bloodSpawnRate = 0)
        {
            if (Player.Role == RoleType.Spectator) 
            {
                return false;
            }

            if (Random.Next(0, 100) < chance)
            {
                Bleedings.Add(new Bleeding(bleedingType, this, damage, time, interval, bloodSize, bloodSpawnRate));
                UpdateBloodSpawn();
                return true;
            }
            return false;
        }

        public void OnHeal(ItemType item)
        {
            switch (item)
            {
                case ItemType.SCP500:
                    HealBleedingsOfThisTypeAndLower(BleedingTypes.Arterial);
                    break;
                case ItemType.Medkit:
                    HealBleedingsOfThisTypeAndLower(BleedingTypes.High);
                    break;
                case ItemType.Adrenaline:
                    HealBleedingsOfThisTypeAndLower(BleedingTypes.Medium);
                    break;
                case ItemType.Painkillers:
                    HealBleedingsOfThisTypeAndLower(BleedingTypes.Low);
                    break;
            }
        }

        public void HealBleedingsOfThisTypeAndLower(BleedingTypes bleedingType)
        {
            for (int i = 0; i < Bleedings.Count; i++)
            {
                if (Bleedings[i].BleedingType <= bleedingType)
                {
                    Bleedings[i].End();
                    i--;
                }
            }
        }

        public void FailStopBlood()
        {
            if (StopBloodStatus == StopBlood.Stoping)
            {
                StopBloodStatus = StopBlood.Failed;
            }
        }

        public void OnHurting(DamageTypes.DamageType damageType)
        {
            if ((Player.Team == Team.SCP)/* && (Player.Role != RoleType.Scp049) && (Player.Role != RoleType.Scp93953) && (Player.Role != RoleType.Scp93989)*/)
            {
                return;
            }


            bool flag = false;

            /*if ((Player.Role == RoleType.Scp93953) || (Player.Role == RoleType.Scp93989))
            {
                goto SCP939Immune;
            }*/

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
           // SCP939Immune:
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

        public void CmdStopBlood()
        {
            StopBloodStatus = StopBlood.None;

            switch (Player.Role)
            {
                case RoleType.ClassD:
                    switch (CurrentHighestBleedingType)
                    {
                        case BleedingTypes.Low:
                            CalculateStopBlood(70, 20, 10);
                            break;
                        case BleedingTypes.Medium:
                            CalculateStopBlood(25, 50, 10);
                            break;
                        case BleedingTypes.High:
                            CalculateStopBlood(5, 50, 10);
                            break;
                        case BleedingTypes.Arterial:
                            CalculateStopBlood(1, 50, 49);
                            break;
                    }
                    break;
                case RoleType.Scientist:
                case RoleType.NtfScientist:
                case RoleType.NtfLieutenant:
                case RoleType.NtfCommander:
                case RoleType.NtfCadet:
                case RoleType.ChaosInsurgency:
                case RoleType.FacilityGuard:
                    switch (CurrentHighestBleedingType)
                    {
                        case BleedingTypes.Low:
                            CalculateStopBlood(100, 0, 0);
                            break;
                        case BleedingTypes.Medium:
                            CalculateStopBlood(60, 35, 5);
                            break;
                        case BleedingTypes.High:
                            CalculateStopBlood(10, 70, 20);
                            break;
                        case BleedingTypes.Arterial:
                            CalculateStopBlood(5, 50, 45);
                            break;
                    }
                    break;
                case RoleType.Scp049:
                    CalculateStopBlood(100, 0, 0);
                    break;
            }
        }

        void CalculateStopBlood(int stopChance, int failChance, int failWithDamageChance)
        {
            int random = Random.Next(0, 100);

            if (random < stopChance)
            {
                HealBleedingsOfThisTypeAndLower(CurrentHighestBleedingType);
                Player.ClearBroadcasts();
                Player.Broadcast(5, "<color=#00ff00>Вы остановили кровотечение</color>");
                return;
            }

            random -= stopChance;

            if (random < failChance)
            {
                Player.ClearBroadcasts();
                Player.Broadcast(5, "<color=#dc314c>Вы попытались остановить кровотечение, но у вас не вышло</color>");
                UpdateBleedingBroadcast();
                return;
            }

            random -= failChance;

            if (random < failWithDamageChance)
            {
                Player.Hurt(10, DamageTypes.Bleeding);
                Player.ClearBroadcasts();
                Player.Broadcast(5, "<color=#8b0000>Вы попытались остановить кровотечение, но у вас не вышло и вы нанесли себе урон</color>");
                UpdateBleedingBroadcast();
                return;
            }

            return;
        }

        void UpdateBloodSpawn()
        {
            if ((CurrentHighestBleedingType != BleedingTypes.None) && (BloodSpawnCoroutineHandle == null))
            {
                BloodSpawnCoroutineHandle = Timing.RunCoroutine(BloodSpawnCoroutine());
            }
        }

        IEnumerator<float> BloodSpawnCoroutine()
        {
            while (CurrentHighestBleedingType != BleedingTypes.None)
            {
                if (CurrentHighestBloodSize == 0)
                {

                    CharacterClassManager.RpcPlaceBlood(Player.Position, 0, (int)CurrentHighestBleedingType * 0.3f);

                }
                else
                {
                    CharacterClassManager.RpcPlaceBlood(Player.Position, 0, CurrentHighestBloodSize);
                }
                if (CurrentHighestBloodSpawnRate == 0)
                {
                    yield return Timing.WaitForSeconds(Convert.ToSingle(Random.NextDouble()) + 0.3f);
                }
                else
                {
                    yield return Timing.WaitForSeconds(CurrentHighestBloodSpawnRate);
                }
            }

            BloodSpawnCoroutineHandle = null;
        }

        public void UpdateBleedingBroadcast()
        {
            if (BleedingBroadcastCoroutineHandle != null)
            {
                Timing.KillCoroutines(BleedingBroadcastCoroutineHandle.GetValueOrDefault());
            }

            BleedingBroadcastCoroutineHandle = Timing.RunCoroutine(BleedingBroadcastCoroutine());
        }

        IEnumerator<float> BleedingBroadcastCoroutine()
        {
            yield return Timing.WaitForSeconds(5);
            int index = (int)CurrentHighestBleedingType - 1;
            if (index > 0)
            {
                if (StopBloodStatus != StopBlood.Stoping)
                {
                    Player.ClearBroadcasts();
                    Player.Broadcast(300, GetClassBleedingBroadcast());
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Bleedings.Count;)
            {
                Bleedings[i].End();
            }

            Timing.KillCoroutines(BleedingBroadcastCoroutineHandle.GetValueOrDefault());
            BleedingBroadcastCoroutineHandle = null;
            Timing.KillCoroutines(BloodSpawnCoroutineHandle.GetValueOrDefault());
            BloodSpawnCoroutineHandle = null;
            Timing.KillCoroutines(CommandStopBloodCoroutineHandle.GetValueOrDefault());
            CommandStopBloodCoroutineHandle = null;
            StopBloodStatus = StopBlood.None;
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

        public float GetHighestBloodSize(Bleeding ignore) 
        {
            float bloodSize = 0;

            for (int i = 0; i < Bleedings.Count; i++) 
            {
                if (!System.Object.ReferenceEquals(Bleedings[i], ignore)) 
                {
                    if (Bleedings[i].IsCustom)
                    {
                        if (bloodSize < Bleedings[i].BloodSize)
                        {
                            bloodSize = Bleedings[i].BloodSize;
                        }
                    }
                    else 
                    {
                        if (bloodSize < (int)Bleedings[i].BleedingType * 0.4f)
                        {
                            bloodSize = (int)Bleedings[i].BleedingType * 0.4f;
                        }
                    }
                }
            }

            return bloodSize;
        }

        public float GetHighestBloodSpawnRate(Bleeding ignore)
        {
            float bloodSpawnRate = 0;

            for (int i = 0; i < Bleedings.Count; i++)
            {
                if (!System.Object.ReferenceEquals(Bleedings[i], ignore))
                {
                    if (Bleedings[i].IsCustom)
                    {
                        if (bloodSpawnRate < Bleedings[i].BloodSpawnRate)
                        {
                            bloodSpawnRate = Bleedings[i].BloodSpawnRate;
                        }
                    }
                }
            }

            return bloodSpawnRate;
        }


        string GetClassBleedingBroadcast(BleedingTypes bleedingTypes = BleedingTypes.None) 
        {
            int i;

            if (bleedingTypes == BleedingTypes.None)
            {
                i = (int)CurrentHighestBleedingType - 1;
            }
            else 
            {
                i = (int)bleedingTypes - 1;
            }

            if (i < 0) 
            {
                return "";
            }

            if (Player.Team != Team.SCP)
            {
                return Bleeding.BleedingTypesData[i].BroadcastMessageHuman;
            } 
            else if (Player.Role == RoleType.Scp049)
            {
                return Bleeding.BleedingTypesData[i].BroadcastMessageSCP049;
            }

            return "";
        }    
    
    }
}
