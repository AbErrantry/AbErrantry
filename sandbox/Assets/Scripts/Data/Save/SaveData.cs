using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Character2D;
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
    private void OnDestroy()
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
        PlayerInventory.OnLooseItemChanged += WriteLooseItem;
        PlayerInventory.OnInventoryItemChanged += WritePlayerItemChange;
    }

    private void UnsubscribeFromEvents()
    {
        Openable.OnOpenableStateChanged -= WriteOpenableStateChange;
        PlayerInventory.OnLooseItemChanged -= WriteLooseItem;
        PlayerInventory.OnInventoryItemChanged -= WritePlayerItemChange;
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

    private void WritePlayerItemChange(ItemTuple item)
    {
        //check to see if item record exists in player item table via the string name of the item
        cmd.CommandText = "SELECT name FROM PlayerItems WHERE name = @name";
        cmd.Parameters.Add("@name", DbType.String).Value = item.name;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (reader.Read())
            {
                reader.Close();
                reader = null;
                if (item.quantity > 0)
                {
                    //if it does, update the record with the item quantity.
                    cmd.CommandText = "UPDATE PlayerItems SET quantity = @quantity WHERE name = @name";
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.Parameters.Add("@quantity", DbType.Int32).Value = item.quantity;
                    cmd.ExecuteNonQuery();
                    //Debug.Log("Updated item " + item.name + " in PlayerItems to quantity " + item.quantity);
                }
                else
                {
                    //if the quantity is zero, remove the record.
                    cmd.CommandText = "DELETE FROM PlayerItems WHERE name = @name";
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.ExecuteNonQuery();
                    //Debug.Log("Removed item " + item.name + " from PlayerItems");
                }
            }
            else
            {
                reader.Close();
                reader = null;
                if (item.quantity > 0)
                {
                    //otherwise, add a record with the string name with the given quantity.
                    cmd.CommandText = "INSERT INTO PlayerItems (name, quantity) VALUES (@name, @quantity)";
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.Parameters.Add("@quantity", DbType.Int32).Value = item.quantity;
                    cmd.ExecuteNonQuery();
                    //Debug.Log("Added item " + item.name + " to PlayerItems with quantity " + item.quantity);
                }
                else
                {
                    //a record inserted with a quantity zero is not allowed
                    throw new Exception("attempted to write an item change to PlayerItems table with a record that does not exist and a quantity of zero.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public List<ItemTuple> ReadPlayerItems()
    {
        var itemsInInventory = new List<ItemTuple>();
        cmd.CommandText = "SELECT name, quantity FROM PlayerItems";
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                var item = new ItemTuple();
                item.name = reader.GetString(reader.GetOrdinal("name"));
                item.quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                itemsInInventory.Add(item);
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
        return itemsInInventory;
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

    private void WriteLooseItem(LevelItemTuple item, bool add, string level)
    {
        if (add)
        {
            cmd.CommandText = "INSERT INTO LooseItems (id, name, xLoc, yLoc, level) VALUES (@id, @name, @xLoc, @yLoc, @level)";
            cmd.Parameters.Add("@id", DbType.Int32).Value = item.id;
            cmd.Parameters.Add("@name", DbType.String).Value = item.name;
            cmd.Parameters.Add("@xLoc", DbType.Decimal).Value = item.xLoc;
            cmd.Parameters.Add("@yLoc", DbType.Decimal).Value = item.yLoc;
            cmd.Parameters.Add("@level", DbType.String).Value = level;
            cmd.ExecuteNonQuery();
        }
        else
        {
            cmd.CommandText = "DELETE FROM LooseItems WHERE id = @id";
            cmd.Parameters.Add("@id", DbType.Int32).Value = item.id;
            cmd.ExecuteNonQuery();
            //Debug.Log("Removed item " + item.id + " from level " + level);
        }
    }

    public bool ValidateUniqueItemID(int id)
    {
        cmd.CommandText = "SELECT id FROM LooseItems WHERE id = @id";
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        SqliteDataReader reader = cmd.ExecuteReader();
        bool result = false;
        if (reader.Read())
        {
            result = false;
        }
        else
        {
            result = true;
        }
        reader.Close();
        reader = null;
        return result;
    }

    public List<LevelItemTuple> ReadLooseItems(string level)
    {
        var itemsInLevel = new List<LevelItemTuple>();
        cmd.CommandText = "SELECT id, name, xLoc, yLoc FROM LooseItems WHERE level = @level";
        cmd.Parameters.Add("@level", DbType.String).Value = level;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                var item = new LevelItemTuple();
                item.id = reader.GetInt32(reader.GetOrdinal("id"));
                item.name = reader.GetString(reader.GetOrdinal("name"));
                item.xLoc = reader.GetFloat(reader.GetOrdinal("xLoc"));
                item.yLoc = reader.GetFloat(reader.GetOrdinal("yLoc"));
                itemsInLevel.Add(item);
                //Debug.Log(level + ", id=" + item.id + ", name=" + item.name + ", xLoc=" + item.xLoc + ", yLoc=" + item.yLoc);
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
        return itemsInLevel;
    }
}
