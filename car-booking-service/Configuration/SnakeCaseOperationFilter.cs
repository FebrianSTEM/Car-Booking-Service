using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace car_booking_service.Configuration
{
    public class SnakeCaseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            else
            {
                foreach (var item in operation.Parameters)
                {
                    item.Name = item.Name.ToSnakeCase();
                }
            }
        }
    }
}