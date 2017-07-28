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

namespace com.github.zvreifnitz.JsonLib.Parser
{
    using System.Globalization;
    using System.Text;

    internal static class JsonHelper
    {
        internal const char QuotationMark = '"';
        internal const string QuotationMarkJson = @"\""";
        
        internal const char ReverseSolidus = '\\';
        internal const string ReverseSolidusJson = @"\\";
        
        internal const char Solidus = '/';
        internal const string SolidusJson = @"\/";
        
        internal const char Backspace = '\b';
        internal const string BackspaceJson = @"\b";
        
        internal const char Formfeed = '\f';
        internal const string FormfeedJson = @"\f";
        
        internal const char Newline = '\n';
        internal const string NewlineJson = @"\n";
        
        internal const char CarriageReturn = '\r';
        internal const string CarriageReturnJson = @"\r";
        
        internal const char HorizontalTab = '\t';
        internal const string HorizontalTabJson = @"\t";
        
        internal const string UnicodeJson = @"\u";

        public static string EncodeToJsonString(this string input)
        {
            if (input == null)
            {
                return JsonLiterals.Null;
            }

            StringBuilder sb = new StringBuilder((input.Length + 1) << 1);

            sb.Append(QuotationMark);
            foreach (var ch in input)
            {
                switch (ch)
                {
                    case QuotationMark:
                    {
                        sb.Append(QuotationMarkJson);
                        break;
                    }
                    case ReverseSolidus:
                    {
                        sb.Append(ReverseSolidusJson);
                        break;
                    }
                    case Solidus:
                    {
                        sb.Append(SolidusJson);
                        break;
                    }
                    case Backspace:
                    {
                        sb.Append(BackspaceJson);
                        break;
                    }
                    case Formfeed:
                    {
                        sb.Append(FormfeedJson);
                        break;
                    }
                    case Newline:
                    {
                        sb.Append(NewlineJson);
                        break;
                    }
                    case CarriageReturn:
                    {
                        sb.Append(CarriageReturnJson);
                        break;
                    }
                    case HorizontalTab:
                    {
                        sb.Append(HorizontalTabJson);
                        break;
                    }
                    default:
                    {
                        sb.Append(ch);
                        break;
                    }
                }
            }
            sb.Append(QuotationMark);

            return sb.ToString();
        }

        public static string DecodeFromJsonString(this string input)
        {
            if ((input == null) || (input == JsonLiterals.Null))
            {
                return null;
            }

            input = input.Trim();
            StringBuilder sb = new StringBuilder(input.Length);
            int maxIndex = input.Length - 1;
            for (int index = 0; index <= maxIndex; index++)
            {
                char current = input[index];

                if (((index == 0) || (index == maxIndex)) && (current == QuotationMark))
                {
                    continue;
                }

                if ((current == ReverseSolidus) && (index < maxIndex))
                {
                    var composed = new string(new[] {current, input[index + 1]});
                    switch (composed)
                    {
                        case QuotationMarkJson:
                        {
                            sb.Append(QuotationMark);
                            index++;
                            break;
                        }
                        case ReverseSolidusJson:
                        {
                            sb.Append(ReverseSolidus);
                            index++;
                            break;
                        }
                        case SolidusJson:
                        {
                            sb.Append(Solidus);
                            index++;
                            break;
                        }
                        case BackspaceJson:
                        {
                            sb.Append(Backspace);
                            index++;
                            break;
                        }
                        case FormfeedJson:
                        {
                            sb.Append(Formfeed);
                            index++;
                            break;
                        }
                        case NewlineJson:
                        {
                            sb.Append(Newline);
                            index++;
                            break;
                        }
                        case CarriageReturnJson:
                        {
                            sb.Append(CarriageReturn);
                            index++;
                            break;
                        }
                        case HorizontalTabJson:
                        {
                            sb.Append(HorizontalTab);
                            index++;
                            break;
                        }
                        case UnicodeJson:
                        {
                            if (maxIndex - 5 >= index)
                            {
                                composed = input.Substring(index, 6);
                                var intChar = UnicodeEscapedStringToChar(composed);
                                if (intChar > -1)
                                {
                                    sb.Append((char)intChar);
                                    index += 5;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    sb.Append(current);
                }
            }

            return sb.ToString();
        }

        private static int UnicodeEscapedStringToChar(string input)
        {
            int result;
            if (input.Length == 6 && input.StartsWith(UnicodeJson)
                && int.TryParse(input.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            return -1;
        }
    }
}