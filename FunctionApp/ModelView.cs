using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class OutPutModel
    {
        public string MeterID { get; set; }

        public string serviceName { get; set; }
        public string typeName { get; set; }
        public string sizeName { get; set; }
        public string featureName { get; set; }

        public string csvFileName { get; set; }

        public string message { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }
        public List<Type> Types { get; set; }
    }

    public class Type
    {
        public string Name { get; set; }
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        public string Name { get; set; }
        public string PriceUnit { get; set; }
        public double MinUnit { get; set; }
        public double MaxUnit { get; set; }
        public List<Price> Sizes { get; set; }
    }

    public class Price
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PriceTier { get; set; }
        public string PricePerTier { get; set; }
        public string PriceUnit { get; set; }
    }

    public class ListPrices
    {
        public Dictionary<string, Service> Services { get; set; }
    }
}
