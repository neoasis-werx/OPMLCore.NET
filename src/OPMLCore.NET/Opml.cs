using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace OPMLCore.NET {
    using System;
    using static CommonUtils;

    public class Opml {
        ///<summary>
        /// Version of OPML
        ///</summary>
        public string Version { get; set;}

        ///<summary>
        /// Encoding of OPML
        ///</summary>
        public string Encoding { get; set;}

        ///<summary>
        /// Head of OPML
        ///</summary>
        public Head Head { get; set;} = new Head();

        ///<summary>
        /// Body of OPML
        ///</summary>
        public Body Body { get; set;} = new Body();

        ///<summary>
        /// Other OPML attributes not explicitly handled
        ///</summary>
        public IDictionary<string, string> OtherAttributes { get; set; } = new Dictionary<string, string>();

        ///<summary>
        /// Constructor
        ///</summary>
        public Opml()
        {
        }

        ///<summary>
        /// Constructor
        ///</summary>
        /// <param name="location">Location of the OPML file</param>
        public Opml(string location)
        {
            var doc = new XmlDocument();
            doc.Load(location);
            ReadOpmlNodes(doc);
        }

        ///<summary>
        /// Constructor
        ///</summary>
        /// <param name="doc">XMLDocument of the OPML</param>
        public Opml(XmlDocument doc)
        {
            ReadOpmlNodes(doc);
        }


        private void ReadOpmlNodes(XmlDocument doc) {
            // Get encoding from XmlDocument if available
            if (doc.FirstChild is XmlDeclaration decl && !string.IsNullOrEmpty(decl.Encoding))
            {
                Encoding = decl.Encoding.Equals("iso-8859-1", StringComparison.InvariantCultureIgnoreCase) ? "UTF-8" : decl.Encoding;
            }

            // Find the first 'opml' element node
            XmlNode opmlNode = null;
            foreach (XmlNode node in doc)
            {
                if (node.NodeType == XmlNodeType.Element && node.Name == "opml")
                {
                    opmlNode = node;
                    break;
                }
            }
            if (opmlNode == null) return;

            // Read attributes using a switch statement
            if (opmlNode.Attributes != null)
                foreach (XmlAttribute attr in opmlNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "version":
                            Version = attr.Value;
                            break;
                        default:
                            OtherAttributes.TryAdd(attr.Name, attr.Value);
                            break;
                    }
                }

            // Loop through children of 'opml' node only once
            foreach (XmlNode childNode in opmlNode.ChildNodes)
            {
                if (childNode.NodeType != XmlNodeType.Element) continue;
                switch (childNode.Name)
                {
                    case "head":
                        Head = new Head((XmlElement)childNode);
                        break;
                    case "body":
                        Body = new Body((XmlElement)childNode);
                        break;
                }
            }
        }

        public override string ToString()
        {
            var buf = new StringBuilder();
            var encoding = string.IsNullOrEmpty(Encoding)?"UTF-8":Encoding;
            buf.Append($"<?xml version=\"1.0\" encoding=\"{encoding}\" ?>{NewLine}");
            var version = string.IsNullOrEmpty(Version)?"2.0":Version;
            buf.Append($"<opml version=\"{version}\">{NewLine}");
            buf.Append(Head.ToString());
            buf.Append(Body.ToString());
            buf.Append("</opml>");

            return buf.ToString();
        }

    }
}