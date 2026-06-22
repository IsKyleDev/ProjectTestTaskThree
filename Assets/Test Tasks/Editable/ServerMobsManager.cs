using System;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestTask.Editable
{
    public class ServerMobsManager
    {
        [field: SerializeField] public MonsterData MonsterData { get; private set; }
        public int MonsterStateVersion { get; private set; }

        public ServerMobsManager()
        {
            MonsterData = SpawnMonster();
        }

        public void IncrementVersion()
        {
            MonsterStateVersion++;
        }

        public MonsterData SpawnMonster()
        {
            var monsterId = Random.Range(1, 1000);
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(monsterId);
            var monsterMaxHealth = Random.Range(50, 201);
            var monsterCurrentHealth = monsterMaxHealth;

            MonsterData = new MonsterData(monsterId, monsterType, monsterMaxHealth, monsterCurrentHealth);
            MonsterData.MonsterDeath += OnMonsterDied;
            MonsterData.MonsterDamaged += OnMonsterDamaged;

            return MonsterData;
        }



        public void OnMonsterDied()
        {
            MonsterData.MonsterDeath -= OnMonsterDied;
            MonsterData.MonsterDamaged -= OnMonsterDamaged;

            MonsterData = SpawnMonster();
            IncrementVersion();
            ServerPacketsHandler.SendMonsterDataResponse();
        }

        public void OnMonsterDamaged(float obj)
        {
            if (MonsterData.MonsterCurrentHealth > 0)
            {
                IncrementVersion();
                ServerPacketsHandler.SendMonsterDataResponse();
            }
        }

        public void HandleMonsterTakeDamage()
        {
            MonsterData.TakeDamage(25);
            Debug.Log("SERVER MONSTER ON DAMAGE: " + "Current Monster: " + MonsterData.MonsterName + " ID: " + MonsterData.MonsterId +
            " Current HP: " + MonsterData.MonsterCurrentHealth + "/" + MonsterData.MonsterMaxHealth);
        }
    }
}  
