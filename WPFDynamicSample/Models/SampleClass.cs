using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WPFDynamicSample.Models
{
    public class SampleClass
    {
        public enum TestEnum
        {
            Item1,
            Item2,
            Item3,
        }



        public SampleClass()
        {
            if (ArrayProperty == null)
            {
                ArrayProperty = new List<string>();
                ArrayProperty.Add("Test1");
                ArrayProperty.Add("Test2");
            }

            if(ContainerProperty == null)
            {
                ContainerProperty = new List<InfoContainer>();
                ContainerProperty.Add(new InfoContainer() { Name = "John", Detail="aaa", ID=0 });
                ContainerProperty.Add(new InfoContainer() { Name = "Sam", Detail = "bbb", ID = 2 });
            }
        }

        public int IntProperty { get; private set; }
        public void SetIntProperty(int value) { IntProperty = value; } 

        public TestEnum EnumProperty { get; private set; }
        public void SetEnumProperty(TestEnum value) { EnumProperty = value; }


        public string StringProperty { get; private set; }
        public void SetStringProperty(string value){ StringProperty = value; }

        public bool BoolProperty { get; private set; }
        public void SetBoolProperty(bool value){ BoolProperty = value; }

        public List<string> ArrayProperty { get; private set; }
        public void AddRangeArrayProperty (IEnumerable<string> values) {
            ArrayProperty.AddRange(values);
        }

        public List<InfoContainer> ContainerProperty { get; private set; }
        public void AddRangeContainerProperty (IEnumerable<InfoContainer> values) {
            ContainerProperty.AddRange(values);
        }

    }

    public class InfoContainer
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public int ID { get; set; }

        public override string ToString()
        {
            return $"{ID} - {Name} - {Detail} ";
        }
    }
}
