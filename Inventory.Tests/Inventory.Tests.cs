using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventory.Models;
using System;
using System.Collections.Generic;

namespace Inventory.Tests
{

  [TestClass]
  public class InventoryTests : IDisposable
  {
    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrance, Act
      int result = Category.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Category()
    {
      Category firstCategory = new Category("stamps");
      Category secondCategory = new Category("stamps");

      Assert.AreEqual(firstCategory, secondCategory);
    }
    [TestMethod]
    public void Save_SavesToDatabase_CategoryList()
    {
      Category newCategory = new Category("stamps");
      newCategory.Save();
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{newCategory};

      CollectionAssert.AreEqual(testList, result);
    }


    public InventoryTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=inventory;";
      }

    public void Dispose()
      {
        //Category.ClearAll();
      }
  }



}
