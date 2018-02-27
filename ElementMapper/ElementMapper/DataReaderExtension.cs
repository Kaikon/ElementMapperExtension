using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace ElementMapperExtension
{
    public static class DataReaderExtension
    {
        public enum MapWith { Propertie, Attribute, All };

        /// <summary>
        /// Map data from DataReader to an Object
        /// </summary>
        /// <typeparam name="T">Object to map</typeparam>
        /// <param name="DataRead">Datareader</param>
        /// <param name="UseElementMapper">Map maked with ElementMapper Attribute</param>
        /// <returns>List of Object with data contains in DataReader</returns>
        public static List<T> MapToList<T>(this DbDataReader DataRead, MapWith MappedWith = MapWith.Propertie) where T : new()
        {
            List<T> ListReturn = null;
            // Define object type
            var Entity = typeof(T);
            // Define dictionary
            var PropDict = new Dictionary<string, PropertyInfo>();
            var AttrDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (DataRead != null && DataRead.HasRows)
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
                            // Get the ElementMapper name
                            ElementMapper Element = Attr as ElementMapper;
                            AttrDict.Add(Element.ElementName.ToUpper(), PropInfo);
                        }
                    }

                    while (DataRead.Read())
                    {
                        T newObject = new T();
                        for (int Index = 0; Index < DataRead.FieldCount; Index++)
                        {
                            if ((MappedWith == MapWith.Propertie) || (MappedWith == MapWith.All))
                            {
                                // Search reader name in Dictionary properties
                                if (PropDict.ContainsKey(DataRead.GetName(Index).ToUpper()))
                                {
                                    var Info = PropDict[DataRead.GetName(Index).ToUpper()];
                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        // Set value in Object
                                        var Val = DataRead.GetValue(Index);

                                        try
                                        {
                                            if (Info.PropertyType == Val.GetType())
                                            {
                                                Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                            }
                                            else if (Info.PropertyType == typeof(string))
                                            {
                                                Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val.ToString(), null);
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }

                            if ((MappedWith == MapWith.Attribute) || (MappedWith == MapWith.All))
                            {
                                // Search reader name in Dictionary attributes
                                if (AttrDict.ContainsKey(DataRead.GetName(Index).ToUpper()))
                                {
                                    var Info = AttrDict[DataRead.GetName(Index).ToUpper()];
                                    if ((Info != null) && Info.CanWrite)
                                    {
                                        // Set value in Object
                                        var Val = DataRead.GetValue(Index);

                                        try
                                        {
                                            if (Info.PropertyType == Val.GetType())
                                            {
                                                Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                            }
                                            else if (Info.PropertyType == typeof(string))
                                            {
                                                Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val.ToString(), null);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                        }
                        // Add current object in the list
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
        /// Map data from DataReader to an object
        /// </summary>
        /// <typeparam name="T">Object to map</typeparam>
        /// <param name="DataRead">DataReader</param>
        /// <param name="UseElementMapper">Map maked with ElementMapper Attribute</param>
        /// <returns>Object with data contains in DataReader</returns>
        public static T MapToSingle<T>(this DbDataReader DataRead, MapWith MappedWith = MapWith.Propertie) where T : new()
        {
            T ObjectReturn = new T();
            // Define object type
            var Entity = typeof(T);
            // Define dictionary
            var PropDict = new Dictionary<string, PropertyInfo>();
            var AttrDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (DataRead != null && DataRead.HasRows)
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
                            // Get the ElementMapper name
                            ElementMapper Element = Attr as ElementMapper;
                            AttrDict.Add(Element.ElementName.ToUpper(), PropInfo);
                        }
                    }

                    DataRead.Read();
                    for (int Index = 0; Index < DataRead.FieldCount; Index++)
                    {
                        if ((MappedWith == MapWith.Propertie) || (MappedWith == MapWith.All))
                        {
                            // Search reader name in Dictionary properties
                            if (PropDict.ContainsKey(DataRead.GetName(Index).ToUpper()))
                            {
                                var Info = PropDict[DataRead.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    // Set value in Object
                                    var Val = DataRead.GetValue(Index);
                                    if (Info.PropertyType == Val.GetType())
                                    {
                                        Info.SetValue(ObjectReturn, (Val == DBNull.Value) ? null : Val, null);
                                    }
                                    else if (Info.PropertyType == typeof(string))
                                    {
                                        Info.SetValue(ObjectReturn, (Val == DBNull.Value) ? null : Val.ToString(), null);
                                    }
                                }
                            }
                        }

                        if ((MappedWith == MapWith.Attribute) || (MappedWith == MapWith.All))
                        {
                            // Search reader name in Dictionary attributes
                            if (AttrDict.ContainsKey(DataRead.GetName(Index).ToUpper()))
                            {
                                var Info = AttrDict[DataRead.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    // Set value in Object
                                    var Val = DataRead.GetValue(Index);
                                    if (Info.PropertyType == Val.GetType())
                                    {
                                        Info.SetValue(ObjectReturn, (Val == DBNull.Value) ? null : Val, null);
                                    }
                                    else if (Info.PropertyType == typeof(string))
                                    {
                                        Info.SetValue(ObjectReturn, (Val == DBNull.Value) ? null : Val.ToString(), null);
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
