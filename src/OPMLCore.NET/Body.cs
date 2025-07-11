using System.Text;
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
        /// Constructor for LINQ to XML (XElement)
        ///</summary>
        public Body(System.Xml.Linq.XElement element)
        {
            if (element.Name.LocalName == "body")
            {
                foreach (var node in element.Elements("outline"))
                {
                    Outlines.Add(new Outline(node));
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