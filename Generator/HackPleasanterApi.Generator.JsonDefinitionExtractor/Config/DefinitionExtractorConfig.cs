/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 * */

using System;
using System.Collections.Generic;
using System.Text;

namespace HackPleasanterApi.Generator.JsonDefinitionExtractor.Config
{

    /// <summary>
    /// 定義入出力設定
    /// </summary>
    public class DefinitionExtractorConfig
    {
        public class Definition
        {
            /// <summary>
            /// 入力関係
            /// </summary>
            public class Input
            {
                /// <summary>
                /// サイトエクスポート情報
                /// </summary>
                public string SiteExportDefinitionFile { get; set; }

            }

            /// <summary>
            /// 出力関係
            /// </summary>
            public class Output
            {
                /// <summary>
                /// サイト定義ファイル名
                /// </summary>
                public string SiteDefinitionFile { get; set; }


                /// <summary>
                /// インターフェース定義ファイル名
                /// </summary>
                public string InterfaceDefinitionFile { get; set; }

                /// <summary>
                /// 入力ファイルのエンコード設定
                /// </summary>
                public string Encoding { get; set; }

                /// <summary>
                /// ディスクリプションを変数名として使うか
                /// </summary>
                public bool UseDescriptionAsVariableName { get; set; } = true;

            }

        }

        public Definition.Input Input { get; set; }

        public Definition.Output Output { get; set; }

    }
}
