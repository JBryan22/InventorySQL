using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using System.Collections.Generic;
using System;

namespace Inventory.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
    [HttpGet("/categories/new")]
    public ActionResult CategoryForm()
    {
      return View();
    }
    [HttpGet("/categories")]
    public ActionResult CategoriesGet()
    {
      return View("Categories", Category.GetAll());
    }
    [HttpPost("/categories")]
    public ActionResult Categories()
    {
      string newType = Request.Form["category-type"];

      Category newCategory = new Category(newType);
      newCategory.Save();

      return View(Category.GetAll());
    }

    [HttpGet("/categories/{id}")]
    public ActionResult CategoryDetail(int id)
    {
      Dictionary<string, object> model = new Dictionary<string,object>();

      Category selectedCategory = Category.Find(id);
      List<Collectable> categoryCollectables = selectedCategory.GetCollectables();

      model.Add("category", selectedCategory);
      model.Add("collectables", categoryCollectables);
      return View(model);
    }

    [HttpPost("/categories/{id}")]
    public ActionResult CategoryDetailPost(int id)
    {
      string collectableDescription = Request.Form["collectable-description"];
      Collectable newCollectable = new Collectable(collectableDescription, id);
      newCollectable.Save();

      Dictionary<string, object> model = new Dictionary<string,object>();

      Category selectedCategory = Category.Find(id);
      List<Collectable> categoryCollectables = selectedCategory.GetCollectables();

      model.Add("category", selectedCategory);
      model.Add("collectables", categoryCollectables);
      return View("CategoryDetail",model);
    }

    [HttpGet("/categories/{id}/collectables/new")]
    public ActionResult CollectableForm(int id)
    {
      Dictionary<string, object> model = new Dictionary<string,object>();
      Category selectedCategory = Category.Find(id);
      List<Collectable> allCollectables = selectedCategory.GetCollectables();
      model.Add("category",selectedCategory);
      model.Add("collectables",allCollectables);
      return View(model);
    }

    [HttpPost("/categories/{id}/clear")]
    public ActionResult CollectableClear(int id)
    {
      Category selectedCategory = Category.Find(id);
      selectedCategory.ClearAll();

      return View("Categories", Category.GetAll());
    }

    [HttpPost("/categories/{id}/remove")]
    public ActionResult CategoryClear(int id)
    {
      Category selectedCategory = Category.Find(id);
      selectedCategory.ClearAll();
      selectedCategory.ClearCategory();

      return View("Categories", Category.GetAll());
    }
  }
}
