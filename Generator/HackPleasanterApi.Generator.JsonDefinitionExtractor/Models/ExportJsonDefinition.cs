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

namespace HackPleasanterApi.Generator.JsonDefinitionExtractor.Models
{
    /// <summary>
    /// プリザンター側で展開されてきたJSON定義
    /// </summary>
    public class ExportJsonDefinition
    {

        public class Rootobject
        {
            public Headerinfo HeaderInfo { get; set; }
            public Site[] Sites { get; set; }
            public object[] Data { get; set; }
            public Permission[] Permissions { get; set; }
            public Permissionidlist PermissionIdList { get; set; }
        }

        public class Headerinfo
        {
            public int BaseSiteId { get; set; }
            public string Server { get; set; }
            public string CreatorName { get; set; }
            public DateTime PackageTime { get; set; }
            public Convertor[] Convertors { get; set; }
            public bool IncludeSitePermission { get; set; }
            public bool IncludeRecordPermission { get; set; }
            public bool IncludeColumnPermission { get; set; }
        }

        public class Convertor
        {
            public int SiteId { get; set; }
            public string SiteTitle { get; set; }
            public string ReferenceType { get; set; }
            public bool IncludeData { get; set; }
            public string Order { get; set; }
        }

        public class Permissionidlist
        {
            public object[] DeptIdList { get; set; }
            public object[] GroupIdList { get; set; }
            public Useridlist[] UserIdList { get; set; }
        }

        public class Useridlist
        {
            public int UserId { get; set; }
            public string LoginId { get; set; }
        }

        public class Site
        {
            public int TenantId { get; set; }
            public int SiteId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string GridGuide { get; set; }
            public string EditorGuide { get; set; }
            public string ReferenceType { get; set; }
            public int ParentId { get; set; }
            public int InheritPermission { get; set; }
            public Sitesettings SiteSettings { get; set; }
            public bool Publish { get; set; }
            public object[] Comments { get; set; }
        }

        public class Sitesettings
        {
            public float Version { get; set; }
            public string ReferenceType { get; set; }
            public Editorcolumnhash EditorColumnHash { get; set; }
            public Column[] Columns { get; set; }
        }

        public class Editorcolumnhash
        {
            public string[] General { get; set; }
        }

        public class Column
        {
            public string ColumnName { get; set; }
            public string LabelText { get; set; }

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
        }

        public class Permission
        {
            public int SiteId { get; set; }
            public Permission1[] Permissions { get; set; }
        }

        public class Permission1
        {
            public int ReferenceId { get; set; }
            public int DeptId { get; set; }
            public int GroupId { get; set; }
            public int UserId { get; set; }
            public int PermissionType { get; set; }
        }


    }
}
