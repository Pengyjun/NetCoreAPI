using GHMonitoringCenterApi.Application.Contracts.Dto.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GHMonitoringCenterApi.Application.Contracts.Dto.Job.SubmitJobOfProjectWBSRequestDto;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 项目结构模型Dto
    /// </summary>
    public abstract  class ProjectWBSModelDto<TDto> : IModelState where TDto: ProjectWBSModelDto<TDto>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// keyId
        /// </summary>
        public string? KeyId { get; set; }

        /// <summary>
        /// Pid
        /// </summary>
        public string? Pid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 工程量
        /// </summary>
        public decimal? EngQuantity { get; set; }

        /// <summary>
        /// 项目编号（清单编码）
        /// </summary>
        public string? ProjectNum { get; set; }

        /// <summary>
        /// 业务数据状态（0:无状态，1：新增，2：修改，3：删除）
        /// </summary>
        public ModelState State { get; set; }

        /// <summary>
        /// 项目结构子级
        /// </summary>
        public TDto[]? Children { get; set; }

    }
}
