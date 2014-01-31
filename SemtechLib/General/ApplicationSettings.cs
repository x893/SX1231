using System;
using System.Collections;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml;

namespace SemtechLib.General
{
	public sealed class ApplicationSettings : IDisposable
	{
		private const string FileName = "ApplicationSettings.xml";
		private const string PathSeperator = "/";
		private const string RootElement = "ApplicationSettings";
		private const string SettingElement = "Setting";

		private XmlDocument Document;

		public ApplicationSettings()
		{
			Document = OpenDocument();
		}

		public void ClearSettings()
		{
			Document = CreateDocument();
		}

		private static XmlDocument CreateDocument()
		{
			XmlDocument document = new XmlDocument();
			document.CreateXmlDeclaration("1.0", null, "yes");
			XmlElement settings = document.CreateElement(RootElement);
			document.AppendChild(settings);
			return document;
		}

		public void Dispose()
		{
			SaveDocument(Document, FileName);
		}

		public Hashtable GetSettings()
		{
			XmlNodeList settings = Document.SelectNodes("/ApplicationSettings/Setting");
			Hashtable settingTable = new Hashtable(settings.Count);
			foreach (XmlNode setting in settings)
				settingTable.Add(setting.Attributes["Name"].Value, setting.Attributes["Value"].Value);
			return settingTable;
		}

		public string GetValue(string Name)
		{
			foreach (XmlNode node in Document.SelectNodes("/ApplicationSettings/Setting"))
				if (node.Attributes["Name"].Value.Equals(Name))
					return node.Attributes["Value"].Value;
			return null;
		}

		private static XmlDocument OpenDocument()
		{

			try
			{
				IsolatedStorageFileStream store = new IsolatedStorageFileStream(FileName, FileMode.Open, FileAccess.Read);
				XmlDocument document = new XmlDocument();
				XmlTextReader reader = new XmlTextReader(store);
				document.Load(reader);
				reader.Close();
				store.Close();
				return document;
			}
			catch (FileNotFoundException)
			{
				return CreateDocument();
			}
		}

		public bool RemoveValue(string Name)
		{
			foreach (XmlNode setting in Document.SelectNodes("/ApplicationSettings/Setting"))
				if (setting.Attributes["Name"].Value.Equals(Name))
				{
					setting.ParentNode.RemoveChild(setting);
					return true;
				}
			return false;
		}

		public void SaveConfiguration()
		{
			SaveDocument(Document, FileName);
		}

		private static void SaveDocument(XmlDocument document, string filename)
		{
			IsolatedStorageFileStream store = new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
			store.SetLength(0L);
			XmlTextWriter writer = new XmlTextWriter(store, new UnicodeEncoding());
			writer.Formatting = Formatting.Indented;
			document.Save(writer);
			writer.Close();
			store.Close();
		}

		public bool SetValue(string name, string Value)
		{
			foreach (XmlNode node in Document.SelectNodes("/ApplicationSettings/Setting"))
				if (node.Attributes["Name"].Value.Equals(name))
				{
					node.Attributes["Value"].Value = Value;
					return false;
				}

			XmlNode appSettings = Document.SelectSingleNode("/ApplicationSettings");
			XmlNode setting = Document.CreateElement(SettingElement);
			setting.Attributes.Append(Document.CreateAttribute("Name"));
			setting.Attributes.Append(Document.CreateAttribute("Value"));
			setting.Attributes["Name"].Value = name;
			setting.Attributes["Value"].Value = Value;
			appSettings.AppendChild(setting);
			return true;
		}

		public XmlDocument XmlDocument
		{
			get { return Document; }
		}
	}
}
