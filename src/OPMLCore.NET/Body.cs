using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace OPMLCore.NET {
    using static CommonUtils;


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
        /// Constructor
        ///</summary>
        /// <param name="element">element of Body</param>
        public Body(XmlElement element)
        {
            if (element.Name == "body")
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.Name == "outline")
                    {
                        Outlines.Add(new Outline((XmlElement)node));
                    }
                }
            }
        }

        public override string ToString() {
            StringBuilder buf = new StringBuilder();
            buf.Append($"<body>{NewLine}");
            foreach (Outline outline in Outlines)
            {
                buf.Append(outline);
            }
            buf.Append($"</body>{NewLine}");

            return buf.ToString();
        }
    }
}