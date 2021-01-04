using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour{

    #region Variables variables
    //Turns out HideInInspector is a wonderful little tool to a clearner life. 
    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public List<string> interationDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<int> exitIDs = new List<int>();
    [HideInInspector] public List<string> tempExitLocations = new List<string>();
    public Room[] Rooms;
    public Race[] Races;
    public Class[] Classes;
    public Enemy[] Enemies;
    public Attack[] Attacks;
    public Loot[] Common, Uncommon, Rare, Epic, Legendary;
    [HideInInspector] private int[] currentExitLocations;

    //Enabling and disabling choices, pretty clumbsy not going to lie but it works. 
    private bool Choice1, Choice2, Choice3, Choice4, StartIsActive, RaceSelected, TitleIsActive, FirstRoom;
    private int ChoiceAmount;
    [SerializeField] private bool isAlive, isEnemyAlive, CombatEnd, CombatCanHappen,CombatIsActive;

    public int RoomTotalCount;

    [HideInInspector] public bool Button1Triggered, Button2Triggered, Button3Triggered, Button4Triggered;


    //Turns out you view s in inspector which I personally find annoying.
    private Dictionary<int, Room> roomDictionary = new Dictionary<int, Room>();
    private Dictionary<int, Race> raceDictionary = new Dictionary<int, Race>();
    private Dictionary<int, Class> classDictionary = new Dictionary<int, Class>();
    private Dictionary<int, Loot> lootDictionary = new Dictionary<int, Loot>();
    private Dictionary<int, Attack> knownAttackDictionary = new Dictionary<int, Attack>();
    private Dictionary<int, Attack> attackDictionary = new Dictionary<int, Attack>();

    [HideInInspector] public Character Character;
    [HideInInspector] public List<string> raceChoices = new List<string>();
    [HideInInspector] public List<int> raceID = new List<int>();
    [HideInInspector] public List<string> classChoices = new List<string>();
    [HideInInspector] public List<int> classID = new List<int>();

    public DiceRoller RollA;
    public int RollOutcome;

    Attack currentAttack;
    Race currentRace;
    Class currentClass;
    Loot currentLoot;
    Loot LootReward;
    public HP HP;

    private int chosenRoom, chosenRace, chosenClass, chosenAttack;

    private int EncounterCooldown;
    private float CombatEncounterCheck;
    private bool PlayerTurn, EnemyTurn;

    List<string> actionLog = new List<string>();

    public TextMeshProUGUI displayText;
    public TextMeshProUGUI characterNameRaceClass, characterLevel, characterGold, HPText;
    public TextMeshProUGUI strText, dexText, conText, intText, wisText, chaText;
    private int STR, DEX, CON, INT, WIS, CHA, Gold, Level, experiencePoints;
    private float C_Chance, UC_Chance, R_Chance, E_Chance, L_Chance;
    bool AttacksSet, EnemyLoaded;

    public GameObject HPBarVisual, Stats;

    public ScreenShake ScreenShake;

    private List<int> AttackIDList = new List<int>();
    private List<string> AttackDescriptions = new List<string>();
    private List<string> AttackDescriptionsSuccess = new List<string>();
    private List<string> AttackDescriptionsFailure = new List<string>();
    private List<string> AttackDescSmall = new List<string>();
    private List<int> AttackDiceType = new List<int>();
    private List<int> AttackDiceAmount = new List<int>();
    private List<int> AttackBonus = new List<int>();
    private List<int> AttackDC = new List<int>();

    public int[] AttackID_Array;
    public string[] AttackDescription_Array;
    public string[] AttackDescriptionSuccess_Array;
    public string[] AttackDescriptionFailure_Array;
    public string[] AttackDescSmall_Array;
    public int[] AttackDiceType_Array;
    public int[] AttackDiceAmount_Array;
    public int[] AttackBonus_Array;
    public int[] AttackDC_Array;

    private bool firstRoomAssigned; 
    
    #endregion

    // Use this for initialization
    void Awake() {

        roomNavigation = GetComponent<RoomNavigation>();
        //Anim = GetComponent<Animator>();

        //Adding all rooms, character stuff and enemies to arrays to be added to dictionarys 
        Rooms = Resources.LoadAll("Rooms", typeof(Room)).Cast<Room>().ToArray();
        Races = Resources.LoadAll("PlayerObjects/Races", typeof(Race)).Cast<Race>().ToArray();
        Classes = Resources.LoadAll("PlayerObjects/Classes", typeof(Class)).Cast<Class>().ToArray();
        Enemies = Resources.LoadAll("Enemys", typeof(Enemy)).Cast<Enemy>().ToArray();
        Attacks = Resources.LoadAll("Attacks", typeof(Attack)).Cast<Attack>().ToArray();

        //Loot Loading
        Common = Resources.LoadAll("PlayerObjects/Loot/Common", typeof(Loot)).Cast<Loot>().ToArray();
        Uncommon = Resources.LoadAll("PlayerObjects/Loot/Uncommon", typeof(Loot)).Cast<Loot>().ToArray();
        Rare = Resources.LoadAll("PlayerObjects/Loot/Rare", typeof(Loot)).Cast<Loot>().ToArray();
        Epic = Resources.LoadAll("PlayerObjects/Loot/Epic", typeof(Loot)).Cast<Loot>().ToArray();
        Legendary = Resources.LoadAll("PlayerObjects/Loot/Legendary", typeof(Loot)).Cast<Loot>().ToArray();



        //KeyValuePair stuff for roomDictionary, adding each room with an ID so that I can call the room based on the choice.
        for (var i = 0; i < Rooms.Length; i++)
        {
            var t = Rooms[i];
            roomNavigation.currentRoom = t;
            roomDictionary.Add(roomNavigation.currentRoom.roomID, t);

            //My solution to seeing if the ScriptableObject rooms are lining up with there actual roomIDs
            Debug.Log("Room ID: " + roomNavigation.currentRoom.roomID + " AND Room Object: " + Rooms[i]);
        }

        foreach (var t in Races)
        {
            currentRace = t;
            raceDictionary.Add(currentRace.RaceID, t);
            //Debug.Log(raceDictionary[i+1] + "matches to the ID of:" + currentRace.RaceID);
        }
        foreach (var t in Classes)
        {
            currentClass = t;
            classDictionary.Add(currentClass.ClassID, t);
            //Debug.Log(classDictionary[i+1] + "matches to the ID of:" + currentClass.ClassID);
        }
        foreach (var t in Common)
        {
            currentLoot = t;
            lootDictionary.Add(t.LootID, t);
        }
        foreach (var t in Uncommon)
        {
            currentLoot = t;
            lootDictionary.Add(t.LootID, t);
        }
        foreach (var t in Rare)
        {
            currentLoot = t;
            lootDictionary.Add(t.LootID, t);
        }
        foreach (var t in Epic)
        {
            currentLoot = t;
            lootDictionary.Add(t.LootID, t);
        }
        foreach (var t in Legendary)
        {
            currentLoot = t;
            lootDictionary.Add(t.LootID, t);
        }

        StartIsActive = true;
        TitleIsActive = true;
        AttacksSet = false;
        isAlive = true;
        EnemyLoaded = false;
        Level = 1;
        actionLog.Clear();
    }

    private void Start()
    {
        TextBob.TextBob1();
    }

    #region Rooms

    //-----------------------------------------------------------------------------------------
    //                                   Room  Loading
    //-----------------------------------------------------------------------------------------

    private void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());

        displayText.text = logAsText;
    }

    private void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();
        UnpackRoom();

        string joinedInteractionDescriptions = string.Join("\n", interationDescriptionsInRoom.ToArray());

        string exitLocations = string.Join("\n", tempExitLocations.ToArray());

        string combinedText = roomNavigation.currentRoom.description + "\n" + "\n" + exitLocations;

        LogStringWithReturn(combinedText);
    }

    private void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
        currentExitLocations = exitIDs.ToArray();
        ChoiceAmount = currentExitLocations.Length;

        Choice1 = false;
        Choice2 = false;
        Choice3 = false;
        Choice4 = false;

        switch (ChoiceAmount)
        {
            case 1:
                Choice1 = true;
                break;
            case 2:
                Choice1 = true;
                Choice2 = true;
                break;
            case 3:
                Choice1 = true;
                Choice2 = true;
                Choice3 = true;
                break;
            case 4:
                Choice1 = true;
                Choice2 = true;
                Choice3 = true;
                Choice4 = true;
                break;
        }
    }

    private void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
        DisplayLoggedText();
    }

    void ClearCollectionsForNewRoom()
    {
        interationDescriptionsInRoom.Clear();
    }

    public void ConvertExits()
    {
        exitIDs.ToArray();
    }


    private void ProgressToNextRoom()
    {
        Debug.Log("Room Proceed");
        exitIDs.Clear();
        tempExitLocations.Clear();

        CombatEncounterCheck = Random.value * 100;

        if (firstRoomAssigned)
        {
            if(CombatEncounterCheck < 20.0f && CombatCanHappen && !CombatIsActive)
            {
                CombatIsActive = true;
                LoadEnemy(0);

            }
            else
            {
                Debug.Log("ThirdTrigger");

                roomNavigation.currentRoom = roomDictionary[chosenRoom];
                DisplayRoomText();
                DisplayLoggedText();
            }
        }
        else
        {
            DisplayRoomText();
            DisplayLoggedText();
        }

        RoomTotalCount++;
    }
    
    #endregion

    #region CharacterCreation
    

    //-----------------------------------------------------------------------------------------
    //                              Character Creation Loading
    //-----------------------------------------------------------------------------------------

    #region Arrays arrays

    private List<int> AbilityScores = new List<int>();
    private List<int> AbilityScoreRaceMod = new List<int>();

    private List<int> AbilityScoreMod = new List<int>();
    private List<int> AbilityScoreFinal = new List<int>();
    private List<int> AbilityScoreSwitch = new List<int>();

    private int[] AS_Array;
    private int[] ASM_Array;
    private string[] ASM_Array_String;
    private int[] AS_Array_Switch;

    #endregion

    private void LoadChosenCharacter()
    {
        //Change Chacter Name
        string CharacterClass = classDictionary[chosenClass].className;
        currentClass = classDictionary[chosenClass];
        string CharacterRace = raceDictionary[chosenRace].raceName;
        currentRace = raceDictionary[chosenRace];

        characterNameRaceClass.text = "Klara the " + CharacterRace + " " + CharacterClass;
        Gold = classDictionary[chosenClass].startingGold;
        characterGold.text = "Gold: " + classDictionary[chosenClass].startingGold;
        characterLevel.text = "Level: " + Level;

        HP.totalHP = classDictionary[chosenClass].HP;
        HP.currentHP = classDictionary[chosenClass].HP;
        HPText.text = HP.currentHP + "/" + HP.totalHP + "HP";


        //Compile Character Ability Scores
        for (int i = 0; i < 6; i++)
        {
            AbilityScores.Add(classDictionary[chosenClass].abilityScores[i]);
            AbilityScoreRaceMod.Add(raceDictionary[chosenRace].raceMod[i]);

        }

        for (int i = 0; i < 6; i++)
        {
            float tempFloat = (float)AbilityScores[i] + AbilityScoreRaceMod[i];
            Mathf.Round(tempFloat / 2);
            AbilityScoreFinal.Add(AbilityScores[i] + AbilityScoreRaceMod[i]);

            int tempInt = (int)tempFloat;
            AbilityScoreSwitch.Add(tempInt);
        }

        AS_Array = AbilityScoreFinal.ToArray();
        AS_Array_Switch = AbilityScoreSwitch.ToArray();

        CalculateAbilityBonus();
        
        //UnpackClassAttacks();
        
        StartIsActive = false;
        firstRoomAssigned = true;
        DisplayRoomText();
        DisplayLoggedText();

        HPBarVisual.SetActive(true);
        Stats.SetActive(true);

    }

    private void CalculateAbilityBonus()
    {
        for (int i = 0; i < AS_Array.Length; i++)
        {
            if (AS_Array_Switch[i] == 1)                                   AbilityScoreMod.Add(-5);
            else if (AS_Array_Switch[i] == 2 || AS_Array_Switch[i] == 3)   AbilityScoreMod.Add(-4);
            else if (AS_Array_Switch[i] == 4 || AS_Array_Switch[i] == 5)   AbilityScoreMod.Add(-3);
            else if (AS_Array_Switch[i] == 6 || AS_Array_Switch[i] == 7)   AbilityScoreMod.Add(-2);
            else if (AS_Array_Switch[i] == 8 || AS_Array_Switch[i] == 9)   AbilityScoreMod.Add(-1);
            else if (AS_Array_Switch[i] == 10 || AS_Array_Switch[i] == 11) AbilityScoreMod.Add(0);
            else if (AS_Array_Switch[i] == 12 || AS_Array_Switch[i] == 13) AbilityScoreMod.Add(1);
            else if (AS_Array_Switch[i] == 14 || AS_Array_Switch[i] == 15) AbilityScoreMod.Add(2);
            else if (AS_Array_Switch[i] == 16 || AS_Array_Switch[i] == 17) AbilityScoreMod.Add(3);
            else if (AS_Array_Switch[i] == 18 || AS_Array_Switch[i] == 19) AbilityScoreMod.Add(4);
        }

        ASM_Array = AbilityScoreMod.ToArray();
        ASM_Array_String = ASM_Array.Select(x => x.ToString()).ToArray();

        strText.text = "STR" + "\n" + AS_Array[0] + "(" + ASM_Array_String[0] + ")";
        dexText.text = "DEX" + "\n" + AS_Array[1] + "(" + ASM_Array_String[1] + ")";
        conText.text = "CON" + "\n" + AS_Array[2] + "(" + ASM_Array_String[2] + ")";
        intText.text = "INT" + "\n" + AS_Array[3] + "(" + ASM_Array_String[3] + ")";
        wisText.text = "WIS" + "\n" + AS_Array[4] + "(" + ASM_Array_String[4] + ")";
        chaText.text = "CHA" + "\n" + AS_Array[5] + "(" + ASM_Array_String[5] + ")";
    }

    private void DisplayRaceChoices()
    {
        UnpackCharacterStuff();

        string raceChoiceString = string.Join("\n", raceChoices.ToArray());
        string combinedTextRace = "What race do you want to take on this campaign?" + "\n" + "\n" + raceChoiceString;

        LogStringWithReturn(combinedTextRace);
        DisplayLoggedText();
        raceChoices.Clear();
    }

    private void DisplayClassChoices(string ChosenRace)
    {
        string classChoiceString = string.Join("\n", classChoices.ToArray());
        string combinedTextClass = "What does your " + ChosenRace + "'s job in this world?" + "\n" + "\n" + classChoiceString;

        LogStringWithReturn(combinedTextClass);
        DisplayLoggedText();
        classChoices.Clear();
    }

    private void UnpackCharacterStuff()
    {
        for (int i = 0; i < Races.Length; i++)
        {
            string tempRaceChoice = (i + 1 + "." + Races[i]);
            raceChoices.Add(tempRaceChoice);

        }

        for (int i = 0; i < Classes.Length; i++)
        {
            string tempClassChoice = (i + 1 + "." + Classes[i]);
            classChoices.Add(tempClassChoice);
        }


    }

    private void UpdateGoldIncrease(int goldIncrease)
    {
        Gold += goldIncrease;
        characterGold.text = "Gold: " + Gold;
        
    }
    
    private void UpdateGoldDecrease(int goldDecrease)
    {
        Gold += goldDecrease;
        characterGold.text = "Gold: " + Gold;
    }
    
    #endregion

    #region VisualLoading
    
    //-----------------------------------------------------------------------------------------
    //                              Visual Loading etc.
    //-----------------------------------------------------------------------------------------

    [FormerlySerializedAs("AreaLocation")] public GameObject areaLocation;


    //Currently not in use

    private IEnumerator SceneTransition()
    {
        //Debug.Log("StartedRoutine1");
        //FadeAnimation.TriggerAnimation();
        StartCanvas.SetActive(false);
        DisplayRaceChoices();
        TitleIsActive = false;
        yield return null;
    }

    public GameObject StartCanvas;
    bool ClassChosen;
    bool RaceChosen;
    
    #endregion

    #region CombatSystem

    
    
    //-----------------------------------------------------------------------------------------
    //                              Combat System
    //-----------------------------------------------------------------------------------------

    bool BattleWon, FleeorCharm, Death;
    string tempName;
    int tempEnemyHP;
    private int tempXP;
    int tempEnemyDamage;
    int tempEnemyCR;
    bool lootFind;
    string combinedLootText;
    int RewardValue;
    int Reward;
    bool CommonFind, UncommonFind, RareFind, EpicFind, LegendaryFind;
    bool StartText;

    bool ContinueCombat = true;

    void LoadEnemy(int enemyID)
    {
        tempXP = Enemies[enemyID].enemyXPValue;
        tempName = Enemies[enemyID].enemyName;
        tempEnemyHP = Enemies[enemyID].enemyHP;
        tempEnemyDamage = Enemies[enemyID].enemyDamage;
        tempEnemyCR = Enemies[enemyID].enemyCR;

        StartText = true;

        CombatEncounter();      
    }

    private void CombatEncounter()
    {

        if (CombatIsActive)
        {
            if (StartText)
            {
                StartText = false;
                string Introduction = "While moving to the next room you get attacked by a " + tempName + "!";
                LogStringWithReturn(Introduction);
                PlayerTurnMethod();
            }
        }
    }
    

    private void UnpackPlayerAttacks()
    {
        for (var i = 0; i < currentClass.attacks.Length; i++)
        {
            //Attack Visuals.
            var tempAttackString = (currentClass.attacks[i].attackDescriptionBase);
            AttackDescriptions.Add(tempAttackString);

            var tempAttackStringSuccess = (currentClass.attacks[i].attackDescriptionSuccess);
            AttackDescriptionsSuccess.Add(tempAttackStringSuccess);

            var tempAttackStringFailure = (currentClass.attacks[i].attackDescriptionFailure);
            AttackDescriptionsFailure.Add(tempAttackStringFailure);

            var attackDescSmalls = (i + 1 + "." + currentClass.attacks[i].attackDescSmall);
            AttackDescSmall.Add(attackDescSmalls);


            //Attack Data

            AttackIDList.Add(currentClass.attacks[i].AttackID);
            AttackDiceAmount.Add(currentClass.attacks[i].damageDiceAmount);
            AttackDiceType.Add(currentClass.attacks[i].damageDiceType);
            AttackBonus.Add(currentClass.attacks[i].attackBonus);
            AttackDC.Add(currentClass.attacks[i].AttackDC);

        }

        

        AttackDescription_Array = AttackDescriptions.ToArray();
        AttackDescriptionSuccess_Array = AttackDescriptionsSuccess.ToArray();
        AttackDescriptionFailure_Array = AttackDescriptionsFailure.ToArray();
        AttackDescSmall_Array = AttackDescSmall.ToArray();
        AttackID_Array = AttackIDList.ToArray();
        AttackDiceAmount_Array = AttackDiceAmount.ToArray();
        AttackDiceType_Array = AttackDiceType.ToArray();
        AttackBonus_Array = AttackBonus.ToArray();

        AttacksSet = true;
        
    }

    private void AttackHitCheck(int attackID)
    {
        string attackString = AttackDescription_Array[0];

        LogStringWithReturn(attackString);
        
        WhatDiceToRoll(AttackDiceType_Array[0]);
        
        string attackSuccessString = AttackDescriptionSuccess_Array[0];
        string damageAmount = " You did " + RollOutcome + " damage to the " + tempName;
        tempEnemyHP -= RollOutcome;
        string combinedAttackString = attackSuccessString + damageAmount;

        LogStringWithReturn(combinedAttackString);
        
        if (tempEnemyHP <= 0)
        {
            BattleWon = true;
            ExitCombat();
        }
        else
        {
            EnemyTurnMethod();
        }
            
        
            

    }

    private void WhatDiceToRoll(int diceData)
    {
        int diceAmount = 3;

        switch (diceData)
        {
            case 4:
                RollA.d4(diceAmount);
            break;
            case 6:
                RollA.d6(diceAmount);
                break;
            case 8:
                RollA.d8(diceAmount);
                break;
            case 10:
                RollA.d10(diceAmount);
                break;
            case 12:
                RollA.d12(diceAmount);
                break;
            case 20:
                RollA.d20(diceAmount);
                break;

        }
    }
    
    private void PlayerTurnMethod()
    {
        if (!AttacksSet)
        {
            UnpackPlayerAttacks();
        }

        var joinedAttackDescriptions = string.Join("\n", AttackDescSmall.ToArray());

        string combinedText = "You need to fight back. What do you do?" + "\n" + joinedAttackDescriptions;
        
        LogStringWithReturn(combinedText);

        Debug.Log("BOI");
    }

    private void EnemyTurnMethod()
    {

        if (!BattleWon)
        {
            float enemyDamage = Random.Range(1, 5);
            int intDamage = (int)enemyDamage;
            string goblinAttackSpeech = "The" + tempName + " attacks you. For " + intDamage + " Damage!";
            HP.currentHP -= intDamage;
            HP.CalculateHealthSize();
        
            LogStringWithReturn(goblinAttackSpeech);
            

            if (!isAlive)
            {
                Death = true;
                ExitCombat();
            }
            else
            {
                PlayerTurnMethod();
            }
        }
        else
        {
            ContinueCombat = false;
            CombatCanHappen = false;
            ExitCombat();
        }
        
    }



    private void ExitCombat()
    {

        if (BattleWon)
        {
            Debug.Log("Reward");
            int battleReward = Random.Range(1, 11);
            //CheckForLoot(tempEnemyCR);

            string lootString;

            var combatExitWinGold = "With a final strike the " + tempName + " falls to the ground motionless... You search its body and find; "
                + battleReward + " gold piece(s). ";

            if (lootFind)
            {

                lootString = "Also on the body you find a '" + LootReward.name + "' which is worth roughly about: " + LootReward.GoldValue.ToString() + " gold pieces! ";
            }
            else
            {
                lootString = "";
            }

            int totalGainedGold = battleReward;
            string exitMessage = "Now that the " + tempName + " is dead you can continue your quest.." + "\n";
            combinedLootText = string.Concat(combatExitWinGold, lootString, exitMessage);
            experiencePoints += tempXP;
            CombatIsActive = false;
            
            LogStringWithReturn(combinedLootText);

            Gold += totalGainedGold;
            characterGold.text = "Gold: " + Gold;
            ExperienceManager();
            
            
            
        }

//        if (FleeorCharm)
//        {
//            string CharmFleeString = "You managed to run away from the creature (For now you will continue your quest, in the future you will end up in a random room)";
//            LogStringWithReturn(CharmFleeString);
//            ProgressToNextRoom();
//        }

        if (Death)
        {
            LogStringWithReturn("You died. Good job. Restart Game to try again");
            
        }

        CombatIsActive = false;

    }

    private void CheckForLoot(int CR)
    {
        switch (CR)
        {
            case 1:
                C_Chance = 25f;
                UC_Chance = 12.5f;
                R_Chance = 6.75f;
                E_Chance = 3.32f;
                L_Chance = 1.66f;
                break;
            case 2:
                C_Chance = 30f;
                UC_Chance = 15f;
                R_Chance = 7.5f;
                E_Chance = 3.75f;
                L_Chance = 1.87f;
                break;
        }

        float lootChance = Random.value * 100;
 

        if(lootChance < C_Chance)
        {
            RewardValue++;
            if(lootChance < UC_Chance)
            {
                RewardValue++;
                if (lootChance < R_Chance)
                {
                    RewardValue++;
                    if (lootChance < E_Chance)
                    {
                        RewardValue++;
                        if (lootChance < L_Chance)
                        {
                            RewardValue++;
                        }
                    }
                }
            }
        }
        else
        {
            RewardValue = 0;
        }

        if(RewardValue != 0)
        {
            ChooseReward(RewardValue);
            lootFind = true;
        }
        else
        {
            lootFind = false;
        }

    }

    private void ChooseReward(int RewardType)
    {
        int selectedLootItem;

        if (RewardType == 1)
        {
            selectedLootItem = Random.Range(Common.Length - 1, Common.Length + 1);
            Reward = Common[selectedLootItem].LootID;

        }
        if (RewardType == 2)
        {
            selectedLootItem = Random.Range(Uncommon.Length - 1, Uncommon.Length + 1);
            Reward = Uncommon[selectedLootItem].LootID;

        }
        if (RewardType == 3)
        {
            selectedLootItem = Random.Range(Rare.Length - 1, Rare.Length + 1);
            Reward = Rare[selectedLootItem].LootID;

        }
        if (RewardType == 4)
        {
            selectedLootItem = Random.Range(Epic.Length - 1, Epic.Length + 1);
            Reward = Epic[selectedLootItem].LootID;

        }
        if (RewardType == 5)
        {
            selectedLootItem = Random.Range(Legendary.Length - 1, Legendary.Length + 1);
            Reward = Legendary[selectedLootItem].LootID;

        }

        LootReward = lootDictionary[Reward];

    }
    #endregion

    #region XPManager

    


    //-----------------------------------------------------------------------------------------
    //                              Character Level's Experience
    //-----------------------------------------------------------------------------------------


    void ExperienceManager()
    {
        var previousLevel = Level;
        
        if(experiencePoints < 10)
        {
            Level = 1;
        }
        if(experiencePoints > 10)
        {
            Level = 2;
        }
        if(experiencePoints > 25)
        {
            Level = 3;
        }

        if (Level != previousLevel)
        {
            LogStringWithReturn("[LEVEL UP!] You are now Level: " + Level);
            characterLevel.text = "Level: " + Level;
            HP.totalHP += 5;
            HP.currentHP += 5;
            HP.CalculateHealthSize();
        }
        
        ProgressToNextRoom();
    }
    

    #endregion

    void Update ()
    {
        //AnimatorClipInfo[] fadeInfo = this.Anim.GetCurrentAnimatorClipInfo(0);
        //float FadeLength = fadeInfo[0].clip.length;
        //Debug.Log(FadeLength);

        HPText.text = HP.currentHP + "/" + HP.totalHP + "HP";
        
        if(RoomTotalCount == 2)
        {
            CombatCanHappen = true;

        }


        //Restart Button
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        
        if (Button1Triggered || Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (StartIsActive)
            {
                if (TitleIsActive)
                {
                    StartCoroutine(SceneTransition());
                }
                else
                {
                    if (!RaceChosen)
                    {
                        chosenRace = 1;
                        RaceChosen = true;
                        DisplayClassChoices("Dwarf");
                    }
                    else
                    {
                        if (!ClassChosen)
                        {
                            chosenClass = 1;
                            ClassChosen = true;
                            LoadChosenCharacter();
                        }
                    }
                }
            }
            else
            {
                if (CombatIsActive)
                {
                    chosenAttack = AttackID_Array[0];               
                    AttackHitCheck(chosenAttack);
                    Button1Triggered = false;

                }
                else if(firstRoomAssigned)
                {
                    chosenRoom = currentExitLocations[0];
                    ProgressToNextRoom();
                    Button1Triggered = false;
                }
                if(!firstRoomAssigned)
                {
                    ProgressToNextRoom();
                    Button1Triggered = false;
                    firstRoomAssigned = true;
                }
            }
            
            Button1Triggered = false;
            
        }

        if (Button2Triggered || Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (StartIsActive)
            {
                if (TitleIsActive)
                {
                    StartCoroutine(SceneTransition());
                }
                else
                {
                    if (!RaceChosen)
                    {
                        chosenRace = 2;
                        RaceChosen = true;
                        DisplayClassChoices("Elf");
                    }
                    else
                    {
                        if (!ClassChosen)
                        {
                            chosenClass = 2;
                            ClassChosen = true;
                            LoadChosenCharacter();
                        }
                    }
                }
            }
            else
            {
                if (CombatIsActive)
                {
                    chosenAttack = AttackID_Array[1];               
                    AttackHitCheck(chosenAttack);
                }
                else if(firstRoomAssigned)
                {
                    chosenRoom = currentExitLocations[1];
                    ProgressToNextRoom();
                }
                if(!firstRoomAssigned)
                {
                    ProgressToNextRoom();
                    firstRoomAssigned = true;
                }
            }
            
            Button2Triggered = false;
        }

        if (Button3Triggered || Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (StartIsActive)
            {
                if (TitleIsActive)
                {
                    StartCoroutine(SceneTransition());
                }
                else
                {
                    if (!RaceChosen)
                    {
                        chosenRace = 3;
                        RaceChosen = true;
                        DisplayClassChoices("Human");
                    }
                    else
                    {
                        if (!ClassChosen)
                        {
                            chosenClass = 3;
                            ClassChosen = true;
                            LoadChosenCharacter();
                        }
                    }
                }
            }
            else
            {
                if (CombatIsActive)
                {
                    chosenAttack = AttackID_Array[2];               
                    AttackHitCheck(chosenAttack);
                }
                else if(firstRoomAssigned)
                {
                    Debug.Log("TriggerFirst");
                    chosenRoom = currentExitLocations[2];
                    ProgressToNextRoom();
                }
                if(!firstRoomAssigned)
                {
                    Debug.Log("TriggerSecond");
                    ProgressToNextRoom();
                    firstRoomAssigned = true;
                }
            }
            
            Button3Triggered = false;
        }

        
    

        if (Button4Triggered || Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (StartIsActive)
            {
                if (TitleIsActive)
                {
                    StartCoroutine(SceneTransition());
                }
                else
                {
                    if (!RaceChosen)
                    {
                        chosenRace = 4;
                        RaceChosen = true;
                        DisplayClassChoices("Tiefling");
                    }
                    else
                    {
                        if (!ClassChosen)
                        {
                            chosenClass = 4;
                            ClassChosen = true;
                            LoadChosenCharacter();
                        }
                    }
                }
            }
            else
            {
                if (CombatIsActive)
                {
                    chosenAttack = AttackID_Array[3];               
                    AttackHitCheck(chosenAttack);
                    Button1Triggered = false;

                }
                else if(firstRoomAssigned)
                {
                    chosenRoom = currentExitLocations[3];
                    ProgressToNextRoom();
                    Button1Triggered = false;
                }
                if(!firstRoomAssigned)
                {
                    ProgressToNextRoom();
                    Button1Triggered = false;
                    firstRoomAssigned = true;
                }
            }
            
            Button4Triggered = false;
        }
    }
}
