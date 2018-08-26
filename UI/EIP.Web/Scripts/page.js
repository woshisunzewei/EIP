//定义使用的js
define([], function () {
    return {
        //Common
        'common-usercontrol-chosen-dictionary-all': 'app/common/usercontrol/chosen-dictionary-all', //选择所有字典
        'common-usercontrol-chosen-dictionary-edit-all': 'app/common/usercontrol/chosen-dictionary-edit-all', //选择所有字典
        'common-usercontrol-chosen-menu': 'app/common/usercontrol/chosen-menu', //选择菜单
        'common-usercontrol-chosen-icon': 'app/common/usercontrol/chosen-icon', //选择图标
        'common-usercontrol-chosen-dictionary': 'app/common/usercontrol/chosen-dictionary', //选择字典
        'common-usercontrol-chosen-privilegemaster-user': 'app/common/usercontrol/chosen-privilegemaster-user', //选择人员
        'common-usercontrol-privilegemaster-user': 'app/common/usercontrol/privilegemaster-user', //选择人员
        'common-usercontrol-chosen-group-all': 'app/common/usercontrol/chosen-group-all', //选择所有组
        'common-usercontrol-chosen-organization-ueditor-all': 'app/common/usercontrol/chosen-organization-ueditor-all', //选择所有组织机构
        'common-usercontrol-chosen-organization-all': 'app/common/usercontrol/chosen-organization-all', //选择所有组织机构
        'common-usercontrol-chosen-organization': 'app/common/usercontrol/chosen-organization', //选择所有组织机构
        'common-usercontrol-chosen-post-all': 'app/common/usercontrol/chosen-post-all', //选择所有岗位

        //Console
        'console-home-index': 'app/console/home/index', //首页
        'console-main-changepassword': 'app/console/main/changepassword', //修改密码

        //Demo
        'test-list': 'app/demo/test/list',

        //System
        'system-emailaccount-list': 'app/system/emailaccount/list', //邮箱账号列表
        'system-emailaccount-edit': 'app/system/emailaccount/edit', //邮箱账号编辑
        'system-config-logconfigedit': 'app/system/config/logconfigedit', //日志配置编辑
        'system-config-baseconfigedit': 'app/system/config/baseconfigedit', //基础配置编辑
        'system-loginslide-list': 'app/system/loginslide/list', //登录幻灯片列表
        'system-loginslide-edit': 'app/system/loginslide/edit', //登录幻灯片编辑
        'system-database-list': 'app/system/database/list', //数据库信息列表
        'system-database-edit': 'app/system/database/edit', //数据库信息编辑
        'system-database-spaceused': 'app/system/database/spaceused', //数据库空间信息
        'system-database-backuporrestore': 'app/system/database/backuporrestore', //数据库备份或还原
        'system-menu-list': 'app/system/menu/list', //菜单集合
        'system-menu-edit': 'app/system/menu/edit', //菜单编辑
        'system-menubutton-list': 'app/system/menubutton/list', //菜单按钮集合
        'system-menubutton-edit': 'app/system/menubutton/edit', //菜单按钮编辑
        'system-menubutton-havefunctions': 'app/system/menubutton/havefunctions', //关联模块菜单
        'system-menubutton-chosenfunctions': 'app/system/menubutton/chosenfunctions', //选择关联模块菜单
        'system-data-list': 'app/system/data/list', //数据权限列表
        'system-data-edit': 'app/system/data/edit', //数据权限编辑
        'system-field-list': 'app/system/field/list', //字段列表
        'system-field-edit': 'app/system/field/edit', //字段编辑
        'system-dictionary-list': 'app/system/dictionary/list', //字典集合
        'system-dictionary-edit': 'app/system/dictionary/edit', //字典编辑
        'system-district-list': 'app/system/district/list', //行政区划集合
        'system-district-edit': 'app/system/district/edit', //行政区划编辑
        'system-app-list': 'app/system/app/list', //应用系统集合
        'system-app-edit': 'app/system/app/edit', //应用系统编辑
        'system-app-checkcodelist': 'app/system/app/checkcodelist', //系统检查
        'system-user-list': 'app/system/user/list', //人员信息列表
        'system-user-edit': 'app/system/user/edit', //人员信息编辑
        'system-role-list': 'app/system/role/list', //角色信息列表
        'system-role-edit': 'app/system/role/edit', //角色信息编辑
        'system-role-chosen': 'app/system/role/chosen', //角色信息选择
        'system-organization-list': 'app/system/organization/list', //组织机构列表
        'system-organization-edit': 'app/system/organization/edit', //组织机构编辑
        'system-post-list': 'app/system/post/list', //岗位列表
        'system-post-edit': 'app/system/post/edit', //岗位编辑
        'system-group-list': 'app/system/group/list', //组列表
        'system-group-edit': 'app/system/group/edit', //组编辑
        'system-permission-menu': 'app/system/permission/menu', //菜单权限
        'system-permission-function': 'app/system/permission/function', //功能性权限
        'system-permission-field': 'app/system/permission/field', //字段权限
        'system-permission-data': 'app/system/permission/data', //数据权限
        'system-log-operationlog': 'app/system/log/operationlog', //操作日志
        'system-log-exceptionlog': 'app/system/log/exceptionlog', //异常日志
        'system-log-txtlog': 'app/system/log/txtlog', //文本日志
        'system-log-loginlog': 'app/system/log/loginlog', //登录日志
        'system-log-sqllog': 'app/system/log/sqllog', //Sql日志
        'system-log-datalog': 'app/system/log/datalog', //数据日志
        'system-running-assemblylist': 'app/system/running/assemblylist', //程序集
        'system-running-sqlcachelist': 'app/system/running/sqlcachelist', //sql缓存
        'system-codegeneration-pc': 'app/system/codegeneration/pc', //pc代码生成
        'system-codegeneration-pc-codegeneration-form-edit': 'app/system/codegeneration/pc-codegeneration-form-edit', //程序集
        'system-article-list': 'app/system/article/list',//文章新闻列表
        'system-article-edit': 'app/system/article/edit',//文章新闻列表
        'system-download-list': 'app/system/download/list',//下载列表
        'system-download-edit': 'app/system/download/edit',//下载编辑

        //工作流
        'workflow-designer-list': 'app/workflow/designer/list', //流程列表
        'workflow-designer-edit': 'app/workflow/designer/edit', //流程编辑
        'workflow-designer-activity': 'app/workflow/designer/activity', //流程活动编辑
        'workflow-designer-line': 'app/workflow/designer/line', //连线
        'workflow-form-ueditor': 'app/workflow/form/ueditor', //设计器
        'workflow-form-list': 'app/workflow/form/list', //流程表单列表
        'workflow-form-edit': 'app/workflow/form/edit', //流程表单编辑
        'workflow-comment-list': 'app/workflow/comment/list', //意见列表
        'workflow-comment-mylist': 'app/workflow/comment/mylist', //意见列表
        'workflow-comment-edit': 'app/workflow/comment/edit', //意见编辑
        'workflow-button-list': 'app/workflow/button/list', //按钮列表
        'workflow-button-edit': 'app/workflow/button/edit', //按钮编辑
        'workflow-needdo-list': 'app/workflow/needdo/list', //待处理
        'workflow-havedo-list': 'app/workflow/havedo/list', //已处理
        'workflow-havesend-list': 'app/workflow/havesend/list',//已发送
        'workflow-agent-list': 'app/workflow/agent/list',//代理流程
        'workflow-delegate-list': 'app/workflow/deletate/list',//委托流程
        'workflow-monitoring-list': 'app/workflow/monitoring/list'//流程监控

    }
})