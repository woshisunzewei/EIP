var template;
define(['artTemplate'],
    function (artTemplate) {
        template = artTemplate;
    });

//获取详细信息
function getById(access, id) {
    if (id != null && id != "") {
        UtilAjaxPost("/Common/Global/GetSystemPrivilegeDetailOutputsByAccessAndValue", { id: id, access: access },
            function (val) {
                //角色
                var roleHtml = template('role-template', val);
                $("#tabs-role").html(roleHtml);

                //岗位
                var postHtml = template('post-template', val);
                $("#tabs-post").html(postHtml);

                //组
                var groupHtml = template('group-template', val);
                $("#tabs-group").html(groupHtml);

                //组织机构
                var organizationHtml = template('organization-template', val);
                $("#tabs-organization").html(organizationHtml);

                //人员
                var userHtml = template('user-template', val);
                $("#tabs-user").html(userHtml);
            }
        );
    } else {

        var val = [];
        //角色
        var roleHtml = template('role-template', val);
        $("#tabs-role").html(roleHtml);

        //岗位
        var postHtml = template('post-template', val);
        $("#tabs-post").html(postHtml);

        //组
        var groupHtml = template('group-template', val);
        $("#tabs-group").html(groupHtml);

        //组织机构
        var organizationHtml = template('organization-template', val);
        $("#tabs-organization").html(organizationHtml);

        //人员
        var userHtml = template('user-template', val);
        $("#tabs-user").html(userHtml);
    }
}