using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class SaveData : ScriptableObject
{
    private string path;
    private string file;

    private SqliteConnection conn;
    private SqliteCommand cmd;

    //default constructor
    private void OnEnable()
    {
        file = "TestDatabase.db"; //TODO: get database file to load and set it into file
        path = "URI=file:" + Application.streamingAssetsPath + "/Save/" + file;

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
    }

    private void CloseConnection()
    {
        conn.Close();
        conn = null;
    }

    private void SubscribeToEvents()
    {
        Openable.OnOpenableStateChanged += WriteOpenableStateChange;
    }

    private void UnsubscribeFromEvents()
    {
        Openable.OnOpenableStateChanged -= WriteOpenableStateChange;
    }

    private void WriteOpenableStateChange(int id, bool isOpened, bool isLocked)
    {
        cmd.CommandText = "UPDATE Openables SET isOpened = @isOpened, isLocked = @isLocked WHERE id = @id";
        cmd.Parameters.Add("@isOpened", DbType.Boolean).Value = isOpened;
        cmd.Parameters.Add("@isLocked", DbType.Boolean).Value = isLocked;
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        cmd.ExecuteNonQuery();
    }

    public bool[] ReadOpenableState(int id, string name)
    {
        cmd.CommandText = "SELECT isOpened, isLocked FROM Openables WHERE id = @id";
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        SqliteDataReader reader = cmd.ExecuteReader();
        bool[] result = new bool[2];
        try
        {
            if (id == 0)
            {
                throw new Exception("the id of openable (" + name + ") has not been set in the editor.");
            }
            if (reader.Read())
            {
                result[0] = reader.GetBoolean(0);
                result[1] = reader.GetBoolean(1);
                //Debug.Log(id + ", isOpen=" + result[0] + ", isLocked=" + result[1]);
            }
            else
            {
                throw new Exception("the id (" + id + ") of openable (" + name + ") does not exist in the Openables table.");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            reader.Close();
            reader = null;
        }
        return result;
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
