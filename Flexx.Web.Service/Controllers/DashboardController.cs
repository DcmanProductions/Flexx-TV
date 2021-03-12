﻿using com.drewchaseproject.net.Flexx.Core.Data;
using com.drewchaseproject.net.Flexx.Media.Libraries;
using com.drewchaseproject.net.Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }
        public IActionResult Overview()
        {
            return View();
        }
        public IActionResult Movies()
        {
            return View();
        }
        [Route("/Movies/{movieName}")]
        public IActionResult MediaView(string movieName)
        {
            MovieModel movie = LibraryListModel.Singleton.GetByName("movies").Movies.GetByName(movieName);
            return View(movie);
        }

    }
}
