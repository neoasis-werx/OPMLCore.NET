using System.Text;
using System.Xml;

namespace OPMLCore.NET.Tests;


 public class OpmlCoreTests
 {
     [Test]
     public void NormalTest()
     {
         var xml = new StringBuilder();
         xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
         xml.Append("<opml version=\"2.0\">");
         xml.Append("<head>");
         xml.Append("<title>mySubscriptions.opml</title>");
         xml.Append("<dateCreated>Sat, 18 Jun 2005 12:11:52 GMT</dateCreated>");
         xml.Append("<dateModified>Tue, 02 Aug 2005 21:42:48 GMT</dateModified>");
         xml.Append("<ownerName>fnya</ownerName>");
         xml.Append("<ownerEmail>fnya@example.com</ownerEmail>");
         xml.Append("<ownerId>http://news.com.com/</ownerId>");
         xml.Append("<docs>http://news.com.com/</docs>");
         xml.Append("<expansionState>1, 6, 13, 16, 18, 20</expansionState>");
         xml.Append("<vertScrollState>1</vertScrollState>");
         xml.Append("<windowTop>106</windowTop>");
         xml.Append("<windowLeft>106</windowLeft>");
         xml.Append("<windowBottom>558</windowBottom>");
         xml.Append("<windowRight>479</windowRight>");
         xml.Append("</head>");
         xml.Append("<body>");
         xml.Append("<outline ");
         xml.Append("text=\"CNET News.com\" ");
         xml.Append("isComment=\"true\" ");
         xml.Append("isBreakpoint=\"true\" ");
         xml.Append("created=\"Tue, 02 Aug 2005 21:42:48 GMT\" ");
         xml.Append("category=\"/Harvard/Berkman,/Politics\" ");
         xml.Append("description=\"Tech news and business reports by CNET News.com.\" ");
         xml.Append("htmlUrl=\"http://news.com.com/\" ");
         xml.Append("language=\"unknown\" ");
         xml.Append("title=\"CNET News.com\" ");
         xml.Append("type=\"rss\" ");
         xml.Append("version=\"RSS2\" ");
         xml.Append("xmlUrl=\"http://news.com.com/2547-1_3-0-5.xml\" ");
         xml.Append("/>");
         xml.Append("</body>");
         xml.Append("</opml>");

         XmlDocument doc = new XmlDocument();
         doc.LoadXml(xml.ToString());
         Opml opml = new Opml(doc);

         Assert.That(opml.Head.Title, Is.EqualTo("mySubscriptions.opml"));
         Assert.That(opml.Head.DateCreated, Is.EqualTo(DateTime.Parse("Sat, 18 Jun 2005 12:11:52 GMT", Opml.MyCultureInfo)));
         Assert.That(opml.Head.DateModified, Is.EqualTo(DateTime.Parse("Tue, 02 Aug 2005 21:42:48 GMT", Opml.MyCultureInfo)));
         Assert.That(opml.Head.OwnerName, Is.EqualTo("fnya"));
         Assert.That(opml.Head.OwnerEmail, Is.EqualTo("fnya@example.com"));
         Assert.That(opml.Head.OwnerId, Is.EqualTo("http://news.com.com/"));
         Assert.That(opml.Head.Docs, Is.EqualTo("http://news.com.com/"));
         Assert.That(opml.Head.ExpansionState, Is.EqualTo("1,6,13,16,18,20".Split(',')));
         Assert.That(opml.Head.VertScrollState, Is.EqualTo("1"));
         Assert.That(opml.Head.WindowTop, Is.EqualTo("106"));
         Assert.That(opml.Head.WindowLeft, Is.EqualTo("106"));
         Assert.That(opml.Head.WindowBottom, Is.EqualTo("558"));
         Assert.That(opml.Head.WindowRight, Is.EqualTo("479"));

         foreach (var outline in opml.Body.Outlines)
         {
             Assert.That(outline.Text, Is.EqualTo("CNET News.com"));
             Assert.That(outline.IsComment, Is.EqualTo("true"));
             Assert.That(outline.IsBreakpoint, Is.EqualTo("true"));
             Assert.That(outline.Created, Is.EqualTo(DateTime.Parse("Tue, 02 Aug 2005 21:42:48 GMT", Opml.MyCultureInfo)));
             Assert.That(outline.Category, Is.EqualTo("/Harvard/Berkman,/Politics".Split(',')));
             Assert.That(outline.Description, Is.EqualTo("Tech news and business reports by CNET News.com."));
             Assert.That(outline.HTMLUrl, Is.EqualTo("http://news.com.com/"));
             Assert.That(outline.Language, Is.EqualTo("unknown"));
             Assert.That(outline.Title, Is.EqualTo("CNET News.com"));
             Assert.That(outline.Type, Is.EqualTo("rss"));
             Assert.That(outline.Version, Is.EqualTo("RSS2"));
             Assert.That(outline.XMLUrl, Is.EqualTo("http://news.com.com/2547-1_3-0-5.xml"));
         }
     }

     [Test]
     public void ChildNodeTest()
     {
         StringBuilder xml = new StringBuilder();
         xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
         xml.Append("<opml version=\"2.0\">");
         xml.Append("<head>");
         xml.Append("<title>mySubscriptions.opml</title>");
         xml.Append("</head>");
         xml.Append("<body>");
         xml.Append("<outline ");
         xml.Append("text=\"IT\" >");
         xml.Append("<outline ");
         xml.Append("text=\"washingtonpost.com\" ");
         xml.Append("htmlUrl=\"http://www.washingtonpost.com\" ");
         xml.Append("xmlUrl=\"http://www.washingtonpost.com/rss.xml\" ");
         xml.Append("/>");
         xml.Append("</outline>");
         xml.Append("</body>");
         xml.Append("</opml>");

         XmlDocument doc = new XmlDocument();
         doc.LoadXml(xml.ToString());
         Opml opml = new Opml(doc);

         foreach (var outline in opml.Body.Outlines)
         {
             foreach(var childOutline in outline.Outlines) {
                 Assert.That(childOutline.Text, Is.EqualTo("washingtonpost.com"));
                 Assert.That(childOutline.HTMLUrl, Is.EqualTo("http://www.washingtonpost.com"));
                 Assert.That(childOutline.XMLUrl, Is.EqualTo("http://www.washingtonpost.com/rss.xml"));
             }
         }
     }

    [Test]
     public void CreateNormalOpmlTest()
     {
         Opml opml = new Opml
         {
             Encoding = "UTF-8",
             Version = "2.0"
         };

         Head head = new Head
         {
             Title = "mySubscriptions.opml",
             DateCreated = DateTime.Parse("Sat, 18 Jun 2005 12:11:52 GMT", Opml.MyCultureInfo).ToUniversalTime(),
             DateModified = DateTime.Parse("Tue, 02 Aug 2005 21:42:48 GMT", Opml.MyCultureInfo).ToUniversalTime(),
             OwnerName = "fnya",
             OwnerEmail = "fnya@example.com",
             OwnerId = "http://news.com.com/",
             Docs = "http://news.com.com/"
         };
         head.ExpansionState.Add("1");
         head.ExpansionState.Add("6");
         head.ExpansionState.Add("13");
         head.ExpansionState.Add("16");
         head.ExpansionState.Add("18");
         head.ExpansionState.Add("20");
         head.VertScrollState = "1";
         head.WindowTop = "106";
         head.WindowLeft = "106";
         head.WindowBottom = "558";
         head.WindowRight = "479";
         opml.Head = head;

         Outline outline = new Outline
         {
             Text = "CNET News.com",
             IsComment = "true",
             IsBreakpoint = "true",
             Created = DateTime.Parse("Tue, 02 Aug 2005 21:42:48 GMT", Opml.MyCultureInfo).ToUniversalTime()
         };
         outline.Category.Add("/Harvard/Berkman");
         outline.Category.Add("/Politics");
         outline.Description = "Tech news and business reports by CNET News.com.";
         outline.HTMLUrl = "http://news.com.com/";
         outline.Language = "unknown";
         outline.Title = "CNET News.com";
         outline.Type = "rss";
         outline.Version = "RSS2";
         outline.XMLUrl = "http://news.com.com/2547-1_3-0-5.xml";

         Body body = new Body();
         body.Outlines.Add(outline);
         opml.Body = body;

         StringBuilder xml = new StringBuilder();
         xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n");
         xml.Append("<opml version=\"2.0\">\r\n");
         xml.Append("<head>\r\n");
         xml.Append("<title>mySubscriptions.opml</title>\r\n");
         xml.Append("<dateCreated>Sat, 18 Jun 2005 12:11:52 GMT</dateCreated>\r\n");
         xml.Append("<dateModified>Tue, 02 Aug 2005 21:42:48 GMT</dateModified>\r\n");
         xml.Append("<ownerName>fnya</ownerName>\r\n");
         xml.Append("<ownerEmail>fnya@example.com</ownerEmail>\r\n");
         xml.Append("<ownerId>http://news.com.com/</ownerId>\r\n");
         xml.Append("<docs>http://news.com.com/</docs>\r\n");
         xml.Append("<expansionState>1,6,13,16,18,20</expansionState>\r\n");
         xml.Append("<vertScrollState>1</vertScrollState>\r\n");
         xml.Append("<windowTop>106</windowTop>\r\n");
         xml.Append("<windowLeft>106</windowLeft>\r\n");
         xml.Append("<windowBottom>558</windowBottom>\r\n");
         xml.Append("<windowRight>479</windowRight>\r\n");
         xml.Append("</head>\r\n");
         xml.Append("<body>\r\n");
         xml.Append("<outline ");
         xml.Append("text=\"CNET News.com\" ");
         xml.Append("isComment=\"true\" ");
         xml.Append("isBreakpoint=\"true\" ");
         xml.Append("created=\"Tue, 02 Aug 2005 21:42:48 GMT\" ");
         xml.Append("category=\"/Harvard/Berkman,/Politics\" ");
         xml.Append("description=\"Tech news and business reports by CNET News.com.\" ");
         xml.Append("htmlUrl=\"http://news.com.com/\" ");
         xml.Append("language=\"unknown\" ");
         xml.Append("title=\"CNET News.com\" ");
         xml.Append("type=\"rss\" ");
         xml.Append("version=\"RSS2\" ");
         xml.Append("xmlUrl=\"http://news.com.com/2547-1_3-0-5.xml\" ");
         xml.Append("/>\r\n");
         xml.Append("</body>\r\n");
         xml.Append("</opml>");

         Assert.That(opml.ToString(), Is.EqualTo(xml.ToString()));

     }

    [Test]
     public void CreateChildOpmlTest()
     {
         Opml opml = new Opml();
         opml.Encoding = "UTF-8";
         opml.Version = "2.0";

         Head head = new Head();
         head.Title = "mySubscriptions.opml";
         opml.Head = head;

         Outline outline = new Outline();
         outline.Text = "IT";

         Outline childOutline = new Outline();
         childOutline.Text = "CNET News.com";
         childOutline.HTMLUrl = "http://news.com.com/";
         childOutline.XMLUrl = "http://news.com.com/2547-1_3-0-5.xml";

         outline.Outlines.Add(childOutline);

         Body body = new Body();
         body.Outlines.Add(outline);
         opml.Body = body;

         StringBuilder xml = new StringBuilder();
         xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n");
         xml.Append("<opml version=\"2.0\">\r\n");
         xml.Append("<head>\r\n");
         xml.Append("<title>mySubscriptions.opml</title>\r\n");
         xml.Append("</head>\r\n");
         xml.Append("<body>\r\n");
         xml.Append("<outline text=\"IT\">\r\n");
         xml.Append("<outline text=\"CNET News.com\" ");
         xml.Append("htmlUrl=\"http://news.com.com/\" ");
         xml.Append("xmlUrl=\"http://news.com.com/2547-1_3-0-5.xml\" />\r\n");
         xml.Append("</outline>\r\n");
         xml.Append("</body>\r\n");
         xml.Append("</opml>");

         Assert.That(opml.ToString(), Is.EqualTo(xml.ToString()));
     }


     [Test]
     public void LoadOpmlTest()
     {
         Opml opml = new Opml("C:\\Users\\Deerwood McCord\\OneDrive - Craneware\\Desktop\\CHI Product Containment.opml");

         Assert.That(opml, Is.Not.Null);
         foreach (Outline outline in opml.Body.Outlines)
         {
             //Output outline node
             Console.WriteLine(outline.Text);
             Console.WriteLine(outline.XMLUrl);

             //output child's output node
             foreach (Outline childOutline in outline.Outlines)
             {
                 Console.WriteLine(childOutline.Text);
                 Console.WriteLine(childOutline.XMLUrl);
             }
         }
     }
 }