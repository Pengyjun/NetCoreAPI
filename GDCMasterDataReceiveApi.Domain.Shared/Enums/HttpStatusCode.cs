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
        /// 上传成功
        /// </summary>
        UploadSuccess = 100005,
        /// <summary>
        /// 上传失败
        /// </summary>
        UploadFail = 100006,

        /// <summary>
        /// 上传文件类型不允许
        /// </summary>
        UploadFileTypeNoAllow = 100007,
        /// <summary>
        /// 上传文件大小不允许
        /// </summary>
        UploadFileSizeNoAllow = 100008,

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
        /// 该角色已经有人任职
        /// </summary>
        RoleEmployed = 100016,
        /// <summary>
        /// 请先上传文件
        /// </summary>
        FileFirst = 100017,
        /// <summary>
        /// 系统角色不能再次添加其他角色
        /// </summary>
        SystemRole = 100018,

        /// <summary>
        /// 导出错误
        /// </summary>
        ImportExcel = 100019,

        /// <summary>
        /// 该结构只能移动到二级分类下
        /// </summary>
        NotShiftStructure = 100020,

        /// <summary>
        /// 不允许更改
        /// </summary>
        NotAllowChange = 100021,

        /// <summary>
        /// 找不到文件
        /// </summary>
        NotFindFile = 100022,

        /// <summary>
        /// 数据不匹配
        /// </summary>
        DataNotMatch = 100023,
        /// <summary>
        /// 激活当前登录用户失败
        /// </summary>
        ActivateUserFile = 100024,
        /// <summary>
        /// 当前角色下面已存在用户
        /// </summary>
        RoleUserExist = 100025,
        /// <summary>
        /// 最多上传多少个（默认10）个文件
        /// </summary>
        MaximumUpload = 100026,
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
        /// 审批失败
        /// </summary>
        ApproveFail = 100030,
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
        /// 找不到项目经理
        /// </summary>
        NotFoundProjectLeader = 100034,
        /// <summary>
        /// 找不到项目经理信息
        /// </summary>
        NotFoundProjectLeaderInfo = 100035,
        /// <summary>
        /// 提交失败
        /// </summary>
        FailSubmit = 100036,
        /// <summary>
        /// 授权失败
        /// </summary>
        AuthoryFail= 100037,
    }
}
