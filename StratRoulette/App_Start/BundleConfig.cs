using System.Web;
using System.Web.Optimization;

namespace StratRoulette
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/image-picker").Include(
						"~/Scripts/image-picker.min.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.min.js",
					  "~/Scripts/respond.min.js",
					  "~/Scripts/app.min.js",
					  "~/Scripts/image-picker.min.js",
					  "~/Scripts/chartist.min.js",
					  "~/Scripts/jquery.dataTables.min.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/css/bootstrap.min.css",
						"~/Content/css/site.css",
						"~/Content/css/AdminLTE.min.css",
						"~/Content/css/skins/_all-skins.min.css",
						"~/Content/css/ionicons.min.css",
						"~/Content/css/font-awesome.min.css",
						"~/Content/css/image-picker.css",
						"~/Content/css/chartist.min.css",
						"~/Content/css/jquery.dataTables.min.css"));
		}
	}
}
