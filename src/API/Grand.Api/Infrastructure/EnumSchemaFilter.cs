using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Text;

namespace Grand.Api.Infrastructure
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumValues = schema.Enum.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var item in enumValues)
                {
                    var value = (OpenApiPrimitive<int>)item;
                    var name = Enum.GetName(context.Type, value.Value);
                    sb.Append($"{value.Value} - {name}; ");
                }
                schema.Description = sb.ToString();
            }
        }
    }
}

