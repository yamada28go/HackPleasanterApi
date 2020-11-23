﻿using HackPleasanterApi.Generator.CodeGenerator.Configs;
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
        private OutputPathInfo MakeOutputDirectory(GeneratorConfig config)
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
                var sp = Path.Combine(config.OutputConfig.OutputDirectory, "Service");
                r.ServicePath = sp;
                if (false == Directory.Exists(sp))
                {
                    // 出力パスが存在しなければディレクトを生成する
                    Directory.CreateDirectory(sp);
                }
            }

            // モデルの出力パス
            {
                var mp = Path.Combine(config.OutputConfig.OutputDirectory, "Models");
                r.ModelsPath = mp;
                if (false == Directory.Exists(mp))
                {
                    // 出力パスが存在しなければディレクトを生成する
                    Directory.CreateDirectory(mp);
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
                public static string ReadTemplate(GeneratorConfig config, string fileName)
                {
                    using (StreamReader sr = new StreamReader(fileName,
                        System.Text.Encoding.GetEncoding(config.TemplateFiles.Encoding)))
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
                /// サービス定義を生成する
                /// </summary>
                /// <param name="config"></param>
                /// <param name="ServiceTemplate"></param>
                /// <param name="outPath"></param>
                /// <param name="s"></param>
                public static void Service(GeneratorConfig config,
                    string ServiceTemplate,
                    OutputPathInfo outPath,
                    SiteInfos s)
                {
                    var result = TemplateExpansion("ServiceTemplate",ServiceTemplate,s);
                    {
                        // 文字コードを指定
                        System.Text.Encoding enc = System.Text.Encoding.GetEncoding(config.OutputConfig.Encoding);

                        // 出力ファイル名
                        var outFileName = Path.Combine(outPath.ServicePath,
                            $"{s.SiteDefinition.SiteVariableName}Service.{config.OutputConfig.OutputExtension}");
                        // ファイルを開く
                        using (StreamWriter writer = new StreamWriter(outFileName, false, enc))
                        {
                            // テキストを書き込む
                            writer.WriteLine(result);
                        }
                    }
                }


                /// <summary>
                /// モデル定義を生成する
                /// </summary>
                /// <param name="config"></param>
                /// <param name="ServiceTemplate"></param>
                /// <param name="outPath"></param>
                /// <param name="s"></param>
                public static void Model(GeneratorConfig config,
                    string ModelTemplate,
                    OutputPathInfo outPath,
                    SiteInfos s)
                {
                    var result = TemplateExpansion("ModelTemplateKey", ModelTemplate, s);
                    {
                        // 文字コードを指定
                        System.Text.Encoding enc = System.Text.Encoding.GetEncoding(config.OutputConfig.Encoding);

                        // 出力ファイル名
                        var outFileName = Path.Combine(outPath.ModelsPath,
                            $"{s.SiteDefinition.SiteVariableName}.{config.OutputConfig.OutputExtension}");
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
            // コードの出力対象パスを生成する
            var outPath = MakeOutputDirectory(config);

            using (StreamReader sr = new StreamReader(config.TemplateFiles.TemplateService,
                System.Text.Encoding.GetEncoding(config.TemplateFiles.Encoding)))
            {
                // サービス用テンプレート文字列
                var ServiceTemplate = Helper.General.ReadTemplate(config, config.TemplateFiles.TemplateService);
                // モデル用テンプレート文字列を読み込む
                var ModelTemplate = Helper.General.ReadTemplate(config, config.TemplateFiles.TemplateModel);

                // テンプレートの生成
                foreach (var s in context.Sites)
                {
                    // サービス用定義を生成
                    Helper.Generation.Service(config, ServiceTemplate, outPath, s);
                    // モデル用定義を生成
                    Helper.Generation.Model(config, ModelTemplate, outPath, s);
                }

            }

        }
    }
}
