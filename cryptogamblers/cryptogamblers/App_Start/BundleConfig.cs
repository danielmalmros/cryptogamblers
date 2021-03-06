﻿using System.Web;
using System.Web.Optimization;

namespace cryptoGamblers
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/queue").Include(
                       "~/Scripts/queue/queue.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/circles").Include(
                      "~/Scripts/circles.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/GameEnigne").Include(
                      "~/Scripts/GameEngine/Engine.js"));

            bundles.Add(new StyleBundle("~/bundles/match").Include(
                     "~/Content/Match/index.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Login/LoginForm.css",
					  "~/Content/Styles/Nav.css",
                      "~/Content/Styles/UserProfile.css",
                      "~/Content/_loginPartial/_loginPartial.css",
					  "~/Content/HallOfFame/HallOfFame.css",
					  "~/Content/Match/index.css",
					  "~/Content/Styles/mediaqueries.css"));
        }
    }
}
