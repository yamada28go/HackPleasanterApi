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
using HackPleasanterApi.Generator.CodeGenerator.Loder;
using Microsoft.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace HackPleasanterApi.Generator.CodeGenerator
{
    class Program
    {
        /// <summary>
        /// 実装補助関数
        /// </summary>
        private class Helper
        {
            /// <summary>
            /// デフォルト設定名
            /// </summary>
            private static readonly string DefaultConfigurationName = "CodeGeneratorConfig.xml";

            /// <summary>
            /// 定義出力を行う
            /// </summary>
            public class Generate
            {

                public static string GetCommandName()
                {
                    return "Generate";
                }

                public Generate(CommandLineApplication command)
                {

                    // 説明（ヘルプの出力で使用される）
                    command.Description = "Pleasanter用の各種定義を自動生成する";

                    // コマンドについてのヘルプ出力のトリガーとなるオプションを指定
                    command.HelpOption("-?|-h|--help");

                    // コマンドの引数（名前と説明を引数で渡しているが、これはヘルプ出力で使用される）
                    var tArgs = command.Argument("[ConfigFileName]", "設定ファイル名");

                    command.OnExecute(() =>
                    {
                        return On(tArgs);
                    });

                }

                private int On(CommandArgument arg)
                {
                    var cfgName = DefaultConfigurationName;
                    if (0 != arg.Values.Count)
                    {
                        cfgName = arg.Value;
                    }

                    // テンプレートコードを生成する
                    DoGenerate(cfgName);

                    return 0;
                }

                private void DoGenerate(string cfgName)
                {
                    //設定を読み込む
                    var c = HackPleasanterApi.Generator.Library.Utility.XMLSerialize.Deserialize<GeneratorConfig>(cfgName);
                    var l = (new CSVLoader()).DoLoad(c);
                    var ct = new GenerationContext
                    {
                        Sites = l
                    };

                    // コード定義を生成する
                    (new Generators.Generator()).DoGenerae(c, ct);
                }

            }

            /// <summary>
            /// デフォルト設定を生成するクラス
            /// </summary>
            public class OnDefaultConfiguration
            {
                public static string GetCommandName()
                {
                    return "GetConfiguration";
                }

                public OnDefaultConfiguration(CommandLineApplication command)
                {

                    // 説明（ヘルプの出力で使用される）
                    command.Description = "デフォルト設定を生成する";

                    // コマンドについてのヘルプ出力のトリガーとなるオプションを指定
                    command.HelpOption("-?|-h|--help");

                    // コマンドの引数（名前と説明を引数で渡しているが、これはヘルプ出力で使用される）
                    var tArgs = command.Argument("[Hogeの引数]", "Hogeの引数の説明");

                    command.OnExecute(() =>
                    {
                        return On(tArgs);
                    });

                }

                private int On(CommandArgument arg)
                {
                    // デフォルト設定
                    var c = new GeneratorConfig
                    {
                        InputFiles = new GeneratorConfig.Definition.InputFiles
                        {

                            InterfaceDefinitionFile = @"Interface.csv",
                            SiteDefinitionFile = @"Sites.csv",
                            Encoding = "Shift_JIS"

                        },
                        TemplateFiles = new List<GeneratorConfig.Definition.TemplateFiles> {
                            new GeneratorConfig.Definition.TemplateFiles
                            {
                                OutputSubdirectoryName = "Services",
                                TemplateFileName = @"..\Generator\Templates\CSharp\ServiceTemplate.txt",
                                Encoding = "Shift_JIS",
                                OutputExtension = @"cs",
                                HeadPrefix ="",
                                EndPrefix = "Service"
                            },
                            new GeneratorConfig.Definition.TemplateFiles
                            {
                                OutputSubdirectoryName = "Models",
                                TemplateFileName = @"..\Generator\Templates\CSharp\ModelTemplate.txt",
                                Encoding = "Shift_JIS",
                                OutputExtension = @"cs",    
                                HeadPrefix ="",
                                EndPrefix = "Model"
                            }
                        },
                        OutputConfig = new GeneratorConfig.Definition.OutputConfig
                        {
                            OutputDirectory = @".\Generated"
                        },
                        CodeConfig = new GeneratorConfig.Definition.CodeConfig
                        {
                            NameSpace = "",
                        }
                    };

                    // XML形式としてデフォルト設定を生成する
                    var cfgName = DefaultConfigurationName;
                    if (0 != arg.Values.Count)
                    {
                        cfgName = arg.Value;
                    }
                    HackPleasanterApi.Generator.Library.Utility.XMLSerialize.Serialize(c, cfgName);

                    return 0;

                }
            }
        }

        static void Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                // アプリケーション名（ヘルプの出力で使用される）
                Name = "CodeGenerator",
            };

            // ヘルプ出力のトリガーとなるオプションを指定
            app.HelpOption("-?|-h|--help");

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            // 追加コマンド

            // 定義生成コマンド
            app.Command(Helper.Generate.GetCommandName(), (command) =>
            {
                new Helper.Generate(command);
            });

            // デフォルト設定生成コマンド
            app.Command(Helper.OnDefaultConfiguration.GetCommandName(), (command) =>
            {
                new Helper.OnDefaultConfiguration(command);
            });

            app.Execute(args);
        }
    }
}
