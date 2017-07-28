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

using System.Collections.Generic;
using System.Text;

namespace JsonLibExample
{
    public class MediumObject
    {
        public string Name { get; set; }
        public int? MaybeInt { get; set; }
        public List<SimpleObject> SimpleObjectList { get; set; }
        public Dictionary<string, SimpleObject> SimpleObjectDictionary { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{{\"Name\":\"{0}\",\"MaybeInt\":{1},\"SimpleObjectList\":{2},\"SimpleObjectDictionary\":{3}}}",
                Name, MaybeIntToString(), SimpleObjectListToString(), SimpleObjectDictionaryToString());
        }

        private string MaybeIntToString()
        {
            return MaybeInt.HasValue ? "" + MaybeInt.Value : "null";
        }

        private string SimpleObjectListToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            var addComma = false;
            foreach (var member in SimpleObjectList)
            {
                if (addComma)
                {
                    sb.Append(",");
                }
                else
                {
                    addComma = true;
                }
                sb.Append(member);
            }
            sb.Append("]");
            return sb.ToString();
        }

        private string SimpleObjectDictionaryToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            var addComma = false;
            foreach (var member in SimpleObjectDictionary)
            {
                if (addComma)
                {
                    sb.Append(",");
                }
                else
                {
                    addComma = true;
                }
                sb.Append("\"").Append(member.Key).Append("\"");
                sb.Append(":");
                sb.Append(member.Value);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}