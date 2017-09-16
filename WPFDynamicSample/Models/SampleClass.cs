using System;
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

        public int IntProperty { get; private set; }
        public void SetIntProperty(int value) { IntProperty = value; } 

        public TestEnum EnumProperty { get; private set; }
        public void SetEnumProperty(TestEnum value) { EnumProperty = value; }

        public string StringProperty { get; private set; }
        public void SetStringProperty(string value){ StringProperty = value; }

    }
}
