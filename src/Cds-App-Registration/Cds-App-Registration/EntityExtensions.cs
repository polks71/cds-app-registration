using Microsoft.Xrm.Sdk;
using System;

namespace Cds_App_Registration
{
    public static class EntityExtensions
    {
        //A simple extension I wrote to avoid re-writing the ternary statement over and over again.
        public static T TryGetAttributeValue<T>(this Entity entity, string attribute, T defaultValue)
        {
            if (entity.Contains(attribute))
            {
                return entity.GetAttributeValue<T>(attribute);
            }
            else
            {
                //If used in a Plugin this should be logged using the Trace Service
                Console.WriteLine($"{attribute} was null using default value of {defaultValue}");
                return defaultValue;
            }
        }
    }
}
