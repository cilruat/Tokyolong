using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;


namespace RFLib
{
	/// <summary>
	/// Static class to provide various data storage options
	///  All FILE related storage stuffs data into APPDATAPATH
	/// </summary>
	public static class RFDataStore
	{
		/// <summary>
		///  Create a complete path to a file located in Application.persistentDataPath
		///   prepends / character if necessary
		/// </summary>
		/// <returns>Full, valid path to file</returns>
		/// <param name="filename">The file name</param>
		public static string GetFullPath(string filename)
		{
			// Is this some kind of joke? Why would you pass me a null string?!
			if( string.IsNullOrEmpty( filename ) )
				return null;
			
			string tFileName = filename;
			//Force it to start with a /
			if( tFileName[ 0 ] != '/' )	tFileName = "/" + tFileName;
			return Application.persistentDataPath + tFileName;
		}

		public static void SetEnvironmentVarsForBinary()
		{
			// Required for binary serialization for the specific playform
			if( Application.platform == RuntimePlatform.IPhonePlayer )
			{
				System.Environment.SetEnvironmentVariable( "MONO_REFLECTION_SERIALIZER", "yes" );
			}

		}


		/// <summary>
		///  Load and deserialize binary data from a file
		/// </summary>
		/// <returns>
		///  Returns a generic object or NULL if there was some sort of failure.
		/// </returns>
		/// <param name="filename">
		/// Name of file to load data from
		/// </param>
		public static object LoadBinaryDataFile(string filename)
		{
			object outvalue = null;

			string fullPath = GetFullPath( filename );
			if( !string.IsNullOrEmpty(fullPath)  && File.Exists( fullPath ) )
			{
				SetEnvironmentVarsForBinary();
				FileStream file = null;
				try
				{
					file = File.Open( fullPath, FileMode.Open );

					BinaryFormatter bf = new BinaryFormatter();
					bf.Binder = new BinaryDeserializationBinder();

					outvalue = bf.Deserialize( file ) ;
				} 

				// High level catch
				catch( Exception e )
				{
					outvalue = null;
					Debug.Log( "RFLIB::RFDataStore::LoadBinaryData fail: " + e.Message );
				} 
				finally
				{
					if( file != null )		file.Close();
				}	
			}

			return outvalue;
		}

		/// <summary>
		/// Save binary serialized data to a file
		/// </summary>
		/// <returns><c>true</c>, if binary data file was saved, <c>false</c> otherwise.</returns>
		/// <param name="filename">File name</param>
		/// <param name="data">Data to store (Must be serializable)</param>
		public static bool SaveBinaryDataFile(string filename, object data)
		{
			string fullPath = GetFullPath( filename );

			if( string.IsNullOrEmpty( fullPath ) )	return false;

			FileStream file = null;
			bool saved = false;
			SetEnvironmentVarsForBinary();
			try
			{
				file = File.Open( fullPath, FileMode.OpenOrCreate );

				BinaryFormatter bf = new BinaryFormatter();
				bf.Binder = new BinaryDeserializationBinder();
				bf.Serialize( file, data);
				saved = true;
			}
			// Generic catch
			catch( Exception e )
			{
				Debug.Log( "RFLIB::RFDataStore::SaveBinaryDataFile fail: " + e.Message );
			}
			finally
			{
				if( file != null )
					file.Close();
			}

			return saved;
		}

		/// <summary>
		/// Binary deserialization binder. Necessary according to all the wise folks...
		/// http://answers.unity3d.com/questions/8480/how-to-scrip-a-saveload-game-option.html
		/// https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter.binder(v=vs.110).aspx
		/// https://msdn.microsoft.com/en-us/library/system.reflection.assembly.getexecutingassembly(v=vs.110).aspx
		/// </summary>
		sealed class BinaryDeserializationBinder : SerializationBinder
		{
			public override Type BindToType(string assemblyName, string typeName)
			{
				if( string.IsNullOrEmpty( assemblyName ) || string.IsNullOrEmpty( typeName ) )	return null;
				assemblyName = Assembly.GetExecutingAssembly().FullName;
				return Type.GetType( String.Format("{0},{1}", typeName, assemblyName ));

			}

		}

	}
}