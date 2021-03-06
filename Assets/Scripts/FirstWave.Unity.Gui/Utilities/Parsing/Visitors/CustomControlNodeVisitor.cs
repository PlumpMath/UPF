﻿using FirstWave.Unity.Gui.Panels;
using System;
using System.Xml;
using UnityEngine;

namespace FirstWave.Unity.Gui.Utilities.Parsing.Visitors
{
	public class CustomControlNodeVisitor : IXamlNodeVisitor
	{
		public void Visit(XmlNode node, ParseContext context)
		{
			var control = VisitWithResult(node, context);

			if (control != null)
			{
				if (context.CurrentPanel != null)
					context.CurrentPanel.AddChild(control);
				else
					context.Controls.Add(control);
			}
		}

		public Control VisitWithResult(XmlNode node, ParseContext context)
		{
			var typeName = string.Format("{0}.{1}", node.NamespaceURI, node.LocalName);

			var childType = context.GetCustomControlType(typeName);

			if (childType != null)
			{
				var control = Activator.CreateInstance(childType) as Control;

				XamlProcessor.LoadAttributes(control, node, context);

				// The top-most panel/controls are going to get their DataContexts set to the passed in view model
				if (control.DataContext == null)
					control.DataContext = context.ViewModel;

				if (control is Panel)
					XamlProcessor.LoadPanel(control as Panel, node, context);

				return control;
			}

			Debug.LogError("Could not locate panel class for type: " + typeName);

			return null;
		}
	}
}
