/********************************************************************
 * Game : Freaking Game
 * Scene : Common
 * Description : XML management 
 * History:
 *	2016/09/25	TungNguyen	First Edition
********************************************************************/
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace NumberTest
{

public class cMgrXml : MonoBehaviour
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="file"></param>
	/// <param name="primId"></param>
	static public bool ReadQuiz(string file, string primId, string lang)
	{
		try
		{
			XmlDocument xmlQuiz = new XmlDocument();
			xmlQuiz.LoadXml(file);

			foreach (XmlNode quiz in xmlQuiz.DocumentElement.SelectNodes("/Quiz/"+ lang + "/PrimID[@id='" + primId + "']"))
			{
				cMgrCommon.Suggest = quiz.SelectSingleNode("Suggest").InnerText.ToString();

				cMgrCommon.Answer = quiz.SelectSingleNode("Answer").InnerText.ToString();

				cMgrCommon.ImageLeft = quiz.SelectSingleNode("ImageLeft").InnerText.ToString();

				cMgrCommon.ImageRight = quiz.SelectSingleNode("ImageRight").InnerText.ToString();
			}
		}
		catch
		{
			print("cMgrXML: ReadQuiz");

			return false;
		}

		return true;
	}
}
}
