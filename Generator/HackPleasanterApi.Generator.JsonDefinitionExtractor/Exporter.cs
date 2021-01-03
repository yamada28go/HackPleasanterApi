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

using System.Collections.Generic;
using System.Linq;

using HackPleasanterApi.Generator.JsonDefinitionExtractor.Converter;
using HackPleasanterApi.Generator.JsonDefinitionExtractor.Models;
using HackPleasanterApi.Generator.JsonDefinitionExtractor.Reader;
using HackPleasanterApi.Generator.Library.Models.CSV;
using HackPleasanterApi.Generator.Library.Service;
using HackPleasanterApi.Generator.JsonDefinitionExtractor.Config;

namespace HackPleasanterApi.Generator.JsonDefinitionExtractor
{
    /// <summary>
    /// 主要処理部分
    /// 
    /// </summary>
    class Exporter
    {

        private DefinitionExtractorConfig LoadConfig(string ConfgiPath)
        {
            var r = HackPleasanterApi.Generator.Library.Utility.XMLSerialize.Deserialize<DefinitionExtractorConfig>(ConfgiPath);
            return r;
        }
        private class JsonDefinition
        {
            public IEnumerable<SiteDefinition> SiteDefinitionConverter;
            public IEnumerable<InterfaceDefinition> InterfaceDefinitionConverter;
        }

        /// <summary>
        /// JSON定義を生成する
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private JsonDefinition ReadJsonDefinition(string path)
        {
            // 対象となるファイル群を読み取る
            var r = new JsonReader();
            var def = r.ReadAll<ExportJsonDefinition.Rootobject>(path);

            // データを変換して出力する
            var si = (new SiteDefinitionConverter()).Convert(def);

            var ifd = (new InterfaceDefinitionConverter()).Convert(def);

            return new JsonDefinition
            {
                InterfaceDefinitionConverter = ifd,
                SiteDefinitionConverter = si
            };
        }

        public void DoExporter(string configPath)
        {
            //設定ファイルを読み取る
            var config = LoadConfig(configPath);

            // サイト定義JSONを読み込む
            var r = ReadJsonDefinition(config.Input.SiteExportDefinitionFile);

            // 変数名を説明で上書きするオプションを使っている場合
            // 変換を実行する
            if (true == config.Output.UseDescriptionAsVariableName)
            {
                var tl = r.InterfaceDefinitionConverter.ToList();
                foreach (var ele in
                r.InterfaceDefinitionConverter
                    .Where(e => null != e)
                    .Where(e => false == string.IsNullOrWhiteSpace(e.Description))
                    )
                {

                    ele.VariableName = ele.Description;
                }

                r.InterfaceDefinitionConverter = tl;
            }

            // 読み込み結果をCSVで出力する
            (new CSVExport()).WriteSiteDefinition(r.SiteDefinitionConverter, config.Output.SiteDefinitionFile);
            (new CSVExport()).WriteInterfaceDefinition(r.InterfaceDefinitionConverter, config.Output.InterfaceDefinitionFile);
        }
    }
}
