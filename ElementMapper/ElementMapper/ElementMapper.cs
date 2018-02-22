using System;

namespace ElementMapperExtension
{
    public class ElementMapper : Attribute
    {
        public string ElementName { get; private set; }

        /// <summary>
        /// Set Attribute applied on Object propertie
        /// </summary>
        /// <param name="elementName">Name of Attribute</param>
        public ElementMapper(string elementName)
        {
            this.ElementName = elementName;
        }
    }
}
