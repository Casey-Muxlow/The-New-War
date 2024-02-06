using UnityEngine;
namespace DIG.Tools
{
    public class CustomRangeAttribute : PropertyAttribute
    {
         public string MinVariable;
        public string MaxVariable;

        public CustomRangeAttribute(string minVariable, string maxVariable)
        {
            MinVariable = minVariable;
            MaxVariable = maxVariable;
        }
    }

}