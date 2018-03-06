using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ElementMapperExtension
{
    public static class DictionaryExtension
    {
        public enum MapWith { Propertie, Attribute, All };

        /// <summary>
        /// Map data from List Dictionnary to an Object
        /// </summary>
        /// <typeparam name="T">Object to map</typeparam>
        /// <param name="ListDict">List Dictionary</param>
        /// <param name="UseElementMapper">Map maked with ElementMapper Attribute</param>
        /// <returns>List of Object with data contains in List Dictionary</returns>
        public static List<T> MapToList<T>(this List<Dictionary<string, string>> ListDict, MapWith MappedWith = MapWith.Propertie) where T : new()
        {
            List<T> ListReturn = null;
            // Define object type
            var Entity = typeof(T);
            // Define dictionary
            var PropDict = new Dictionary<string, PropertyInfo>();
            var AttrDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (ListDict != null && ListDict.Count > 0)
                {
                    ListReturn = new List<T>();
                    // Get each properties of the Object
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    // Get each attribute of properites
                    foreach (PropertyInfo PropInfo in Props)
                    {
                        object[] AttrCollection = PropInfo.GetCustomAttributes(true);

                        foreach (object Attr in AttrCollection)
                        {
                            if (Attr is ElementMapper)
                            {
                                // Get the ElementMapper name
                                ElementMapper Element = Attr as ElementMapper;
                                if (Element != null)
                                {
                                    AttrDict.Add(Element.ElementName.ToUpper(), PropInfo);
                                }
                            }
                        }
                    }

                    foreach (Dictionary<string, string> Dict in ListDict)
                    {
                        T newObject = new T();

                        for (int Index = 0; Index < Dict.Count; Index++)
                        {
                            if((MappedWith == MapWith.Propertie) || (MappedWith == MapWith.All))
                            {
                                if (PropDict.ContainsKey(Dict.ElementAt(Index).Key.ToUpper()))
                                {
                                    var Info = PropDict[Dict.ElementAt(Index).Key.ToUpper()];
                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        var Val = Dict.ElementAt(Index).Value;
                                        if (Info.PropertyType == Val.GetType())
                                        {
                                            Info.SetValue(newObject, Val);
                                        }
                                        else if (Info.PropertyType == typeof(string))
                                        {
                                            Info.SetValue(newObject, Val.ToString());
                                        }
                                    }
                                }
                            }

                            if((MappedWith == MapWith.Attribute) || (MappedWith == MapWith.All))
                            { 
                                if (AttrDict.ContainsKey(Dict.ElementAt(Index).Key.ToUpper()))
                                {
                                    var Info = AttrDict[Dict.ElementAt(Index).Key.ToUpper()];
                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        var Val = Dict.ElementAt(Index).Value;
                                        if (Info.PropertyType == Val.GetType())
                                        {
                                            Info.SetValue(newObject, Val);
                                        }
                                        else if (Info.PropertyType == typeof(string))
                                        {
                                            Info.SetValue(newObject, Val.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        ListReturn.Add(newObject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataReaderExtension : " + ex.Message);
                throw;
            }

            return ListReturn;
        }

        /// <summary>
        /// Map data from Dictionnary to an Object
        /// </summary>
        /// <typeparam name="T">Object to map</typeparam>
        /// <param name="Dict">Dictionary</param>
        /// <param name="UseElementMapper">Map maked with ElementMapper Attribute</param>
        /// <returns>Object with data contains in Dictionary</returns>
        public static T MapToSingle<T>(this Dictionary<string, string> Dict, MapWith MappedWith = MapWith.Propertie) where T : new()
        {
            T ObjectReturn = new T();

            // Define object type
            var Entity = typeof(T);
            // Define dictionary
            var PropDict = new Dictionary<string, PropertyInfo>();
            var AttrDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (Dict != null && Dict.Count > 0)
                {
                    // Get each properties of the Object
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    // Get each attribute of properites
                    foreach (PropertyInfo PropInfo in Props)
                    {
                        object[] AttrCollection = PropInfo.GetCustomAttributes(true);

                        foreach (object Attr in AttrCollection)
                        {
                            if (Attr is ElementMapper)
                            {
                                // Get the ElementMapper name
                                ElementMapper Element = Attr as ElementMapper;
                                if (Element != null)
                                {
                                    AttrDict.Add(Element.ElementName.ToUpper(), PropInfo);
                                }
                            }
                        }
                    }

                    for (int Index = 0; Index < Dict.Count; Index++)
                    {
                        if ((MappedWith == MapWith.Propertie) || (MappedWith == MapWith.All))
                        {
                            if (PropDict.ContainsKey(Dict.ElementAt(Index).Key.ToUpper()))
                            {
                                var Info = PropDict[Dict.ElementAt(Index).Key.ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = Dict.ElementAt(Index).Value;
                                    if (Info.PropertyType == Val.GetType())
                                    {
                                        Info.SetValue(ObjectReturn, Val);
                                    }
                                    else if (Info.PropertyType == typeof(string))
                                    {
                                        Info.SetValue(ObjectReturn, Val.ToString());
                                    }
                                }
                            }
                        }
                        
                        if ((MappedWith == MapWith.Attribute) || (MappedWith == MapWith.All))
                        {
                            if (AttrDict.ContainsKey(Dict.ElementAt(Index).Key.ToUpper()))
                            {
                                var Info = AttrDict[Dict.ElementAt(Index).Key.ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = Dict.ElementAt(Index).Value;
                                    if (Info.PropertyType == Val.GetType())
                                    {
                                        Info.SetValue(ObjectReturn, Val);
                                    }
                                    else if (Info.PropertyType == typeof(string))
                                    {
                                        Info.SetValue(ObjectReturn, Val.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataReaderExtension : " + ex.Message);
                throw;
            }

            return ObjectReturn;
        }
    }
}
