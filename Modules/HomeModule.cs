using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Inventory.Objects;



namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["newItemForm.cshtml"];
      };

      Post["/itemList"] = _ => {
        Item newItem = new Item(Request.Form["item-name"]);
        newItem.Save();
        List<Item> allItem = Item.GetAll();
        return View["ItemList.cshtml", allItem];
      };

      Post["/deleted"] = _ =>{
        Item.DeleteAll();
        return View["deleted.cshtml"];
      };

    }
  }
}
