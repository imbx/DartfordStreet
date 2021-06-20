using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BoxScripts;
using UnityEngine;

public class Database {
    // private Dictionary<int, string>  Dialogues;

    private Dictionary<int, Dialogue> Dialogues;

    private Dictionary<string, string> Interact;
    private Dictionary<int, bool> PlayerProgression;
    public EntityData playerData;

    public Database(EntityData entityData, bool ignoreSavedGame = true) {
        Dialogues = new Dictionary<int, Dialogue>();
        Interact = new Dictionary<string, string>();
        PlayerProgression = new Dictionary<int, bool>();
        playerData = entityData;
        // Try to load
        if(!ignoreSavedGame) LoadGame();
        LoadData();
    }

    private bool LoadData() {
        //Dialogues.Add(1, "Test dialogue");
        ParseDialogueData("Database/" + GameController.current.gameCObject.lang + "/dialogues");
        ParseInteractionsData("Database/" + GameController.current.gameCObject.lang + "/interactions");

        AddProgressionID(-1, true);
        AddProgressionID(0, true);
        AddProgressionID(1, true);

        
        //Notes.Add(5223, new NotebookPage("La llave esta en el jarron"));
        

        AddProgressionID(78325, true);
        AddProgressionID(324234, true);
        AddProgressionID(2342342, true);
        AddProgressionID(232341, true);
        AddProgressionID(646533, true);
        //PlayerProgression.Add(5223, true);
        AddProgressionID(3652, true);
        AddProgressionID(1234, true);
        AddProgressionID(6434, true);
        AddProgressionID(3234, true);
        return true;
    }

    
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.PlayerProgression = PlayerProgression;
        save.playerPosition = new SerializableVector3(playerData.PlayerPosition);
        save.cameraRotation = new SerializableVector3(playerData.CameraRotations);
        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("[Database] Game saved");
    }


    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            Debug.Log("[Database] File exists at " + Application.persistentDataPath + "/gamesave.save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            file.Position = 0;
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            playerData.isLoadingData = true;
            Debug.Log("[Database] Loading game");
            playerData.PlayerPosition = save.playerPosition.Vector3;
            playerData.CameraRotations = save.cameraRotation.Vector3;

            foreach(var item in save.PlayerProgression)
            {
                if(PlayerProgression.ContainsKey(item.Key)) PlayerProgression[item.Key] = item.Value;
                else PlayerProgression.Add(item.Key, item.Value);
            }

            Debug.Log("[Database] Game loaded");

        } else playerData.isLoadingData = false;
    }

    public void ParseDialogueData(string fileName)
    {
        var textAsset = Resources.Load<TextAsset>(fileName);

        Dialogue[] d = JsonHelper.FromJson<Dialogue>(textAsset.text);

        foreach(Dialogue singleDialogue in d)
        {
            Dialogues.Add(singleDialogue.id, singleDialogue);
            Debug.Log("[Database] Dialogue added " + singleDialogue.id + " : "  + singleDialogue.dialogueText);
        }


        /*var splitDataset = textAsset.text.Split(new char[] {'\n'});

        for(int i = 0; i < splitDataset.Length; i++)
        {
            string[] row = splitDataset[i].Split(new char[] {';'});
            if(row.Length == 2)
            {
                Dialogues.Add(int.Parse(row[0]), row[1]);
                Debug.Log("[Database] Dialogue added " + row[0] + " : " + row[1]);
            }
        }*/
    }

    public void ParseInteractionsData(string fileName)
    {
        var textAsset = Resources.Load<TextAsset>(fileName);

        Interactions[] d = JsonHelper.FromJson<Interactions>(textAsset.text);

        foreach(Interactions i in d)
        {
            Interact.Add(i.tag, i.interactionText);
            Debug.Log("[Database] Interaction added " + i.tag + " : "  + i.interactionText);
        }
    }

    public string GetInteraction(string tag)
    {
        if(Interact.ContainsKey(tag)) return Interact[tag];

        return " ";
    }

    public bool EditProgression(int eventID, bool check = true) {
        if(ProgressionExists(eventID)){
            Debug.Log("[Edit Progression] " + eventID + " is now " + check);
            PlayerProgression[eventID] = check;
            return true;
        }
        return false;
    }
    public void AddProgressionID(int _id, bool state = false) {
        if(ProgressionExists(_id)) return;
        PlayerProgression.Add(_id, state);
    }

    public bool ProgressionExists(int eventID) {
        return PlayerProgression.ContainsKey(eventID);
    }

    public bool GetProgressionState(int eventID)
    {
        return ProgressionExists(eventID) ? PlayerProgression[eventID] : false;
    }

    public Dialogue GetDialogue(int dialogueID)
    {
        return Dialogues.ContainsKey(dialogueID) ? Dialogues[dialogueID] : null;
    }
}
[System.Serializable]
public class Save
{
    public Dictionary<int, bool> PlayerProgression;
    public SerializableVector3 playerPosition;
    public SerializableVector3 cameraRotation;
}