using System.Web;
using System.Web.Optimization;

namespace RestAPI
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/jquery-2.2.3.min.js",
					  "~/Scripts/tether.min.js",
					  "~/Scripts/bootstrap.min.js",
					  "~/Scripts/mdb.min.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/bundles/css").Include(
					  "~/Content/css/font-awesome.min.css",
					  "~/Content/css/bootstrap.min.css",
					  "~/Content/css/mdb.min.css",
					  "~/Content/css/style.css"));
		}
	}
}
