﻿using CsvHelper.Configuration;
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
            Map(c => c.ColumnName).Index(4);
            Map(c => c.LabelText).Index(5);
            Map(c => c.VariableName).Index(6);
            Map(c => c.IsTarget).Index(7);
            Map(c => c.Description).Index(8);
            Map(c => c.ValidateRequired).Index(8);
        }


    }
}
