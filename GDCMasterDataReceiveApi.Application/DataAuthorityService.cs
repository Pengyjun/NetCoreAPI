using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DataAuthority;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application
{
    /// <summary>
    /// 数据可查看字段实现
    /// </summary>
    public class DataAuthorityService : IDataAuthorityService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 注入上下文
        /// </summary>
        /// <param name="dbContext"></param>
        public DataAuthorityService(ISqlSugarClient dbContext)
        {
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取用户可查看的字段
        /// </summary>
        /// <param name="uId">用户id</param>
        /// <param name="rId">角色id</param>
        /// <param name="instutionId">机构id</param>
        /// <param name="depId">部门id</param>
        /// <param name="pjectId">项目id</param>
        /// <returns></returns>
        public async Task<DataAuthorityDto> GetDataAuthorityAsync(long uId, string rId, long instutionId, long? depId, long? pjectId)
        {
            var value = await _dbContext.Queryable<DataAuthority>()
                .WhereIF(!string.IsNullOrWhiteSpace(depId.ToString()), t => t.DepId == depId)
                .WhereIF(!string.IsNullOrWhiteSpace(pjectId.ToString()), t => t.PjectId == pjectId)
                .Where(t => t.UId == uId && t.InstutionId == instutionId && t.RId == rId && t.IsDelete == 1)
                .FirstAsync();

            /***
             * 查询已授权的可查看的字段
             */
            if (value != null && !string.IsNullOrWhiteSpace(value.AuthorityColumns))
                return new DataAuthorityDto { Id = value.Id, AuthorityColumns = value.AuthorityColumns.Split(',').ToList() };
            else return new DataAuthorityDto();
        }
        /// <summary>
        /// 新增或修改可授权字段（列表选择字段点击确认后使用）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colums">,拼接的字段串</param>
        /// <param name="uId">当前操作人id 不可为空</param>
        /// <param name="rId">当前操作人角色id</param>
        /// <param name="instutionId">当前操作人机构id 不可为空</param>
        /// <param name="depId">当前操作人项目部id</param>
        /// <param name="pjectId">当前操作人项目id</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InsertOrModifyDataAuthoryAsync(long id, string? colums, long uId, string rId, long instutionId, long? depId, long? pjectId)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            if (string.IsNullOrEmpty(colums)) { responseAjaxResult.FailResult(HttpStatusCode.SaveFail, "没有选择展示的列名"); return responseAjaxResult; }

            //获取授权数据
            var value = await _dbContext.Queryable<DataAuthority>()
                .Where(t => t.Id == id && t.IsDelete == 1)
                .FirstAsync();

            /***
             * 如果当前的字段多余已存在的 那么替换
             * 如果更少 判断是否是同一个人进行更改 如果不是找第一次修改的人调整或者让第一次修改的人删除 当前操作人可重新调整
             */
            if (value != null)
            {
                //原本库的字段
                var kColumns = !string.IsNullOrWhiteSpace(value.AuthorityColumns) ? value.AuthorityColumns.Split(',').ToList() : null;

                //传入的字段
                var paramsColums = !string.IsNullOrWhiteSpace(colums) ? colums.Split(",").ToList() : null;

                //库字段与传入字段不一致
                string newColums = string.Empty;
                if (kColumns != null && paramsColums != null)
                {
                    //如果传入字段更多 修改
                    if (kColumns.Count < paramsColums.Count) newColums = string.Join(",", paramsColums);

                    //如果字段更少 || 相等 
                    if (!(kColumns.Count < paramsColums.Count))
                    {
                        //匹配数据操作人是否一致
                        if (value.UId == uId) newColums = string.Join(",", paramsColums);
                        //数据操作人不一致 查询原操作人
                        else
                        {
                            //获取原操作人
                            var uInfo = await _dbContext.Queryable<Person>()
                                .Where(t => t.IsDelete == 1 && t.Id == value.UId)
                                .FirstAsync();

                            string name = string.Empty;
                            //字段相同
                            if (kColumns.Count == paramsColums.Count) name = "更改";
                            //字段减少
                            if (kColumns.Count > paramsColums.Count) name = "减少";
                            responseAjaxResult.FailResult(HttpStatusCode.SaveFail, $"不可{name}原列名数量，如需更改，进入维护页面删除此数据；或联系" + uInfo?.NAME + "（" + uInfo?.PHONE + "）更改/删除");
                            return responseAjaxResult;
                        }
                    }
                }
                //修改数据
                value.AuthorityColumns = newColums;
                await _dbContext.Updateable(value).WhereColumns(t => t.Id).UpdateColumns(t => new { t.AuthorityColumns, t.UpdateId, t.UpdateTime }).ExecuteCommandAsync();
            }
            else
            {
                //新增数据
                var addValue = new DataAuthority
                {
                    DepId = depId,
                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    AuthorityColumns = colums,
                    InstutionId = instutionId,
                    PjectId = pjectId,
                    RId = rId,
                    UId = uId
                };
                await _dbContext.Insertable(addValue).ExecuteCommandAsync();
            }

            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
    }
}
