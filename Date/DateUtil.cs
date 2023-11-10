//
//  Copyright 2022  Soluciones Modernas 10x
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using NodaTime;

namespace DiezX.Api.Commons.Utils
{

    /// <summary>
    /// Utility to get date based on time zone
    /// This class exists derived from the time representation problems that .net has
    /// For more information:
    /// https://blog.nodatime.org/2011/08/what-wrong-with-datetime-anyway.html
    /// </summary>
    public class DateUtil
    {
        private readonly DateTimeZone _timeZone;

        /// <summary>
        /// Gets the current date and time, considering the timezone with which the class was instantiated
        /// with an "unspecified" kind 
        /// </summary>
        /// <returns></returns>
        public DateTime GetTime()
        {
            return Instant.FromDateTimeUtc(DateTime.UtcNow)
                .InZone(_timeZone)
                .ToDateTimeUnspecified();
        }

        /// <summary>
        /// Constructor for specific timeZone for getTime()
        /// This class must be instantiated by explicitly timezone by default
        /// </summary>
        /// <param name="timeZone"></param>
        public DateUtil(string timeZone)
        {
            _timeZone = DateTimeZoneProviders.Tzdb[timeZone];
        }

        //set to private to prevent it from being instantiated without the timezone
        private DateUtil()
        {

        }

    }
}
