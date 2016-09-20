using System;
using Xunit;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Inventory;
using Inventory.Objects;

namespace Testing
{
  public class Tests : IDisposable
  {
    public Tests()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_tests;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test1_TestForAnEmptyDatabase_True()
    {
        int result = Item.GetAll().Count;
        Assert.Equal(0, result);
    }
    [Fact]
    public void Test2_TestForCorrectSave_true()
    {
      Item newItem = new Item("Computer");

      newItem.Save();
      Item result = Item.GetAll()[0];

      Assert.Equal(newItem, result);
    }

    [Fact]
    public void Test3_Find_IteminDatabase()
    {
      Item testItem = new Item("Car");
      testItem.Save();

      Item foundItem = Item.Find(testItem.GetId());

      Assert.Equal(testItem, foundItem);
    }

    [Fact]
    public void Test4_DeleteOneItem_True()
    {
      Item firstItem =new Item("Computer");
      firstItem.Save();

      Item secondItem = new Item("Car");
      secondItem.Save();

      secondItem.Delete();
      List<Item> allItems = Item.GetAll();
      List<Item> afterDeleteFristItem = new List<Item> {firstItem};

      Assert.Equal(afterDeleteFristItem, allItems);
    }
//   tests










    public void Dispose()
    {
      Item.DeleteAll();
    }
  }
}
