using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightParameters
{
    public interface IParameterGroup
    {
        IParameterPersistence PersistenceContext { get; set; }
        bool SaveAll();
        void LoadAll();
    }
}
