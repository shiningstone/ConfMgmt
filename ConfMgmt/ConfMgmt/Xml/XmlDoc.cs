using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JbConf
{
    public class XmlDoc : XmlDocument
    {
        public XmlDoc AddSibling(ConfTree refer, ConfTree tree)
        {
            XmlNode sibling = XmlOp.Find(this, tree.Name);
            if (sibling == ChildNodes[ChildNodes.Count - 1])//非唯一
            {
                RemoveChild(sibling);

                var parent = new ConfTree($"{tree.Name}s");
                parent.Add(refer);
                parent.Add(tree);
                AppendChild(XmlConf.CreateNode(this, parent));
            }
            else
            {
                sibling.ParentNode.AppendChild(XmlConf.CreateNode(this, tree));
            }

            return this;
        }

        public void Modify(string path, string tag, string name, string value)
        {
            var xmlNode = XmlOp.Find2(this, path);
            XmlOp.Modify(xmlNode, name, value);
        }
        public void ModifyAttr(string path, string tag, string attr, string value)
        {
            var xmlNode = XmlOp.Find(this, path, tag);
            XmlOp.ModifyAttribute(xmlNode, attr, value);
        }
    }
}
