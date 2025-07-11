using System.Xml.Linq;

namespace OPMLCore.NET.Tests
{
    public class OpmlCoreXLinqTests
    {
        [Test]
        public void NormalTest()
        {
            var xml = OpmlTestData.GetNormalOpmlXmlString();
            var xdoc = XDocument.Parse(xml);
            Opml opml = new Opml(xdoc);

            Assert.That(opml.Head.Title, Is.EqualTo("mySubscriptions.opml"));
            Assert.That(opml.Head.DateCreated, Is.EqualTo(CommonUtils.ParseDateTime("Sat, 18 Jun 2005 12:11:52 GMT")));
            Assert.That(opml.Head.DateModified, Is.EqualTo(CommonUtils.ParseDateTime("Tue, 02 Aug 2005 21:42:48 GMT")));
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
                Assert.That(outline.Created, Is.EqualTo(CommonUtils.ParseDateTime("Tue, 02 Aug 2005 21:42:48 GMT")));
                Assert.That(outline.Category, Is.EqualTo("/Harvard/Berkman,/Politics".Split(',')));
                Assert.That(outline.Description, Is.EqualTo("Tech news and business reports by CNET News.com."));
                Assert.That(outline.HtmlUrl, Is.EqualTo("http://news.com.com/"));
                Assert.That(outline.Language, Is.EqualTo("unknown"));
                Assert.That(outline.Title, Is.EqualTo("CNET News.com"));
                Assert.That(outline.Type, Is.EqualTo("rss"));
                Assert.That(outline.Version, Is.EqualTo("RSS2"));
                Assert.That(outline.XmlUrl, Is.EqualTo("http://news.com.com/2547-1_3-0-5.xml"));
            }
        }

        [Test]
        public void ChildNodeTest()
        {
            var xml = OpmlTestData.GetChildNodeOpmlXmlString();
            var xdoc = XDocument.Parse(xml);
            Opml opml = new Opml(xdoc);

            foreach (var outline in opml.Body.Outlines)
            {
                foreach (var childOutline in outline.Outlines)
                {
                    Assert.That(childOutline.Text, Is.EqualTo("washingtonpost.com"));
                    Assert.That(childOutline.HtmlUrl, Is.EqualTo("http://www.washingtonpost.com"));
                    Assert.That(childOutline.XmlUrl, Is.EqualTo("http://www.washingtonpost.com/rss.xml"));
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
                DateCreated = CommonUtils.ParseDateTime("Sat, 18 Jun 2005 12:11:52 GMT")?.ToUniversalTime(),
                DateModified = CommonUtils.ParseDateTime("Tue, 02 Aug 2005 21:42:48 GMT")?.ToUniversalTime(),
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
                Created = CommonUtils.ParseDateTime("Tue, 02 Aug 2005 21:42:48 GMT")?.ToUniversalTime()
            };
            outline.Category.Add("/Harvard/Berkman");
            outline.Category.Add("/Politics");
            outline.Description = "Tech news and business reports by CNET News.com.";
            outline.HtmlUrl = "http://news.com.com/";
            outline.Language = "unknown";
            outline.Title = "CNET News.com";
            outline.Type = "rss";
            outline.Version = "RSS2";
            outline.XmlUrl = "http://news.com.com/2547-1_3-0-5.xml";

            Body body = new Body();
            body.Outlines.Add(outline);
            opml.Body = body;

            var expectedXml = OpmlTestData.GetCreateNormalOpmlXmlString();
            Assert.That(opml.ToXmlString(), Is.EqualTo(expectedXml));
        }

        [Test]
        public void CreateChildOpmlTest()
        {
            Opml opml = new Opml
            {
                Encoding = "UTF-8",
                Version = "2.0"
            };

            Head head = new Head
            {
                Title = "mySubscriptions.opml"
            };
            opml.Head = head;

            Outline outline = new Outline
            {
                Text = "IT"
            };

            Outline childOutline = new Outline
            {
                Text = "CNET News.com",
                HtmlUrl = "http://news.com.com/",
                XmlUrl = "http://news.com.com/2547-1_3-0-5.xml"
            };

            outline.Outlines.Add(childOutline);

            Body body = new Body();
            body.Outlines.Add(outline);
            opml.Body = body;

            var expectedXml = OpmlTestData.GetChildOpmlXmlString();
            Assert.That(opml.ToXmlString(), Is.EqualTo(expectedXml));
        }
        [Test]
        public void LoadOpmlTest()
        {
            Opml opml = new Opml("C:\\Users\\Deerwood McCord\\OneDrive - Craneware\\Desktop\\CHI Product Containment.opml");

            Assert.That(opml, Is.Not.Null);

            CommonUtils.TraverseOpmlOutline(opml, CommonUtils.PrintEventAction);
        }
    }
}
