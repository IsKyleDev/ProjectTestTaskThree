using System;
using TestTask.NonEditable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtMonsterName;
        [SerializeField] private Slider sliderMonsterHealth;
        [SerializeField] private Image imgMonster;
        [SerializeField] private Button btnTakeDamage;
        [SerializeField] private Sprite[] monsterSprites;
        [field: SerializeField] public MonsterData MonsterData { get; private set; }

        private void Start()
        {
            btnTakeDamage.onClick.AddListener(HandleOnBtnTakeDamage);
        }

        private void HandleOnBtnTakeDamage()
        {
            ClientPacketsHandler.SendMonsterTakeDamageRequest();
        }

        public void SetMonster(MonsterData monsterData)
        {
            MonsterData = monsterData;

            RefreshUI();
        }

        private void RefreshUI()
        {
            txtMonsterName.text = MonsterData.MonsterName;

            SetMonsterSprite(MonsterData.MonsterType);

            sliderMonsterHealth.maxValue = MonsterData.MonsterMaxHealth;
            sliderMonsterHealth.value = MonsterData.MonsterCurrentHealth;
        }

        private void SetMonsterSprite(MonsterNames monsterType)
        {
            switch (monsterType)
            {
                case MonsterNames.Dragon:
                    imgMonster.sprite = monsterSprites[0];
                    break;

                case MonsterNames.Goblin:
                    imgMonster.sprite = monsterSprites[1];
                    break;

                case MonsterNames.Orc:
                    imgMonster.sprite = monsterSprites[2];
                    break;

                case MonsterNames.Skeleton:
                    imgMonster.sprite = monsterSprites[3];
                    break;

                case MonsterNames.Troll:
                    imgMonster.sprite = monsterSprites[4];
                    break;
            }
        }

    }
}
