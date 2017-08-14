using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Inventory.Models
{
  public class Collectable
  {
    private int _id;
    private string _description;
    private int _categoryId;

    public Collectable(string description, int categoryId = 0, int id = 0)
    {
      _description = description;
      _id = id;
      _categoryId = categoryId;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }
    public string GetDescription()
    {
      return _description;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `collectables` (`description`, `categoryId`) VALUES (@CollectablesDescription, @CollectablesCategoryId);";

      Console.WriteLine("Description for the collectable you just created: " + _description);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@CollectablesDescription";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@CollectablesCategoryId";
      categoryId.Value = this._categoryId;
      cmd.Parameters.Add(categoryId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
    }

    public static Collectable Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `collectables` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int collectableId = 0;
      string collectableDescription = "";
      int collectableCategoryId = 0;

      while(rdr.Read())
      {
        collectableId = rdr.GetInt32(0);
        collectableDescription = rdr.GetString(1);
        collectableCategoryId = rdr.GetInt32(2);
      }

      Collectable foundCollectable = new Collectable(collectableDescription, collectableCategoryId, collectableId);
      return foundCollectable;
    }

    public static List<Collectable> GetAll()
    {
      List<Collectable> allCollectables = new List<Collectable>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM collectables;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int collectableId = rdr.GetInt32(0);
        string collectableDescription = rdr.GetString(1);
        int collectableCategoryId = rdr.GetInt32(2);

        Collectable newCollectable = new Collectable(collectableDescription, collectableCategoryId, collectableId);

        allCollectables.Add(newCollectable);
      }
      return allCollectables;
    }

  }
}
