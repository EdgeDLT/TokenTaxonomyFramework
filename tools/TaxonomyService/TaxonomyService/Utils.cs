using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using log4net;
using log4net.Config;
using Org.BouncyCastle.Crypto.Digests;

namespace TTI.TTF.Taxonomy
{
	public static class Utils
	{
		public static void InitLog()
		{
			var xmlDocument = new XmlDocument();
			try
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
					                               "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
					                               "/log4net.config"));
			}
			catch (Exception)
			{
				if (Os.IsWindows())
					xmlDocument.Load(File.OpenRead(
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log4net.config"));
				else
					xmlDocument.Load(File.OpenRead(
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log4net.config"));
			}

			XmlConfigurator.Configure(
				LogManager.CreateRepository(Assembly.GetEntryAssembly(),
					typeof(log4net.Repository.Hierarchy.Hierarchy)), xmlDocument["log4net"]);
		}

		public static string FirstToUpper(string nameString)
		{
			var ch = nameString.ToCharArray();
			for (var i = 0; i < nameString.Length; i++)
			{

				// If first character of a word is found 
				if ((i != 0 || ch[i] == ' ') && (ch[i] == ' ' || ch[i - 1] != ' ')) continue;
				// If it is in lower-case 
				if (ch[i] >= 'a' && ch[i] <= 'z')
				{

					// Convert into Upper-case 
					ch[i] = (char) (ch[i] - 'a' + 'A');
				}
			}

			return new string(ch);
		}

		public static (string Name, string visual, string tooling) GetRandomArtifactFromArtifact(string name,
			string visual ,string tooling)
		{
			var randStr = Randomize("");
			return (name + randStr, visual + randStr, tooling + randStr);
		}
		
		public static string GetRandomNameFromName(string name)
		{
			var randStr = Randomize("");
			return name + randStr;
		}
		
		public static (string Name, string visual, string tooling) GetRandomTemplate(string name,
			string visual ,string tooling)
		{
			var randStr = Randomize("");
			return (name + randStr, visual + randStr, tooling + randStr);
		}

		public static string Randomize(string input)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[1];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			return new string(input+stringChars);
		}
		
		public static string CalculateSha3Hash(string value)
		{
			var input = Encoding.UTF8.GetBytes(value);
			var output = CalculateSha3Hash(input);
			return output.ToHex();
		}

		private static byte[] CalculateSha3Hash(byte[] value)
		{
			var digest = new KeccakDigest(256);
			var output = new byte[digest.GetDigestSize()];
			digest.BlockUpdate(value, 0, value.Length);
			digest.DoFinal(output, 0);
			return output;
		}

		private static string ToHex(this byte[] value, bool prefix = false)
		{
			var strPrex = prefix ? "0x" : "";
			return strPrex + string.Concat(value.Select(b => b.ToString("x2")));
		}
	}

	public static class Os
	{
		public static bool IsWindows()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
		}

		public static bool IsMacOs()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
		}

		public static bool IsLinux()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		public static string WhatIs()
		{
			var os = (IsWindows() ? "win" : null) ??
			         (IsMacOs() ? "mac" : null) ??
			         (IsLinux() ? "gnu" : null);
			return os;
		}
	}
}