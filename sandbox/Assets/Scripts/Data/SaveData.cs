using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SaveData : ScriptableObject
{
    private string path;
    private string file;

    private SqliteConnection conn;
    private SqliteCommand cmd;
    private SqliteDataReader reader;

    //default constructor
    private void OnEnable()
    {
        file = "TestDatabase.db"; //TODO: get database file to load and set it into file
        path = "URI=file:" + Application.dataPath + "/Data/Save/" + file;

        SubscribeToEvents();
        OpenConnection();
    }

    //default destructor
    private void OnDisable()
    {
        CloseConnection();
        UnsubscribeFromEvents();
    }

    private void OpenConnection()
    {
        conn = new SqliteConnection(path);
        conn.Open();

        cmd = conn.CreateCommand();
        string sqlQuery = "SELECT value,name, randomSequence " + "FROM PlaceSequence";
        cmd.CommandText = sqlQuery;

        reader = cmd.ExecuteReader();
    }

    private void CloseConnection()
    {
        reader.Close();
        reader = null;
        cmd.Dispose();
        cmd = null;
        conn.Close();
        conn = null;
    }

    private void ReadData(string query)
    {
        cmd.CommandText = query;
    }

    private void WriteData(string query)
    {

    }

    private void SubscribeToEvents()
    {
        SideDoor.OnSideDoorStateChanged += WriteOpenableStateChange;
        Chest.OnChestStateChanged += WriteOpenableStateChange;
        BackDoor.OnBackDoorStateChanged += WriteOpenableStateChange;
    }

    private void UnsubscribeFromEvents()
    {
        SideDoor.OnSideDoorStateChanged -= WriteOpenableStateChange;
        Chest.OnChestStateChanged -= WriteOpenableStateChange;
        BackDoor.OnBackDoorStateChanged -= WriteOpenableStateChange;
    }

    private void WriteOpenableStateChange(int id, bool isOpened, bool isLocked)
    {
        //check to see if openable record exists in openable table
        //if it does, update the opened and locked values for the selected id
        //otherwise, add a record with the id, opened, and locked values.
        Debug.Log("The openable of id " + id + " is opened=" + isOpened + ", locked=" + isLocked);
    }

    private void WritePlayerItemChange(List<InventoryItem> items, bool add)
    {
        if (add)
        {
            //check to see if item record exists in character item table via the string name of the item
            //if it does, update the record with the item quantity.
            //otherwise, add a record with the string name with the given quantity.
        }
        else
        {
            //check to see if item record exists in character item table via the string name of the item
            //if it does, decrement the quantity
            //otherwise, we have a problem :(
        }
    }

    private void WritePlayerInfo(float health, float gold, Vector2 checkpoint, string[] equipped)
    {
        //update the record in the playerinfo table with each of the five parameters
    }

    private void WriteUnlockedLocation(int id)
    {
        //update the record in the location table with the fact that it has been visited
    }

    private void WriteCharacterLocation(string name, int level, Vector2 loc)
    {
        //update the record in the characterinfo table with the specified name with each of the three parameters
    }

    private void WriteCharacterState(string name, bool isHostile, int conversation, int gold)
    {
        //update the record in the characterstate table with the specified name with each of the three parameters
    }

    private void WriteCharacterInventory(string name, List<InventoryItem> items, bool buy)
    {
        //update the record in the characterinventory table with the specified name with each of the three parameters
        if (buy)
        {
            //check to see if item record exists in characterinventory table via the string name of the item
            //if it does, update the record with the item quantity.
            //otherwise, add a record with the string name with the given quantity.
        }
        else
        {
            //check to see if item record exists in characterinventory table via the string name of the item
            //if it does, decrement the quantity
            //otherwise, we have a problem :(
        }
    }

    private void WriteQuestState(string name, int step)
    {
        //update the record in the queststate table with the specified current step
    }

    private void WriteBossDefeated(int id)
    {
        //update the bosses table to remove this boss so that it does not spawn again in the future
    }

    private void WriteLooseItem(int id, bool add, string[] nameAndLevel, Vector2 loc)
    {
        if (add)
        {
            //add record to table id, name, level, xloc, yloc
        }
        else
        {
            //remove record from table with id
        }
    }
}
