using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Analytics : MonoBehaviour
{
    private List<ObjectiveInteraction> objectives = new List<ObjectiveInteraction>();
    private List<GeneralInteraction> generals = new List<GeneralInteraction>();

    private float lastDistance;

    private readonly string _savegameFolder = "Analytics";

    private Picovoice Picovoice;

    public void setLastDistance(float distance)
    {
        lastDistance = distance;
    }

    private void Start()
    {
        Picovoice = GameObject.Find("Picovoice").GetComponent<Picovoice>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Cursor.lockState = CursorLockMode.None;
            Picovoice.Delete();
            SceneManager.LoadScene("StartMenu");
        }
    }

    public float getLastDistance()
    {
        return lastDistance;
    }

    public void AddObjective(string objective)
    {
        GeneralInteraction general = generals[generals.Count - 1];

        ObjectiveInteraction objectiveInteraction =
            new ObjectiveInteraction(objective, general.Intent, general.Time, general.Character, general.Distance);
        objectives.Add(objectiveInteraction);
    }

    public void AddGeneral(string intent, float time, string character, float distance)
    {
        GeneralInteraction generalInteraction =
            new GeneralInteraction(intent, time.ToString().Replace(',', '.'), character,
                distance.ToString().Replace(',', '.'));
        generals.Add(generalInteraction);
    }

    public void CreateCSV()
    {
        WriteToCSV();
    }

    public void WriteToCSV()
    {
        if (!Directory.Exists(_savegameFolder))
        {
            Directory.CreateDirectory(_savegameFolder);
        }

        using (var streamWriter =
               new StreamWriter(Path.Combine(_savegameFolder, SceneManager.GetActiveScene().name + ".csv")))
        {
            int maxLength = Math.Max(generals.Count, objectives.Count);

            for (int i = 0; i < maxLength; i++)
            {
                string line = "";
                if (i < generals.Count)
                {
                    line += string.Format("{0},{1},{2},{3}", generals[i].Intent, generals[i].Time,
                        generals[i].Character, generals[i].Distance);
                }

                line += ", ,";
                if (i < objectives.Count)
                {
                    line += string.Format("{0},{1},{2},{3},{4}", objectives[i].Objective, objectives[i].Intent,
                        objectives[i].Time,
                        objectives[i].Character, objectives[i].Distance);
                }

                streamWriter.WriteLine(line);
            }

            streamWriter.Flush();
            streamWriter.Close();
        }
    }
}

public class ObjectiveInteraction
{
    public ObjectiveInteraction(string objective, string intent, string time, string character, string distance)
    {
        Objective = objective;
        Intent = intent;
        Time = time;
        Character = character;
        Distance = distance;
    }

    public string Objective;
    public string Intent;
    public string Time;
    public string Character;
    public string Distance;
}

public class GeneralInteraction
{
    public GeneralInteraction(string intent, string time, string character, string distance)
    {
        Intent = intent;
        Time = time;
        Character = character;
        Distance = distance;
    }

    public string Intent;
    public string Time;
    public string Character;
    public string Distance;
}