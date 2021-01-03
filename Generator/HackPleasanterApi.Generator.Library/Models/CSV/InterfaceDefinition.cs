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

namespace HackPleasanterApi.Generator.Library.Models.CSV

{
    /// <summary>
    /// インターフェース定義用構造体
    /// </summary>
    public class InterfaceDefinition
    {
        #region Siteテーブル関係

        // みんな同じ値を持つ
        public string Title { get; set; }
        public long SiteId { get; set; }
        public long ParentId { get; set; }
        public long InheritPermission { get; set; }

        /// <summary>
        /// 説明文字列
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 必須レコード
        /// </summary>
        public bool? ValidateRequired { get; set; }

        /// <summary>
        /// 選択項目(プルダウン、リンクなど)
        /// </summary>
        public string ChoicesText { get; set; }

        /// <summary>
        /// 選択項目定義
        /// </summary>
        public List<Definition.ChoicesTextInfo> ChoicesTextInfos { get; set; }

        /// <summary>
        /// 定義
        /// </summary>
        public class Definition
        {
            /// <summary>
            /// 選択項目の特別定義
            /// </summary>
            public class ChoicesTextInfo
            {

                /// <summary>
                /// 変数名
                /// </summary>
                public string VariableName { get; set; }

                /// <summary>
                /// 説明文字列
                /// </summary>
                public string Description { get; set; }

                /// <summary>
                /// 値
                /// </summary>
                public string Value { get; set; }

            }

        }

        #endregion

        #region カラム毎の値

        /// <summary>
        /// カラム名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 画面上の表示名
        /// </summary>
        public string LabelText { get; set; }



        #endregion

        #region ユーザーがTSV上で指定する項目

        /// <summary>
        /// 新たに指定する変数名
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// 変換対処テーブルか判定する
        /// </summary>
        public bool IsTarget { get; set; }


        #endregion
    }
}
