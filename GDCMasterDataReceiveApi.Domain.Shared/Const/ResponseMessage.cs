namespace GDCMasterDataReceiveApi.Domain.Shared.Const
{
    /// <summary>
	/// 响应信息常量设置
	/// </summary>
	public class ResponseMessage
    {
        public const string SYSTEM_ERROR = "系统错误";
        public const string OPERATION_SUCCESS = "响应成功";
        public const string OPERATION_FAIL = "响应失败";
        public const string OPERATION_Token_FAIL = "Token不合法";
        public const string OPERATION_PARAMETER_ERROR = "请求参数输入错误请检查";
        public const string OPERATION_DATA_NOTEXIST = "数据不存在";
        public const string OPERATION_UPDATE_FAIL = "更新失败";
        public const string OPERATION_UPDATE_SUCCESS = "更新成功";
        public const string OPERATION_SAVE_SUCCESS = "保存成功";
        public const string OPERATION_SAVE_FAIL = "保存失败";
        public const string OPERATION_DELETE_FAIL = "删除失败";
        public const string OPERATION_NOT_DATA = "无数据";
        public const string OPERATION_DELETE_SUCCESS = "删除成功";
        public const string OPERATION_INSERT_FAIL = "新增失败";
        public const string OPERATION_INSERT_SUCCESS = "新增成功";
        public const string OPERATION_AUTHORIZATION_SUCCESS = "授权成功";
        public const string OPERATION_NOLOGIN_FAIL = "您还没有登录请先登录";
        public const string OPERATION_LOGINOUT_SUCCESS = "您已在其他终端退出，请重新登录";
        public const string ACCESSINTERFACE_ERROR = "接口未开通权限，请联系管理员";
    }
    /// <summary>
    /// 响应信息状态
    /// </summary>
    public class ResponseStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string SUCCESS = "S";
        /// <summary>
        /// 错误
        /// </summary>
        public const string ERROR = "E";
    }
}
