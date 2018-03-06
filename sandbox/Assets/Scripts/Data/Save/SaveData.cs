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

    // default constructor
    private void OnEnable()
    {
        file = "TestDatabase.db"; //TODO: get database file to load and set it into file
        path = "URI=file:" + Application.streamingAssetsPath + "/Save/" + file;

        SubscribeToEvents();
        OpenConnection();
    }

    // default destructor
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
        PlayerInventory.OnInventoryItemChanged += WritePlayerItem;
        Player.OnPlayerInfoChanged += WritePlayerInfo;
        //WriteUnlockedCheckpoint
        //WriteCharacterInfo
        //WriteCharacterItem
        //WriteQuestStep
        //WriteBossDefeated
    }

    private void UnsubscribeFromEvents()
    {
        Openable.OnOpenableStateChanged -= WriteOpenableStateChange;
        PlayerInventory.OnLooseItemChanged -= WriteLooseItem;
        PlayerInventory.OnInventoryItemChanged -= WritePlayerItem;
        Player.OnPlayerInfoChanged -= WritePlayerInfo;
    }

    private void WriteOpenableStateChange(int id, OpenableTuple tuple)
    {
        cmd.CommandText = "UPDATE Openables SET isOpened = @isOpened, isLocked = @isLocked WHERE id = @id";
        cmd.Parameters.Add("@isOpened", DbType.Boolean).Value = tuple.isOpen;
        cmd.Parameters.Add("@isLocked", DbType.Boolean).Value = tuple.isLocked;
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        cmd.ExecuteNonQuery();
    }

    // Gets the state of an openable interactable
    public OpenableTuple ReadOpenableState(int id, string name)
    {
        cmd.CommandText = "SELECT isOpened, isLocked FROM Openables WHERE id = @id";
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        SqliteDataReader reader = cmd.ExecuteReader();
        OpenableTuple tuple = new OpenableTuple();
        try
        {
            if (id == 0)
            {
                throw new Exception("the id of openable (" + name + ") has not been set in the editor.");
            }
            if (reader.Read())
            {
                tuple.isOpen = reader.GetBoolean(reader.GetOrdinal("isOpened"));
                tuple.isLocked = reader.GetBoolean(reader.GetOrdinal("isLocked"));
                Debug.Log(id + ", isOpen=" + tuple.isOpen + ", isLocked=" + tuple.isLocked);
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
        return tuple;
    }

    // Sets an update to the player character's inventory
    private void WritePlayerItem(ItemTuple item)
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
                    throw new Exception("attempted to write an item change to PlayerItems table with a record " +
                        "that does not exist and a quantity of zero.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets the list of items that are in the player inventory
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

    // Sets the updated state of the player character
    private void WritePlayerInfo(PlayerInfoTuple playerInfo)
    {
        try
        {
            cmd.CommandText = "UPDATE PlayerInfo SET maxHealth = @maxHealth, currentHealth = @currentHealth, " +
                "currentQuest = @currentQuest, gold = @gold, checkpointName = @checkpointName, " +
                "equippedArmor = @equippedArmor, equippedWeapon = @equippedWeapon";
            cmd.Parameters.Add("@maxHealth", DbType.Int32).Value = playerInfo.maxHealth;
            cmd.Parameters.Add("@currentHealth", DbType.Int32).Value = playerInfo.currentHealth;
            cmd.Parameters.Add("@currentQuest", DbType.String).Value = playerInfo.currentQuest;
            cmd.Parameters.Add("@gold", DbType.Int32).Value = playerInfo.gold;
            cmd.Parameters.Add("@checkpointName", DbType.String).Value = playerInfo.checkpointName;
            cmd.Parameters.Add("@equippedArmor", DbType.String).Value = playerInfo.equippedArmor;
            cmd.Parameters.Add("@equippedWeapon", DbType.String).Value = playerInfo.equippedWeapon;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets the information related to the player character
    public PlayerInfoTuple ReadPlayerInfo()
    {
        var playerInfo = new PlayerInfoTuple();
        cmd.CommandText = "SELECT maxHealth, currentHealth, currentQuest, gold, " +
            "checkPointName, equippedArmor, equippedWeapon FROM PlayerInfo";
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (reader.Read())
            {
                playerInfo.maxHealth = reader.GetInt32(reader.GetOrdinal("maxHealth"));
                playerInfo.currentHealth = reader.GetInt32(reader.GetOrdinal("currentHealth"));
                playerInfo.currentQuest = reader.GetString(reader.GetOrdinal("currentQuest"));
                playerInfo.gold = reader.GetInt32(reader.GetOrdinal("gold"));
                playerInfo.checkpointName = reader.GetString(reader.GetOrdinal("checkPointName"));
                playerInfo.equippedArmor = reader.GetString(reader.GetOrdinal("equippedArmor"));
                playerInfo.equippedWeapon = reader.GetString(reader.GetOrdinal("equippedWeapon"));
                Debug.Log("maxHealth=" + playerInfo.maxHealth + ", currentHealth=" + playerInfo.currentHealth +
                    ", currentQuest=" + playerInfo.currentQuest + ", gold=" + playerInfo.gold +
                    ", checkpointName=" + playerInfo.checkpointName + ", equippedArmor=" + playerInfo.equippedArmor +
                    ", equippedWeapon=" + playerInfo.equippedWeapon);
            }
            else
            {
                throw new Exception("No record exists within the PlayerInfo table.");
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
        return playerInfo;
    }

    // Sets that a specified checkpoint has been unlocked
    private void WriteUnlockedCheckpoint(string name)
    {
        try
        {
            cmd.CommandText = "UPDATE Checkpoints SET isUnlocked = @true WHERE name = @name";
            cmd.Parameters.Add("@isUnlocked", DbType.Boolean).Value = true;
            cmd.ExecuteNonQuery();
            Debug.Log("Updated checkpoint " + name + " in Checkpoints to be unlocked.");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets whether a selected checkpoint has been unlocked
    public bool GetCheckpointUnlocked(string name)
    {
        cmd.CommandText = "SELECT isUnlocked FROM Checkpoints WHERE name = @name";
        cmd.Parameters.Add("@name", DbType.String).Value = name;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (name == null)
            {
                throw new Exception("A checkpoint name has not been set in the editor.");
            }
            if (reader.Read())
            {
                bool result = reader.GetBoolean(reader.GetOrdinal("isUnlocked"));
                Debug.Log("Checkpoint=" + name + ", isUnlocked=" + result);
                return result;
            }
            else
            {
                throw new Exception("The id of checkpoint (" + name + ") does not exist in the Checkpoints table.");
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
        return false;
    }

    // Updates the state of a non-player character (specifically related to dialogue)
    private void WriteCharacterInfo(CharacterInfoTuple tuple)
    {
        try
        {
            cmd.CommandText = "UPDATE CharacterInfo SET level = @level, xLoc = @xLoc, " +
                "yLoc = @yLoc, conversation = @conversation, gold = @gold WHERE name = @name";
            cmd.Parameters.Add("@name", DbType.String).Value = tuple.name;
            cmd.Parameters.Add("@level", DbType.String).Value = tuple.level;
            cmd.Parameters.Add("@xLoc", DbType.Decimal).Value = tuple.xLoc;
            cmd.Parameters.Add("@yLoc", DbType.Decimal).Value = tuple.yLoc;
            cmd.Parameters.Add("@conversation", DbType.Int32).Value = tuple.conversation;
            cmd.Parameters.Add("@gold", DbType.Int32).Value = tuple.gold;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets the list of characters that exist within a level (to be loaded)
    public List<CharacterInfoTuple> ReadCharacterInfo(string level)
    {
        List<CharacterInfoTuple> tuples = new List<CharacterInfoTuple>();
        cmd.CommandText = "SELECT name, xLoc, yLoc, conversation, gold FROM CharacterInfo WHERE level = @level";
        cmd.Parameters.Add("@name", DbType.String).Value = name;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (level == null)
            {
                throw new Exception("A level name has not been set in the editor in LevelLoaders.");
            }
            while (reader.Read())
            {
                CharacterInfoTuple tuple = new CharacterInfoTuple();
                tuple.name = reader.GetString(reader.GetOrdinal("name"));
                tuple.level = level;
                tuple.xLoc = reader.GetFloat(reader.GetOrdinal("xLoc"));
                tuple.yLoc = reader.GetFloat(reader.GetOrdinal("yLoc"));
                tuple.conversation = reader.GetInt32(reader.GetOrdinal("conversation"));
                tuple.gold = reader.GetInt32(reader.GetOrdinal("gold"));
                Debug.Log("Character: name=" + tuple.name + ", level=" + tuple.level +
                    ", xLoc=" + tuple.xLoc + ", yLoc=" + tuple.yLoc +
                    ", conversation=" + tuple.conversation + ", gold=" + tuple.gold);
                tuples.Add(tuple);
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
        return tuples;
    }

    // Sets an update to the non-player character's inventory
    private void WriteCharacterItem(ItemTuple item, string character)
    {
        //check to see if item record exists in player item table via the string name of the item
        cmd.CommandText = "SELECT name FROM CharacterItems WHERE character = @character";
        cmd.Parameters.Add("@name", DbType.String).Value = item.name;
        cmd.Parameters.Add("@character", DbType.String).Value = character;
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
                    cmd.CommandText = "UPDATE CharacterItems SET quantity = @quantity WHERE name = @name AND character = @character";
                    cmd.Parameters.Add("@character", DbType.String).Value = character;
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.Parameters.Add("@quantity", DbType.Int32).Value = item.quantity;
                    cmd.ExecuteNonQuery();
                    Debug.Log("Updated item " + item.name + " for character " + character + " in CharacterItems to quantity " + item.quantity);
                }
                else
                {
                    //if the quantity is zero, remove the record.
                    cmd.CommandText = "DELETE FROM CharacterItems WHERE name = @name AND character = @character";
                    cmd.Parameters.Add("@character", DbType.String).Value = character;
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.ExecuteNonQuery();
                    Debug.Log("Removed item " + item.name + " from CharacterItems for character: " + character);
                }
            }
            else
            {
                reader.Close();
                reader = null;
                if (item.quantity > 0)
                {
                    //otherwise, add a record with the string name with the given quantity.
                    cmd.CommandText = "INSERT INTO CharacterItems (character, name, quantity) VALUES (@character, @name, @quantity)";
                    cmd.Parameters.Add("@character", DbType.String).Value = character;
                    cmd.Parameters.Add("@name", DbType.String).Value = item.name;
                    cmd.Parameters.Add("@quantity", DbType.Int32).Value = item.quantity;
                    cmd.ExecuteNonQuery();
                    Debug.Log("Added item " + item.name + " to character " + character + " in CharacterItems with quantity " + item.quantity);
                }
                else
                {
                    throw new Exception("attempted to write an item change to CharacterItems table with a record " +
                        "that does not exist and a quantity of zero for character: " + character);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets the list of items that are in the specified non-player character's inventory
    public List<ItemTuple> ReadCharacterItems(string character)
    {
        var itemsInInventory = new List<ItemTuple>();
        cmd.CommandText = "SELECT name, quantity FROM CharacterItems WHERE character = @character";
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

    // Sets an update to the selected quest step
    private void WriteQuestStep(string name, int step)
    {
        try
        {
            cmd.CommandText = "UPDATE Quests SET step = @step WHERE name = @name";
            cmd.Parameters.Add("@step", DbType.Int32).Value = step;
            cmd.Parameters.Add("@name", DbType.String).Value = name;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets the current step in a selected quest
    private int GetQuestStep(string name)
    {
        cmd.CommandText = "SELECT step FROM Quests WHERE name = @name";
        cmd.Parameters.Add("@name", DbType.String).Value = name;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (name == null)
            {
                throw new Exception("A quest name has not been set.");
            }
            if (reader.Read())
            {
                int result = reader.GetInt32(reader.GetOrdinal("step"));
                Debug.Log("Quest name=" + name + ", step=" + result);
                return result;
            }
            else
            {
                throw new Exception("The quest (" + name + ") does not exist in the Quests table.");
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
        return 0;
    }

    // Set that the boss with the specified id has been killed
    private void WriteBossDefeated(int id)
    {
        try
        {
            cmd.CommandText = "UPDATE Bosses SET isDefeated = @true WHERE id = @id";
            cmd.Parameters.Add("@isDefeated", DbType.Boolean).Value = true;
            cmd.Parameters.Add("@id", DbType.Int32).Value = id;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets whether a boss has been killed or not
    public bool ReadBossState(int id)
    {
        cmd.CommandText = "SELECT isDefeated FROM Bosses WHERE id = @id";
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        SqliteDataReader reader = cmd.ExecuteReader();
        try
        {
            if (id == 0)
            {
                throw new Exception("A boss id has not been set in the editor.");
            }
            if (reader.Read())
            {
                bool result = reader.GetBoolean(reader.GetOrdinal("isDefeated"));
                Debug.Log("Boss id=" + id + ", isDefeated=" + result);
                return result;
            }
            else
            {
                throw new Exception("The id of a boss (" + id + ") does not exist in the Bosses table.");
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
        return false;
    }

    // Saves a item that has been spawned into a level with the level
    private void WriteLooseItem(LevelItemTuple item, bool add, string level)
    {
        try
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
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Gets a list of items that exist within a level (to be loaded)
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

    // Ensures that the generated ID for an item does not exist in the database already
    public bool ValidateUniqueItemID(int id)
    {
        cmd.CommandText = "SELECT id FROM LooseItems WHERE id = @id";
        cmd.Parameters.Add("@id", DbType.Int32).Value = id;
        SqliteDataReader reader = cmd.ExecuteReader();
        bool result = false;
        try
        {
            if (reader.Read())
            {
                result = false;
            }
            else
            {
                result = true;
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
}
