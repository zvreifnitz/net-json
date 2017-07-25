/*
 * (C) Copyright 2017 zvreifnitz
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace com.github.zvreifnitz.JsonLib.Helper
{
    using System;
    
    public static class DateTimeHelper
    {
        private const long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long EpochTicks = Epoch.Ticks;

        public static long ToUnixMillis(this DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();
            var ticks = utcDateTime.Ticks - EpochTicks;
            return ticks / TicksPerMillisecond;
        }

        public static DateTime ToDateTime(this long unixMillis)
        {
            var ticks = unixMillis * TicksPerMillisecond;
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        public static long ToUnixMillis(this DateTimeOffset dateTime)
        {
            return dateTime.ToUnixTimeMilliseconds();
        }

        public static DateTimeOffset ToDateTimeOffset(this long unixMillis)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixMillis);
        }
    }
}