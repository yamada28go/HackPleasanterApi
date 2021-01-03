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
using HackPleasanterApi.Generator.CodeGenerator.Models;
using RazorLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HackPleasanterApi.Generator.CodeGenerator.Generators
{
    /// <summary>
    /// コード生成機
    /// </summary>
    class Generator
    {
        /// <summary>
        /// 出力パス情報
        /// </summary>
        class OutputPathInfo
        {

            /// <summary>
            /// サービス定義用のパス
            /// </summary>
            public string ServicePath;

            /// <summary>
            /// モデル定義用のパス
            /// </summary>
            public string ModelsPath;

        }

        /// <summary>
        /// 出力先のディレクトリを生成する
        /// </summary>
        private OutputPathInfo MakeOutputDirectory(GeneratorConfig config ,
            GeneratorConfig.Definition.TemplateFiles template)
        {
            var r = new OutputPathInfo();

            // コード全体の出力パス
            if (false == Directory.Exists(config.OutputConfig.OutputDirectory))
            {
                // 出力パスが存在しなければディレクトを生成する
                Directory.CreateDirectory(config.OutputConfig.OutputDirectory);
            }

            // サービスコードの出力パス
            {
                var sp = Path.Combine(config.OutputConfig.OutputDirectory, template.OutputSubdirectoryName);
                r.ServicePath = sp;
                if (false == Directory.Exists(sp))
                {
                    // 出力パスが存在しなければディレクトを生成する
                    Directory.CreateDirectory(sp);
                }
            }

            return r;
        }

        #region 補助関数

        class Helper
        {
            /// <summary>
            /// 一般的な処理実装
            /// </summary>
            public class General
            {
                /// <summary>
                /// テンプレートデータを読み込む
                /// </summary>
                /// <returns></returns>
                public static string ReadTemplate(GeneratorConfig.Definition.TemplateFiles config)
                {
                    using (StreamReader sr = new StreamReader(config.TemplateFileName,
                        System.Text.Encoding.GetEncoding(config.Encoding)))
                    {
                        //サービス用テンプレート文字列
                        string ServiceTemplate = sr.ReadToEnd();
                        return ServiceTemplate;
                    }
                }
            }

            /// <summary>
            /// 生成関係
            /// </summary>
            public class Generation
            {

                /// <summary>
                /// テンプレートファイル展開を行う
                /// </summary>
                /// <param name="TemplateString"></param>
                /// <param name="info"></param>
                /// <returns></returns>
                public static string TemplateExpansion(
                    string TemplateKey,
                    string TemplateString,
                    SiteInfos info)
                {
                    // NetCoreへの対応状況により
                    // 以下ライブラリに差し替えする
                    // https://github.com/toddams/RazorLight


                    // ToDo 
                    // 暫定で一番簡単な方法で実装する。
                    // 別途パフォーマンス調整の方法があるハズ
                    var engine = new RazorLightEngineBuilder()
                        // required to have a default RazorLightProject type,
                        // but not required to create a template from string.
                        .UseEmbeddedResourcesProject(typeof(Program))
                        .UseMemoryCachingProvider()
                        .Build();

                    var result = engine.CompileRenderStringAsync(TemplateKey, TemplateString, info);
                    result.Wait();

                    var cacheResult = engine.Handler.Cache.RetrieveTemplate(TemplateKey);
                    if (cacheResult.Success)
                    {
                        var templatePage = cacheResult.Template.TemplatePageFactory();
                        var tresult = engine.RenderTemplateAsync(templatePage, info);

                        tresult.Wait();

                        var v = tresult.Result;

                        return v;

                    }

                    return "";
                }


                /// <summary>
                /// コード定義を生成する
                /// </summary>
                /// <param name="config"></param>
                /// <param name="ServiceTemplate"></param>
                /// <param name="outPath"></param>
                /// <param name="s"></param>
                public static void DoCodeGeneration(GeneratorConfig.Definition.TemplateFiles config,
                    string templateSteing ,
                    OutputPathInfo outPath,
                    SiteInfos s)
                {
                    // 内部でテンプレートの出力設定を参照できるように書き換える
                    s.TemplateFilesConfig = config;
                    var result = TemplateExpansion(config.TemplateFileName, templateSteing, s);
                    {
                        // 文字コードを指定
                        System.Text.Encoding enc = System.Text.Encoding.GetEncoding(config.Encoding);

                        // 出力ファイル名
                        var outFileName = Path.Combine(outPath.ServicePath,
                            $"{config.HeadPrefix}{s.SiteDefinition.SiteVariableName}{config.EndPrefix}.{config.OutputExtension}");
                        // ファイルを開く
                        using (StreamWriter writer = new StreamWriter(outFileName, false, enc))
                        {
                            // テキストを書き込む
                            writer.WriteLine(result);
                        }
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// 対象コードを生成する
        /// </summary>
        /// <param name="config"></param>
        /// <param name="context"></param>
        public void DoGenerae(GeneratorConfig config, GenerationContext context)
        {

            foreach (var templateConfig in config.TemplateFiles)
            {
                // テンプレート用文字列
                var templateString = Helper.General.ReadTemplate(templateConfig);

                // コードの出力対象パスを生成する
                var outPath = MakeOutputDirectory(config , templateConfig);

                // テンプレートの生成
                foreach (var s in context.Sites)
                {
                    // サービス用定義を生成
                    Helper.Generation.DoCodeGeneration(templateConfig, templateString, outPath, s);
                }
            }

        }
    }
}
