using LightParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersTest
{
    public class ClientParameters : IParameterGroup
    {
        public IParameterPersistence PersistenceContext { get; set; }

        [Parameter(CreateAutomatically = true, DefaultValue = false, Range = ParameterRange.Global)]
        public bool UseLastName { get; set; }

        [Parameter(CreateAutomatically = true, DefaultValue = true, Range = ParameterRange.Global)]
        public bool AllUpperCase { get; set; }

        [Parameter(CreateAutomatically = true, DefaultValue = "diego", Range = ParameterRange.Global)]
        public string Name { get; set; }

        [Parameter(CreateAutomatically = false, Range = ParameterRange.Global)]
        public DateTime Birthday { get; set; }

        public ClientParameters(IParameterPersistence persistenceContext)
        {
            PersistenceContext = persistenceContext;
            PersistenceContext.Connect();
        }

        public bool SaveAll()
        {
            try
            {
                PersistenceContext.Save(this);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadAll()
        {
            try
            {
                PersistenceContext.Load(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
