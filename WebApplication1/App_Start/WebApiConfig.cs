using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Formatter.Serialization;
using Microsoft.Data.Edm.Library;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            

            // Web API routes

            RegisterJsonFormatter(config);
            RegisterODataFormatter(config);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }

        private static void RegisterODataFormatter(HttpConfiguration config)
        {
            var defaultODataSerializerProvider = DefaultODataSerializerProvider.Instance;

            var odataFormatters = System.Web.Http.OData.Formatter.ODataMediaTypeFormatters.Create(
            defaultODataSerializerProvider,
            System.Web.Http.OData.Formatter.Deserialization.DefaultODataDeserializerProvider.Instance);

            config.Formatters.AddRange(odataFormatters);
        }

        private static void RegisterJsonFormatter(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                // Parse dates as date/time
                DateParseHandling = DateParseHandling.DateTime,

                // Use ISO date formatting
                DateFormatHandling = DateFormatHandling.IsoDateFormat,

                // Assume all dates/times stored output as local time zone format
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                NullValueHandling = NullValueHandling.Ignore
            };
            jsonFormatter.SerializerSettings = jsonSerializerSettings;
        }
    }
}
