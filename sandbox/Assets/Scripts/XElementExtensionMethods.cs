using System.Xml.Linq;

//extension methods to null check XElements from a LINQ query
public static class XElementExtensionMethods
{
    //null checks a string value
    public static string ElementValueNull_String(this XElement element)
    {
        if (element != null)
        {
            return element.Value;
        }
        else
        {
            return System.String.Empty;
        }
    }

    //null checks an integer value
    public static int ElementValueNull_Integer(this XElement element)
    {
        if (element != null)
        {
            return int.Parse(element.Value);
        }
        else
        {
            return -1;
        }
    }

    //null checks a boolean value
    public static bool ElementValueNull_Boolean(this XElement element)
    {
        if (element != null)
        {
            if(int.Parse(element.Value) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    //null checks a float value
    public static float ElementValueNull_Float(this XElement element)
    {
        if (element != null)
        {
            return float.Parse(element.Value);
        }
        else
        {
            return -1;
        }
    }

    //null checks a string attribute
    public static string AttributeValueNull_String(this XElement element, string attributeName)
    {
        if (element != null)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr != null)
            {
                return attr.Value;
            }
            else
            {
                return System.String.Empty;
            }
        }
        else
        {
            return System.String.Empty;
        }
    }

    //null checks an integer attribute
    public static int AttributeValueNull_Integer(this XElement element, string attributeName)
    {
        if (element != null)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr != null)
            {
                return int.Parse(attr.Value);
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    } 
}
