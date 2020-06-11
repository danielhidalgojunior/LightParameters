using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightParameters
{
    public interface IParameterPersistence
    {
        void Connect();
        void Save(IParameterGroup group);
        void Load(IParameterGroup group);
    }
}
