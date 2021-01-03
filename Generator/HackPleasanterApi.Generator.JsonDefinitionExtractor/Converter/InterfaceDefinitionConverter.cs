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

using HackPleasanterApi.Generator.JsonDefinitionExtractor.Models;
using HackPleasanterApi.Generator.Library.Models.CSV;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HackPleasanterApi.Generator.Libraryrary.Constant;

namespace HackPleasanterApi.Generator.JsonDefinitionExtractor.Converter
{
    /// <summary>
    /// インターフェース定義形式に変換する
    /// </summary>
    class InterfaceDefinitionConverter
    {
        /// <summary>
        /// サイト定義形式に変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public IEnumerable<InterfaceDefinition> Convert(ExportJsonDefinition.Rootobject src)
        {
            IEnumerable<InterfaceDefinition> r = src.Sites.Select(x =>
            {
                if (null == x.SiteSettings)
                {
                    return null;
                }
                if (null == x.SiteSettings.Columns)
                {
                    return null;
                }

                return x.SiteSettings.Columns.Select(e =>
                {
                    return new InterfaceDefinition
                    {
                        SiteId = x.SiteId,
                        Title = x.Title,
                        ColumnName = e.ColumnName,
                        LabelText = e.LabelText,
                        Description = e.Description,
                        ValidateRequired = e.ValidateRequired,
                        ChoicesText = e.ChoicesText
                    };
                });
            })
            .Where(e => null != e)
            .SelectMany(e => e)
            .Where(e => null != e);

            // ChoicesTextに関しては、
            // CSV形式ではないデータが入っている。
            // そこで、一度CSV形式として変換可能な形式に変換する

            // データフォーマット変更時に準拠した形に変換する
            r = ChoicesTextAdjustment(r);

            // データの区切が分かりにくいので、
            // 区切り部分で空文字の行を入れる
            {
                var x = r.GroupBy(e => e.SiteId).OrderBy(x => x.Key);
                var rl = new List<InterfaceDefinition>();
                foreach (var e in x)
                {

                    rl.AddRange(e.OrderBy(w => w.ColumnName));
                    rl.Add(new InterfaceDefinition());

                }
                return rl;
            }

        }

        /// <summary>
        /// ChoicesTextを調整する
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private List<InterfaceDefinition> ChoicesTextAdjustment(IEnumerable<InterfaceDefinition> target)
        {

            var r = target.ToList();

            foreach (var e in r.Where(x => null != x))
            {

                if (false == String.IsNullOrWhiteSpace(e.ChoicesText))
                {

                    if (e.ChoicesText.Contains(",")
                        && e.ChoicesText.Contains("\n")
                        )
                    {

                        var t = e.ChoicesText;
                        t = t.Replace(",", CSVConstant.ChoicesText_ColumnSeparator);
                        t = t.Replace("\r", "").Replace("\n", CSVConstant.ChoicesText_NewLine);
                        e.ChoicesText = t;

                    }
                }
            }

            return r;
        }
    }
}
