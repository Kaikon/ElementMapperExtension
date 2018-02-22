# ElementMapperExtension
C# ElementMapperExtension makes it easier for developers to create an Object from DbDataReader or Dictionary


### How to use
Simply add the .DLL in the reference of the project

You can map the object from the properties or from the attributes added in the object

```c#
public class MyObject
{
    [ElementMapper("id")]
    public int Id { get; set; }

    [ElementMapper("name")]
    public string Name { get; set; }

    [ElementMapper("date")]
    public DateTime UpdDate { get; set; }

    [ElementMapper("comment")]
    public string Comment { get; set; }
}
```


Create object from DbDataReader

```c#
// Single object
MyObject Object = CurrentDataReader.MapToSingle<MyObject>();

// List of object
List<MyObject> ListObject = CurrentDataReader.MapToList<MyObject>();
```

Create object Dictionary

```c#
// Single object
Dictionary<string, string> DictString = new Dictionary<string, string>();
DictString.Add("name", "hello");

MyObject Object = DictString.MapToSingle<MyObject>();

// List of object
List<Dictionary<string, string>> ListDict = new List<Dictionary<string, string>>();
ListDict.Add(DictString);

List<MyObject> ListObj = ListDict.MapToList<MyObject>();
```

In default the object is created with the properties name, you can change it in added a parameter

```c#
// Created with properties of object
MyObject Object = CurrentDataReader.MapToSingle<MyObject>();
// Or
MyObject Object = CurrentDataReader.MapToSingle<MyObject>(DataReaderExtension.MapWith.Propertie);

// Created with attributes
MyObject Object = CurrentDataReader.MapToSingle<MyObject>(DataReaderExtension.MapWith.Attribute);

// Created with properties and attributes
MyObject Object = CurrentDataReader.MapToSingle<MyObject>(DataReaderExtension.MapWith.All);
```