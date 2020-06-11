using LightParameters;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersTest
{
    public class MongoContext : IParameterPersistence
    {

        public void Connect()
        {
            try
            {
                var config = JsonConfigurator.GetConnectionStringFromConfigFile();
                Mongo.Initialize(config);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(IParameterGroup group)
        {
            var props = group
                .GetType()
                .GetProperties()
                .Where(x => x.CustomAttributes.Where(y => y.AttributeType == typeof(ParameterAttribute)).Any());

            foreach (var prop in props)
            {
                var dv = (ParameterAttribute)prop.GetCustomAttributes(false).ToList().Single(x => x is ParameterAttribute);

                if (!dv.CreateAutomatically)
                {
                    continue;
                }    

                var mp = new MongoParameter 
                { 
                    Key = prop.Name, 
                    Value = prop.GetValue(group).ToString() 
                };

                var filter = Builders<MongoParameter>.Filter.Eq(x => x.Key, mp.Key);
                if (Mongo.Exists(filter))
                {
                    var updateFilter = Builders<MongoParameter>.Update.Set(x => x.Value, mp.Value);
                    
                    if (Mongo.UpdateField(filter, updateFilter))
                    {
                        Console.WriteLine($"{mp.Key} updated value to \"{mp.Value}\"");
                    }
                }
            }
        }

        public void Load(IParameterGroup group)
        {
            var props = group
                .GetType()
                .GetProperties()
                .Where(x => x.CustomAttributes.Where(y => y.AttributeType == typeof(ParameterAttribute)).Any());

            foreach (var prop in props)
            {
                var dv = (ParameterAttribute)prop.GetCustomAttributes(false).ToList().Single(x => x is ParameterAttribute);

                if (!dv.CreateAutomatically)
                {
                    continue;
                }

                var filterKey = Builders<MongoParameter>.Filter.Eq(x => x.Key, prop.Name);
                var filterComputer = Builders<MongoParameter>.Filter.Eq(x => x.SpecificComputer, Environment.MachineName);
                var filterComplex = Builders<MongoParameter>.Filter.And(filterKey, filterComputer);

                MongoParameter mp = null;

                if (dv.Range == ParameterRange.Local)
                {
                    mp = Mongo.GetOne(filterComplex);
                }
                else
                {
                    mp = Mongo.GetOne(filterKey);
                }

                if (mp == null)
                {
                    if (!dv.CreateAutomatically)
                    {
                        continue;
                    }

                    mp = new MongoParameter
                    {
                        Key = prop.Name,
                        Value = dv.DefaultValue.ToString(),
                        SpecificComputer = dv.Range == ParameterRange.Global ? null : Environment.MachineName
                    };

                    Console.WriteLine($"Creating {mp.Key} with default value of {mp.Value}");
                    Mongo.InsertOne(mp);
                }

                TypeConverter typeConverter = TypeDescriptor.GetConverter(dv.DefaultValue);
                object propValue = typeConverter.ConvertFromString(mp.Value);

                prop.SetValue(group, propValue);
                Console.WriteLine($"{prop.Name} loaded with value: {propValue}");
            }
        }
    }
}
