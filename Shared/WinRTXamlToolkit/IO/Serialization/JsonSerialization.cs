﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Renren.Components.IO;
using Windows.Storage;

// Thanks for Xmal toolkit
// http://winrtxamltoolkit.codeplex.com/

namespace Renren.Components.IO.Serialization
{
    /// <summary>
    /// Extension and helper methods for serializing an object graph
    /// to JSON or loading one from a JSON file.
    /// </summary>
    public static class JsonSerialization
    {
        /// <summary>
        /// Serializes the object graph as JSON and saves.
        /// </summary>
        /// <typeparam name="T">The type of the object graph reference.</typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder to save to.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder.
        /// </param>
        /// <returns></returns>
        public async static Task SerializeAsJson<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            CreationCollisionOption options = CreationCollisionOption.FailIfExists)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.CreateFileAsync(fileName, options);

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var type = objectGraph.GetType();
                    var ser = new DataContractJsonSerializer(type);
                    ser.WriteObject(stream, objectGraph);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                if (Debugger.IsAttached)
                    Debugger.Break();

                throw;
            }
        }

        /// <summary>
        /// Serializes the object graph as json.
        /// </summary>
        /// <typeparam name="T">The type of the object graph reference.</typeparam>
        /// <param name="graph">The object graph.</param>
        /// <returns></returns>
        public static string SerializeAsJson<T>(this T graph)
        {
            if (graph == null)
                return null;

            var type = graph.GetType();
            var ser = new DataContractJsonSerializer(type);
            var ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            var bytes = ms.ToArray();

            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Serializes the object graph as json bytes.
        /// </summary>
        /// <typeparam name="T">The type of the object graph reference.</typeparam>
        /// <param name="graph">The object graph.</param>
        /// <returns></returns>
        public static byte[] SerializeAsBytes<T>(this T graph)
        {
            if (graph == null)
                return null;

            var type = graph.GetType();
            var ser = new DataContractJsonSerializer(type);
            var ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            return ms.ToArray();
        }

        /// <summary>
        /// Loads an object graph from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of the expected object graph reference.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static async Task<T> LoadFromJsonFile<T>(
            string fileName,
            StorageFolder folder = null)
        {
            var json = await StringIOExtensions.ReadFromFile(fileName, folder);

            return LoadFromJsonString<T>(json,typeof(T));
        }

        /// <summary>
        /// Loads an object graph from a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the expected object graph reference.</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns></returns>
        public static T LoadFromJsonString<T>(string json,Type currentType)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(currentType);
            T result = (T)ser.ReadObject(ms);

            return result;
        }
    }
}
