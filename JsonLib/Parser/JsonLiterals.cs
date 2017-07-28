﻿/*
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
    internal static class JsonLiterals
    {
        internal const string Null = "null";
        internal const string True = "true";
        internal const string False = "false";
        
        internal const char ArrayStart = '[';
        internal const char ArrayEnd = ']';
        
        internal const char ObjectStart = '{';
        internal const char ObjectEnd = '}';
        
        internal const char Comma = ',';
        internal const char Colon = ':';
        
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
    }
}