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

using HackPleasanterApi.Generator.CodeGenerator.Configs;
using HackPleasanterApi.Generator.Library.Models.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HackPleasanterApi.Generator.CodeGenerator.Models
{
    /// <summary>
    /// 分類済みのインターフェース定義一覧
    /// </summary>
   public class ClassifiedInterface
    {

        private ClassifiedInterface() { }

        public static ClassifiedInterface Generate(List<InterfaceDefinition> RawInterfaceDefinition)
        {

            #region ローカル関数

            // 対象をフィルタする
            List<InterfaceDefinition> LF_DoFilter(List<InterfaceDefinition> src, string prefix)
            {
                return src.Where(e => Regex.IsMatch(e.ColumnName, $"^{prefix}[a-zA-Z]"))
                    .OrderBy(e => e.ColumnName).ToList();
            }

            #endregion

            var r = new ClassifiedInterface();

            // 個別要素でフィルタする
            r.ClassHash = LF_DoFilter(RawInterfaceDefinition, "Class");
            r.NumHash = LF_DoFilter(RawInterfaceDefinition, "Num");
            r.DateHash = LF_DoFilter(RawInterfaceDefinition, "Date");
            r.DescriptionHash = LF_DoFilter(RawInterfaceDefinition, "Description");
            r.CheckHash = LF_DoFilter(RawInterfaceDefinition, "Check");
            r.AttachmentsHash = LF_DoFilter(RawInterfaceDefinition, "Attachments");

            return r;

        }


        /// <summary>
        /// 分類A～Z
        /// </summary>
        public List<InterfaceDefinition> ClassHash;

        /// <summary>
        /// 数値A～Z
        /// </summary>
        public List<InterfaceDefinition> NumHash { get; set; }
        /// <summary>
        /// 日付A～Z
        /// </summary>
        public List<InterfaceDefinition> DateHash { get; set; }

        /// <summary>
        /// 説明A～Z
        /// </summary>
        public List<InterfaceDefinition> DescriptionHash { get; set; }

        /// <summary>
        /// チェックA～Z
        /// </summary>
        public List<InterfaceDefinition> CheckHash { get; set; }

        /// <summary>
        /// 添付ファイルA～Z
        /// </summary>
        public List<InterfaceDefinition> AttachmentsHash { get; set; }

    }


    public class SiteInfos
    {

        /// <summary>
        /// 対象となるサイトの情報
        /// </summary>
        public SiteDefinition SiteDefinition;

        /// <summary>
        /// 対象とするカラムリスト
        /// </summary>
        public List<InterfaceDefinition> RawInterfaceDefinition;

        /// <summary>
        /// 分類済みのインターフェース定義
        /// </summary>
        public ClassifiedInterface ClassifiedInterface;

        /// <summary>
        /// コード生成日
        /// </summary>
        public string GeneratedDate = DateTime.Now.ToShortDateString();

        /// <summary>
        /// テンプレートの設定項目
        /// </summary>
        public GeneratorConfig.Definition.TemplateFiles TemplateFilesConfig;

    }
}
