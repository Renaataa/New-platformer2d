using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class DBConnection //: MonoBehaviour 
{
	IDbConnection dbcon;
	IDbCommand dbcmd;
	IDataReader reader;
	public DBConnection()
	{
		InitConnection();
	}

	public void InitDatabaseConnection()
	{
		OpenDatabase();
		if(!CheckIfUsersTableExists())
		{
			Debug.Log("inside if");
			CreateUsersTable();
		}

		//AddAvatarColorColumn();

		CloseDatabase();
	}

	// void AddAvatarColorColumn()
	// {
	// 	try 
	// 	{
	// 		IDbCommand dbcmd;
	// 		dbcmd = dbcon.CreateCommand();
	// 		string q_add_column = "ALTER TABLE users ADD COLUMN avatarColor TEXT default null";
	// 		dbcmd.CommandText = q_add_column;
	//  		dbcmd.ExecuteReader();
	// 	} 
	// 	catch (Exception ex) 
	// 	{
   	// 		Debug.Log("sql ex: " + ex);
	// 	} 
	// }

	void InitConnection()
	{
		if(dbcon == null)
		{
			// Create database
			string connection = "URI=file:" + Application.persistentDataPath + "/" + "Platformer2D_Database";
			// Open connection
			dbcon = new SqliteConnection(connection);
		}
	}

	void OpenDatabase()
	{
		InitConnection();
	 	dbcon.Open();
		Debug.Log("open db");
	}

	bool CheckIfUsersTableExists()
	{
	 	dbcmd = dbcon.CreateCommand();
	 	string q_select = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='players'";

		dbcmd.CommandText = q_select;
	 	reader = dbcmd.ExecuteReader();
		int count = Convert.ToInt32(reader[0]);
		return (bool)(count==1);
	}

	void CreateUsersTable()
	{
	 	dbcmd = dbcon.CreateCommand();
	 	string q_createTable = "CREATE TABLE IF NOT EXISTS players (id INTEGER PRIMARY KEY, login TEXT , pass TEXT, progress TEXT)";

		dbcmd.CommandText = q_createTable;
	 	dbcmd.ExecuteNonQuery();

		CreateDefaultAccount();
	}

	void CreateDefaultAccount()
	{
		dbcmd = dbcon.CreateCommand();
		string defaultProgress = GetDefaultProgress();
	 	dbcmd.CommandText = "INSERT INTO players (id, login, pass, progress) VALUES (0, 'player','player','"+defaultProgress+"')";
		dbcmd.ExecuteNonQuery();

		defaultProgress = GetDefaultProgress2();
	 	dbcmd.CommandText = "INSERT INTO players (id, login, pass, progress) VALUES (1, 'player2','player2','"+defaultProgress+"')";
	 	dbcmd.ExecuteNonQuery();
	}

	string GetDefaultProgress()
	{
		PlayerProgress progress = new PlayerProgress();
		progress.level =1;
		progress.coins = 0;
		progress.health = 10f;
		progress.energy = 0f;
		PlayerBoost boost = new PlayerBoost();
		boost.health =0;
		boost.jump =0;
		boost.speed =0;
		boost.protect =0;
		progress.boost = boost;
		string json = JsonUtility.ToJson(progress);
		Debug.Log(json);
		return json;
	}

	string GetDefaultProgress2()
	{
		PlayerProgress progress = new PlayerProgress();
		progress.level =11;
		progress.coins = 500;
		progress.health = 10f;
		progress.energy = 0f;
		PlayerBoost boost = new PlayerBoost();
		boost.health =5;
		boost.jump =5;
		boost.speed =5;
		boost.protect =5;
		progress.boost = boost;
		string json = JsonUtility.ToJson(progress);
		Debug.Log(json);
		return json;
	}

	public bool LoginUser(string login ,string pass)
	{
		OpenDatabase();
		bool isCorrect = CheckCredencials(login , pass);
		CloseDatabase();
		return isCorrect;
	}

	bool CheckCredencials(string login ,string pass)
	{
	 	dbcmd = dbcon.CreateCommand();
	 	string q_select = "SELECT count(*) FROM players WHERE login='"+login+"' AND pass='"+pass+"'";
		dbcmd.CommandText = q_select;
	 	reader = dbcmd.ExecuteReader();
		int count = Convert.ToInt32(reader[0]);
		return (bool)(count>0);
	}

	public bool CheckIfLoginUsed(string login)
	{
		OpenDatabase();
	 	dbcmd = dbcon.CreateCommand();
	 	string q_select = "SELECT count(*) FROM players WHERE login='"+login+"'";

		dbcmd.CommandText = q_select;
	 	reader = dbcmd.ExecuteReader();
		int count = Convert.ToInt32(reader[0]);
		CloseDatabase();
		return (bool)(count>0);	
	}

	public void RegisterUser(string login ,string pass)
	{
		OpenDatabase();
	 	dbcmd = dbcon.CreateCommand();
		string defaultProgress = GetDefaultProgress();
	 	dbcmd.CommandText = "INSERT INTO players (login, pass,progress) VALUES ('"+login+"','"+pass+"','"+defaultProgress+"')";
	 	dbcmd.ExecuteNonQuery();
		CloseDatabase();
	}

	public PlayerProgress GetPlayerProgress()
	{
		OpenDatabase();
	 	dbcmd = dbcon.CreateCommand();
	 	string q_select = "SELECT * FROM players WHERE login='"+LoginPanel.USER_LOGIN+"'";
		dbcmd.CommandText = q_select;
	 	reader = dbcmd.ExecuteReader();
		string progressJson = (string)reader[3];
		CloseDatabase();
		PlayerProgress progress = JsonUtility.FromJson<PlayerProgress>(progressJson);
	
		return progress;
	}

	public void UpdatePlayerProgressInDB()
	{
		string json = JsonUtility.ToJson(LoginPanel.loggedPlayerProgress);
		OpenDatabase();
	 	dbcmd = dbcon.CreateCommand();
		string q_update = "UPDATE players SET progress = '"+json+"' WHERE login ='"+LoginPanel.USER_LOGIN+"'";
		dbcmd.CommandText = q_update;
		dbcmd.ExecuteNonQuery();
		CloseDatabase();
	}

	public void ResetPlayerProgress()
	{
		string json = GetDefaultProgress();
		OpenDatabase();
		dbcmd = dbcon.CreateCommand();
		string q_update = "UPDATE players SET progress = '"+json+"' WHERE login ='"+LoginPanel.USER_LOGIN+"'";
		dbcmd.CommandText = q_update;
		dbcmd.ExecuteNonQuery();
		CloseDatabase();
	}
	
	void CloseDatabase()
	{
		dbcon.Close();
	}
}