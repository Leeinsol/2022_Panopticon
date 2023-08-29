using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameClear : MonoBehaviour
{
    public GameObject oneStar;
    public GameObject twoStar;
    public GameObject threeStar;

    public SharedData sharedData;

    public List<StageStar> myStageList;
    string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/MyStageText.txt";

        Load();

        Debug.Log(PlayerPrefs.GetInt("remainTower"));
        

        if (PlayerPrefs.GetInt("remainTower")/10000f >= 0.8f)
        {
            //StageSetting.Instance.SetStar(sharedData.stage, 3);
            setStar(sharedData.stage, 3);

            return;
        }
        else if (PlayerPrefs.GetInt("remainTower") / 10000f >= 0.4f)
        {
            //StageSetting.Instance.SetStar(sharedData.stage, 2);
            setStar(sharedData.stage, 2);

            threeStar.SetActive(false);
        }
        else
        {
            //StageSetting.Instance.SetStar(sharedData.stage, 1);
            setStar(sharedData.stage, 1);

            twoStar.SetActive(false);
            threeStar.SetActive(false);
        }

    }

    void setStar(string mode, int currentStar)
    {
        StageStar stageStar = myStageList.Find(x => x.Name == mode);
        int origin = int.Parse(stageStar.StarNum);

        if (origin < currentStar)
        {
            if (stageStar != null)
            {
                stageStar.StarNum = currentStar.ToString();
            }
            Save();
        }
    }
    void Save()
    {
        string jdata = JsonUtility.ToJson(new Serialization<StageStar>(myStageList));

        File.WriteAllText(filePath, jdata);
    }

    void Load()
    {
        //if (!File.Exists(filePath)) { ResetStar(); return; }

        string jdata = File.ReadAllText(filePath);
        myStageList = JsonUtility.FromJson<Serialization<StageStar>>(jdata).target;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
