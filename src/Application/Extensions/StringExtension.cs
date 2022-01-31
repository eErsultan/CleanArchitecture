using System.Collections;
using MediatR;

namespace Application.Extensions
{
    public static class StringExtension
    {
        private const int FIRST_ELEM = 0;

        public static string ConcatBraces(this string name)
        {
            return "{" + name + "}";
        }

        public static string AddQuery<T>(this string requestUri, IBaseRequest queries) where T : IBaseRequest 
        {
            requestUri += "?";
            var properties = typeof(T).GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                var value = typeof(T).GetProperty(properties[i].Name)?.GetValue(queries);
                if (value != null)
                {
                    if (value.GetType().IsGenericType)
                    {
                        value = Concat((IList)value);
                    }

                    if (i == FIRST_ELEM)
                    {
                        requestUri += properties[i].Name + "=" + value;
                    }
                    else
                    {
                        requestUri += "&" + properties[i].Name + "=" + value;
                    }
                }
            }
            
            return requestUri;
        }

        private static string Concat(IList values)
        {
            var value = "";
            for (var i = 0; i < values.Count; i++)
            {
                if (i == FIRST_ELEM)
                {
                    value += values[i];
                }
                else
                {
                    value += "," + values[i];
                };
            }

            return value;
        }
    }
}