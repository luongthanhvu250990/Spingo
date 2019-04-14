using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class GameUtils {
	public static string EncryptString(string str, string pass)
	{
		byte[] res;
		System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

		//hash string using MD5
		MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
		byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(pass));

		//create TripleDESCryptoServiceProvider
		TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

		//Setting
		TDESAlgorithm.Key = TDESKey;
		TDESAlgorithm.Mode = CipherMode.ECB;
		TDESAlgorithm.Padding = PaddingMode.PKCS7;

		//convert msg to byte
		byte[] DataToEncrypt = UTF8.GetBytes(str);

		// Encrypt
		try
		{
			ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
			res = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
		}
		finally
		{
			// clear Triple DES and HashProvider
			TDESAlgorithm.Clear();
			HashProvider.Clear();
		}

		// return string Base64
		return Convert.ToBase64String(res);
	}

	public static string DecryptString(string str, string pass)
	{
		byte[] res;
		System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

		//hash string using MD5		
		MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
		byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(pass));

		//create TripleDESCryptoServiceProvider
		TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

		//setting
		TDESAlgorithm.Key = TDESKey;
		TDESAlgorithm.Mode = CipherMode.ECB;
		TDESAlgorithm.Padding = PaddingMode.PKCS7;

		//convert msg to byte
		byte[] DataToDecrypt = Convert.FromBase64String(str);

		// Decrypt
		try
		{
			ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
			res = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
		}
		finally
		{
			// clear Triple DES and HashProvider
			TDESAlgorithm.Clear();
			HashProvider.Clear();
		}

		// string UTF8
		return UTF8.GetString(res);
	}

	public static DateTime ParseFromString(string str){//2016-03-11 16:53:00
		string[] temp = str.Split (' ');
		string[] days = temp [0].Split ('-');
		string[] hours = temp [1].Split (':');
		DateTime date = new DateTime (int.Parse (days [0]), int.Parse (days [1]), int.Parse (days [2]),
			int.Parse (hours [0]), int.Parse (hours [1]), int.Parse (hours [2]));
		return date;			
	}

	public static void SaveTextFile(string path, string content){
		StreamWriter writer = new StreamWriter (path, false);
		writer.WriteLine (content);
		writer.Close ();
	}

	public static string LoadTextFile(string path){
		if (!File.Exists (path))
			return "";
		StreamReader reader = new StreamReader (path);
		string rs = reader.ReadLine ();
		reader.Close ();
		return rs;
	}

	public static void SaveImageAsPNG(Sprite sprite, string path){
		byte[] bytes = sprite.texture.EncodeToPNG ();
		File.WriteAllBytes (path, bytes);
	}

	public static float GetKeyboardHeight(){
		float keyHeight = 0;

		#if UNITY_IOS
		if(TouchScreenKeyboard.visible){
		keyHeight = TouchScreenKeyboard.area.height;				
		}
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
		AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

		using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
		{
		View.Call("getWindowVisibleDisplayFrame", Rct);

		keyHeight = Screen.height - Rct.Call<int>("height");
		keyHeight+=170;
		}
		}
		#endif
		return keyHeight;
	}

	public static string ConvertToUrl(string input){
		input = input.Replace (" ", "%20");
		return input;
	}


	/// <summary>
	/// Compares the two strings based on letter pair matches
	/// </summary>
	/// <param name="str1"></param>
	/// <param name="str2"></param>
	/// <returns>The percentage match from 0.0 to 1.0 where 1.0 is 100%</returns>
	public static double CompareStrings(string str1, string str2)
	{
		List<string> pairs1 = WordLetterPairs(str1.ToUpper());
		List<string> pairs2 = WordLetterPairs(str2.ToUpper());

		int intersection = 0;
		int union = pairs1.Count + pairs2.Count;

		for (int i = 0; i < pairs1.Count; i++)
		{
			for (int j = 0; j < pairs2.Count; j++)
			{
				if (pairs1[i] == pairs2[j])
				{
					intersection++;
					pairs2.RemoveAt(j);//Must remove the match to prevent "GGGG" from appearing to match "GG" with 100% success

					break;
				}
			}
		}

		return (2.0 * intersection) / union;
	}

	/// <summary>
	/// Gets all letter pairs for each
	/// individual word in the string
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	private static List<string> WordLetterPairs(string str)
	{
		List<string> AllPairs = new List<string>();

		// Tokenize the string and put the tokens/words into an array
		string[] Words = Regex.Split(str, @"\s");

		// For each word
		for (int w = 0; w < Words.Length; w++)
		{
			if (!string.IsNullOrEmpty(Words[w]))
			{
				// Find the pairs of characters
				String[] PairsInWord = LetterPairs(Words[w]);

				for (int p = 0; p < PairsInWord.Length; p++)
				{
					AllPairs.Add(PairsInWord[p]);
				}
			}
		}

		return AllPairs;
	}

	/// <summary>
	/// Generates an array containing every 
	/// two consecutive letters in the input string
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	private static string[] LetterPairs(string str)
	{
		int numPairs = str.Length - 1;

		string[] pairs = new string[numPairs];

		for (int i = 0; i < numPairs; i++)
		{
			pairs[i] = str.Substring(i, 2);
		}

		return pairs;
	}

	public static string GetMostMatchingString(string input, string[] arrString){
		double p = 0;
		string temp = input;
		for (int i = 0; i < arrString.Length; i++) {
			if (CompareStrings (input, arrString [i]) > p) {
				p = CompareStrings (input, arrString [i]);
				temp = arrString [i];
			}
		}
		return temp;
	}

	public static string GenerateID()
	{
		return Guid.NewGuid().ToString("N");
	}

	public static void Shuffle<T>(IList<T> list, System.Random rnd)
	{
		for (var i = 0; i < list.Count; i++)
			GameUtils.Swap (list, i, rnd.Next (i, list.Count));	
	}

	public static void Swap<T>(IList<T> list, int i, int j)
	{
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
	}


}
