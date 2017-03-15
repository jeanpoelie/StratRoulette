namespace StratRoulette.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq.Expressions;
	using System.Web.Mvc;
	using System.Web.Mvc.Html;

	/// <summary>
	/// http://stackoverflow.com/questions/4710447/asp-net-mvc-html-hiddenfor-with-wrong-value
	/// </summary>
	public static class HiddenForExtensions
	{
		public static MvcHtmlString HiddenForSkipModelState<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			ReplacePropertyState(htmlHelper, expression);
			return htmlHelper.HiddenFor(expression);
		}

		public static MvcHtmlString HiddenForSkipModelState<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			ReplacePropertyState(htmlHelper, expression);
			return htmlHelper.HiddenFor(expression, htmlAttributes);
		}

		public static MvcHtmlString HiddenForSkipModelState<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			ReplacePropertyState(htmlHelper, expression);
			return htmlHelper.HiddenFor(expression, htmlAttributes);
		}

		private static void ReplacePropertyState<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			var text = ExpressionHelper.GetExpressionText(expression);
			var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(text);
			var modelState = htmlHelper.ViewContext.ViewData.ModelState;
			var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			if (modelState.ContainsKey(fullName))
			{
				var currentValue = modelState[fullName].Value;
				modelState[fullName].Value = new ValueProviderResult(metadata.Model, Convert.ToString(metadata.Model), currentValue.Culture);
			}
			else
			{
				modelState[fullName] = new ModelState
									   {
										   Value = new ValueProviderResult(metadata.Model, Convert.ToString(metadata.Model), CultureInfo.CurrentUICulture)
									   };
			}
		}
	}
}
