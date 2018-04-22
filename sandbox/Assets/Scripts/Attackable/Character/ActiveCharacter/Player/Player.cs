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
        public static event Action<PlayerInfoTuple> OnPlayerInfoChanged;

        public int gold;
        public int karma;

        public bool isSavingPrincess;

        public string currentQuest;
        public string equippedArmor;
        public string equippedWeapon;

        public Item weapon;
        public Item armor;

        public Animator weaponAnim;

        public CinemachineVirtualCamera virtualCamera;

        private PlayerInput playerInput;
        private PlayerInventory playerInventory;
        private PlayerQuests playerQuests;
        private TravelMenu travelMenu;

        public Vector2 spawnPoint; //the spawnpoint upon death (one of the fast travel points)
        public SpawnManager spawnManager;

        private SpawnManager travelSpawnManager;

        public TMP_Text healthText;
        public TMP_Text goldText;
        public TMP_Text locationText;
        public TMP_Text questText;

        private FMOD.Studio.EventInstance damageNoise;
        private FMOD.Studio.EventInstance deathNoise;
        private FMOD.Studio.EventInstance healNoise;

        private FMOD.Studio.EventInstance openMenuNoise;
        private FMOD.Studio.EventInstance closeMenuNoise;

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
            playerQuests = GetComponent<PlayerQuests>();
            travelMenu = GetComponent<TravelMenu>();

            healthText.text = currentVitality + "/" + maxVitality;

            //set from CharacterData
            spawnPoint = transform.position;
            canFlinch = false;
            canKnockBack = true;
            canTakeDamage = true;

            SetPlayerInfo();

            damageNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Knight/take_damage");
            damageNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            deathNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Knight/death");
            deathNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            healNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Knight/heal");
            healNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            openMenuNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Menus_Inventory/open_menu");
            openMenuNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));

            closeMenuNoise = FMODUnity.RuntimeManager.CreateInstance("event:/Menus_Inventory/close_menu");
            closeMenuNoise.setVolume(PlayerPrefs.GetFloat("SfxVolume") * PlayerPrefs.GetFloat("MasterVolume"));
        }

        private void SetPlayerInfo()
        {
            PlayerInfoTuple playerInfo = GameData.data.saveData.ReadPlayerInfo();
            maxVitality = playerInfo.maxHealth;

            currentVitality = playerInfo.currentHealth;
            if (currentVitality == 0)
            {
                currentVitality = maxVitality;
            }

            currentQuest = playerInfo.currentQuest;
            gold = playerInfo.gold;
            karma = playerInfo.karma;
            spawnManager = SpawnManager.SetSpawnManager(playerInfo.checkpointName);
            spawnPoint = spawnManager.gameObject.transform.position;
            SetArmor(playerInfo.equippedArmor, isLoad : true);
            SetWeapon(playerInfo.equippedWeapon, isLoad : true);

            transform.position = spawnPoint;

            goldText.text = gold.ToString();
            if (currentQuest != string.Empty)
            {
                questText.text = playerQuests.GetQuestString(currentQuest);
            }
            else
            {
                questText.text = "No active quest.";
            }

            isSavingPrincess = playerInfo.isSavingPrincess;

            locationText.text = spawnManager.persistentLevel.levelInfo.displayName;

            SetSavingCharacter();
        }

        public void PlayOpenMenuNoise()
        {
            openMenuNoise.start();
        }

        public void PlayCloseMenuNoise()
        {
            closeMenuNoise.start();
        }

        private void SetSavingCharacter()
        {
            if (PlayerPrefs.GetInt("IsNewFile") == 1)
            {
                if (PlayerPrefs.GetInt("IsSavingPrince") == 1)
                {
                    isSavingPrincess = false;
                }
                else
                {
                    isSavingPrincess = true;
                }
                InvokePlayerInfoChange();
            }
        }

        public void SetQuest(string name, bool update)
        {
            if (update)
            {
                if (name == currentQuest || currentQuest == "")
                {
                    if (playerQuests.QuestIsActive(name))
                    {
                        currentQuest = name;
                        questText.text = playerQuests.GetQuestString(currentQuest);
                    }
                    else
                    {
                        currentQuest = "";
                        questText.text = "No active quest.";
                    }
                }
            }
            else
            {
                currentQuest = name;
                questText.text = playerQuests.GetQuestString(currentQuest);
                EventDisplay.instance.AddEvent("Set tracked quest to: " + currentQuest);
            }
            InvokePlayerInfoChange();
        }

        public void SetKarma(int delta)
        {
            karma += delta;
            InvokePlayerInfoChange();
        }

        public void SetGold(int delta, bool stolen = false, bool dead = false)
        {
            gold += delta;
            if (gold < 0)
            {
                gold = 0;
            }
            goldText.text = gold.ToString();
            InvokePlayerInfoChange();
            if (delta > 0 && !stolen && !dead)
            {
                EventDisplay.instance.AddEvent("Received " + Mathf.Abs(delta) + " gold.");
            }
            else if (delta <= 0 && !stolen && !dead)
            {
                EventDisplay.instance.AddEvent("Gave " + Mathf.Abs(delta) + " gold.");
            }
            else if (stolen && !dead)
            {
                EventDisplay.instance.AddEvent(Mathf.Abs(delta) + " gold was taken from you.");
            }
            else if (dead)
            {
                EventDisplay.instance.AddEvent("Lost " + Mathf.Abs(delta) + " gold due to dying.");
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
            healNoise.start();
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

        public override void TakeDamage(GameObject attacker, int damage, bool appliesKnockback = true)
        {
            int dmg = damage - GameData.data.itemData.itemDictionary[equippedArmor].strength >= 0 ? Mathf.RoundToInt(damage - GameData.data.itemData.itemDictionary[equippedArmor].strength) : 1;
            if (canTakeDamage)
            {
                damageNoise.start();
            }
            base.TakeDamage(attacker, dmg, appliesKnockback);
            if (currentVitality < 0)
            {
                currentVitality = 0;
            }
            healthText.text = currentVitality + "/" + maxVitality;
            InvokePlayerInfoChange();
            StartCoroutine(InvincibilityDelay());
        }

        private IEnumerator InvincibilityDelay()
        {
            canTakeDamage = false;
            yield return new WaitForSeconds(1.0f);
            canTakeDamage = true;
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

            //death penalty of 5% of gold
            SetGold(-Mathf.RoundToInt(gold * 0.05f), false, true);

            isDying = true;
            anim.SetBool("isDying", isDying);
            weaponAnim.SetBool("isDying", isDying);
            //enemies no longer target player
            StartCoroutine(TravelMenuDelay("You died.", 2.0f));
        }

        private IEnumerator TravelMenuDelay(string type, float time, bool fastTravel = false)
        {
            yield return new WaitForSeconds(time);
            travelMenu.Open(type, false, fastTravel);
        }

        public void InitialLoad()
        {
            playerInput.InvokeSleep();
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
            }
            travelMenu.Open("Start", true);
        }

        public void FastTravel(string location)
        {
            StartCoroutine(TravelMenuDelay("Traveling to " + location + ".", 1.0f, true));
        }

        public override void FinalizeDeath()
        {
            deathNoise.start();
            playerInput.InvokeSleep();
        }

        public void Respawn(bool isDead)
        {
            if (isDead)
            {
                isDying = false;
                anim.SetBool("isDying", isDying);
                weaponAnim.SetBool("isDying", isDying);
                currentVitality = maxVitality;
                healthText.text = currentVitality + "/" + maxVitality;
            }
            locationText.text = spawnManager.persistentLevel.levelInfo.displayName;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = spawnPoint;
            if (spawnManager != null)
            {
                spawnManager.RefreshLevels();
                if (travelSpawnManager != null)
                {
                    travelSpawnManager.FlushLevels();
                    travelSpawnManager = null;
                }
            }
            StartCoroutine(CameraToggleDelay());
            InvokePlayerInfoChange();
        }

        public void ToggleCamera(bool isActive)
        {
            CameraShift.instance.ToggleDamping(isActive);
            virtualCamera.enabled = isActive;
        }

        public void SetSpawn(Vector2 loc, SpawnManager mgr, bool flush = false)
        {
            if (flush)
            {
                travelSpawnManager = spawnManager;
            }
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
