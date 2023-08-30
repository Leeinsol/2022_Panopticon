using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


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
enum Stage
{
    tutorial,
    easy,
    normal,
    hard
};

public class StageSetting : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource SfxSource;

    public TextAsset StageDatabase;
    public List<StageStar> stageList, myStageList;
    string filePath;


    private static StageSetting instance = null;

    [SerializeField]
    Stage stage = Stage.tutorial;



    private void Awake()
    {
        getAudioSource();
        if (instance == null)
        {
            Debug.Log("instance==null");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static StageSetting Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        string[] line = StageDatabase.text.Substring(0, StageDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            stageList.Add(new StageStar(row[0], row[1]));
        }
        filePath = Application.persistentDataPath + "/MyStageText.txt";

        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void getAudioSource()
    {
        BGMSource = GetComponent<AudioSource>();

    }

    void Save()
    {
        string jdata = JsonUtility.ToJson(new Serialization<StageStar>(myStageList));

        File.WriteAllText(filePath, jdata);
    }

    void Load()
    {
        if (!File.Exists(filePath)) { ResetStar(); return; }

        string jdata = File.ReadAllText(filePath);
        myStageList = JsonUtility.FromJson<Serialization<StageStar>>(jdata).target;
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
    }

}
