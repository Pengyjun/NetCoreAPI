namespace GDCMasterDataReceiveApi.Domain.Shared.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum HttpStatusCode
    {
        /// <summary>
        /// 接口请求成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 系统错误
        /// </summary>
        Fail = 500,
        /// <summary>
        /// 请求参数错误
        /// </summary>
        ParameterError = 400,
        /// <summary>
        /// token不合法
        /// </summary>
        TokenError = 401,
        /// <summary>
        /// 更新失败
        /// </summary>
        UpdateFail = 100001,
        /// <summary>
        /// 数据不存在
        /// </summary>
        DataNotEXIST = 100002,
        /// <summary>
        /// 删除失败
        /// </summary>
        DeleteFail = 100003,
        /// <summary>
        /// 新增失败
        /// </summary>
        InsertFail = 100004,
        /// <summary>
        /// ContentType必须是multipart/from-data格式
        /// </summary>
        ContentTypeNoAllow = 100009,
        /// <summary>
        /// 您还没有登录请先登录
        /// </summary>
        NoLogin = 100010,
        /// <summary>
        /// 对不起您没有权限操作
        /// </summary>
        NoPermission = 100011,
        /// <summary>
        /// 角色不存在
        /// </summary>
        RoleNotEXIST = 100012,
        /// <summary>
        /// 保存失败
        /// </summary>
        SaveFail = 100013,
        /// <summary>
        /// 该公司下已存在相同项目名称
        /// </summary>
        CompanyIdentical = 100014,
        /// <summary>
        /// 机构同种类型只能有一个
        /// </summary>
        SametypeIdentical = 100015,
        /// <summary>
        /// 系统角色不能再次添加其他角色
        /// </summary>
        SystemRole = 100018,
        /// <summary>
        /// 导出错误
        /// </summary>
        ImportExcel = 100019,
        /// <summary>
        /// 不允许更改
        /// </summary>
        NotAllowChange = 100021,
        /// <summary>
        /// 激活当前登录用户失败
        /// </summary>
        ActivateUserFile = 100024,
        /// <summary>
        /// 当前角色下面已存在用户
        /// </summary>
        RoleUserExist = 100025,
        /// <summary>
        /// 推送失败
        /// </summary>
        PushFail = 100027,
        /// <summary>
        /// 消息发送失败
        /// </summary>
        MsgSendFail = 100027,
        /// <summary>
        /// 被占用
        /// </summary>
        Occupied = 100028,
        /// <summary>
        /// 消息撤回失败
        /// </summary>
        RecallFail = 100029,
        /// <summary>
        /// 无权限操作
        /// </summary>
        NoAuthorityOperateFail = 100031,
        /// <summary>
        /// 验证失败
        /// </summary>
        VerifyFail = 100032,
        /// <summary>
        /// 不是定时任务发起禁止调用
        /// </summary>
        NotTimingTask = 100033,
        /// <summary>
        /// 授权失败
        /// </summary>
        AuthoryFail = 100037,
        /// <summary>
        /// 链接失败
        /// </summary>
        LinkFail = 100038,
        /// <summary>
        /// 查询失败
        /// </summary>
        SelectFail = 100040,
        /// <summary>
        /// 请设置密码
        /// </summary>
        SetPassword = 100041,
        /// <summary>
        /// 账号信息不存在
        /// </summary>
        AccountNotEXIST = 100042,
        /// <summary>
        /// 接口未开通权限，请联系管理员
        /// </summary>
        InterfaceAuth = 100043,
    }
}
