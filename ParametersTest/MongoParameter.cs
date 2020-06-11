using MongoDBConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersTest
{
    public class MongoParameter : Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string SpecificComputer { get; set; }
    }
}
