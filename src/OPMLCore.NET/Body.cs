using System.Collections.Generic;
using System.Xml.Linq;

namespace OPMLCore.NET {
    public class Body
    {
        ///<summary>
        /// Outline list
        ///</summary>
        public List<Outline> Outlines { get; set; } = new List<Outline>();

        ///<summary>
        /// Constructor
        ///</summary>
        public Body()
        {

        }

        ///<summary>
        /// Constructor for LINQ to XML (XElement)
        ///</summary>
        public Body(XElement element)
        {
            if (element.Name.LocalName == "body")
            {
                foreach (var node in element.Elements("outline"))
                {
                    Outlines.Add(new Outline(node));
                }
            }
        }


        public override string ToString()
        {
            return ToXmlString(SaveOptions.None);
        }

        /// <summary>
        /// Returns an XElement representing this Body and its children, using LINQ to XML.
        /// </summary>
        public XElement ToXml()
        {
            var element = new XElement("body");
            foreach (var outline in Outlines)
            {
                element.Add(outline.ToXml());
            }
            return element;
        }

        /// <summary>
        /// Returns the XML string representation of this Body and its children, using LINQ to XML.
        /// </summary>
        /// <summary>
        /// Returns the XML string representation of this Outline and its children, using LINQ to XML.
        /// </summary>
        public string ToXmlString(SaveOptions saveOptions = SaveOptions.DisableFormatting)
        {
            return ToXml().ToString(saveOptions);
        }
    }
}