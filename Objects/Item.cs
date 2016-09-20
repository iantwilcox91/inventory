using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inventory.Objects
{
  public class Item
  {
    private string _description;
    private int _id;

    public Item(string description, int id = 0)
    {
      _description = description;
      _id = id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public int GetId()
    {
      return _id;
    }
    public void SetDescription(string description)
    {
      _description = description;
    }
    public void SetId(int id)
    {
      _id = id;
    }
    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        return (idEquality && descriptionEquality);
      }
    }
    public static List<Item> GetAll()
    {
      List<Item> allItem = new List<Item>{};

      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM items", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while ( rdr.Read() )
      {
        int ItemId = rdr.GetInt32(1);
        string ItemDescription = rdr.GetString(0);
        Item newItem = new Item(ItemDescription, ItemId);
        allItem.Add(newItem);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allItem;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      String command = "INSERT INTO items (description) OUTPUT INSERTED.id VALUES (@Desription);" ;
      SqlCommand cmd = new SqlCommand(command, conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@Desription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read() )
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Item Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items WHERE id = @ItemId;", conn);
      SqlParameter itemIdParameter = new SqlParameter();
      itemIdParameter.ParameterName = "@itemId";
      itemIdParameter.Value = id.ToString();
      cmd.Parameters.Add(itemIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundItemId = 0;
      string foundItemDescription = null;
      while(rdr.Read())
      {
        foundItemId = rdr.GetInt32(1);
        foundItemDescription = rdr.GetString(0);
      }
      Item foundItem = new Item(foundItemDescription, foundItemId);

      if (rdr !=null)
      {
        rdr.Close();
      }
      if (conn !=null)
      {
        conn.Close();
      }
      return foundItem;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM items WHERE id = @itemId;", conn);

      SqlParameter itemIdParameter = new SqlParameter();
      itemIdParameter.ParameterName = "@itemId";
      itemIdParameter.Value = this.GetId();
      cmd.Parameters.Add(itemIdParameter);
      cmd.ExecuteNonQuery();
         if (conn !=null)
         {
           conn.Close();
         }
       }


    public static void DeleteAll()
    {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM items;", conn);
    cmd.ExecuteNonQuery();
    conn.Close();
    }
  }
}
