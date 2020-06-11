using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightParameters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterAttribute : Attribute
    {
        public bool CreateAutomatically { get; set; } = true;
        public object DefaultValue { get; set; } = null;
        public ParameterRange Range { get; set; } = ParameterRange.Global;
    }
}
