using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Title_Star : MonoBehaviour
{
    public GameObject[] StageButton;


    //GameObject[] easyStars;
    //GameObject[] normalStars;
    //GameObject[] hardStars;
    List<GameObject> easyStars = new List<GameObject>();
    List<GameObject> normalStars = new List<GameObject>();
    List<GameObject> hardStars = new List<GameObject>();


    public List<StageStar> myStageList;
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/MyStageText.txt";

        Load();
    }

    void Load()
    {
        string jdata = File.ReadAllText(filePath);
        myStageList = JsonUtility.FromJson<Serialization<StageStar>>(jdata).target;
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


            //easyStars[i - 1] =StageButton[1].gameObject.transform.GetChild(i).gameObject; //easy
            //normalStars[i - 1] = StageButton[2].gameObject.transform.GetChild(i).gameObject; //normal
            //hardStars[i - 1] = StageButton[3].gameObject.transform.GetChild(i).gameObject; //hard

        }
    }
    void StageStar()
    {
        FindStarImage();
        if (GetStarNum("easy")== 2)
        {
            easyStars[2].SetActive(false);
        }
        else if (GetStarNum("easy") == 1)
        {
            easyStars[1].SetActive(false);
            easyStars[2].SetActive(false);
        }
        else if (GetStarNum("easy")== 0)
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
    // Update is called once per frame
    void Update()
    {
        StageStar();
    }
}
