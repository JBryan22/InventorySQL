using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Inventory.Models
{
  public class Category
  {
    private int _id;
    private string _type;

    public Category(string type, int id = 0)
    {
      _type = type;
      _id = id;
    }

    public string GetCategoryType()
    {
      return _type;
    }

    public int GetId()
    {
      return _id;
    }

    public List<Collectable> GetCollectables()
    {
      List<Collectable> myCollectables = new List<Collectable>();
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM collectables WHERE categoryId = (@thisId);";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = this._id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int collectableId = rdr.GetInt32(0);
        string collectableDescription = rdr.GetString(1);
        int collectableCategoryId = rdr.GetInt32(2);

        Collectable newCollectable = new Collectable(collectableDescription, collectableCategoryId, collectableId);

        myCollectables.Add(newCollectable);
      }
      return myCollectables;
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }
      return allCategories;
    }

    public override bool Equals(System.Object otherCategory)
    {
      if(!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool descriptionEquality = (this.GetCategoryType() == newCategory.GetCategoryType());
        return descriptionEquality;
      }
    }
    public override int GetHashCode()
    {
      return this.GetCategoryType().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `categories` (`type`) VALUES (@CategoryType);";

      MySqlParameter type = new MySqlParameter();
      type.ParameterName = "@CategoryType";
      type.Value = this._type;
      cmd.Parameters.Add(type);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
    }
    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `categories` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int categoryId = 0;
      string categoryType = "";

      while(rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
        categoryType = rdr.GetString(1);
      }
      Category foundCategory = new Category(categoryType, categoryId);
      return foundCategory;
    }

    public void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM `collectables` WHERE categoryId = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = _id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();
    }

    public void ClearCategory()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM `categories` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = _id;
      cmd.Parameters.Add(thisId);

      cmd.ExecuteNonQuery();
    }
  }
}
