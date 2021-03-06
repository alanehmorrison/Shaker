﻿using Microsoft.AspNet.Identity;
using Shaker.Model;
using Shaker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shaker.WebMVC.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ShoppingService(userId);
            var model = service.GetShopping();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShoppingCreate model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var service = CreateShoppingService();
            
            if (service.CreateShopping(model))
            { 
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Note could not be created.");

            return View();
        }

        public ActionResult Details(int id)
        {
            var svc = CreateShoppingService();
            var model = svc.GetShoppingById(id);
            
        return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateShoppingService();
            var detail = service.GetShoppingById(id);
            var model =
                new ShoppingEdit
                {
                    ShoppingId = detail.ShoppingId,
                    ShoppingLiquor = detail.ShoppingLiquor,
                    ShoppingFruit = detail.ShoppingFruit,
                    ShoppingJuice = detail.ShoppingJuice,
                    ShoppingOther = detail.ShoppingOther
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ShoppingEdit model)
        {
            if (!ModelState.IsValid) return View(model);
            
            if (model.ShoppingId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            
             var service = CreateShoppingService();
            
             if (service.UpdateShopping(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }
             
            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }

        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateShoppingService();
            var model = svc.GetShoppingById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateShoppingService();
            
            service.DeleteShopping(id);
            
            TempData["SaveResult"] = "Your note was deleted";
            
             return RedirectToAction("Index");
        }

        private ShoppingService CreateShoppingService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ShoppingService(userId);
            return service;
        }
    }
}