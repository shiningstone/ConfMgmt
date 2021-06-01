using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Utils;
using JbConf;
using System.Collections;

namespace ConfViews
{
    public partial class ConfPropertyGrid : UserControl
    {
        public ConfPropertyGrid()
        {
            InitializeComponent();
        }

        private ConfTree _conf;
        public void Bind(ConfTree conf)
        {
            _conf = conf;

            var dict = new Dictionary<string, string>();
            foreach (var kv in _conf.AllItems)
            {
                dict[kv.Name] = kv.Value;
            }
            propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(dict);
        }
        public void Bind(string xmlfile)
        {
            Bind(ConfMgmt.Default.GetTree(xmlfile));
        }

        class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
        {
            IDictionary _dictionary;

            public DictionaryPropertyGridAdapter(IDictionary d)
            {
                _dictionary = d;
            }

            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                ArrayList properties = new ArrayList();
                foreach (DictionaryEntry e in _dictionary)
                {
                    properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key));
                }

                PropertyDescriptor[] props = (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));
                return new PropertyDescriptorCollection(props);
            }
            public PropertyDescriptor GetDefaultProperty()
            {
                return null;
            }
            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return _dictionary;
            }

            public string GetComponentName()
            {
                return TypeDescriptor.GetComponentName(this, true);
            }
            public EventDescriptor GetDefaultEvent()
            {
                return TypeDescriptor.GetDefaultEvent(this, true);
            }
            public string GetClassName()
            {
                return TypeDescriptor.GetClassName(this, true);
            }
            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return TypeDescriptor.GetEvents(this, attributes, true);
            }
            EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
            {
                return TypeDescriptor.GetEvents(this, true);
            }
            public TypeConverter GetConverter()
            {
                return TypeDescriptor.GetConverter(this, true);
            }
            public AttributeCollection GetAttributes()
            {
                return TypeDescriptor.GetAttributes(this, true);
            }

            public object GetEditor(Type editorBaseType)
            {
                return TypeDescriptor.GetEditor(this, editorBaseType, true);
            }
            PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
            {
                return ((ICustomTypeDescriptor)this).GetProperties(new Attribute[0]);
            }

            class DictionaryPropertyDescriptor : PropertyDescriptor
            {
                IDictionary _dictionary;
                object _key;

                internal DictionaryPropertyDescriptor(IDictionary d, object key)
                    : base(key.ToString(), null)
                {
                    _dictionary = d;
                    _key = key;
                }
                public override Type PropertyType
                {
                    get { return _dictionary[_key].GetType(); }
                }
                public override void SetValue(object component, object value)
                {
                    _dictionary[_key] = value;
                }

                public override object GetValue(object component)
                {
                    return _dictionary[_key];
                }
                public override bool IsReadOnly
                {
                    get { return false; }
                }

                public override Type ComponentType
                {
                    get { return null; }
                }

                public override bool CanResetValue(object component)
                {
                    return false;
                }

                public override void ResetValue(object component)
                {
                }

                public override bool ShouldSerializeValue(object component)
                {
                    return false;
                }
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _conf[e.ChangedItem.Label] = e.ChangedItem.Value as string;
            _conf.Save();
        }
    }
}
