using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public class ConfigurationMessages
    {
        public const string ConnectionStringNotFound = "Connection string 'MeetingScheduleAppConnection' not found.";
        public const string ConnectionStringMissingOrEmpty = "Connection string 'MeetingScheduleAppConnection' is missing or empty.";
        public const string RateLimitRealIpHeaderNotConfigured = "Rate limit 'RealIpHeader' is not configured (RateLimit:RealIpHeader).";
        public const string RateLimitHttpStatusCodeNotValidInteger = "Rate limit 'HttpStatusCode' is not a valid integer (RateLimit:HttpStatusCode).";
        public const string RateLimitLimitNotValidNumber = "Rate limit 'Limit' is not a valid number (RateLimit:Limit).";
        public const string RateLimitPeriodNotConfigured = "Rate limit period is not configured (RateLimit:Period).";
        public const string RateLimitEndpointNotConfigured = "Rate limit endpoint is not configured (RateLimit:Endpoint).";

        public const string CORSPolicyNameNotConfigured = "CORS policy name is not configured (CorsSettings:PolicyName).";
        public const string CORSOriginsNotConfigured = "CORS origins are not configured (CorsSettings:Origins).";
        public const string CORSMethodsNotConfigured = "CORS methods are not configured (CorsSettings:Methods).";

        public const string ErrorDuringMigration = "An error occurred during migration";
    }
}
