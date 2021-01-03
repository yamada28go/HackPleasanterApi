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
using System.Text.Json;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;

using CsvHelper.Configuration;
using HackPleasanterApi.Generator.Library.Models.DB;
using HackPleasanterApi.Generator.Library.Models.JSON;
using HackPleasanterApi.Generator.Library.Models.CSV;
using HackPleasanterApi.Generator.Library.Models.CSV.Map;

namespace HackPleasanterApi.Generator.Library.Service
{
    /// <summary>
    /// CSV形式で出力する
    /// </summary>
    public class CSVExport
    {
        /// <summary>
        /// ファイルに書き出す
        /// </summary>
        /// <typeparam name="DataType"></typeparam>
        /// <typeparam name="MapType"></typeparam>
        /// <param name="datas"></param>
        /// <param name="filePath"></param>
        private void write<DataType, MapType>(IEnumerable<DataType> datas, string filePath)
                where MapType : ClassMap
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sjisEnc = Encoding.GetEncoding("Shift_JIS");
            using (var writer = new StreamWriter(filePath, false, sjisEnc))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                var config = csv.Configuration;
                config.HasHeaderRecord = true; // ヘッダーが存在する場合 true
                config.RegisterClassMap<MapType>();
                //tsv.Configuration.Delimiter = "\t";
                csv.WriteRecords(datas);
            }

        }

        /// <summary>
        /// サイト情報をエクスポートする
        /// </summary>
        public void doExportSiteInformation(IEnumerable<SiteModel> models, string filePath)
        {
            // エクスポート可能な形式へ変換する
            var el = Convert(models);

            var sites = el.GroupBy(e => e.SiteId).Select(e => e.FirstOrDefault())
                .Where(e => e != null)
                .Select(e => new SiteDefinition
                {
                    SiteId = e.SiteId,
                    Title = e.Title
                })
                .OrderBy(e => e.SiteId)
                .ToList();

            write<SiteDefinition, SiteDefinitionMap>(sites, filePath);
        }

        public void WriteSiteDefinition(IEnumerable<SiteDefinition> models, string filePath) {
            write<SiteDefinition, SiteDefinitionMap>(models, filePath);
        }


        public void WriteInterfaceDefinition(IEnumerable<InterfaceDefinition> models, string filePath)
        {
            write<InterfaceDefinition, InterfaceDefinitionMap>(models, filePath);
        }

        /// <summary>
        /// DBから読んできた形式ほ内部形式に変換する
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private IEnumerable<InterfaceDefinition> Convert(IEnumerable<SiteModel> models)
        {
            var l =
            models
            .Where(e => e.SiteSettings != null).Select(e =>
              {
                  var siteSettings = System.Text.Json.JsonSerializer.Deserialize<SiteSettings>(e.SiteSettings);
                  if (null == siteSettings.Columns)
                  {
                      return null;
                  }
                  else
                  {
                      return siteSettings.Columns.Select(si =>
                      {

                          return new InterfaceDefinition
                          {
                              Title = e.Title,
                              SiteId = e.SiteId,
                              ParentId = e.ParentId,
                              InheritPermission = e.InheritPermission,

                              ColumnName = si.ColumnName,
                              LabelText = si.LabelText,
                              VariableName = String.Empty

                          };
                      }  );
                  }

              })
             .Where(e => e != null)
            .SelectMany(e => e)
            .Where(e => e != null)
            .OrderBy(e => e.SiteId)
            .ThenBy(e => e.ColumnName)
            .ToList();

            return l;
        }

    }
}
