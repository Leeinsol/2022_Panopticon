using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DB_Control : MonoBehaviour
{
    MySqlConnection sqlconn = null;
    private string sqlDBip = "127.0.0.1";
    private string sqlDBname = "test1";
    private string sqlDBid = "root";
    private string sqlDBpw = "bitnami";
    
    private void sqlConnect()
    {
        //DB���� �Է�
        string sqlDatabase = "Server=" + sqlDBip + ";Database=" + sqlDBname + ";UserId=" + sqlDBid + ";Password=" + sqlDBpw + "";

        //���� Ȯ���ϱ�
        try
        {
            sqlconn = new MySqlConnection(sqlDatabase);
            sqlconn.Open();
            Debug.Log("SQL�� ���� ���� : " + sqlconn.State); //������ �Ǹ� OPEN�̶�� ��Ÿ��
    }
        catch (System.Exception msg)
        {
            Debug.Log(msg); //��Ÿ�ٸ������� ��Ÿ���� ������ ���� ������ ��Ÿ��
        }
    }

    private void sqldisConnect()
    {
        sqlconn.Close();
        Debug.Log("SQL�� ���� ���� : " + sqlconn.State); //������ ����� Close�� ��Ÿ�� 
    }
    public void sqlcmdall(string allcmd) //�Լ��� �ҷ��ö� ��ɾ ���� String�� ���ڷ� �޾ƿ�
    {
        sqlConnect(); //����

        MySqlCommand dbcmd = new MySqlCommand(allcmd, sqlconn); //��ɾ Ŀ�ǵ忡 �Է�
        dbcmd.ExecuteNonQuery(); //��ɾ SQL�� ����

        sqldisConnect(); //��������
    }
    public DataTable selsql(string sqlcmd)  //���� ������ DataTable�� ������
    {
        DataTable dt = new DataTable(); //������ ���̺��� ������

        sqlConnect();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmd, sqlconn);
        adapter.Fill(dt); //������ ���̺�  ä���ֱ⸦��
        sqldisConnect();

        return dt; //������ ���̺��� ������
    }

}
