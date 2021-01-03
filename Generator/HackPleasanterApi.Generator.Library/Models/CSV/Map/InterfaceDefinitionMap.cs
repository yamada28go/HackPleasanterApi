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

using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HackPleasanterApi.Generator.Library.Models.CSV.Map
{
    public class InterfaceDefinitionMap : ClassMap<InterfaceDefinition>
    {

        public InterfaceDefinitionMap()
        {
            Map(c => c.Title).Index(0);
            Map(c => c.SiteId).Index(1);
            Map(c => c.ParentId).Index(2);
            Map(c => c.InheritPermission).Index(3);
            Map(c => c.Description).Index(4);
            Map(c => c.ValidateRequired).Index(5);
            Map(c => c.ColumnName).Index(6);
            Map(c => c.LabelText).Index(7);
            Map(c => c.VariableName).Index(8);
            Map(c => c.IsTarget).Index(9);
            Map(c => c.ChoicesText).Index(10);

        }


    }
}
