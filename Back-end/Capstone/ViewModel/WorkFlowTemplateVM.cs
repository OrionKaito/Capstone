﻿using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplatePaginVM
    {
        public int TotalRecord { get; set; }
        public IEnumerable<WorkFlowTemplateVM> WorkFlowTemplates { get; set; }
    }

    public class WorkFlowTemplateVM
    {
        public Guid ID { get; set; }
        public Guid OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string Icon { get; set; }
        public Guid PermissionToUseID { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsViewDetail { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class WorkFlowTemplateCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string Icon { get; set; }
        public bool IsViewDetail { get; set; }
        public Guid PermissionToUseID { get; set; }
    }

    //public class WorkFlowTemplateUM
    //{
    //    public Guid WorkFlowTemplateID { get; set; }
    //    public string Data { get; set; }
    //    public bool IsViewDetail { get; set; }
    //    public string Icon { get; set; }
    //    public IEnumerable<WorkFlowActionCM> Actions { get; set; }
    //    public IEnumerable<WorkFlowTemplateActionConnectionCM> Connections { get; set; }
    //}

    public class WorkFlowActionCM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ActionTypeID { get; set; }
        public Guid? PermissionToUseID { get; set; }
        public bool IsApprovedByLineManager { get; set; }
        public bool IsApprovedByInitiator { get; set; }
        public string ToEmail { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }

    public class WorkFlowConnectionCM
    {
        public Guid FromWorkFlowTemplateActionID { get; set; }
        public Guid ToWorkFlowTemplateActionID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int TimeInterval { get; set; }
        public TimeEnum Type { get; set; }
    }

    public class SaveWowkFlowTemplateUM
    {
        public Guid WorkFlowTemplateID { get; set; }
        public string Data { get; set; }
        public string Icon { get; set; }
        public bool IsViewDetail { get; set; }
        public IEnumerable<WorkFlowActionCM> Actions { get; set; }
        public IEnumerable<WorkFlowConnectionCM> Connections { get; set; }
    }

    public class SaveCraftTemplateUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string Icon { get; set; }
        public bool IsViewDetail { get; set; }
        public Guid PermissionToUseID { get; set; }
    }
}
