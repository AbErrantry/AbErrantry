using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character2D
{
    public class Player : Attackable
    {
        public static Player instance;
        public static Action<PlayerInfoTuple> OnPlayerInfoChanged;

        public int gold;
        public int karma;
        public string currentQuest;
        public string equippedArmor;
        public string equippedWeapon;

        public Item weapon;
        public Item armor;

        public Animator weaponAnim;

        public CinemachineVirtualCamera virtualCamera;
        private PlayerInput playerInput;
        private PlayerInventory playerInventory;
        private TravelMenu travelMenu;

        public Vector2 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        public SpawnManager spawnManager;

        public TMP_Text healthText;
        public TMP_Text goldText;
        public TMP_Text locationText;
        public TMP_Text questText;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        //used for initialization
        protected new void Start()
        {
            base.Start();

            playerInput = GetComponent<PlayerInput>();
            playerInventory = GetComponent<PlayerInventory>();
            travelMenu = GetComponent<TravelMenu>();

            healthText.text = currentVitality + "/" + maxVitality;

            //set from CharacterData
            spawnPoint = transform.position; //todo: set from file
            canFlinch = false;
            canKnockBack = true;
            canTakeDamage = true;

            SetPlayerInfo();
        }

        private void SetPlayerInfo()
        {
            PlayerInfoTuple playerInfo = GameData.data.saveData.ReadPlayerInfo();
            maxVitality = playerInfo.maxHealth;
            currentVitality = playerInfo.currentHealth;
            currentQuest = playerInfo.currentQuest;
            gold = playerInfo.gold;
            karma = playerInfo.karma;
            spawnManager = SpawnManager.SetSpawnManager(playerInfo.checkpointName);
            spawnPoint = spawnManager.gameObject.transform.position;
            SetArmor(playerInfo.equippedArmor, isLoad : true);
            SetWeapon(playerInfo.equippedWeapon, isLoad : true);
            TakeDamage(gameObject, 0);

            transform.position = spawnPoint;

            goldText.text = gold.ToString();
            questText.text = currentQuest.ToString();
            locationText.text = spawnManager.persistentLevel.levelInfo.displayName;
        }

        public void SetKarma(int delta)
        {
            karma += delta;
            InvokePlayerInfoChange();
        }

        public void SetGold(int delta, bool stolen = false)
        {
            gold += delta;
            goldText.text = gold.ToString();
            InvokePlayerInfoChange();
            if (delta > 0 && !stolen)
            {
                EventDisplay.instance.AddEvent("Received " + Mathf.Abs(delta) + " gold.");
            }
            else if (delta <= 0 && !stolen)
            {
                EventDisplay.instance.AddEvent("Gave " + Mathf.Abs(delta) + " gold.");
            }
            else
            {
                EventDisplay.instance.AddEvent(Mathf.Abs(delta) + " gold was taken from you.");
            }
        }

        public void Heal(int amount)
        {
            if (amount + currentVitality > maxVitality)
            {
                amount = maxVitality - currentVitality;
            }
            currentVitality += amount;
            healthText.text = currentVitality + "/" + maxVitality;
            EventDisplay.instance.AddEvent("The potion restored " + amount + " health.");
            InvokePlayerInfoChange();
        }

        public void SetArmor(string name, bool isLoad = false)
        {
            if (armor != null)
            {
                playerInventory.AddItem(armor.name, false, true);
            }

            armor = GameData.data.itemData.itemDictionary[name];
            equippedArmor = armor.name;
            gameObject.GetComponent<SpriteRenderer>().material = armor.material;
            if (!isLoad)
            {
                GetComponent<Animator>().SetTrigger("isShowingOff");
                weaponAnim.SetTrigger("isShowingOff");
                InvokePlayerInfoChange();
            }
        }

        public void SetWeapon(string name, bool isLoad = false)
        {
            if (weapon != null)
            {
                playerInventory.AddItem(weapon.name, false, true);
            }

            weapon = GameData.data.itemData.itemDictionary[name];
            equippedWeapon = weapon.name;
            weaponAnim.gameObject.GetComponent<SpriteRenderer>().material = weapon.material;
            weaponAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Weapons/" + weapon.spriteName);
            if (!isLoad)
            {
                GetComponent<Animator>().SetTrigger("isShowingOff");
                weaponAnim.SetTrigger("isShowingOff");
                InvokePlayerInfoChange();
            }
        }

        public void ResetState()
        {
            GetComponent<Animator>().Play("IDLE");
            weaponAnim.Play("IDLE");
        }

        public void InvokePlayerInfoChange()
        {
            PlayerInfoTuple playerInfo = new PlayerInfoTuple();
            playerInfo.maxHealth = maxVitality;
            playerInfo.currentHealth = currentVitality;
            playerInfo.currentQuest = currentQuest;
            playerInfo.gold = gold;
            playerInfo.karma = karma;
            playerInfo.checkpointName = spawnManager.managerName;
            playerInfo.equippedArmor = equippedArmor;
            playerInfo.equippedWeapon = equippedWeapon;
            OnPlayerInfoChanged(playerInfo);
        }

        public override void TakeDamage(GameObject attacker, int damage)
        {
            base.TakeDamage(attacker, damage);
            if (currentVitality < 0)
            {
                currentVitality = 0;
            }
            healthText.text = currentVitality + "/" + maxVitality;
            InvokePlayerInfoChange();
        }

        protected override void InitializeDeath()
        {
            if (GetComponent<BackpackMenu>().isOpen)
            {
                GetComponent<BackpackMenu>().CloseBackpackMenu();
            }
            if (GetComponent<Dialogue2D.DialogueManager>().isOpen)
            {
                GetComponent<Dialogue2D.DialogueManager>().EndDialogue();
            }
            if (GetComponent<PlayerInteraction>().isOpen)
            {
                GetComponent<PlayerInteraction>().CloseContainer();
            }
            //take away player input
            ToggleCamera(false);
            playerInput.DisableInput();

            isDying = true;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            //enemies no longer target player
            StartCoroutine(TravelMenuDelay());
        }

        private IEnumerator TravelMenuDelay()
        {
            yield return new WaitForSeconds(2.0f);
            travelMenu.Open("You died", true);
        }

        public void InitialLoad()
        {
            playerInput.InvokeSleep();
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
            }
            travelMenu.Open("Start", false);
        }

        public override void FinalizeDeath()
        {
            //enemies target player
            //give player back input
            //death penalty: 25% of gold?
            playerInput.InvokeSleep();
        }

        public void Respawn()
        {
            isDying = false;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            currentVitality = maxVitality;
            healthText.text = currentVitality + "/" + maxVitality;
            locationText.text = spawnManager.persistentLevel.levelInfo.displayName;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = spawnPoint;
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
            }
            StartCoroutine(CameraToggleDelay());
            InvokePlayerInfoChange();
        }

        public void ToggleCamera(bool isActive)
        {
            CameraShift.instance.ToggleDamping(isActive);
            virtualCamera.enabled = isActive;
        }

        public void SetSpawn(Vector2 loc, SpawnManager mgr)
        {
            if (mgr != spawnManager)
            {
                spawnPoint = loc;
                spawnManager = mgr;
                InvokePlayerInfoChange();
            }
        }

        private IEnumerator CameraToggleDelay()
        {
            yield return new WaitForSeconds(0.2f);
            ToggleCamera(true);
        }
    }
}
