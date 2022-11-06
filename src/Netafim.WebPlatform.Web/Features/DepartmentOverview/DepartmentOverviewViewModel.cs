using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.DepartmentOverview
{

    public class DepartmentOverviewViewModel : IBlockViewModel<DepartmentOverviewBlock>
    {
        public DepartmentOverviewBlock CurrentBlock { get; }
        public string FilterAnchorId { get; }

        public DepartmentOverviewViewModel(DepartmentOverviewBlock currentBlock, string filterAnchorId)
        {
            this.CurrentBlock = currentBlock;
            this.FilterAnchorId = filterAnchorId;
        }
    }
}