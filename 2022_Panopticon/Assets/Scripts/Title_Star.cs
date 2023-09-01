using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}


[System.Serializable]
public class StageStar
{

    public StageStar(string _Name, string _StarNum) { Name = _Name; StarNum = _StarNum; }

    public string Name, StarNum;
}
public class Title_Star : MonoBehaviour
{
    public GameObject[] StageButton;


    List<GameObject> easyStars = new List<GameObject>();
    List<GameObject> normalStars = new List<GameObject>();
    List<GameObject> hardStars = new List<GameObject>();


    public TextAsset StageDatabase;
    public List<StageStar> stageList, myStageList;
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        FindStarImage();

        string[] line = StageDatabase.text.Substring(0, StageDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; ++i)
        {
            string[] row = line[i].Split('\t');
            stageList.Add(new StageStar(row[0], row[1]));
        }
        filePath = Application.persistentDataPath + "/MyStageText.txt";

        Load();

        StageStar();

    }

    void Load()
    {
        if (!File.Exists(filePath)) { defaultStar(); return; }

        string jdata = File.ReadAllText(filePath);
        myStageList = JsonUtility.FromJson<Serialization<StageStar>>(jdata).target;
    }

    void defaultStar()
    {
        myStageList = new List<StageStar>();
        myStageList.Add(new StageStar("easy", "0"));
        myStageList.Add(new StageStar("normal", "0"));
        myStageList.Add(new StageStar("hard", "0"));
        Debug.Log(myStageList.Count);
    }


    public int GetStarNum(string mode)
    {
        StageStar stageStar = myStageList.Find(x => x.Name == mode);
        return int.Parse(stageStar.StarNum);
    }


    void FindStarImage()
    {
        for (int i = 1; i < 4; i++)
        {
            easyStars.Add(StageButton[0].gameObject.transform.GetChild(i).gameObject); //easy
            normalStars.Add(StageButton[1].gameObject.transform.GetChild(i).gameObject); //normal
            hardStars.Add(StageButton[2].gameObject.transform.GetChild(i).gameObject); //hard
        }
    }
    void StageStar()
    {
        FindStarImage();
        if (GetStarNum("easy") == 2)
        {
            easyStars[2].SetActive(false);
        }
        else if (GetStarNum("easy") == 1)
        {
            easyStars[1].SetActive(false);
            easyStars[2].SetActive(false);
        }
        else if (GetStarNum("easy") == 0)
        {
            easyStars[0].SetActive(false);
            easyStars[1].SetActive(false);
            easyStars[2].SetActive(false);
        }

        if (GetStarNum("normal") == 2)
        {
            normalStars[2].SetActive(false);
        }
        else if (GetStarNum("normal") == 1)
        {
            normalStars[1].SetActive(false);
            normalStars[2].SetActive(false);
        }
        else if (GetStarNum("normal") == 0)
        {
            normalStars[0].SetActive(false);
            normalStars[1].SetActive(false);
            normalStars[2].SetActive(false);
        }

        if (GetStarNum("hard") == 2)
        {
            hardStars[2].SetActive(false);
        }
        else if (GetStarNum("hard") == 1)
        {
            hardStars[1].SetActive(false);
            hardStars[2].SetActive(false);
        }
        else if (GetStarNum("hard") == 0)
        {
            hardStars[0].SetActive(false);
            hardStars[1].SetActive(false);
            hardStars[2].SetActive(false);
        }
    }
    void offStarImage(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(false);
        }
    }

    public void ResetStar()
    {
        foreach (StageStar stageStar in myStageList)
        {
            stageStar.StarNum = "0";
            print(stageStar.StarNum);
        }
        Save();
        Load();
        StageStar();

    }

    void Save()
    {
        string jdata = JsonUtility.ToJson(new Serialization<StageStar>(myStageList));

        File.WriteAllText(filePath, jdata);
    }

    // Update is called once per frame
    void Update()
    {
    }
}