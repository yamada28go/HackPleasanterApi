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

namespace HackPleasanterApi.Generator.CodeGenerator.Configs
{
    /// <summary>
    /// コード設定機の設定
    /// </summary>
    public class GeneratorConfig
    {
        /// <summary>
        /// 設定関係
        /// </summary>
        public class Definition
        {

            /// <summary>
            /// 入力ファイル定義
            /// </summary>
            public class InputFiles
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

            }

            /// <summary>
            /// テンプレートファイル定義
            /// </summary>
            public class TemplateFiles
            {

                /// <summary>
                /// テンプレート用ファイル名
                /// </summary>
                public string TemplateFileName { get; set; }


                /// <summary>
                /// 出力サブディレクトリ名称
                /// </summary>
                public string OutputSubdirectoryName { get; set; }

                /// <summary>
                /// 出力プレフィックス　先頭部分
                /// </summary>
                public string HeadPrefix { get; set; }

                /// <summary>
                /// 出力プレフィックス　末尾部分
                /// </summary>
                public string EndPrefix { get; set; }

                /// <summary>
                /// 出力ファイル拡張子
                /// </summary>
                public string OutputExtension { get; set; }

                /// <summary>
                /// テンプレートファイルのファイルのエンコード設定
                /// </summary>
                public string Encoding { get; set; }

            }

            #region 出力ディレクト関係

            /// <summary>
            /// 出力設定
            /// </summary>
            public class OutputConfig
            {

                /// <summary>
                /// 出力ディレクトパス
                /// </summary>
                public string OutputDirectory { get; set; }

            }


            #endregion


            #region コード生成関係

            /// <summary>
            /// コード関係の設定
            /// </summary>
            public class CodeConfig
            {

                /// <summary>
                /// 生成される名前空間
                /// </summary>
                public string NameSpace { get; set; }

            }

        }

        #endregion

        #region 実態定義

        /// <summary>
        /// 入力ファイル
        /// </summary>
        public Definition.InputFiles InputFiles;

        /// <summary>
        /// 出力設定
        /// </summary>
        public Definition.OutputConfig OutputConfig;

        /// <summary>
        /// テンプレート設定
        /// </summary>
        public List<Definition.TemplateFiles> TemplateFiles;

        /// <summary>
        ///　生成コード設定
        /// </summary>
        public Definition.CodeConfig CodeConfig;

        #endregion

    }
}
