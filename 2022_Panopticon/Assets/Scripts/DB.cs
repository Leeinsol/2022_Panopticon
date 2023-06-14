using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DB : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DB_Control dB_Control = new DB_Control();
        //dB_Control.sqlcmdall("INSERT INTO testTable VALUES('1', '2')");
        dB_Control.sqlcmdall("show tables");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
